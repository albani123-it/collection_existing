using AutoMapper;
using Collectium.Model.Helper;
using Collectium.Model;
using RestSharp;
using static Collectium.Model.Bean.Request.IntegrationRequestBean;
using System.Data;
using RestSharp.Serializers;
using System.Text.Json;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using RestSharp.Authenticators;
using Collectium.Model.Bean.Response;
using Collectium.Model.Bean;
using Collectium.Model.Bean.Request;
using Azure.Core;
using ContentType = RestSharp.Serializers.ContentType;
using Collectium.Model.Entity;
using Microsoft.Identity.Client;
using System.Globalization;
using DocumentFormat.OpenXml.Drawing.Charts;
using Collectium.Model.Bean.ListRequest;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Bibliography;

namespace Collectium.Service
{
    public class IntegrationService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<IntegrationService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration conf;

        public IntegrationService(CollectiumDBContext ctx,
                                ILogger<IntegrationService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                ToolService toolService,
                                IMapper mapper,
                                IHttpContextAccessor httpContextAccessor,
                                IConfiguration conf)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.pagination = pagination;
            this.statusService = statusService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.toolService = toolService;
            this.conf = conf;
        }

        public GenericResponse<PaymentResponseBean> CheckDailyPayment(string accountNo, string LoanNumber, string date, string daten)
        {
            this.logger.LogInformation("CheckDailyPayment >>> init: " + accountNo + " date: " + date);

            var ml = this.ctx.MasterLoan.Where(q => q.AccNo!.Equals(accountNo)).FirstOrDefault();

            var wrap = new GenericResponse<PaymentResponseBean>();
            wrap.Status = false;

            if (ml == null || ml.PayInAccount == null)
            {
                wrap.Message = "Akun tidak ditemukan";
                return wrap;
            }

            var now = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK");
            var tx = DateTime.Now.ToString("yyyyMMddHHmmss");
            var tx2 = DateTime.Now.ToString("yyyyMMdd");

            var client = new RestClient("http://localhost:30982");
            var request = new RestRequest("skycollection/Collection/getaccountpayment", Method.Post);
            
            
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJJRENBUElBY2Nlc3MiLCJqdGkiOiJkZWIyMGE4MS0xZTM5LTQwODctOWQ2My1kNjcxZTBlMzdlMmYiLCJpYXQiOjE2NTgzMDYxMDksIm5iZiI6MTY1ODMwNjEwOSwiZXhwIjoxNjU4MzA3OTA5LCJpc3MiOiJpRGVjaXNpb25Ub2tlbiIsImF1ZCI6ImlEZWNpc2lvbkF1ZGllbmNlIn0.s4xIXsU_24hus1ygF72oiQGBBJ4GEe9V22RPFq7e8_0");
            /*
            request.AddHeader("TimeStamp", now);
            request.AddHeader("baseurl", "https://10.0.232.139:38065");
            request.AddHeader("requestmethod", "POST");
            request.AddHeader("relativepath", "/api/v1/digitalloan/inquiry/accountstatement");
            request.AddHeader("Content-Type", "application/json");
            */

            var pb = new PaymentOffRequest();
            pb.Loan_number = LoanNumber;
            pb.Accountid = LoanNumber;
            pb.Payinaccount = ml.PayInAccount;
            pb.Tanggal = date;

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(pb, serializeOptions);
            request.AddStringBody(json, ContentType.Json);

            this.logger.LogInformation("CheckDailyPayment >>> payload: " + json);
            
            var resp = client.Execute(request);

            if (resp.IsSuccessful)
            {
                var tmp = resp.Content!.ToString();
                this.logger.LogInformation("response from core: " + tmp);

                try
                {
                    var obj = JsonSerializer.Deserialize<IntegRoot>(tmp);
                    this.logger.LogInformation("deserialize from core: " + JsonSerializer.Serialize(obj, serializeOptions));
                    if (obj != null && obj.status! != null && obj.status.Equals("true"))
                    {
                        var rt = new PaymentResponseBean();
                        var ac = obj.account;
                        if (ac != null && ac.Count > 0)
                        {
                            var lac = new List<StatementsBeanV2>();
                            foreach(var i in ac)
                            {
                                if (i.amount != null && i.date != null)
                                {
                                    var dt = DateTime.ParseExact(i.date, "yyyyMMdd", CultureInfo.InvariantCulture);

                                    var sv2 = new StatementsBeanV2();
                                    sv2.Date = dt.ToString("dd MMM yyyy");
                                    sv2.EndBalance = i.endBalance;
                                    sv2.Amount = i.amount;
                                    sv2.Status = i.sign;
                                    lac.Add(sv2);
                                }
                            }
                            rt.Account = lac;
                        }

                        var py = obj.payment;
                        if (py != null && py.Count > 0)
                        {
                            var lpy = new List<StatementsBeanV2>();
                            foreach (var i in py)
                            {

                                
                                if (i.PaymentDate != null && i.BillsAmount != null && i.Status != null && i.StatusDate != null)
                                {
                                    var sv2 = new StatementsBeanV2();
                                    sv2.Date = i.StatusDate;
                                    sv2.Date2 = i.PaymentDate;
                                    sv2.Amount = i.BillsAmount;
                                    sv2.Outstanding = i.OutsantdingAmount;
                                    sv2.Status = i.Status;

                                    var dt = DateTime.ParseExact(i.PaymentDate, "dd MMM yyyy", CultureInfo.InvariantCulture);
                                    sv2.Sorter = dt.Date;

                                    lpy.Add(sv2);

                                }
                               
                            }

                            var lpy2 = lpy.OrderByDescending(o => o.Sorter);
                            rt.Payment = lpy2.ToList();
                        }

                        if (obj.payment1 != null) { 

                            var pay1 = obj.payment1;
                            if (pay1.StatusCode.Equals("00"))
                            {
                                foreach (var itu in pay1.ServiceOrderItem.ServiceList)
                                {
                                    if (itu.BalanceType.Equals("Total Amount Due"))
                                    {
                                        var ff = rt.Payment[0];
                                        ff.AmountDue = itu.BalanceAmount;
                                    }
                                }
                            }
                        }


                        wrap.Status = true;
                        wrap.AddData(rt);
                    }
                    else
                    {
                        this.logger.LogError("Gagal ambil data dari CORE");
                        wrap.Message = "Gagal ambil data dari CORE";
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogError(e.Message);
                    wrap.Message = e.Message;
                }

            }
            else
            {
                this.logger.LogError("call error: " + resp.ErrorException);
                wrap.Message = resp.ErrorException!.Message;
            }
            
            /*
            var rtx = new PaymentResponseBean();
            var lacx = new List<StatementsBeanV2>();
            var acx = new StatementsBeanV2();
            acx.Date = "25 FEB 2021";
            acx.Amount = "10000";
            lacx.Add(acx);
            acx = new StatementsBeanV2();
            acx.Date = "25 FEB 2021";
            acx.Amount = "10000";
            lacx.Add(acx);
            acx = new StatementsBeanV2();
            acx.Date = "25 FEB 2021";
            acx.Amount = "10000";
            lacx.Add(acx);
            rtx.Account = lacx;

            var lpyx = new List<StatementsBeanV2>();
            var pyx = new StatementsBeanV2();
            pyx.Date = "25 FEB 2021";
            pyx.Amount = "20000";
            lpyx.Add(pyx);
            pyx = new StatementsBeanV2();
            pyx.Date = "25 FEB 2021";
            pyx.Amount = "20000";
            lpyx.Add(pyx);
            pyx = new StatementsBeanV2();
            pyx.Date = "25 FEB 2021";
            pyx.Amount = "20000";
            lpyx.Add(pyx);
            rtx.Payment = lpyx;
            

            wrap.Status = true;
            wrap.AddData(rtx);
            */

            return wrap;
        }


        public GenericResponse<PaymentResponseBean> CheckDailyPaymentDummy(string accountNo, string date, string daten)
        {
            this.logger.LogInformation("CheckDailyPayment >>> init: " + accountNo + " date: " + date);

            var ml = this.ctx.MasterLoan.Where(q => q.AccNo!.Equals(accountNo)).FirstOrDefault();

            var wrap = new GenericResponse<PaymentResponseBean>();
            wrap.Status = false;


            
            var rtx = new PaymentResponseBean();
            var lacx = new List<StatementsBeanV2>();
            var acx = new StatementsBeanV2();
            acx.Date = "25 JAN 2021";
            acx.Amount = "1000000";
            acx.Status = "C";
            lacx.Add(acx);
            acx = new StatementsBeanV2();
            acx.Date = "25 FEB 2021";
            acx.Amount = "1000000";
            acx.Status = "C";
            lacx.Add(acx);
            acx = new StatementsBeanV2();
            acx.Date = "25 MAR 2021";
            acx.Amount = "1000000";
            acx.Status = "C";
            lacx.Add(acx);
            rtx.Account = lacx;

            var lpyx = new List<StatementsBeanV2>();
            var pyx = new StatementsBeanV2();
            pyx.Date = "25 APR 2021";
            pyx.Amount = "2000000";
            acx.Status = "C";
            lpyx.Add(pyx);
            pyx = new StatementsBeanV2();
            pyx.Date = "25 MEI 2021";
            pyx.Amount = "2000000";
            acx.Status = "C";
            lpyx.Add(pyx);
            pyx = new StatementsBeanV2();
            pyx.Date = "25 JUN 2021";
            pyx.Amount = "20000000";
            acx.Status = "C";
            lpyx.Add(pyx);
            rtx.Payment = lpyx;
            

            wrap.Status = true;
            wrap.AddData(rtx);
            

            return wrap;
        }

        public GenericResponse<PaymentResponseBean> CheckDailyPaymentDummy3()
        {
            var wrap = new GenericResponse<PaymentResponseBean>();
            wrap.Status = false;

            this.logger.LogInformation("CheckDailyPaymentDummy3");

            var str = "{\"status\":\"true\",\"account\":[{\"referenceNumber\":\"1074708320-1\",\"sign\":\"D\",\"time\":\"2320\",\"amount\":\"-170741.49\",\"date\":\"20230902\",\"description\":\"Pembayaran Angsuran\",\"endBalance\":\"1691471.63\",\"pageNo\":\"0\",\"pageCount\":\"1\"},{\"referenceNumber\":\"1074675791\",\"sign\":\"D\",\"time\":\"0313\",\"amount\":\"-816471.63\",\"date\":\"20230911\",\"description\":\"<br/>Repayment Instructions  Settle via Instructions\",\"endBalance\":\"875000\",\"pageNo\":\"0\",\"pageCount\":\"1\"}],\"payment\":[{\"PaymentDate\":\"11 JUN 2023\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUN 2023\"},{\"PaymentDate\":\"11 MAY 2023\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAY 2023\"},{\"PaymentDate\":\"11 FEB 2023\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 FEB 2023\"},{\"PaymentDate\":\"11 JAN 2023\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JAN 2023\"},{\"PaymentDate\":\"11 DEC 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 DEC 2022\"},{\"PaymentDate\":\"11 NOV 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 NOV 2022\"},{\"PaymentDate\":\"11 OCT 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 OCT 2022\"},{\"PaymentDate\":\"11 SEP 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 SEP 2022\"},{\"PaymentDate\":\"11 AUG 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 AUG 2022\"},{\"PaymentDate\":\"11 JUL 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUL 2022\"},{\"PaymentDate\":\"11 JUN 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUN 2022\"},{\"PaymentDate\":\"11 MAY 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAY 2022\"},{\"PaymentDate\":\"11 APR 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 APR 2022\"},{\"PaymentDate\":\"11 MAR 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAR 2022\"},{\"PaymentDate\":\"11 FEB 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 FEB 2022\"},{\"PaymentDate\":\"11 JAN 2022\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JAN 2022\"},{\"PaymentDate\":\"11 DEC 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 DEC 2021\"},{\"PaymentDate\":\"11 NOV 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 NOV 2021\"},{\"PaymentDate\":\"11 OCT 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 OCT 2021\"},{\"PaymentDate\":\"11 SEP 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 SEP 2021\"},{\"PaymentDate\":\"11 AUG 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 AUG 2021\"},{\"PaymentDate\":\"11 JUL 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUL 2021\"},{\"PaymentDate\":\"11 JUN 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUN 2021\"},{\"PaymentDate\":\"11 MAY 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAY 2021\"},{\"PaymentDate\":\"11 APR 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 APR 2021\"},{\"PaymentDate\":\"11 MAR 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAR 2021\"},{\"PaymentDate\":\"11 FEB 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 FEB 2021\"},{\"PaymentDate\":\"11 JAN 2021\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JAN 2021\"},{\"PaymentDate\":\"11 DEC 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 DEC 2020\"},{\"PaymentDate\":\"11 NOV 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 NOV 2020\"},{\"PaymentDate\":\"11 OCT 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 OCT 2020\"},{\"PaymentDate\":\"11 SEP 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 SEP 2020\"},{\"PaymentDate\":\"11 AUG 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 AUG 2020\"},{\"PaymentDate\":\"11 JUL 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUL 2020\"},{\"PaymentDate\":\"11 JUN 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUN 2020\"},{\"PaymentDate\":\"11 MAY 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAY 2020\"},{\"PaymentDate\":\"11 APR 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 APR 2020\"},{\"PaymentDate\":\"11 MAR 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAR 2020\"},{\"PaymentDate\":\"11 FEB 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 FEB 2020\"},{\"PaymentDate\":\"11 JAN 2020\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JAN 2020\"},{\"PaymentDate\":\"11 DEC 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 DEC 2019\"},{\"PaymentDate\":\"11 NOV 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 NOV 2019\"},{\"PaymentDate\":\"11 OCT 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 OCT 2019\"},{\"PaymentDate\":\"11 SEP 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 SEP 2019\"},{\"PaymentDate\":\"11 AUG 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 AUG 2019\"},{\"PaymentDate\":\"11 JUL 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUL 2019\"},{\"PaymentDate\":\"11 JUN 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUN 2019\"},{\"PaymentDate\":\"11 MAY 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAY 2019\"},{\"PaymentDate\":\"11 APR 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 APR 2019\"},{\"PaymentDate\":\"11 MAR 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAR 2019\"},{\"PaymentDate\":\"11 FEB 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 FEB 2019\"},{\"PaymentDate\":\"11 JAN 2019\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JAN 2019\"},{\"PaymentDate\":\"11 DEC 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 DEC 2018\"},{\"PaymentDate\":\"11 NOV 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 NOV 2018\"},{\"PaymentDate\":\"11 OCT 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 OCT 2018\"},{\"PaymentDate\":\"11 SEP 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 SEP 2018\"},{\"PaymentDate\":\"11 AUG 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 AUG 2018\"},{\"PaymentDate\":\"11 JUL 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUL 2018\"},{\"PaymentDate\":\"11 JUN 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUN 2018\"},{\"PaymentDate\":\"11 MAY 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAY 2018\"},{\"PaymentDate\":\"11 APR 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 APR 2018\"},{\"PaymentDate\":\"11 MAR 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAR 2018\"},{\"PaymentDate\":\"11 FEB 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 FEB 2018\"},{\"PaymentDate\":\"11 JAN 2018\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JAN 2018\"},{\"PaymentDate\":\"11 DEC 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 DEC 2017\"},{\"PaymentDate\":\"11 NOV 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 NOV 2017\"},{\"PaymentDate\":\"11 OCT 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 OCT 2017\"},{\"PaymentDate\":\"11 SEP 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 SEP 2017\"},{\"PaymentDate\":\"11 AUG 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 AUG 2017\"},{\"PaymentDate\":\"11 JUL 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUL 2017\"},{\"PaymentDate\":\"11 JUN 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JUN 2017\"},{\"PaymentDate\":\"11 MAY 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAY 2017\"},{\"PaymentDate\":\"11 APR 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873826.96\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 APR 2017\"},{\"PaymentDate\":\"11 MAR 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873826.96\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 MAR 2017\"},{\"PaymentDate\":\"11 FEB 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873826.96\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 FEB 2017\"},{\"PaymentDate\":\"11 JAN 2017\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873826.96\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 JAN 2017\"},{\"PaymentDate\":\"11 DEC 2016\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873826.96\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 DEC 2016\"},{\"PaymentDate\":\"11 NOV 2016\",\"Type\":\"DISBURSEMENT\",\"BillsAmount\":\"110500000\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 NOV 2016\"},{\"PaymentDate\":\"11 NOV 2016\",\"Type\":\"Activity Charge\",\"BillsAmount\":\"552500\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 NOV 2016\"},{\"PaymentDate\":\"11 NOV 2016\",\"Type\":\"Activity Charge\",\"BillsAmount\":\"1350000\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 NOV 2016\"},{\"PaymentDate\":\"11 NOV 2016\",\"Type\":\"Activity Charge\",\"BillsAmount\":\"250000\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"11 NOV 2016\"},{\"PaymentDate\":\"11 SEP 2023\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"57356.26\",\"Status\":\"Due\",\"StatusDate\":\"11 SEP 2023\"},{\"PaymentDate\":\"11 AUG 2023\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"02 SEP 2023\"},{\"PaymentDate\":\"11 JUL 2023\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"05 AUG 2023\"},{\"PaymentDate\":\"11 APR 2023\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"06 MAY 2023\"},{\"PaymentDate\":\"11 MAR 2023\",\"Type\":\"Scheduled Payment\",\"BillsAmount\":\"873827.89\",\"OutsantdingAmount\":\"0\",\"Status\":\"Settled\",\"StatusDate\":\"14 MAR 2023\"}],\"payment1\":{\"StatusCode\":\"00\",\"StatusDescription\":\"Success\",\"ErrorCode\":\"1\",\"ErrorDescription\":\"\",\"TransactionObject\":{\"ClientID\":\"CRMS01\",\"BranchID\":\"\",\"RequestTimestamp\":\"2023-09-11T11:45:36\",\"ResponseTimestamp\":\"2023-09-11T11:45:45\",\"TransactionID\":\"20230911114700356\",\"T24TransactionID\":\"\"},\"ServiceOrderItem\":{\"OverdueStatus\":\"\",\"AccuralStatus\":\"\",\"ServiceList\":[{\"BalanceType\":\"Commitment (Total)\",\"BalanceAmount\":\"110,500,000\"},{\"BalanceType\":\"Total Principal\",\"BalanceAmount\":\"70,246,108.87\"},{\"BalanceType\":\"Total Amount Due\",\"BalanceAmount\":\"57,356.26\"},{\"BalanceType\":\"Principal (Current)\",\"BalanceAmount\":\"70,188,752.61\"}]}}}";


            var obj = JsonSerializer.Deserialize<IntegRoot>(str);

            if (obj != null && obj.status! != null && obj.status.Equals("true"))
            {
                var rt = new PaymentResponseBean();
                var ac = obj.account;
                if (ac != null && ac.Count > 0)
                {
                    var lac = new List<StatementsBeanV2>();
                    foreach (var i in ac)
                    {
                        if (i.amount != null && i.date != null)
                        {
                            var dt = DateTime.ParseExact(i.date, "yyyyMMdd", CultureInfo.InvariantCulture);

                            var sv2 = new StatementsBeanV2();
                            sv2.Date = dt.ToString("dd MMM yyyy");
                            sv2.EndBalance = i.endBalance;
                            sv2.Amount = i.amount;
                            sv2.Status = i.sign;
                            lac.Add(sv2);
                        }
                    }
                    rt.Account = lac;
                }

                var py = obj.payment;
                if (py != null && py.Count > 0)
                {
                    var lpy = new List<StatementsBeanV2>();
                    foreach (var i in py)
                    {


                        if (i.PaymentDate != null && i.BillsAmount != null && i.Status != null && i.StatusDate != null)
                        {
                            var sv2 = new StatementsBeanV2();
                            sv2.Date = i.StatusDate;
                            sv2.Date2 = i.PaymentDate;
                            sv2.Amount = i.BillsAmount;
                            sv2.Outstanding = i.OutsantdingAmount;
                            sv2.Status = i.Status;

                            var dt = DateTime.ParseExact(i.PaymentDate, "dd MMM yyyy", CultureInfo.InvariantCulture);
                            sv2.Sorter = dt.Date;

                            lpy.Add(sv2);

                        }

                    }

                    var lpy2 = lpy.OrderByDescending(o => o.Sorter);
                    rt.Payment = lpy2.ToList();
                }

                if (obj.payment1 != null)
                {

                    var pay1 = obj.payment1;
                    if (pay1.StatusCode.Equals("00"))
                    {
                        foreach (var itu in pay1.ServiceOrderItem.ServiceList)
                        {
                            if (itu.BalanceType.Equals("Total Amount Due"))
                            {
                                var ff = rt.Payment[0];
                                ff.AmountDue = itu.BalanceAmount;
                            }
                        }
                    }
                }


                wrap.Status = true;
                wrap.AddData(rt);
            }
            else
            {
                this.logger.LogError("Gagal ambil data dari CORE");
                wrap.Message = "Gagal ambil data dari CORE";
            }

            wrap.Status = true;

            return wrap;
        }

        public bool SendSms(string msisdn, string message)
        {
            var ipSms = this.ctx.RfGlobal.Where(q => q.Code!.Equals("SMSIP")).FirstOrDefault();
            if (ipSms != null)
            {

                if (msisdn.StartsWith("0"))
                {
                    msisdn = "62" + msisdn.Substring(1);
                }
                var client = new RestClient(ipSms.Val!);

                string vars = "PromediaSMS?msisdn=" + msisdn + "&message=" + message + "&sender=COLL&division=default &batchname=test&uploadby=BAGI&channel=0";
                this.logger.LogInformation("url: " + vars);

                var request = new RestRequest(vars, Method.Get);
                var res = client.Execute(request);
                var tmp = res.Content!.ToString();
                this.logger.LogInformation("response from core: " + tmp);

                return true;
            }

            return false;

        }

        public bool CallCustomer(int id)
        {
            this.logger.LogInformation("CallCustomer >>> id " + id);

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                return false;
            }

            if (reqUser.TelCode == null || reqUser.TelDevice == null)
            {
                return false;
            }

            var addr = this.ctx.CollectionAddContact.Find(id);
            if (addr == null)
            {
                return false;
            }

            var hp = addr.AddPhone;

            var mls = this.ctx.MasterLoan.Where(q => q.Cif!.Equals(addr.CuCif)).ToList();

            var ml = mls[0];

            var cc = this.ctx.CollectionCall.Where(q => q.LoanId == ml.Id).ToList();

            var crq = new CallRequest();
            crq.CollectionCallId = cc[0].Id;
            crq.CreateDate = DateTime.Now;
            crq.UserId = reqUser.Id;
            crq.PhoneNo = hp;
            crq.StatusId = 8;
            this.ctx.CallRequest.Add(crq);
            this.ctx.SaveChanges();

            var ticket = this.LoginTelp();
            if (ticket == null)
            {
                crq.StatusId = 9;
                this.ctx.CallRequest.Add(crq);
                this.ctx.SaveChanges();

                return false;
            }

            Random rnd = new Random();
            var ran = rnd.Next();

            var log = this.LoginAgent(ticket, reqUser.TelDevice!, reqUser.TelCode!, crq!.Id!.ToString());
            if (log == false)
            {
                crq.StatusId = 9;
                this.ctx.CallRequest.Add(crq);
                this.ctx.SaveChanges();

                return false;
            }
            this.logger.LogInformation("do call:" + hp);
            this.DoCall(ticket, hp, reqUser.TelDevice!, crq!.Id!.ToString());
            this.LogoutAgent(ticket, reqUser.TelDevice!, reqUser.TelCode!, "1000");
            return true;

        }

        public bool CallCustomerMobile(int id)
        {
            var addr = this.ctx.CollectionAddContact.Find(id);
            if (addr == null)
            {
                return false;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                return false;
            }

            var hp = addr.AddPhone;

            var cc = this.ctx.CollectionCall.Where(q => q.AccNo.Equals(addr.AccNo)).ToList();

            var crq = new CallRequest();
            crq.CollectionCallId = cc[0].Id;
            crq.CreateDate = DateTime.Now;
            crq.UserId = reqUser.Id;
            crq.PhoneNo = hp;
            crq.StatusId = 8;
            this.ctx.CallRequest.Add(crq);
            this.ctx.SaveChanges();

            var ticket = this.LoginTelp();
            if (ticket == null)
            {
                crq.StatusId = 9;
                this.ctx.CallRequest.Add(crq);
                return false;
            }

            Random rnd = new Random();
            var ran = rnd.Next();


            var log = this.LoginAgent(ticket, reqUser.TelDevice!, reqUser.TelCode!, "1000");
            if (log == false)
            {
                crq.StatusId = 9;
                this.ctx.CallRequest.Add(crq);
                return false;
            }
            this.logger.LogInformation("do call:" + hp);
            this.DoCall(ticket, hp, reqUser.TelDevice!, crq!.Id!.ToString());
            this.LogoutAgent(ticket, reqUser.TelDevice!, reqUser.TelCode!, "1000");
            return true;

        }

        private string LoginTelp()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };

            var brikIp = this.ctx.RfGlobal.Where(q => q.Code!.Equals("BRKIP")).FirstOrDefault();
            if (brikIp != null)
            {
                var url = "https://" + brikIp.Val! + ":10021";

                var options = new RestClientOptions(url);
                options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var client2 = new RestClient(options);

                var brikUsr = this.ctx.RfGlobal.Where(q => q.Code!.Equals("BRKUSR")).FirstOrDefault();
                var brikSec = this.ctx.RfGlobal.Where(q => q.Code!.Equals("BRKSEC")).FirstOrDefault();

                var request2 = new RestRequest("api", Method.Post);
                request2.AddHeader("adm", brikUsr!.Val!);
                request2.AddHeader("pass", brikSec!.Val!);
                request2.AddHeader("Content-Type", "application/json");

                var adm = new AdmLoginRequest();
                adm.Cmd = "admlogin";

                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(adm, serializeOptions);
                this.logger.LogInformation("payload: " + json);
                request2.AddStringBody(json, ContentType.Json);

                var resp2 = client2.Execute(request2);

                this.logger.LogInformation(json);

                if (resp2.IsSuccessful)
                {
                    var tmp = resp2.Content!.ToString();
                    this.logger.LogInformation("response from core: " + tmp);

                    try
                    {
                        var obj = JsonSerializer.Deserialize<AdmLoginResponse>(tmp, serializeOptions);
                        this.logger.LogInformation("deserialize from core: " + JsonSerializer.Serialize(obj, serializeOptions));
                        this.logger.LogInformation(obj.Status);
                        if (obj != null && obj.Status! != null && obj.Status.Equals("OK"))
                        {
                            return obj.Data![0].Ticket!;
                        }

                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e.Message);
                    }
                }
            }

            return null;
        }


        private bool LoginAgent(string ticket, string agent, string code, string random)
        {
            var brip = this.ctx.RfGlobal.Where(q => q.Code!.Equals("BRKIP")).FirstOrDefault();
            if (brip != null)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };

                var options = new RestClientOptions("https://" + brip.Val +":10021");
                options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var client2 = new RestClient(options);

                var request2 = new RestRequest("api", Method.Post);
                request2.AddHeader("ticket", ticket);
                request2.AddHeader("Content-Type", "application/json");

                var adm = new AgentLoginRequest();
                adm.Cmd = "agentcc";
                adm.Agent = agent;
                adm.Event = "login";
                //adm.Queue = random;
                adm.Queue = "1000";
                adm.Device = code;

                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(adm, serializeOptions);
                request2.AddStringBody(json, ContentType.Json);

                var resp2 = client2.Execute(request2);

                this.logger.LogInformation(json);

                if (resp2.IsSuccessful)
                {
                    var tmp = resp2.Content!.ToString();
                    this.logger.LogInformation("response from core: " + tmp);

                    try
                    {
                        var obj = JsonSerializer.Deserialize<AgentLoginResponse>(tmp, serializeOptions);
                        this.logger.LogInformation("deserialize from core: " + JsonSerializer.Serialize(obj, serializeOptions));
                        this.logger.LogInformation(obj.Status);
                        if (obj != null && obj.Status! != null)
                        {
                            if (obj.Status.Equals("OK"))
                            {
                                return true;
                            }
                            else
                            {
                                var ch = obj.Data![0];
                                if (ch.Status!.Equals("login"))
                                {
                                    return true;
                                }
                            }

                        }

                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e.Message);
                    }
                }
            }

            return false;
        }

        private bool LogoutAgent(string ticket, string agent, string code, string random)
        {

            var brip = this.ctx.RfGlobal.Where(q => q.Code!.Equals("BRKIP")).FirstOrDefault();
            if (brip != null)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };

                var options = new RestClientOptions("https://" + brip.Val + ":10021");
                options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var client2 = new RestClient(options);

                var request2 = new RestRequest("api", Method.Post);
                request2.AddHeader("ticket", ticket);
                request2.AddHeader("Content-Type", "application/json");

                var adm = new AgentLoginRequest();
                adm.Cmd = "agentcc";
                adm.Agent = agent;
                adm.Event = "logoff";
                adm.Queue = "1000";
                adm.Device = code;

                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(adm, serializeOptions);
                request2.AddStringBody(json, ContentType.Json);

                var resp2 = client2.Execute(request2);

                this.logger.LogInformation(json);

                if (resp2.IsSuccessful)
                {
                    var tmp = resp2.Content!.ToString();
                    this.logger.LogInformation("response from core: " + tmp);

                    try
                    {
                        var obj = JsonSerializer.Deserialize<AgentLoginResponse>(tmp, serializeOptions);
                        this.logger.LogInformation("deserialize from core: " + JsonSerializer.Serialize(obj, serializeOptions));
                        this.logger.LogInformation(obj.Status);
                        if (obj != null && obj.Status! != null)
                        {
                            if (obj.Status.Equals("OK"))
                            {
                                return true;
                            }
                            else
                            {
                                var ch = obj.Data![0];
                                if (ch.Status!.Equals("login"))
                                {
                                    return true;
                                }
                            }

                        }

                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e.Message);
                    }
                }
            }
            
            return false;
        }

        private bool DoCall(string ticket, string no, string agent, string vid)
        {

            var brip = this.ctx.RfGlobal.Where(q => q.Code!.Equals("BRKIP")).FirstOrDefault();
            if (brip != null)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };


                var options = new RestClientOptions("https://" + brip.Val + ":10021");
                options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var client2 = new RestClient(options);

                var request2 = new RestRequest("api", Method.Post);
                request2.AddHeader("ticket", ticket);
                request2.AddHeader("Content-Type", "application/json");

                var adm = new AgentCallRequest();
                adm.Cmd = "ctc";
                adm.Dialfrom = agent;
                adm.Dialto = no;
                adm.Cid = agent;
                adm.Timeout = "30";
                adm.V = no + "#" + vid;

                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(adm, serializeOptions);
                request2.AddStringBody(json, ContentType.Json);

                var resp2 = client2.Execute(request2);

                this.logger.LogInformation(json);

                if (resp2.IsSuccessful)
                {
                    var tmp = resp2.Content!.ToString();
                    this.logger.LogInformation("response from core: " + tmp);

                    try
                    {
                        var obj = JsonSerializer.Deserialize<AgentCallResponse>(tmp, serializeOptions);
                        this.logger.LogInformation("deserialize from core: " + JsonSerializer.Serialize(obj, serializeOptions));
                        this.logger.LogInformation(obj.Status);
                        if (obj != null && obj.Status! != null && obj.Status.Equals("OK"))
                        {
                            return true;
                        }

                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e.Message);
                    }
                }
            }

            return false;
        }

        public ReportPerfKacab ReportPerfKacab(ReportPerfKacabReq bean)
        {
            var err = new ReportPerfKacab();
            err.Status = "false";

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                return err;
            }
            if (reqUser.Role!.Name!.Equals("CABANG") == false)
            {
                return err;
            }

            var ts = bean.Formdate;
            if (ts != null)
            {
                ts = ts.Replace("-", "");
            } 
            else
            {
                ts = DateTime.Today.ToString("yyyyMMdd");
            }

            var te = bean.Todate;
            if (te != null)
            {
                te = te.Replace("-", "");
            }
            else
            {
                te = DateTime.Today.ToString("yyyyMMdd");
            }

            bean.Formdate = ts;
            bean.Todate= te;

            var bid = Convert.ToInt32(reqUser.ActiveBranchId);
            var br = this.ctx.Branch.Where(q => q.Id.Equals(bid)).FirstOrDefault();
            if (br == null)
            {
                return err;
            }

            bean.Branch = br.Code;

            var client = new RestClient("http://localhost:30982");
            var request = new RestRequest("skycollection/Collection/reportkacab", Method.Post);
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJTS1lXT1JYQVBJQWNjZXNzIiwianRpIjoiNDM1ZjEyOGUtNTZmOS00ODJiLWFmZGMtNTk5NGQ4Zjk2ODIyIiwiaWF0IjoxNjc0NTM1ODkxLCJuYmYiOjE2NzQ1MzU4OTEsImV4cCI6MTY3NDUzNzY5MSwiaXNzIjoiU0tZV09SWFRva2VuIiwiYXVkIjoiU0tZV09SWEF1ZGllbmNlIn0.OcNsEm3QGWZrTEdNsasaBf3aYZ_Ctv5A8u2YOb8nxpU");

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(bean, serializeOptions);
            request.AddStringBody(json, ContentType.Json);

            this.logger.LogInformation("ReportPerfKacab >>> payload: " + json);

            var resp = client.Execute(request);
            if (resp.IsSuccessful)
            {
                var tmp = resp.Content!.ToString();
                this.logger.LogInformation("ReportPerfKacab >>> response from core: " + tmp);

                try
                {
                    var obj = JsonSerializer.Deserialize<ReportPerfKacab>(tmp);
                    this.logger.LogInformation("ReportPerfKacab >>> deserialize from core: " + JsonSerializer.Serialize(obj, serializeOptions));
                    if (obj != null && obj.Status! != null && obj.Status.Equals("true"))
                    {
                        return obj;
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogError(e.Message);
                }

            }

            return err;

        }

        public ReportPerfSpv ReportPerfSpv(ReportPerfSpvReq bean)
        {
            var err = new ReportPerfSpv();
            err.Status = "false";

            var ts = bean.Formdate;
            if (ts != null)
            {
                ts = ts.Replace("-", "");
            }
            else
            {
                ts = DateTime.Today.ToString("yyyyMMdd");
            }

            var te = bean.Todate;
            if (te != null)
            {
                te = te.Replace("-", "");
            }
            else
            {
                te = DateTime.Today.ToString("yyyyMMdd");
            }

            bean.Formdate = ts;
            bean.Todate = te;

            var client = new RestClient("http://localhost:30982");
            var request = new RestRequest("skycollection/reportspv/detailspv", Method.Post);
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJTS1lXT1JYQVBJQWNjZXNzIiwianRpIjoiNDM1ZjEyOGUtNTZmOS00ODJiLWFmZGMtNTk5NGQ4Zjk2ODIyIiwiaWF0IjoxNjc0NTM1ODkxLCJuYmYiOjE2NzQ1MzU4OTEsImV4cCI6MTY3NDUzNzY5MSwiaXNzIjoiU0tZV09SWFRva2VuIiwiYXVkIjoiU0tZV09SWEF1ZGllbmNlIn0.OcNsEm3QGWZrTEdNsasaBf3aYZ_Ctv5A8u2YOb8nxpU");

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(bean, serializeOptions);
            request.AddStringBody(json, ContentType.Json);

            this.logger.LogInformation("ReportPerfSpv >>> payload: " + json);

            var resp = client.Execute(request);
            if (resp.IsSuccessful)
            {
                var tmp = resp.Content!.ToString();
                this.logger.LogInformation("ReportPerfSpv >>> response from core: " + tmp);

                try
                {
                    var obj = JsonSerializer.Deserialize<ReportPerfSpv>(tmp);
                    this.logger.LogInformation("ReportPerfSpv >>> deserialize from core: " + JsonSerializer.Serialize(obj, serializeOptions));
                    if (obj != null && obj.Status! != null && obj.Status.Equals("true"))
                    {
                        return obj;
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogError(e.Message);
                }

            }

            return err;

        }

        public ReportPerfSpvSummary ReportPerfSpvSummary(ReportPerfSpvReq bean)
        {
            var err = new ReportPerfSpvSummary();
            err.Status = "false";

            var ts = bean.Formdate;
            if (ts != null)
            {
                ts = ts.Replace("-", "");
            }
            else
            {
                ts = DateTime.Today.ToString("yyyyMMdd");
            }

            var te = bean.Todate;
            if (te != null)
            {
                te = te.Replace("-", "");
            }
            else
            {
                te = DateTime.Today.ToString("yyyyMMdd");
            }

            bean.Formdate = ts;
            bean.Todate = te;

            var client = new RestClient("http://localhost:30982");
            var request = new RestRequest("skycollection/reportspv/summaryspv", Method.Post);
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJTS1lXT1JYQVBJQWNjZXNzIiwianRpIjoiNDM1ZjEyOGUtNTZmOS00ODJiLWFmZGMtNTk5NGQ4Zjk2ODIyIiwiaWF0IjoxNjc0NTM1ODkxLCJuYmYiOjE2NzQ1MzU4OTEsImV4cCI6MTY3NDUzNzY5MSwiaXNzIjoiU0tZV09SWFRva2VuIiwiYXVkIjoiU0tZV09SWEF1ZGllbmNlIn0.OcNsEm3QGWZrTEdNsasaBf3aYZ_Ctv5A8u2YOb8nxpU");

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(bean, serializeOptions);
            request.AddStringBody(json, ContentType.Json);

            this.logger.LogInformation("ReportPerfSpvSummary >>> payload: " + json);

            var resp = client.Execute(request);
            if (resp.IsSuccessful)
            {
                var tmp = resp.Content!.ToString();
                this.logger.LogInformation("ReportPerfSpvSummary >>> response from core: " + tmp);

                try
                {
                    var obj = JsonSerializer.Deserialize<ReportPerfSpvSummary>(tmp);
                    this.logger.LogInformation("ReportPerfSpvSummary >>> deserialize from core: " + JsonSerializer.Serialize(obj, serializeOptions));
                    if (obj != null && obj.Status! != null && obj.Status.Equals("true"))
                    {
                        return obj;
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogError(e.Message);
                }

            }

            return err;

        }

        public GenericResponse<string> CallBackBrikerBox(CallBackBean filter)
        {

            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                wrap.Message = "Data tidak ditemukan";
                return wrap;
            }

            if (filter.File == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }


            if (filter.CallId == null || filter.CallId.Length < 1)
            {
                wrap.Message = "Data Call Id tidak ditemukan";
                return wrap;
            }

            if (filter.Ctc == null || filter.Ctc.Length < 1)
            {
                wrap.Message = "Data Ctc tidak ditemukan";
                return wrap;
            }

            var ctc = filter.Ctc;
            var rctc = ctc.Split("#");
            if (rctc.Length < 2)
            {
                wrap.Message = "Ctc tidak dikenali";
                return wrap;
            }

            var idcreq = int.Parse(rctc[1]);
            var creq = this.ctx.CallRequest.Find(idcreq);
            this.ctx.Entry(creq!).Reference(r => r.CollectionCall).Load();

            creq!.StatusId = 9;

            var path = conf["PhotoPath"];
            path = path + "/recording/" + creq.CollectionCallId.ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var nm = path + "/" + creq.Id.ToString() + ".wav";

            using (FileStream filestream = System.IO.File.Create(nm))
            {
                filter.File.CopyTo(filestream);
                filestream.Flush();
                //  return "\\Upload\\" + objFile.files.FileName;
            }

            var url = creq.CollectionCallId.ToString() + "/" + creq.Id.ToString() + ".wav";
            creq.Url = url;
            this.ctx.CallRequest.Update(creq);
            this.ctx.SaveChanges(true);

            wrap.Status = true;
            wrap.AddData("Ok");

            return wrap;
        }

        public GenericResponse<CallBackResponseBean> ListCallBackBrikerBox(CallBackRequest filter)
        {
            var wrap = new GenericResponse<CallBackResponseBean>
            {
                Status = false,
                Message = ""
            };

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            IQueryable<CallRequest> q = this.ctx.Set<CallRequest>().Include(i => i.CollectionCall).Include(i => i.Status)
                                .Where(q => q.CollectionCall!.LoanId.Equals(filter.Id));

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<CallBackResponseBean>();
            foreach (var it in data)
            {
                var dto = new CallBackResponseBean();
                dto.Id = it.Id;
                dto.AccNo = it.CollectionCall!.AccNo;
                dto.CallDate = it.CreateDate;
                dto.Result = it.Status!.Name;
                dto.PhoneNo = it.PhoneNo;

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public IActionResult HearRecording(UserReqApproveBean filter)
        {

            var wrap = new GenericResponse<UserReqApproveBean>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                return new BadRequestResult();
            }

            if (filter.Id == null)
            {
                return new BadRequestResult();
            }

            var prev = this.ctx.CallRequest.Find(filter.Id);
            if (prev == null)
            {
                return new BadRequestResult();
            }

            var file = conf["PhotoPath"] + "/recording/" + prev.Url;
            if (File.Exists(file) == false)
            {
                return new BadRequestResult();
            }

            var bytes = File.ReadAllBytes(file);
            MemoryStream ms = new MemoryStream(bytes);
            return new FileStreamResult(ms, "audio/wav");
        }

        public GenericResponse<SupersetConfigResponse> SuperSetConfig()
        {
            var wrap = new GenericResponse<SupersetConfigResponse>
            {
                Status = false,

            };

            var cfg = new SupersetConfigResponse();
            var ssip = this.ctx.RfGlobal.Where(q => q.Code!.Equals("SSIP")).FirstOrDefault();
            cfg.Url = ssip!.Val;
            var ssdcfc = this.ctx.RfGlobal.Where(q => q.Code!.Equals("SSDCFC")).FirstOrDefault();
            cfg.IdMainDCFC = ssdcfc!.Val;
            var ssspv = this.ctx.RfGlobal.Where(q => q.Code!.Equals("SSSPV")).FirstOrDefault();
            cfg.IdMainSPV = ssdcfc!.Val;
            var ssmgt = this.ctx.RfGlobal.Where(q => q.Code!.Equals("SSMGT")).FirstOrDefault();
            cfg.IdMainMgt = ssmgt!.Val;

            wrap.AddData(cfg);
            wrap.Status = true;

            return wrap;
        }

        public string SuperSetToken(SupersetPayloadRequest filter)
        {
            var ssip = this.ctx.RfGlobal.Where(q => q.Code!.Equals("SSIP")).FirstOrDefault();
            var ssus = this.ctx.RfGlobal.Where(q => q.Code!.Equals("SSUS")).FirstOrDefault();
            var sspw = this.ctx.RfGlobal.Where(q => q.Code!.Equals("SSPW")).FirstOrDefault();

            var client = new RestClient(ssip!.Val!);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var request = new RestRequest("api/v1/security/login", Method.Post);

            var bean = new SupersetLoginRequest();
            bean.Username = ssus!.Val!;
            bean.Password = sspw!.Val!;
            bean.Refresh = true;
            bean.Provider = "db";

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(bean, serializeOptions);
            request.AddStringBody(json, ContentType.Json);

            this.logger.LogInformation("SuperSetToken >>> payload: " + json);

            var resp = client.Execute(request);
            if (resp.IsSuccessful)
            {
                var tmp = resp.Content!.ToString();
                this.logger.LogInformation("SuperSetToken >>> response from core: " + tmp);

                try
                {
                    var obj = JsonSerializer.Deserialize<SupersetLoginResponse>(tmp);
                    this.logger.LogInformation("SuperSetToken >>> deserialize from core: " + JsonSerializer.Serialize(obj, serializeOptions));
                    var token = obj!.AccessToken;

                    var request2 = new RestRequest("api/v1/security/guest_token/", Method.Post);
                    request2.AddHeader("Authorization", "Bearer " + token);

                    var bean2 = new GuestTokenRequest();
                    var lres = new List<ResourceSupersetRequest>();
                    var ress = new ResourceSupersetRequest();
                    ress.id = filter.id;
                    ress.type = filter.type;
                    lres.Add(ress);
                    bean2.resources = lres;

                    var user = new UserSupersetRequest();
                    user.first_name = "stan";
                    user.last_name = "lee";
                    user.username = "stle";
                    bean2.user = user;

                    var rls = new List<RlSupersetRequest>();
                    var rl = new RlSupersetRequest();
                    rl.clause = "";
                    //rls.Add(rl);
                    bean2.rls = rls;

                    string json2 = JsonSerializer.Serialize(bean2, serializeOptions);
                    request2.AddStringBody(json2, ContentType.Json);

                    this.logger.LogInformation("SuperSetToken >>> guest payload: " + json2);

                    var resp2 = client.Execute(request2);

                    if (resp2.IsSuccessful)
                    {
                        var tmp2 = resp2.Content!.ToString();
                        this.logger.LogInformation("SuperSetToken >>> guest response from core: " + tmp2);
                        var obj2 = JsonSerializer.Deserialize<SupersetGuestResponse>(tmp2);

                        return obj2!.Token;
                    }


                }
                catch (Exception e)
                {
                    this.logger.LogError(e.Message);
                }

            }

            return "";
        }
    }
}
