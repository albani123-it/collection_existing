using AutoMapper;
using Collectium.Model;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Model.Entity.Staging;
using Collectium.Model.Helper;
using Microsoft.EntityFrameworkCore;
using N.EntityFrameworkCore.Extensions;

namespace Collectium.Service
{
    public class StagingService
    {

        private readonly CollectiumDBContext ctx;

        private readonly CollectiumDBStgContext stg;

        private readonly ILogger<StatusService> logger;

        private readonly PaginationHelper pagination;

        private readonly IMapper mapper;

        public StagingService(CollectiumDBContext ctx, 
                                CollectiumDBStgContext stg,
                                PaginationHelper pagination,
                                ILogger<StatusService> logger, 
                                IMapper mapper)
        {
            this.ctx = ctx;
            this.stg = stg;
            this.logger = logger;
            this.mapper = mapper;
            this.pagination= pagination;
        }

        public void processCopyFromPGToSql()
        {
            this.logger.LogInformation("processCopyFromPGToSql >>> started");
            this.processStgBranch();
            this.processStgLoan();
            this.processCopyKreditNasabahStg();
            this.processStgAddr();
            this.processStgSms();
            this.logger.LogInformation("processCopyFromPGToSql >>> finished");

        }

        private void processCopyKreditNasabahStg()
        {
            this.logger.LogInformation("processCopyKreditNasabahStg >>> started");
            var stgs = this.stg.STGDataKredit.ToList();
            var i = 0;
            var cu = new List<string>();
            foreach (var stg in stgs)
            {
                logger.LogInformation("processCopyFromPGToSql >>> processing data kredit: " + stg.CU_CIF);

                var obj = this.mapper.Map<STGDataKredit>(stg);
                cu.Add(stg.CU_CIF!);
                obj.STG_DATE = DateTime.Today;

                this.ctx.Add(obj);

                var cuss = this.stg.STGCustomer.Where(q => q.CU_CIF!.Equals(stg.CU_CIF)).ToList();
                if (cuss.Count > 0)
                {
                    var cus = cuss[0];
                    var obj1 = this.mapper.Map<STGCustomer>(cus);
                    obj1.STG_DATE = DateTime.Today;
                    this.ctx.Add(obj1);
                }


                if (i >= 40)
                {
                    this.ctx.SaveChanges();
                    i = 0;
                }
                else
                {
                    i++;
                }

            }

            this.ctx.SaveChanges();

            this.logger.LogInformation("processCopyKreditNasabahStg >>> started");
        }

        private void processStgLoan()
        {
            this.logger.LogInformation("processStgLoan >>> started");
            var stgs = this.stg.STGLoanDetail.ToList();
            var i = 0;
            var cu = new List<string>();
            foreach (var stg in stgs)
            {
                logger.LogInformation("processCopyFromPGToSql >>> processing data loan detail: " + stg.ACC_NO);

                var obj = this.mapper.Map<STGLoanDetail>(stg);
                obj.STG_DATE = DateTime.Today;
                obj.LAST_PAYMENT_DATE = stg.LastPaymentFinal;
                this.ctx.Add(obj);


                if (i >= 40)
                {
                    this.ctx.SaveChanges();
                    i = 0;
                }
                else
                {
                    i++;
                }

            }

            this.ctx.SaveChanges();

            this.logger.LogInformation("processStgLoan >>> finished");
        }

        private void processStgBranch()
        {
            this.logger.LogInformation("processStgBranch >>> started");

            var stgBranch = this.stg.STGBranchPg.ToList();
            logger.LogInformation("processStgBranch >>> processing branch num: " + stgBranch.Count());
            var i = 0;
            foreach (var stg in stgBranch)
            {
                logger.LogInformation("processStgBranch >>> processing branch: " + stg.COMPANY_CODE);

                var obj = this.mapper.Map<STGBranch>(stg);
                obj.STG_DATE = DateTime.Today;

                this.ctx.Add(obj);

                if (i >= 40)
                {
                    this.ctx.SaveChanges();
                    i = 0;
                }
                else
                {
                    i++;
                }

            }

            this.ctx.SaveChanges();

            this.logger.LogInformation("processStgBranch >>> finished");
        }

        private void processStgAddr()
        {
            this.logger.LogInformation("processStgAddr >>> started");
            var stgBranch = this.stg.STGCustomerPhone.OrderBy(o => o.CU_CIF).OrderBy(o => o.PHONE).ToList();
            var i = 0;
            var lastPhone = "";
            foreach (var stg in stgBranch)
            {
                logger.LogInformation("processStgAddr >>> processing phone: " + stg.CU_CIF + " no: " + stg.PHONE + " last: " + lastPhone);

                var obj = new STGCustomerPhone();
                obj.CIF= stg.CU_CIF;
                obj.PHONE = stg.PHONE;
                obj.STG_DATE = DateTime.Today;

                if (lastPhone != obj.PHONE)
                {
                    lastPhone = obj.PHONE;
                    this.ctx.Add(obj);
                }


                if (i >= 40)
                {
                    this.ctx.SaveChanges();
                    i = 0;
                }
                else
                {
                    i++;
                }

            }

            this.ctx.SaveChanges();

            this.logger.LogInformation("processStgAddr >>> finished");
        }

