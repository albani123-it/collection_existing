using AutoMapper;
using Collectium.Model.Helper;
using Collectium.Model;
using Microsoft.EntityFrameworkCore;
using Collectium.Model.Entity;
using System.Globalization;
using Collectium.Model.Bean.Request;
using System.Diagnostics.Metrics;
using DocumentFormat.OpenXml.Office.CustomUI;
using Collectium.Model.Bean.ListRequest;
using Collectium.Validation;
using Collectium.Model.Bean;
using Collectium.Model.Bean.Response;
using static Collectium.Model.Bean.Response.RestructureResponse;

namespace Collectium.Service
{
    public class RejigService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<DistribusiDataService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly IntegrationService intService;
        private readonly DistribusiDataService dataService;
        private readonly IMapper mapper;


        public RejigService(CollectiumDBContext ctx,
                                ILogger<DistribusiDataService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                IntegrationService intService,
                                DistribusiDataService dataService,
                                IMapper mapper)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.pagination = pagination;
            this.statusService = statusService;
            this.mapper = mapper;
            this.intService = intService;
            this.dataService = dataService;
        }

        public void RejigMasterLoanBranch()
        {

            var qy = this.ctx.MasterLoan.OrderBy(o => o.Id).ToList();
            foreach (var item in qy)
            {
                var prev = item.ChannelBranchCode;
                this.ctx.Entry(item).Reference(r => r.Customer).Load();
                this.ctx.Entry(item.Customer!).Reference(r => r.Branch).Load();

                item.ChannelBranchCode = item.Customer!.Branch!.Code;

                this.logger.LogInformation("RejigMasterLoanBranch >>> rejig " + item.Id + " from: " + prev + " to: " + item.ChannelBranchCode);

                this.ctx.Update(item);
            }
        }

        public void RejigMasterLoanBranchFromStg()
        {

            var qy = this.ctx.Customer.OrderBy(o => o.Id).Include(i => i.Branch).ToList();
            foreach (var item in qy)
            {
                var stg = this.ctx.STGCustomer.Where(q => q.CU_CIF == item.Cif).OrderByDescending(o => o.STG_DATE).ToList();
                if (stg != null && stg.Count() > 0)
                {
                    var stgg = stg![0];
                    if (item.Branch!.Code!.Equals(stgg.BRANCH_CODE) == false)
                    {
                        this.logger.LogInformation("RejigMasterLoanBranchFromStg >>> diff cust: " + item.Id + " code: " + item.Branch.Code + " with stg: " + stgg.BRANCH_CODE);
                        var nb = this.ctx.Branch.FirstOrDefault(o => o.Code!.ToLower().Equals(stgg.BRANCH_CODE));
                        item.Branch = nb;
                        this.ctx.Customer.Update(item);
                       

                        var ml = this.ctx.MasterLoan.Where(q => q.CustomerId == item.Id).ToList();
                        if (ml != null && ml.Count() > 0) {
                            var mlx = ml[0];
                            mlx.ChannelBranchCode = stgg.BRANCH_CODE;
                            this.ctx.MasterLoan.Update(mlx);

                            var ccs = this.ctx.CollectionCall.Where(q => q.LoanId == mlx.Id).OrderByDescending(o => o.Id).ToList();
                            if (ccs != null && ccs.Count() > 0)
                            {
                                var cc = ccs[0];
                                cc.Branch = nb;
                                this.ctx.CollectionCall.Update(cc);
                            }
                        }

                        this.ctx.SaveChanges();
                    }
                }
            }
        }

        public void RejigData()
        {

            this.logger.LogInformation("RejigData >>> started");

            this.RejigDCV2();

            this.RejigFCV2();

            this.ReCount();

            this.logger.LogInformation("RejigData >>> finished");
        }

        public void RejigDataRedist()
        {

            this.logger.LogInformation("RejigDataRedist >>> started");

            this.ctx.Database.ExecuteSqlRaw("UPDATE COLLECTION_CALL SET CALL_BY = NULL");

            this.RejigDC();

            this.RejigFC();

            this.ReCount();

            this.logger.LogInformation("RejigDataRedist >>> finished");
        }

        public void RejigFC()
        {
            this.logger.LogInformation("RejigFC >>> started");
            var dateProcess = DateTime.Now;
            var todayData = this.ctx.MasterLoan.Where(o => o.STG_DATE!.Value.Date == dateProcess.Date)
                                                .Where(o => o.Dpd > 14);

            //var allBranch = this.ctx.Branch.Where(q => q.Id.Equals(1)).ToList();
            var allBranch = this.ctx.Branch.OrderBy(o => o.Id).ToList();

            foreach (var cabang in allBranch)
            {

                this.logger.LogInformation("RejigFC >>> process cabang: " + cabang.Code);
                var dataCabang = todayData.Where(o => o.ChannelBranchCode!.ToLower().Equals(cabang!.Code!.ToLower())).OrderBy(o => o.Cif).ToList();

                if (dataCabang.Count > 0)
                {
                    this.logger.LogInformation("RejigFC >>> process cabang: " + cabang.Code + " di process");

                    var fcCabang = this.ctx.UserBranch.Where(q => q.Branch!.Id == cabang.Id).Where(q => q.User!.Role!.Id == 4)
                                           .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                                           .Include(i => i.User)
                                           .ToList();

                    int countFC = fcCabang.Count;
                    int counterFC = 0;
                    int counterExistingFC = 0;
                    int prevCounterFCId = 0;

                    foreach (var data in dataCabang)
                    {
                        this.logger.LogInformation("RejigFC >>> process loanid: " + data.Id);
                        var checkAssign = this.ctx.CollectionCall.FirstOrDefault(o => o.LoanId == data.Id);
                        if (checkAssign != null) //sudah pernah di assign, data sudah pernah turun
                        {
                            this.logger.LogInformation("RejigFC >>> process loanid: " + data.Id + " " + data.AccNo + " dpd = " + data.Dpd + " existing");

                            this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();

                            this.logger.LogInformation("RejigFC >>> process loanid: " + data.Id + " last 7 day not found");

                            var FCReady = this.ctx.UserBranch.Where(o => o.UserId != checkAssign.CallById)
                                                  .Where(o => o.User!.RoleId!.Equals(4))
                                                  .Where(q => q.BranchId.Equals(cabang.Id))
                                                  .OrderBy(o => o.UserId).ToList();

                            if (FCReady != null && FCReady.Count() > 0)
                            {
                                var xcnt = FCReady.Count();
                                if (counterExistingFC == prevCounterFCId)
                                {
                                    if (counterExistingFC != 0 && prevCounterFCId != 0)
                                    {
                                        counterExistingFC += 1;
                                    }

                                }
                                if (xcnt == 1)
                                {
                                    counterExistingFC = 0;
                                }
                                checkAssign.CallById = FCReady[counterExistingFC].UserId;
                                checkAssign.CallResultId = 10;
                                prevCounterFCId = counterExistingFC;
                                this.ctx.CollectionCall.Update(checkAssign);
                                this.ctx.SaveChanges();
                                this.logger.LogInformation("RejigFC >>> process loanid: " + data.Id + " assign to " + checkAssign.CallById);

                                counterExistingFC += 1;


                                if (counterExistingFC == countFC - 1)
                                {
                                    counterExistingFC = 0;
                                }

                            }
                        }
                    }
                }

            }
            this.logger.LogInformation("RejigFC >>> finished");
        }

        public void RejigDC()
        {
            this.logger.LogInformation("RejigDC >>> started");

            var dateProcess = DateTime.Now;
            var todayData = this.ctx.MasterLoan.Where(o => o.STG_DATE!.Value.Date == dateProcess.Date)
                                                .Where(o => o.Dpd < 15)
                                                .ToList();//data hari ini

            var dcCabang = this.ctx.User.Where(q => q.RoleId == 3).Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();

            if (todayData.Count > 0)
            {
                int countDC = dcCabang.Count;
                int counterDC = 0;
                int counterExistingDC = 0;
                int prevCounterDCId = 0;

                foreach (var data in todayData)
                {
                    this.ctx.Entry(data).Reference(r => r.Customer).Load();

                    this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id);
                    var checkAssign = this.ctx.CollectionCall.Where(o => o.LoanId == data.Id)
                                            .Include(i => i.CallBy)
                                            .FirstOrDefault();
                    if (checkAssign != null) //sudah pernah di assign, data sudah pernah turun
                    {
                        this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id + " existing");

                        this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id + " ");
                        this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();

                        var DCReady = this.ctx.User.Where(o => o.RoleId!.Equals(3))
                                              .Where(o => o.Status!.Name!.Equals("AKTIF"))
                                              .OrderBy(o => o.Id)
                                              .ToList();
                        if (DCReady != null && DCReady.Count() > 0)
                        {
                            var xcnt = DCReady.Count();
                            if (counterExistingDC == prevCounterDCId)
                            {
                                if (counterExistingDC != 0 && prevCounterDCId != 0)
                                {
                                    counterExistingDC += 1;
                                }

                            }

                            if (xcnt == 1)
                            {
                                counterExistingDC = 0;
                            }

                            checkAssign.CallById = DCReady[counterExistingDC].Id;
                            checkAssign.CallResultId = 10;
                            prevCounterDCId = counterExistingDC;
                            this.ctx.CollectionCall.Update(checkAssign);
                            this.ctx.SaveChanges();
                            this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id + " assign to " + checkAssign.CallById);
                            counterExistingDC += 1;
                            if (counterExistingDC == countDC - 1)
                            {
                                counterExistingDC = 0;
                            }
                        }
                    }
                }
            }

            this.logger.LogInformation("DistributionDataDC >>> finished");
        }

        public void RejigDCV2()
        {
            this.logger.LogInformation("RejigDCV2 >>> started");

            var lqu = new List<UserDistribution>();
            var qu = this.ctx.User.Where(q => q.RoleId == 3).Where(q => q.StatusId == 1).ToList();
            foreach (var i in qu)
            {
                var ud = new UserDistribution();
                ud.UserId = i.Id!.Value;
                ud.Count = 0;
                lqu.Add(ud);
            }

            var ql = from cc in this.ctx.CollectionCall group cc by cc.CallById into g select new { g.Key, Cnt = g.Count() };

            foreach (var i in lqu)
            {
                var tt = from xx in ql where xx.Key == i.UserId select xx.Cnt;
                var tl = tt.ToList();
                if (tl.Count > 0)
                {
                    i.Count = tl[0];
                }


            }

            foreach (var i in lqu)
            {
                this.logger.LogInformation("data: " + i.UserId + " value => " + i.Count);
            }

            var cqu = lqu.OrderBy(o => o.Count).ToList();

            var dateProcess = DateTime.Now;
            var todayData = this.ctx.MasterLoan.Where(o => o.STG_DATE!.Value.Date == dateProcess.Date)
                                    .Where(o => o.Dpd < 15)
                                    .OrderBy(s => s.Cif)
                                    .ToList();//data hari ini

            var cnt = 0;
            var prevId = 0;
            var prevCif = "";

            foreach (var data in todayData)
            {
                this.ctx.Entry(data).Reference(r => r.Customer).Load();

                this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id);
                var checkAssign = this.ctx.CollectionCall.Where(o => o.LoanId == data.Id)
                                        .Include(i => i.CallBy)
                                        .FirstOrDefault();
                if (checkAssign != null) //sudah pernah di assign, data sudah pernah turun
                {
                    this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id + " existing");
                    this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();

                    this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id + " check existing role");
                    if (checkAssign.CallBy! != null && checkAssign.CallBy!.RoleId! == 3)
                    {
                        var lastSevenDay = DateTime.Now.AddDays(-7);
                        var firstAssign = this.ctx.MasterLoanHistory
                            .Where(q => q.LoanId.Equals(checkAssign.LoanId))
                            .Where(q => q.CallById.Equals(checkAssign.CallById))
                            .Where(q => q.STG_DATE!.Value.Date >= lastSevenDay.Date)
                            .Count();

                        this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id + " count first assign: " + firstAssign);

                        if (firstAssign >= 7)
                        {
                            this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id + " first assign >= 7 cnt: " + cnt + " current assign is :" + checkAssign.CallById);
                            var next = cqu[cnt].UserId;

                            if (next == checkAssign.CallById)
                            {
                                cnt++;
                                if (cnt >= cqu.Count())
                                {
                                    cnt = 0;
                                }
                                next = cqu[cnt].UserId;
                            }

                            checkAssign.CallById = next;
                            checkAssign.CallResultId = 10;

                            this.ctx.CollectionCall.Update(checkAssign);
                            this.ctx.SaveChanges();
                            this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id + " assign to " + checkAssign.CallById);

                            cnt++;
                        }
                        else
                        {
                            this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id + " first assign < 7 cnt: " + cnt + " current assign is :" + checkAssign.CallById);

                            var ptp = this.ctx.CollectionHistory.Where(q => q.AccNo!.Equals(checkAssign.AccNo))
                              .Where(q => q.CallById.Equals(checkAssign.CallById))
                              .Count();
                            if (ptp > 2)
                            {
                                var next = cqu[cnt].UserId;

                                checkAssign.CallById = next;
                                checkAssign.CallResultId = 10;

                                this.ctx.CollectionCall.Update(checkAssign);
                                this.ctx.SaveChanges();
                                this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id + " assign to " + checkAssign.CallById);
                            }
                            cnt++;
                        }

                        var mlh = new MasterLoanHistory();
                        mlh.LoanId = data.Id;
                        mlh.CallById = checkAssign.CallById;
                        mlh.STG_DATE = DateTime.Now;
                        this.dataService.CopyLoanHistory(data, mlh);
                        this.ctx.MasterLoanHistory.Add(mlh);
                        this.ctx.SaveChanges();
                    }
                    else
                    {
                        this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id + " check existing role is FC, treat as new");

                        var prevCaller = checkAssign.CallById;

                        var next = cqu[cnt].UserId;

                        checkAssign.CallById = next;
                        checkAssign.CallResultId = 10;

                        this.ctx.CollectionCall.Update(checkAssign);
                        this.ctx.SaveChanges();
                        this.logger.LogInformation("RejigDC >>> process loanid: " + data.Id + " assign to " + checkAssign.CallById);


                        var tdy = dateProcess.ToString("yyyy-MM-dd");

                        var pay = new PaymentHistory();
                        //pay.Loan = item;
                        pay.Denda = data.TunggakanDenda;
                        pay.TotalBayar = data.KewajibanTotal;
                        pay.PokokCicilan = data.TunggakanPokok;
                        pay.AccNo = data.AccNo;
                        pay.Bunga = data.TunggakanBunga;
                        pay.Tgl = DateTime.Now;
                        pay.CreateDate = DateTime.Now;

                        pay.CallById = prevCaller;

                        //var stv2 = this.dataService.GetPaymentFromT24(data.AccNo!, data.LoanNumber!, tdy);
                        //if (stv2 != null)
                        //{
                        //    try
                        //    {
                        //        pay.TotalBayar = Convert.ToDouble(stv2.Amount);
                        //        DateTime dt = DateTime.ParseExact(stv2.Date!, "dd MMM yyyy", CultureInfo.InvariantCulture);
                        //        pay.Tgl = dt;
                        //    }
                        //    catch (Exception e)
                        //    {
                        //        this.logger.LogError(e.Message);
                        //    }

                        //}

                        var nw = pay.Tgl.Value.Date;

                        /*
                        var prv = this.ctx.PaymentHistory.Where(q => q.AccNo!.Equals(data.AccNo))
                                                .Where(q => q.Tgl!.Value.Date.Equals(nw)).FirstOrDefault();
                        if (prv == null)
                        {
                            this.ctx.PaymentHistory.Add(pay);
                            this.ctx.SaveChanges();
                        }
                        */

                        //Add payment record

                        var payr = new PaymentRecord();
                        payr.AccNo = data.AccNo;
                        payr.RecordDate = DateTime.Now;
                        payr.CallById = prevCaller;
                        payr.Call = checkAssign;
                        this.ctx.PaymentRecord.Add(payr);
                        this.ctx.SaveChanges();

                        // update generate letter
                        var gl = this.ctx.GenerateLetter.Where(q => q.LoanId.Equals(data.Id)).Where(q => q.StatusId.Equals(1)).FirstOrDefault();
                        if (gl != null)
                        {
                            gl.StatusId = 2;
                            this.ctx.GenerateLetter.Update(gl);
                            this.ctx.SaveChanges();
                        }
                    }

                }
                else
                {
                    var callDC = new CollectionCall();
                    callDC.LoanId = data.Id;
                    callDC.BranchId = data.Customer!.BranchId!;
                    callDC.AccNo = data.AccNo;

                    var next = cqu[cnt].UserId;

                    callDC.CallById = next;
                    callDC.CallResultId = 10;
                    this.ctx.CollectionCall.Add(callDC);
                    this.ctx.SaveChanges();

                    var mlh = new MasterLoanHistory();
                    mlh.LoanId = data.Id;
                    mlh.CallById = callDC.CallById;
                    mlh.STG_DATE = DateTime.Now;
                    this.dataService.CopyLoanHistory(data, mlh);
                    this.ctx.MasterLoanHistory.Add(mlh);
                    this.ctx.SaveChanges();
                }

                if (cnt >= cqu.Count())
                {
                    cnt = 0;
                }
            }

            this.logger.LogInformation("RejigDCV2 >>> finished");
        }

        public void RejigFCV2()
        {

            this.logger.LogInformation("RejigFCV2 >>> started");
            var dateProcess = DateTime.Now;
            var allBranch = this.ctx.Branch.OrderBy(o => o.Id).ToList();

            foreach (var cabang in allBranch)
            {

                this.logger.LogInformation("RejigFCV2 >>> process cabang: " + cabang.Code);
                var todayData = this.ctx.MasterLoan.Where(o => o.STG_DATE!.Value.Date == dateProcess.Date)
                                    .Where(o => o.Dpd > 14)
                                    .Where(o => o.ChannelBranchCode!.ToLower().Equals(cabang!.Code!.ToLower()))
                                    .OrderBy(o => o.Cif)
                                    .ToList();//data hari ini
                if (todayData.Count() < 1)
                {
                    continue;
                }

                var lqu = new List<UserDistribution>();
                var qu = this.ctx.UserBranch.Where(q => q.User!.RoleId == 4)
                                            .Where(q => q.User!.StatusId == 1)
                                            .Where(q => q.BranchId == cabang.Id)
                                            .Include(i => i.User)
                                            .ToList();

                if (qu.Count() < 1)
                {
                    continue;
                }
                foreach (var i in qu)
                {
                    var ud = new UserDistribution();
                    ud.UserId = i.User!.Id!.Value;
                    ud.Count = 0;
                    lqu.Add(ud);
                }

                var ql = from cc in this.ctx.CollectionCall group cc by cc.CallById into g select new { g.Key, Cnt = g.Count() };

                foreach (var i in lqu)
                {
                    var tt = from xx in ql where xx.Key == i.UserId select xx.Cnt;
                    var tl = tt.ToList();
                    if (tl.Count > 0)
                    {
                        i.Count = tl[0];
                    }


                }

                foreach (var i in lqu)
                {
                    this.logger.LogInformation("data: " + i.UserId + " value => " + i.Count);
                }

                var cqu = lqu.OrderBy(o => o.Count).ToList();


                var cnt = 0;
                var prevId = 0;
                var prevCif = "";

                foreach (var data in todayData)
                {
                    this.logger.LogInformation("RejigFCV2 >>> process loanid: " + data.Id);
                    var checkAssign = this.ctx.CollectionCall.FirstOrDefault(o => o.LoanId == data.Id);
                    if (checkAssign != null) //sudah pernah di assign, data sudah pernah turun
                    {
                        this.logger.LogInformation("RejigFCV2 >>> process loanid: " + data.Id + " " + data.AccNo + " dpd = " + data.Dpd + " existing");

                        var role = 4;

                        if (checkAssign.CallBy != null)
                        {
                            this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                            role = checkAssign!.CallBy!.RoleId!.Value;
                        }

                        if (role == 3)
                        {
                            this.logger.LogInformation("RejigFCV2 >>> process loanid: " + data.Id + " prev is dc");
                            var next = cqu[cnt].UserId;
                            checkAssign.CallById = next;
                            checkAssign.CallResultId = 10;
                            this.ctx.CollectionCall.Update(checkAssign);
                            this.ctx.SaveChanges();

                            this.logger.LogInformation("RejigFCV2 >>> process loanid: " + data.Id + " " + data.AccNo + " dpd = " + data.Dpd + " existing assigned to " + checkAssign.CallById);

                            cnt++;
                        }
                        else
                        {
                            this.logger.LogInformation("RejigFCV2 >>> process loanid: " + data.Id + " check last 7 day");

                            var lastSevenDay = DateTime.Now.AddDays(-15);
                            var firstAssign = this.ctx.MasterLoanHistory
                                .Where(q => q.LoanId.Equals(checkAssign.LoanId))
                                .Where(q => q.CallById.Equals(checkAssign.CallById))
                                .Where(q => q.STG_DATE!.Value.Date >= lastSevenDay.Date)
                                .Count();

                            if (firstAssign >= 15)
                            {
                                this.logger.LogInformation("RejigFCV2 >>> process loanid: " + data.Id + " last 7 day greater than 7");
                                var next = cqu[cnt].UserId;

                                if (next == checkAssign.CallById)
                                {
                                    cnt++;
                                    if (cnt >= cqu.Count())
                                    {
                                        cnt = 0;
                                    }
                                    next = cqu[cnt].UserId;
                                }

                                checkAssign.CallById = next;
                                checkAssign.CallResultId = 10;

                                this.ctx.CollectionCall.Update(checkAssign);
                                this.ctx.SaveChanges();

                                this.logger.LogInformation("RejigFCV2 >>> process loanid: " + data.Id + " assign to " + checkAssign.CallById);


                            }
                        }

                        var mlh = new MasterLoanHistory();
                        mlh.LoanId = data.Id;
                        mlh.CallById = checkAssign.CallById;
                        mlh.STG_DATE = DateTime.Now;
                        this.dataService.CopyLoanHistory(data, mlh);
                        this.ctx.MasterLoanHistory.Add(mlh);
                        this.ctx.SaveChanges();
                    }
                    else //data baru
                    {

                        if (todayData.Count < 1)
                        {
                            continue;
                        }

                        Console.WriteLine("sadasd cnt: " + cnt);
                        var next = cqu[cnt].UserId;

                        this.logger.LogInformation("RejigFCV2 >>> process loanid: " + data.Id + " " + data.AccNo + " dpd = " + data.Dpd + " new");
                        var callFC = new CollectionCall();
                        callFC.LoanId = data.Id;
                        callFC.BranchId = cabang.Id;
                        callFC.AccNo = data.AccNo;
                        callFC.CallById = next;
                        callFC.CallResultId = 10;
                        this.ctx.CollectionCall.Add(callFC);
                        this.ctx.SaveChanges();

                        var mlh = new MasterLoanHistory();
                        mlh.LoanId = data.Id;
                        mlh.CallById = callFC.CallById;
                        mlh.STG_DATE = DateTime.Now;
                        this.dataService.CopyLoanHistory(data, mlh);
                        this.ctx.MasterLoanHistory.Add(mlh);
                        this.ctx.SaveChanges();
                    }

                    if (cnt >= cqu.Count())
                    {
                        cnt = 0;
                    }
                }

                this.logger.LogInformation("RejigDCV2 >>> finished");
            }
        }

        public void ReCount()
        {
            this.logger.LogInformation("ReCount >>> started");

            var q = from cc in  this.ctx.CollectionCall where cc.Loan!.Status == 1 orderby cc.Id select new { cc.Id, cc.CallById, cc.AccNo, cc.Loan.KewajibanTotal, cc.Loan!.Kolektibilitas, cc.Loan.Dpd, cc.CallResultId };
            var qq = q.ToList();

            var cnt = 0;
            foreach (var item in qq)
            {
                var trace = new CollectionTrace();
                trace.TraceDate = DateTime.Now;
                trace.CallId = item.Id;
                trace.CallById = item.CallById;
                trace.Kolek = item.Kolektibilitas;
                trace.DPD = item.Dpd;
                trace.AccNo = item.AccNo;
                trace.Amount = item.KewajibanTotal;
                trace.ResultId = item.CallResultId;

                this.ctx.Add(trace);
                cnt++;

                if (cnt > 20)
                {
                    cnt = 0;
                    this.ctx.SaveChanges();
                }
            }

            this.logger.LogInformation("ReCount >>> finished");
        }

        public GenericResponse<string> DistribusiManual(DistribusiBean filter)
        {
            this.logger.LogInformation("DistribusiManual >>> started");

            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.AgentId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            IQueryable<CollectionCall> q = this.ctx.CollectionCall.Include(i => i.Loan);
            if (filter.Dpdmin != null && filter.Dpdmin > 0)
            {
                if ( filter.Dpdmax != null && filter.Dpdmax > 0 && filter.Dpdmax > filter.Dpdmin)
                {
                    q = q.Where(q => q.Loan!.Dpd >= filter.Dpdmin).Where(q => q.Loan!.Dpd <= filter.Dpdmax);
                } 
                else
                {
                    q = q.Where(q => q.Loan!.Dpd == filter.Dpdmin);
                }

            }

            if (filter.Kolmin != null && filter.Kolmin > 0)
            {
                if (filter.Kolmax != null && filter.Kolmax > 0 && filter.Kolmax > filter.Kolmin)
                {
                    q = q.Where(q => q.Loan!.Kolektibilitas >= filter.Kolmin).Where(q => q.Loan!.Kolektibilitas <= filter.Kolmax);
                }
                else
                {
                    q = q.Where(q => q.Loan!.Kolektibilitas == filter.Kolmin);
                }

            }

            if (filter.Tunggakanmin != null && filter.Tunggakanmin > 0)
            {
                if (filter.Tunggakanmax != null && filter.Tunggakanmax > 0 && filter.Tunggakanmax > filter.Tunggakanmin)
                {
                    q = q.Where(q => q.Loan!.KewajibanTotal >= filter.Tunggakanmin).Where(q => q.Loan!.KewajibanTotal <= filter.Tunggakanmax);
                }
                else
                {
                    q = q.Where(q => q.Loan!.KewajibanTotal == filter.Tunggakanmin);
                }

            }

            if (filter.ProductId != null && filter.ProductId > 0)
            {
                q = q.Where(q => q.Loan!.ProductId >= filter.ProductId);
            }

            if (filter.BranchId != null && filter.BranchId > 0)
            {
                q = q.Where(q => q.BranchId >= filter.BranchId);
            }

            var data = q.ToList();
            var i = 0;
            foreach (var item in data)
            {
                this.logger.LogInformation("DistribusiManual >>> assign: " + item.Id + " : " + item.AccNo);

                item.CallById = filter.AgentId;
                item.CallResultId = 10;
                this.ctx.CollectionCall.Update(item);

                var trace = new CollectionTrace();
                trace.TraceDate = DateTime.Now;
                trace.CallId = item.Id;
                trace.CallById = item.CallById;
                trace.Kolek = item.Loan!.Kolektibilitas;
                trace.DPD = item.Loan!.Dpd;
                trace.AccNo = item.AccNo;
                trace.Amount = item.Loan!.KewajibanTotal;
                trace.ResultId = item.CallResultId;
                this.ctx.CollectionTrace.Add(trace);

                i++;

                if (i > 99)
                {
                    i = 0;
                    this.ctx.SaveChanges();
                }
            }
            this.ctx.SaveChanges();

            this.logger.LogInformation("DistribusiManual >>> finished");

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<DistribusiBeanSave> SaveDistribusiManual(DistribusiBeanSave filter)
        {
            this.logger.LogInformation("SaveDistribusiManual >>> started");

            var wrap = new GenericResponse<DistribusiBeanSave>
            {
                Status = false,
                Message = ""
            };
            
            var trace = new DistributionRule();
            trace.Name = filter.Name;
            trace.Code = filter.Code;
            trace.DpdMin = filter.Dpdmin;
            trace.DpdMax = filter.Dpdmax;
            trace.KolMin = filter.Kolmin;
            trace.KolMax = filter.Kolmax;
            trace.TunggakanMin = filter.Tunggakanmin;
            trace.TunggakanMax = filter.Tunggakanmax;
            trace.BranchId = filter.BranchId;
            trace.ProductId = filter.ProductId;
            trace.Group = filter.Group;
            trace.StatusId = 1;
            this.ctx.DistributionRule.Add(trace);
            this.ctx.SaveChanges();

            if (filter.AgentId != null && filter.AgentId.Count() > 0)
            {
                foreach(var i in filter.AgentId)
                {
                    var tr = new AgentDistribution();
                    tr.UserId = i;
                    tr.DistId = trace.Id;
                    this.ctx.AgentDistribution.Add(tr);
                }
            }

            if (filter.LoanId != null && filter.LoanId.Count() > 0)
            {
                foreach (var i in filter.LoanId)
                {
                    var tr = new AgentLoan();
                    tr.LoanId = i;
                    tr.DistId = trace.Id;
                    this.ctx.AgentLoan.Add(tr);
                }
            }

            this.ctx.SaveChanges();

            this.logger.LogInformation("DistribusiManual >>> finished");

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<DistribusiManualList> DistribusiManualList(DistribusiManualReq filter)
        {
            this.logger.LogInformation("DistribusiManualList >>> started");

            var wrap = new GenericResponse<DistribusiManualList>
            {
                Status = false,
                Message = ""
            };

            IQueryable<DistributionRule> q = this.ctx.DistributionRule.Include(i => i.Product).Include(i => i.Branch);
            q = q.Where(q => q.Status!.Name!.Equals("AKTIF"));

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<DistribusiManualList>();
            foreach (var it in data)
            {
                var dto = mapper.Map<DistribusiManualList>(it);
                ldata.Add(dto);
            }

            this.logger.LogInformation("DistribusiManualList >>> finished");

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollResponseBean> DistribusiNasabahList(DistribusiNasabahReq filter)
        {
            this.logger.LogInformation("DistribusiNasabahList >>> started");

            var wrap = new GenericResponse<CollResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<MasterLoan> q = this.ctx.MasterLoan.Include(i => i.Customer).ThenInclude(i => i.Branch).Include(i => i.Product);
            if (filter.Dpdmin != null && filter.Dpdmin > 0)
            {
                if (filter.Dpdmax != null && filter.Dpdmax > 0)
                {
                    q = q.Where(q => q.Dpd >= filter.Dpdmin).Where(q => q.Dpd <= filter.Dpdmax);
                } 
                else
                {
                    q = q.Where(q => q.Dpd == filter.Dpdmin);
                }
            }

            if (filter.Kolmin != null && filter.Kolmin > 0)
            {
                if (filter.Kolmax != null && filter.Kolmax > 0)
                {
                    q = q.Where(q => q.Kolektibilitas >= filter.Kolmin).Where(q => q.Kolektibilitas <= filter.Kolmax);
                }
                else
                {
                    q = q.Where(q => q.Kolektibilitas == filter.Kolmin);
                }
            }

            if (filter.Tunggakanmin != null && filter.Tunggakanmin > 0)
            {
                if (filter.Tunggakanmax != null && filter.Tunggakanmax > 0)
                {
                    q = q.Where(q => q.Kolektibilitas >= filter.Tunggakanmin).Where(q => q.Kolektibilitas <= filter.Tunggakanmax);
                }
                else
                {
                    q = q.Where(q => q.Kolektibilitas == filter.Tunggakanmin);
                }
            }

            if (filter.BranchId != null && filter.BranchId > 0)
            {
                q = q.Where(q => q.Customer!.BranchId == filter.BranchId);
            }

            if (filter.ProductId != null && filter.ProductId > 0)
            {
                q = q.Where(q => q.ProductId == filter.ProductId);
            }

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<CollResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CollResponseBean>(it);
                dto.Branch = it.Customer!.Branch!.Name;
                ldata.Add(dto);
            }

            this.logger.LogInformation("DistribusiManualList >>> finished");

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserResponseBean> DistribusiAgentList(DistribusiAgentReq filter)
        {
            this.logger.LogInformation("DistribusiAgentList >>> started");

            var wrap = new GenericResponse<UserResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<User> q = this.ctx.User;

            var grup = "DC";
            if (filter.Group != null)
            {
                grup = filter.Group;
            }

            List<User> ls = null;

            if (grup == "DC")
            {
                ls = this.ctx.User.Where(q => q.RoleId.Equals(3)).Where(q => q.Status!.Name!.Equals("AKTIF")).Include(i => i.Role).ToList();
            } else
            {
                ls = this.ctx.User.Where(q => q.RoleId.Equals(4)).Where(q => q.Status!.Name!.Equals("AKTIF")).Include(i => i.Role).ToList();
            }


            var ldata = new List<UserResponseBean>();
            foreach (var it in ls)
            {
                var dto = mapper.Map<UserResponseBean>(it);
                ldata.Add(dto);
            }

            this.logger.LogInformation("DistribusiManualList >>> finished");

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<DistribusiManualGet> DistribusiManualGet(UserReqApproveBean filter)
        {
            this.logger.LogInformation("DistribusiManualList >>> started");

            var wrap = new GenericResponse<DistribusiManualGet>
            {
                Status = false,
                Message = ""
            };

            IQueryable<DistributionRule> q = this.ctx.DistributionRule.Include(i => i.Product).Include(i => i.Branch).Where(q => q.Id == filter.Id);
            q = q.Where(q => q.Status!.Name!.Equals("AKTIF"));
            var data = q.ToList();

            var ldata = new List<DistribusiManualGet>();
            foreach (var it in data)
            {
                var dto = mapper.Map<DistribusiManualGet>(it);

                var agt = this.ctx.AgentLoan.Where(q => q.DistId == it.Id).Include(i => i.Loan).ToList();
                if (agt != null && agt.Count() > 0)
                {
                    var al = new List<CollResponseBean>();
                    foreach(var a in agt)
                    {
                        this.ctx.Entry(a.Loan!).Reference(r => r.Customer).Load();
                        this.ctx.Entry(a.Loan!).Reference(r => r.Product).Load();
                        var dtoL = mapper.Map<CollResponseBean>(a.Loan);
                        al.Add(dtoL);
                    }
                    dto.LoanId = al;
                }

                var adt = this.ctx.AgentDistribution.Where(q => q.DistId == it.Id).Include(i => i.User).ToList();
                if (adt != null && adt.Count() > 0)
                {
                    var al = new List<UserResponseBean>();
                    foreach (var a in adt)
                    {
                        this.ctx.Entry(a.User!).Reference(r => r.Role).Load();
                        var dtoL = mapper.Map<UserResponseBean>(a.User);
                        al.Add(dtoL);
                    }
                    dto.AgentId = al;
                }

                ldata.Add(dto);
            }

            this.logger.LogInformation("DistribusiManualList >>> finished");

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public void CopyLoanHistory(MasterLoan src, MasterLoanHistory mh)
        {
            IlKeiCopyObject.Instance.WithSource(src)
                .WithDestination(mh)
                .Include(nameof(src.AccNo))
                .Include(nameof(src.Ccy))
                .Include(nameof(src.ChannelBranchCode))
                .Include(nameof(src.Cif))
                .Include(nameof(src.CustomerId))
                .Include(nameof(src.Dpd))
                .Include(nameof(src.Fasilitas))
                .Include(nameof(src.EconName))
                .Include(nameof(src.EconPhone))
                .Include(nameof(src.EconRelation))
                .Include(nameof(src.Installment))
                .Include(nameof(src.InstallmentPokok))
                .Include(nameof(src.InterestRate))
                .Include(nameof(src.KewajibanTotal))
                .Include(nameof(src.Kolektibilitas))
                .Include(nameof(src.LastPayDate))
                .Include(nameof(src.MarketingCode))
                .Include(nameof(src.MaturityDate))
                .Include(nameof(src.Outstanding))
                .Include(nameof(src.PayTotal))
                .Include(nameof(src.Plafond))
                .Include(nameof(src.ProductId))
                .Include(nameof(src.ProductSegmentId))
                .Include(nameof(src.SisaTenor))
                .Include(nameof(src.Tenor))
                .Include(nameof(src.TunggakanBunga))
                .Include(nameof(src.TunggakanDenda))
                .Include(nameof(src.TunggakanPokok))
                .Include(nameof(src.TunggakanTotal))
                .Include(nameof(src.PayInAccount))
                .Execute();

        }
    }
}
