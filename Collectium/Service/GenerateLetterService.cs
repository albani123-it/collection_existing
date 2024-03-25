using AutoMapper;
using Collectium.Model;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Model.Helper;
using Collectium.Validation;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp.Authenticators;
using RestSharp;
using System.Globalization;
using Action = Collectium.Model.Entity.Action;
using System.IO.Compression;
using System.Runtime.Intrinsics.Arm;

namespace Collectium.Service
{
    public class GenerateLetterService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<ScriptingService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GenerateLetterService(CollectiumDBContext ctx,
                                ILogger<ScriptingService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                ToolService toolService,
                                IMapper mapper,
                                IHttpContextAccessor httpContextAccessor)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.pagination = pagination;
            this.statusService = statusService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.toolService = toolService;
        }

        public GenericResponse<GenerateLetterResponseBean> ListGenerateLetter(GenerateLetterRequestBean filter)
        {
            var wrap = new GenericResponse<GenerateLetterResponseBean>
            {
                Status = false,
                Message = ""
            };

            var lggl = new List<GenerateLetterResponseBean>();

            var ml = this.ctx.MasterLoan.Find(filter.LoanId);
            if (ml == null)
            {
                wrap.Message = "Data Loan tidak ditemukan";
                return wrap;
            }

            var dpd = ml.Dpd;

            if (dpd > 14)
            {
                var ggl = this.GetGLByLoanAndCode(ml.Id!.Value, "ST");
                if (ggl != null)
                {
                    var gl = new GenerateLetterResponseBean();
                    gl.Id = ggl.Id;
                    gl.Code = "ST";
                    gl.Name = "Surat Teguran";
                    gl.NoSurat = ggl.No;
                    lggl.Add(gl);
                }

            }

            if (dpd >= 30)
            {
                var ggl = this.GetGLByLoanAndCode(ml.Id!.Value, "SP1");
                if (ggl != null)
                {
                    var gl = new GenerateLetterResponseBean();
                    gl.Id = ggl.Id;
                    gl.Code = "SP1";
                    gl.Name = "Surat Peringatan Pertama";
                    gl.NoSurat = ggl.No;
                    lggl.Add(gl);
                }

            }

            if (dpd >= 37)
            {
                var ggl = this.GetGLByLoanAndCode(ml.Id!.Value, "SP2");
                if (ggl != null)
                {
                    var gl = new GenerateLetterResponseBean();
                    gl.Id = ggl.Id;
                    gl.Code = "SP2";
                    gl.Name = "Surat Peringatan Kedua";
                    gl.NoSurat = ggl.No;
                    lggl.Add(gl);
                }

            }

            if (dpd >= 44)
            {
                var ggl = this.GetGLByLoanAndCode(ml.Id!.Value, "SP3");
                if (ggl != null)
                {
                    var gl = new GenerateLetterResponseBean();
                    gl.Id = ggl.Id;
                    gl.Code = "SP3";
                    gl.Name = "Surat Peringatan Ketiga";
                    gl.NoSurat = ggl.No;
                    lggl.Add(gl);
                }

            }

            if (dpd >= 90)
            {
                var ggl = this.GetGLByLoanAndCode(ml.Id!.Value, "BAPPD");
                if (ggl != null)
                {
                    var gl = new GenerateLetterResponseBean();
                    gl.Id = ggl.Id;
                    gl.Code = "BAPPD";
                    gl.Name = "Berita Acara Pelaporan Penanganan Debitur";
                    gl.NoSurat = ggl.No;
                    lggl.Add(gl);
                }

            }

            wrap.Data = lggl;
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<GenerateLetterResponseBean> ListGenerateLetterV2(GenerateLetterRequestBean filter)
        {
            var wrap = new GenericResponse<GenerateLetterResponseBean>
            {
                Status = false,
                Message = ""
            };

            var lggl = new List<GenerateLetterResponseBean>();

            var ml = this.ctx.MasterLoan.Find(filter.LoanId);
            if (ml == null)
            {
                wrap.Message = "Data Loan tidak ditemukan";
                return wrap;
            }

            var dpd = ml.Dpd;

            if (dpd > 14)
            {
                var gl = new GenerateLetterResponseBean();
                gl.Code = "ST";
                gl.Name = "Surat Teguran";

                var xs = this.ctx.GenerateLetter.Where(q => q.LoanId == filter.LoanId).Where(q => q.TypeLetter == "ST").Where(q => q.Status.Name.Equals("AKTIF")).ToList();
                if (xs != null && xs.Count > 0)
                {
                    var x = xs[0];
                    gl.NoSurat = x.No;
                }
                lggl.Add(gl);

            }

            if (dpd >= 30)
            {
                var gl = new GenerateLetterResponseBean();
                gl.Code = "SP1";
                gl.Name = "Surat Peringatan Pertama";

                var xs = this.ctx.GenerateLetter.Where(q => q.LoanId == filter.LoanId).Where(q => q.TypeLetter == "SP1").Where(q => q.Status.Name.Equals("AKTIF")).ToList();
                if (xs != null && xs.Count > 0)
                {
                    var x = xs[0];
                    gl.NoSurat = x.No;
                }
                lggl.Add(gl);

            }

            if (dpd >= 37)
            {
                var gl = new GenerateLetterResponseBean();
                gl.Code = "SP2";
                gl.Name = "Surat Peringatan Kedua";

                var xs = this.ctx.GenerateLetter.Where(q => q.LoanId == filter.LoanId).Where(q => q.TypeLetter == "SP2").Where(q => q.Status.Name.Equals("AKTIF")).ToList();
                if (xs != null && xs.Count > 0)
                {
                    var x = xs[0];
                    gl.NoSurat = x.No;
                }
                lggl.Add(gl);

            }

            if (dpd >= 44)
            {
                var gl = new GenerateLetterResponseBean();
                gl.Code = "SP3";
                gl.Name = "Surat Peringatan Ketiga";
                var xs = this.ctx.GenerateLetter.Where(q => q.LoanId == filter.LoanId).Where(q => q.TypeLetter == "SP3").Where(q => q.Status.Name.Equals("AKTIF")).ToList();
                if (xs != null && xs.Count > 0)
                {
                    var x = xs[0];
                    gl.NoSurat = x.No;
                }
                lggl.Add(gl);

            }

            wrap.Data = lggl;
            wrap.Status = true;

            return wrap;
        }

        public IActionResult GenerateLetter(GenerateLetterPdfRequestBean filter)
        {

            var gl = this.ctx.GenerateLetter.Find(filter.Id);
            if (gl == null)
            {
                return new BadRequestResult();
            }

            this.ctx.Entry(gl).Reference(r => r.Loan).Load();

            var acc = gl.Loan.AccNo;

            var senior = "";
            var cl = this.ctx.CollectionCall.Where(q => q.AccNo!.Equals(acc)).FirstOrDefault();
            if (cl != null)
            {
                this.ctx.Entry(cl).Reference(r => r.Branch).Load();
                senior = cl.Branch!.Pic;
            }

            var url = "http://lis.healtri.com:8080/";
            var jasper = this.ctx.RfGlobal.Where(q => q.Code!.Equals("JSPR")).FirstOrDefault();
            if (jasper != null)
            {
                if (jasper.Val != null)
                {
                    url = jasper.Val;
                }
            }

            var client = new RestClient(url + "jasperserver/rest_v2/");
            client.Authenticator = new HttpBasicAuthenticator("jasperadmin", "jasperadmin");

            string vars = "";

            var glNo = Uri.EscapeDataString(gl.No!);
            if (gl.TypeLetter!.Equals("ST"))
            {
                vars = "reports/collectium/surat_teguran.pdf?no_surat=" + glNo + "&tgl=" + gl.Tanggal + "&nama_nasabah=" + gl.NamaNasabah + "&jumlah=" + gl.Jumlah + "&terbilang=" + gl.Terbilang + 
                    " Rupiah&tgl_bayar=" + gl.TanggalBayar + "&cabang=" + gl.Cabang + "&no_sp1=" + gl.NoSP1 + "&no_sp2=" + gl.NoSP2 + "&tgl_sp1=" + gl.TglSp1 + 
                    "&tgl_sp2=" + gl.TglSp2 +"&senior_officer=" + senior + "&kota=" + gl.CabangKota + "&cabang_faks=" + gl.CabangFaks + "&cabang_telepon=" + gl.CabangTelp + "&cabang_alamat=" + gl.CabangAlamat;
            } 
            else if (gl.TypeLetter!.Equals("SP1"))
            {
                vars = "reports/collectium/surat_peringatan_1.pdf?no_surat=" + glNo + "&tgl=" + gl.Tanggal + "&nama_nasabah=" + gl.NamaNasabah + "&jumlah=" + gl.Jumlah + "&terbilang=" + gl.Terbilang +
                    " Rupiah&tgl_bayar=" + gl.TanggalBayar + "&cabang=" + gl.Cabang + "&no_sp1=" + gl.NoSP1 + "&no_sp2=" + gl.NoSP2 + "&tgl_sp1=" + gl.TglSp1 +
                    "&tgl_sp2=" + gl.TglSp2 + "&senior_officer=" + senior + "&kota=" + gl.CabangKota + "&cabang_faks=" + gl.CabangFaks + "&cabang_telepon=" + gl.CabangTelp + "&cabang_alamat=" + gl.CabangAlamat;
            } 
            else if (gl.TypeLetter!.Equals("SP2"))
            {


                vars = "reports/collectium/surat_peringatan_2.pdf?no_surat=" + glNo + "&tgl=" + gl.Tanggal + "&nama_nasabah=" + gl.NamaNasabah + "&jumlah=" + gl.Jumlah + "&terbilang=" + gl.Terbilang +
                    " Rupiah&tgl_bayar=" + gl.TanggalBayar + "&cabang=" + gl.Cabang + "&no_sp1=" + gl.NoSP1 + "&no_sp2=" + gl.NoSP2 + "&tgl_sp1=" + gl.TglSp1 +
                    "&tgl_sp2=" + gl.TglSp2 + "&senior_officer=" + senior + "&kota=" + gl.CabangKota + "&cabang_faks=" + gl.CabangFaks + "&cabang_telepon=" + gl.CabangTelp + "&cabang_alamat=" + gl.CabangAlamat;
            }
            else if (gl.TypeLetter!.Equals("SP3"))
            {
                vars = "reports/collectium/surat_peringatan_3.pdf?no_surat=" + glNo + "&tgl=" + gl.Tanggal + "&nama_nasabah=" + gl.NamaNasabah + "&jumlah=" + gl.Jumlah + "&terbilang=" + gl.Terbilang +
                    " Rupiah&tgl_bayar=" + gl.TanggalBayar + "&cabang=" + gl.Cabang + "&no_sp1=" + gl.NoSP1 + "&no_sp2=" + gl.NoSP2 + "&tgl_sp1=" + gl.TglSp1 +
                    "&tgl_sp2=" + gl.TglSp2 + "&senior_officer=" + senior + "&kota=" + gl.CabangKota + "&cabang_faks=" + gl.CabangFaks + "&cabang_telepon=" + gl.CabangTelp + "&cabang_alamat=" + gl.CabangAlamat;
            }
            else if (gl.TypeLetter!.Equals("BAPPD"))
            {
                vars = "reports/collectium/bappd.pdf?no_surat=" + glNo + "&tgl=" + gl.Tanggal + "&nama_nasabah=" + gl.NamaNasabah + "&jumlah=" + gl.Jumlah + "&terbilang=" + gl.Terbilang +
                    " Rupiah&tgl_bayar=" + gl.TanggalBayar + "&cabang=" + gl.Cabang + "&no_sp1=" + gl.NoSP1 + "&no_sp2=" + gl.NoSP2 + "&tgl_sp1=" + gl.TglSp1 +
                    "&tgl_sp2=" + gl.TglSp2;
            }

            this.logger.LogInformation("url: " + vars);

            var request = new RestRequest(vars, Method.Get);
            var res = client.DownloadData(request);
            MemoryStream ms = new MemoryStream(res);
            return new FileStreamResult(ms, "application/pdf");

        }



        public IActionResult GenerateLetterV2(GenerateLetterPdfRequestBean filter)
        {

            var ml = this.ctx.MasterLoan.Find(filter.Id);
            if (ml == null)
            {
                return new BadRequestResult();
            }

            var dpd = ml.Dpd;

            var ggl = new GenerateLetter();
            if (filter.Code == "ST")
            {
                ggl = this.GetGLByLoanAndCodeV2(ml.Id!.Value, "ST");
                if (ggl == null)
                {
                    ggl = this.GetGLByLoanAndCodeV3(ml.Id!.Value, "ST");
                }
            }

            if (filter.Code == "SP1")
            {
                ggl = this.GetGLByLoanAndCodeV2(ml.Id!.Value, "SP1");
                if (ggl == null)
                {
                    ggl = this.GetGLByLoanAndCodeV3(ml.Id!.Value, "SP1");
                }
            }

            if (filter.Code == "SP2")
            {
                ggl = this.GetGLByLoanAndCodeV2(ml.Id!.Value, "SP2");
                if (ggl == null)
                {
                    ggl = this.GetGLByLoanAndCodeV3(ml.Id!.Value, "SP2");
                }
            }

            if (filter.Code == "SP3")
            {
                ggl = this.GetGLByLoanAndCodeV2(ml.Id!.Value, "SP3");
                if (ggl == null)
                {
                    ggl = this.GetGLByLoanAndCodeV3(ml.Id!.Value, "SP3");
                }
            }

            if (ggl == null)
            {
                return new BadRequestResult();
            }


            var acc = ml.AccNo;

            var senior = "";
            var cl = this.ctx.CollectionCall.Where(q => q.AccNo!.Equals(acc)).FirstOrDefault();
            if (cl != null)
            {
                this.ctx.Entry(cl).Reference(r => r.Branch).Load();
                senior = cl.Branch!.Pic;
            }

            var url = "http://lis.healtri.com:8080/";
            var jasper = this.ctx.RfGlobal.Where(q => q.Code!.Equals("JSPR")).FirstOrDefault();
            if (jasper != null)
            {
                if (jasper.Val != null)
                {
                    url = jasper.Val;
                }
            }

            var client = new RestClient(url + "jasperserver/rest_v2/");
            client.Authenticator = new HttpBasicAuthenticator("jasperadmin", "jasperadmin");

            string vars = "";

            var glNo = Uri.EscapeDataString(ggl.No!);
            if (ggl.TypeLetter!.Equals("ST"))
            {
                vars = "reports/collectium/surat_teguran.pdf?no_surat=" + glNo + "&tgl=" + ggl.Tanggal + "&nama_nasabah=" + ggl.NamaNasabah + "&jumlah=" + ggl.Jumlah + "&terbilang=" + ggl.Terbilang +
                    " Rupiah&tgl_bayar=" + ggl.TanggalBayar + "&cabang=" + ggl.Cabang + "&no_sp1=" + ggl.NoSP1 + "&no_sp2=" + ggl.NoSP2 + "&tgl_sp1=" + ggl.TglSp1 +
                    "&tgl_sp2=" + ggl.TglSp2 + "&senior_officer=" + senior + "&kota=" + ggl.CabangKota + "&cabang_faks=" + ggl.CabangFaks + "&cabang_telepon=" + ggl.CabangTelp + "&cabang_alamat=" + ggl.CabangAlamat;
            }
            else if (ggl.TypeLetter!.Equals("SP1"))
            {
                vars = "reports/collectium/surat_peringatan_1.pdf?no_surat=" + glNo + "&tgl=" + ggl.Tanggal + "&nama_nasabah=" + ggl.NamaNasabah + "&jumlah=" + ggl.Jumlah + "&terbilang=" + ggl.Terbilang +
                    " Rupiah&tgl_bayar=" + ggl.TanggalBayar + "&cabang=" + ggl.Cabang + "&no_sp1=" + ggl.NoSP1 + "&no_sp2=" + ggl.NoSP2 + "&tgl_sp1=" + ggl.TglSp1 +
                    "&tgl_sp2=" + ggl.TglSp2 + "&senior_officer=" + senior + "&kota=" + ggl.CabangKota + "&cabang_faks=" + ggl.CabangFaks + "&cabang_telepon=" + ggl.CabangTelp + "&cabang_alamat=" + ggl.CabangAlamat;
            }
            else if (ggl.TypeLetter!.Equals("SP2"))
            {


                vars = "reports/collectium/surat_peringatan_2.pdf?no_surat=" + glNo + "&tgl=" + ggl.Tanggal + "&nama_nasabah=" + ggl.NamaNasabah + "&jumlah=" + ggl.Jumlah + "&terbilang=" + ggl.Terbilang +
                    " Rupiah&tgl_bayar=" + ggl.TanggalBayar + "&cabang=" + ggl.Cabang + "&no_sp1=" + ggl.NoSP1 + "&no_sp2=" + ggl.NoSP2 + "&tgl_sp1=" + ggl.TglSp1 +
                    "&tgl_sp2=" + ggl.TglSp2 + "&senior_officer=" + senior + "&kota=" + ggl.CabangKota + "&cabang_faks=" + ggl.CabangFaks + "&cabang_telepon=" + ggl.CabangTelp + "&cabang_alamat=" + ggl.CabangAlamat;
            }
            else if (ggl.TypeLetter!.Equals("SP3"))
            {
                vars = "reports/collectium/surat_peringatan_3.pdf?no_surat=" + glNo + "&tgl=" + ggl.Tanggal + "&nama_nasabah=" + ggl.NamaNasabah + "&jumlah=" + ggl.Jumlah + "&terbilang=" + ggl.Terbilang +
                    " Rupiah&tgl_bayar=" + ggl.TanggalBayar + "&cabang=" + ggl.Cabang + "&no_sp1=" + ggl.NoSP1 + "&no_sp2=" + ggl.NoSP2 + "&tgl_sp1=" + ggl.TglSp1 +
                    "&tgl_sp2=" + ggl.TglSp2 + "&senior_officer=" + senior + "&kota=" + ggl.CabangKota + "&cabang_faks=" + ggl.CabangFaks + "&cabang_telepon=" + ggl.CabangTelp + "&cabang_alamat=" + ggl.CabangAlamat;
            }
            else if (ggl.TypeLetter!.Equals("BAPPD"))
            {
                vars = "reports/collectium/bappd.pdf?no_surat=" + glNo + "&tgl=" + ggl.Tanggal + "&nama_nasabah=" + ggl.NamaNasabah + "&jumlah=" + ggl.Jumlah + "&terbilang=" + ggl.Terbilang +
                    " Rupiah&tgl_bayar=" + ggl.TanggalBayar + "&cabang=" + ggl.Cabang + "&no_sp1=" + ggl.NoSP1 + "&no_sp2=" + ggl.NoSP2 + "&tgl_sp1=" + ggl.TglSp1 +
                    "&tgl_sp2=" + ggl.TglSp2;
            }

            this.logger.LogInformation("url: " + vars);

            var request = new RestRequest(vars, Method.Get);
            var res = client.DownloadData(request);
            MemoryStream ms = new MemoryStream(res);
            return new FileStreamResult(ms, "application/pdf");

        }

        public IActionResult GenerateReportMonitor(ReportMonitorRequest filter)
        {

            var url = "http://lis.healtri.com:8080/";
            var jasper = this.ctx.RfGlobal.Where(q => q.Code!.Equals("JSPR")).FirstOrDefault();
            if (jasper != null)
            {
                if (jasper.Val != null)
                {
                    url = jasper.Val;
                }
            }

            var client = new RestClient(url + "/jasperserver/rest_v2/");
            client.Authenticator = new HttpBasicAuthenticator("jasperadmin", "jasperadmin");

            string vars = "";
            if (filter != null)
            {
                if (filter.RoleId == 3 || filter.RoleId ==4)
                {
                    vars = "reports/collectium/report_mon.xlsx?role=" + filter.RoleId + "&branch=" + filter.BranchId + "&start=" + filter.Start + "&end=" + filter.End;
                } 
                else if (filter.RoleId == 99997)
                {
                    var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
                    vars = "reports/collectium/report_mon_2.xlsx?role=" + reqUser!.Id + "&branch=" + filter.BranchId + "&start=" + filter.Start + "&end=" + filter.End;
                }
                else if (filter.RoleId == 99998)
                {
                    var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
                    vars = "reports/collectium/report_mon_1.xlsx?role=" + filter.RoleId + "&branch=" + reqUser!.ActiveBranchId + "&start=" + filter.Start + "&end=" + filter.End;
                } else
                {
                    var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
                    vars = "reports/collectium/report_mon_1_1.xlsx?role=" + filter.RoleId + "&branch=" + reqUser!.ActiveBranchId + "&start=" + filter.Start + "&end=" + filter.End;
                }
            }

            this.logger.LogInformation("url: " + vars);

            var request = new RestRequest(vars, Method.Get);
            var res = client.DownloadData(request);

            var str = System.Text.Encoding.Default.GetString(res);
            Console.WriteLine(str);

            MemoryStream ms = new MemoryStream(res);
            return new FileStreamResult(ms, "application/octet-stream");

        }

        private GenerateLetter GetGLByLoanAndCode(int loanId, string code)
        {
            var gl = this.ctx.Set<GenerateLetter>().Where(q => q.Loan!.Id.Equals(loanId))
                                                .Where(q => q.TypeLetter!.Equals(code))
                                                .Where(q => q.Status!.Name!.Equals("AKTIF"))
                                                .FirstOrDefault();

            if (gl == null)
            {
                var ml = this.ctx.MasterLoan.Find(loanId);
                if (ml == null)
                {
                    return null;
                }
                this.ctx.Entry(ml).Reference(r => r.Customer).Load();
                this.ctx.Entry(ml).Reference(r => r.Call).Load();

                if (ml.Call == null || ml.Call!.BranchId! == null)
                {
                    return null;
                }
                this.ctx.Entry(ml.Call!).Reference(r => r.Branch).Load();

                var ngl = new GenerateLetter();
                ngl.Loan = ml;
                ngl.TypeLetter = code;
                ngl.NamaNasabah = ml.Customer!.Name;

                var now = DateTime.Now;
                var year = now.Year;
                var month = now.Month;
                var yyear = now.ToString("yyyy", CultureInfo.InvariantCulture);
                var mmonth = this.IntToRoman(now.ToString("MM", CultureInfo.InvariantCulture));
                var num = this.GenerateLetterNo(code, ml.Call!.Branch!.Id!.Value, year, month);
                var no = "SK/" +code + "/" + num.ToString("D4") + "/" + ml.Call!.Branch!.CoreCode + "/" + mmonth + "/" + yyear;
                ngl.No = no;

                var today = DateTime.Today;
                ngl.Tanggal = today.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));

                var ter = new Terbilang();

                if (ml.KewajibanTotal != null)
                {
                    var mx = ml.TunggakanTotal!.Value;
                    if (mx < 0)
                    {
                        mx *= -1;
                    }
                    ngl.Terbilang = ter[Convert.ToInt64(Math.Floor(mx))];
                    ngl.Jumlah = String.Format(CultureInfo.CreateSpecificCulture("id-id"), "Rp. {0:N}", Math.Floor(mx));
                } 
                else
                {
                    ngl.Terbilang = "";
                }


                ngl.Status = this.statusService.GetStatusGeneral("AKTIF");

                ngl.Cabang = ml.Call.Branch.Name;
                ngl.CabangAlamat = ml.Call.Branch.Addr1;
                ngl.CabangTelp = ml.Call.Branch.Phone;
                ngl.CabangFaks = ml.Call.Branch.Fax;
                ngl.CabangKota = ml.Call.Branch.City;

                var dpd = ml.Dpd;

                this.logger.LogInformation("dpd: " + dpd);

                if (code == "ST")
                {
                    var tdpd = 14 - dpd;
                    var ttd = DateTime.Now;
                    ngl.Tanggal = ttd.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));

                    var ndpd = tdpd + 7;
                    var td = DateTime.Now;
                    ngl.TanggalBayar = td.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));
                } 
                else if (code == "SP3")
                {
                    var td = DateTime.Now.AddDays(7);
                    ngl.TanggalBayar = td.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));
                }
             

                if (dpd >= 37)
                {
                    var ggl = this.GetGLByLoanAndCodeV2(ml.Id!.Value, "SP1");
                    if (ggl != null)
                    {
                        ngl.NoSP1 = ggl.No;
                        ngl.TglSp1 = ggl.Tanggal;
                    }

                }

                if (dpd >= 44)
                {
                    var ggl = this.GetGLByLoanAndCodeV2(ml.Id!.Value, "SP2");
                    if (ggl != null)
                    {
                        ngl.NoSP2 = ggl.No;
                        ngl.TglSp2 = ggl.Tanggal;
                    }

                }

                ngl.Notaris = "-";
                ngl.NotarisDi = "-";
                ngl.NotarisTgl = "-";

                Console.WriteLine(ngl.No + " - " + ngl.NoSP1 + " - " + ngl.NoSP2);

                this.ctx.Add(ngl);
                this.ctx.SaveChanges();

                return ngl;
            } else
            {
                return gl;
            }
        }

        private GenerateLetter GetGLByLoanAndCodeV3(int loanId, string code)
        {
            var gl = this.ctx.Set<GenerateLetter>().Where(q => q.Loan!.Id.Equals(loanId))
                                                .Where(q => q.TypeLetter!.Equals(code))
                                                .Where(q => q.Status!.Name!.Equals("AKTIF"))
                                                .FirstOrDefault();

            if (gl == null)
            {
                var ml = this.ctx.MasterLoan.Find(loanId);
                if (ml == null)
                {
                    return null;
                }
                this.ctx.Entry(ml).Reference(r => r.Customer).Load();
                this.ctx.Entry(ml).Reference(r => r.Call).Load();

                if (ml.Call == null || ml.Call!.BranchId! == null)
                {
                    return null;
                }
                this.ctx.Entry(ml.Call!).Reference(r => r.Branch).Load();

                var ngl = new GenerateLetter();
                ngl.Loan = ml;
                ngl.TypeLetter = code;
                ngl.NamaNasabah = ml.Customer!.Name;

                var now = DateTime.Now;
                var year = now.Year;
                var month = now.Month;
                var yyear = now.ToString("yyyy", CultureInfo.InvariantCulture);
                var mmonth = this.IntToRoman(now.ToString("MM", CultureInfo.InvariantCulture));
                var num = this.GenerateLetterNo(code, ml.Call!.Branch!.Id!.Value, year, month);
                var no = "SK/" +code + "/" + num.ToString("D4") + "/" + ml.Call!.Branch!.CoreCode + "/" + mmonth + "/" + yyear;
                ngl.No = no;

                var today = DateTime.Today;
                ngl.Tanggal = today.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));

                var ter = new Terbilang();

                if (ml.KewajibanTotal != null)
                {
                    var mx = ml.TunggakanTotal!.Value;
                    if (mx < 0)
                    {
                        mx *= -1;
                    }
                    ngl.Terbilang = ter[Convert.ToInt64(Math.Floor(mx))];
                    ngl.Jumlah = String.Format(CultureInfo.CreateSpecificCulture("id-id"), "Rp. {0:N}", Math.Floor(mx));
                } 
                else
                {
                    ngl.Terbilang = "";
                }


                ngl.Status = this.statusService.GetStatusGeneral("AKTIF");

                ngl.Cabang = ml.Call.Branch.Name;
                ngl.CabangAlamat = ml.Call.Branch.Addr1;
                ngl.CabangTelp = ml.Call.Branch.Phone;
                ngl.CabangFaks = ml.Call.Branch.Fax;
                ngl.CabangKota = ml.Call.Branch.City;

                var dpd = ml.Dpd;

                this.logger.LogInformation("dpd: " + dpd);

                if (code == "ST")
                {
                    var tdpd = 14 - dpd;
                    var ttd = DateTime.Now;
                    ngl.Tanggal = ttd.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));

                    var td = DateTime.Now;
                    ngl.TanggalBayar = td.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));
                } 
                
                if (code == "ST")
                {
                    if (dpd < 14)
                    {
                        return null;
                    }
                }

                if (code == "SP1")
                {
                    if (dpd < 30)
                    {
                        return null;
                    }


                    var ggl = this.GetGLByLoanAndCodeV2(ml.Id!.Value, "ST");
                    if (ggl != null)
                    {
                        var pdt = DateTime.Parse(ggl.Tanggal!, new System.Globalization.CultureInfo("id-ID"));
                        pdt = pdt.AddDays(16);
                        if (pdt > now)
                        {
                            return null;
                        }
                    } 
                    else
                    {
                        return null;
                    }

                }

                if (code == "SP2")
                {
                    if (dpd < 37)
                    {
                        return null;
                    }

                    var ggl = this.GetGLByLoanAndCodeV2(ml.Id!.Value, "SP1");
                    if (ggl != null)
                    {
                        var pdt = DateTime.Parse(ggl.Tanggal!, new System.Globalization.CultureInfo("id-ID"));
                        pdt = pdt.AddDays(7);
                        if (pdt > now)
                        {
                            return null;
                        }

                        ngl.NoSP1 = ggl.No;
                        ngl.TglSp1 = ggl.Tanggal;
                    } 
                    else
                    {
                        return null;
                    }

                }

                if (code == "SP3")
                {
                    if (dpd < 44)
                    {
                        return null;
                    }

                    var ggl = this.GetGLByLoanAndCodeV2(ml.Id!.Value, "SP2");
                    if (ggl != null)
                    {
                        var pdt = DateTime.Parse(ggl.Tanggal!, new System.Globalization.CultureInfo("id-ID"));
                        pdt = pdt.AddDays(7);
                        if (pdt > now)
                        {
                            return null;
                        }

                        ngl.NoSP2 = ggl.No;
                        ngl.TglSp2 = ggl.Tanggal;
                        ngl.NoSP1 = ggl.NoSP1;
                        ngl.TglSp1 = ggl.TglSp1;

                        var td = DateTime.Now.AddDays(7);
                        ngl.TanggalBayar = td.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));
                    }
                    else
                    {
                        return null;
                    }

                }

                ngl.Notaris = "-";
                ngl.NotarisDi = "-";
                ngl.NotarisTgl = "-";

                Console.WriteLine(ngl.No + " - " + ngl.NoSP1 + " - " + ngl.NoSP2);

                this.ctx.Add(ngl);
                this.ctx.SaveChanges();

                return ngl;
            } else
            {
                return gl;
            }
        }

        private GenerateLetter GetGLByLoanAndCodeV2(int loanId, string code)
        {
            var gl = this.ctx.Set<GenerateLetter>().Where(q => q.Loan!.Id.Equals(loanId))
                                                .Where(q => q.TypeLetter!.Equals(code))
                                                .Where(q => q.Status!.Name!.Equals("AKTIF"))
                                                .FirstOrDefault();

            return gl;
        }

        private int GenerateLetterNo(string type, int branchId, int year, int month)
        {
            var ctr = this.ctx.Counter.Where(q => q.Type!.Equals(type))
                                      .Where(q => q.Year!.Equals(year))
                                      .FirstOrDefault();
            if (ctr == null)
            {
                ctr = new Counter();
                ctr.Type = type;
                //ctr.BranchId = branchId;
                ctr.Year = year;
                //ctr.Month = month;
                ctr.Ctr = 1;

                this.ctx.Add(ctr);
                this.ctx.SaveChanges();

                return 1;
            } else
            {
                var cn = ctr.Ctr;
                cn++;
                ctr.Ctr = cn;
                this.ctx.Update(ctr);
                this.ctx.SaveChanges();
                return cn!.Value;
            }
        }

        public IActionResult zipLog(string Date)
        {

            var td = DateTime.Now.ToString("yyyyMMddHHmmss");
            var zipPath = @"./logs/log_" + td + ".zip";
            var logFile = @"./logs/log" + Date + ".txt";

            if (!File.Exists(logFile))
            {
                Console.WriteLine("not found");
                return new BadRequestResult();
            }

            var memoryStream = new MemoryStream();
            var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
            zipArchive.CreateEntryFromFile(logFile, Path.GetFileName(logFile));
            zipArchive.Dispose();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/zip");

        }

        public GenericResponse<GenerateLetterHistoryResponseBean> HistoryGenerateLetter(GenerateLetterHistoryBean filter)
        {
            var wrap = new GenericResponse<GenerateLetterHistoryResponseBean>
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

            var lggl = new List<GenerateLetterResponseBean>();

            IQueryable<GenerateLetter> q = this.ctx.Set<GenerateLetter>().Include(i => i.Loan).ThenInclude(i => i.Customer);
            if (filter!.AccNo != null)
            {
                q = q.Where(q => q.Loan!.AccNo!.Equals(filter.AccNo!));
            }

            if (filter!.Name!= null)
            {
                q = q.Where(q => q.Loan!.Customer!.Name!.Contains(filter.Name));
            }

            if (reqUser.RoleId == 7)
            {
                var br = this.ctx.Branch.Find(reqUser.ActiveBranchId);
                if (br != null)
                {
                    q = q.Where(q => q.Cabang!.Equals(br.Name));
                } else
                {
                    q = q.Where(q => q.Cabang!.Equals("XYZ"));
                }

            }

            q = q.OrderByDescending(q => q.Id);
            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<GenerateLetterHistoryResponseBean>();
            foreach (var it in data)
            {
                var dto = new GenerateLetterHistoryResponseBean();
                dto.Id = it.Id;
                dto.AccNo = it.Loan!.AccNo;
                dto.Name = it.Loan!.Customer!.Name;
                dto.NoSurat = it.No;
                dto.TglSurat = it.Tanggal;
                dto.JenisSurat = it.TypeLetter;
                dto.Cabang = it.Cabang;

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;

        }

            private string Terbilang(long a)
        {
            string[] bilangan = new string[] { "", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan", "Sepuluh", "Sebelas" };
            var kalimat = "";
            // 1 - 11
            if (a < 12)
            {
                kalimat = bilangan[a];
            }
            // 12 - 19
            else if (a < 20)
            {
                kalimat = bilangan[a - 10] + " Belas";
            }
            // 20 - 99
            else if (a < 100)
            {
                var utama = a / 10;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 10;
                kalimat = bilangan[depan] + " Puluh " + bilangan[belakang];
            }
            // 100 - 199
            else if (a < 200)
            {
                kalimat = "Seratus " + Terbilang(a - 100);
            }
            // 200 - 999
            else if (a < 1000)
            {
                var utama = a / 100;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 100;
                kalimat = bilangan[depan] + " Ratus " + Terbilang(belakang);
            }
            // 1,000 - 1,999
            else if (a < 2000)
            {
                kalimat = "Seribu " + Terbilang(a - 1000);
            }
            // 2,000 - 9,999
            else if (a < 10000)
            {
                var utama = a / 1000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 1000;
                kalimat = bilangan[depan] + " Ribu " + Terbilang(belakang);
            }
            // 10,000 - 99,999
            else if (a < 100000)
            {
                var utama = a / 100;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                var belakang = a % 1000;
                kalimat = Terbilang(depan) + " Ribu " + Terbilang(belakang);
            }
            // 100,000 - 999,999
            else if (a < 1000000)
            {
                var utama = a / 1000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                var belakang = a % 1000;
                kalimat = Terbilang(depan) + " Ribu " + Terbilang(belakang);
            }
            // 1,000,000 - 	99,999,999
            else if (a < 100000000)
            {
                var utama = a / 1000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 4));//Substring(0, 4));
                var belakang = a % 1000000;
                kalimat = Terbilang(depan) + " Juta " + Terbilang(belakang);
            }
            else if (a < 1000000000)
            {
                var utama = a / 1000000;
                var sutama = utama.ToString();
                int depan = 0;
                if (sutama.Length > 3)
                {
                    depan = Convert.ToInt32(utama.ToString().Substring(0, 4));
                } 
                else
                {
                    depan = Convert.ToInt32(utama.ToString());
                }

                var belakang = a % 1000000;
                kalimat = Terbilang(depan) + " Juta " + Terbilang(belakang);
            }
            else if (a < 10000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 100000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 1000000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 10000000000000)
            {
                var utama = a / 10000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 10000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }
            else if (a < 100000000000000)
            {
                var utama = a / 1000000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                var belakang = a % 1000000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }

            else if (a < 1000000000000000)
            {
                var utama = a / 1000000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                var belakang = a % 1000000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }

            else if (a < 10000000000000000)
            {
                var utama = a / 1000000000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 1000000000000000;
                kalimat = Terbilang(depan) + " Kuadriliun " + Terbilang(belakang);
            }

            var pisah = kalimat.Split(' ');
            List<string> full = new List<string>();// = [];
            for (var i = 0; i < pisah.Length; i++)
            {
                if (pisah[i] != "") { full.Add(pisah[i]); }
            }
            return CombineTerbilang(full.ToArray());// full.Concat(' '); .join(' ');
        }
        private string CombineTerbilang(string[] arr)
        {
            return string.Join(" ", arr);
        }

        public string IntToRoman(string nums)
        {
            var num = Convert.ToInt32(nums);
            string romanResult = string.Empty;
            string[] romanLetters = {"M","CM","D","CD","C","XC","L","XL","X","IX","V","IV","I"};
            int[] numbers = {1000,900,500,400,100,90,50,40,10,9,5,4,1};
            int i = 0;
            while (num != 0)
            {
                if (num >= numbers[i])
                {
                    num -= numbers[i];
                    romanResult += romanLetters[i];
                }
                else
                {
                    i++;
                }
            }
            return romanResult;
        }
    }
}