        private void processStgSms()
        {
            var stgBranch = this.stg.STGSmsReminder.OrderBy(o => o.CU_CIF).OrderBy(o => o.HP).ToList();
            var i = 0;
            var lastCif = "";
            foreach (var stg in stgBranch)
            {
                logger.LogInformation("processStgSms >>> processing sms: " + stg.CU_CIF + " lastCif: " + lastCif);

                var obj = new STGSmsReminder();
                obj.CU_CIF = stg.CU_CIF;
                obj.ACC_NO = stg.ACC_NO;
                obj.DAY =   stg.DAY;
                obj.NAMA = stg.NAMA;
                obj.HP= stg.HP;
                obj.DUE_DATE= stg.DUE_DATE;
                obj.STG_DATE = DateTime.Today;

                if (lastCif != stg.CU_CIF)
                {
                    lastCif= stg.CU_CIF;
                    this.ctx.Add(obj);
                }


                if (i >= 40)
                {
                    this.ctx.SaveChanges();
                    i = 0;
                }
                else
                {
                    i++;
                }

            }

            this.ctx.SaveChanges();
        }

        public GenericResponse<STGBranch> listStgBranch(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGBranch>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGBranch> q = this.ctx.Set<STGBranch>();

            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.STG_DATE);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<STGCustomer> listSTGCustomer(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGCustomer>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGCustomer> q = this.ctx.Set<STGCustomer>();

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.NO_REKENING!.Equals(filter.AccNo));
            }

            if (filter.Name != null)
            {
                q = q.Where(q => q.CU_FIRSTNAME!.Equals(filter.Name));
            }

            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.STG_DATE);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<STGDataJaminan> listSTGDataJaminan(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGDataJaminan>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGDataJaminan> q = this.ctx.Set<STGDataJaminan>();


            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.STG_DATE);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<STGDataKredit> listSTGDataKredit(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGDataKredit>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGDataKredit> q = this.ctx.Set<STGDataKredit>();

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.ACC_NO!.Equals(filter.AccNo));
            }


            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.STG_DATE);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<STGLoanDetail> listSTGLoanDetail(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGLoanDetail>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGLoanDetail> q = this.ctx.Set<STGLoanDetail>();

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.ACC_NO!.Equals(filter.AccNo));
            }

            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.STG_DATE);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<STGSmsReminder> listSTGSmsReminder(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGSmsReminder>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGSmsReminder> q = this.ctx.Set<STGSmsReminder>();


            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.STG_DATE);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }


        public GenericResponse<STGBranchPg> listSTGBranchPg(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGBranchPg>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGBranchPg> q = this.stg.Set<STGBranchPg>();
            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<STGCustomerPg> listSTGCustomerPg(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGCustomerPg>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGCustomerPg> q = this.stg.Set<STGCustomerPg>();
            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<STGDataJaminanPg> listSTGDataJaminanPg(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGDataJaminanPg>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGDataJaminanPg> q = this.stg.Set<STGDataJaminanPg>();
            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<STGDataKreditPg> listSTGDataKreditPg(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGDataKreditPg>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGDataKreditPg> q = this.stg.Set<STGDataKreditPg>();

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.ACC_NO!.Equals(filter.AccNo));
            }

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<STGLoanDetailPg> listSTGLoanDetailPg(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGLoanDetailPg>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGLoanDetailPg> q = this.stg.Set<STGLoanDetailPg>();

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.ACC_NO!.Equals(filter.AccNo));
            }

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<STGSmsReminderPg> listSTGSmsReminderPg(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<STGSmsReminderPg>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<STGSmsReminderPg> q = this.stg.Set<STGSmsReminderPg>();
            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollResponseBean> listMasterLoan(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<CollResponseBean>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<MasterLoan> q = this.ctx.Set<MasterLoan>();

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.AccNo!.Equals(filter.AccNo));
            }

            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.STG_DATE);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var tmp = new List<CollResponseBean>();

            foreach(var i in data)
            {
                var dto = this.mapper.Map<CollResponseBean>(i);
                tmp.Add(dto);
            }

            wrap.Data = tmp;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<MasterLoan> listMasterLoanV2(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<MasterLoan>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<MasterLoan> q = this.ctx.Set<MasterLoan>();

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.AccNo!.Equals(filter.AccNo));
            }

            if (filter.StatusLoan != null)
            {
                q = q.Where(q => q.Status == filter.StatusLoan);
            }

            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.STG_DATE);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollectionCallBean> listCollectionCall(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<CollectionCallBean>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<CollectionCall> q = this.ctx.Set<CollectionCall>();

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.AccNo!.Equals(filter.AccNo));
            }

            //q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.Id);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var tmp = new List<CollectionCallBean>();

            foreach (var i in data)
            {
                var dto = this.mapper.Map<CollectionCallBean>(i);
                tmp.Add(dto);
            }

            wrap.Data = tmp;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollectionCall> listCollectionCallV2(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<CollectionCall>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<CollectionCall> q = this.ctx.Set<CollectionCall>();

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.AccNo!.Equals(filter.AccNo));
            }

            //q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.Id);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CustomerDetailResponseBean> listMasterCustomer(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<CustomerDetailResponseBean>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<Customer> q = this.ctx.Set<Customer>().Include(i => i.Branch);

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.Cif!.Equals(filter.AccNo));
            }

            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.Id);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var tmp = new List<CustomerDetailResponseBean>();

            foreach (var i in data)
            {
                var dto = this.mapper.Map<CustomerDetailResponseBean>(i);
                tmp.Add(dto);
            }

            wrap.Data = tmp;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<Customer> listMasterCustomerV2(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<Customer>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<Customer> q = this.ctx.Set<Customer>().Include(i => i.Branch);

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.Cif!.Equals(filter.AccNo));
            }

            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.Id);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            wrap.Data = data;

            wrap.Status = true;

            return wrap;
        }
    }
}
