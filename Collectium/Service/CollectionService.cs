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
using Microsoft.EntityFrameworkCore;
using RestSharp.Authenticators;
using RestSharp;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using ClosedXML.Report;
using ClosedXML.Excel;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Collectium.Service
{
    public class CollectionService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<CollectionService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly IntegrationService integrationService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration conf;

        public CollectionService(CollectiumDBContext ctx,
                                ILogger<CollectionService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                ToolService toolService,
                                IntegrationService integrationService,
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
            this.integrationService = integrationService;
            this.conf = conf;
        }

        public GenericResponse<CallResultColResponseBean> ListCallResult()
        {
            var wrap = new GenericResponse<CallResultColResponseBean>
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

            var ld = new List<CallResultColResponseBean>();
            var q = this.ctx.CallResult.Where(q => q.Status!.Name!.Equals("AKTIF"));
            if (reqUser.Role!.Name!.Equals("FC"))
            {
                q = q.Where(q => q.isFC.Equals(1));
            } else if (reqUser.Role!.Name!.Equals("DC"))
            {
                q = q.Where(q => q.isDC.Equals(1));
            }

            var ls = q.ToList();
            if (ls != null && ls.Count > 0)
            {
                foreach (var o in ls)
                {
                    var n = this.mapper.Map<CallResultColResponseBean>(o);
                    ld.Add(n);
                }

            }

            wrap.Data = ld;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ReasonResponseBean> ListReason()
        {
            var wrap = new GenericResponse<ReasonResponseBean>
            {
                Status = false,
                Message = ""
            };

            var ld = new List<ReasonResponseBean>();
            var ls = this.ctx.Reason.Where(q => q.Status!.Name!.Equals("AKTIF") && (q.isDC! == 1)).ToList();
            if (ls != null && ls.Count > 0)
            {
                foreach (var o in ls)
                {
                    var n = this.mapper.Map<ReasonResponseBean>(o);
                    ld.Add(n);
                }

            }

            wrap.Data = ld;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CallResultColResponseBean> ListCallResultFC()
        {
            var wrap = new GenericResponse<CallResultColResponseBean>
            {
                Status = false,
                Message = ""
            };

            var ld = new List<CallResultColResponseBean>();
            var ls = this.ctx.CallResult.Where(q => q.Status!.Name!.Equals("AKTIF") && (q.isFC! == 1)).ToList();
            if (ls != null && ls.Count > 0)
            {
                foreach (var o in ls)
                {
                    var n = this.mapper.Map<CallResultColResponseBean>(o);
                    ld.Add(n);
                }

            }

            wrap.Data = ld;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ReasonResponseBean> ListReasonFC()
        {
            var wrap = new GenericResponse<ReasonResponseBean>
            {
                Status = false,
                Message = ""
            };

            var ld = new List<ReasonResponseBean>();
            var ls = this.ctx.Reason.Where(q => q.Status!.Name!.Equals("AKTIF") && (q.isFC! == 1)).ToList();
            if (ls != null && ls.Count > 0)
            {
                foreach (var o in ls)
                {
                    var n = this.mapper.Map<ReasonResponseBean>(o);
                    ld.Add(n);
                }

            }

            wrap.Data = ld;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserResponseBean> ListTeam()
        {
            var wrap = new GenericResponse<UserResponseBean>
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
            if (reqUser.Role!.Name!.Equals("SPVDC") == false && reqUser.Role!.Name!.Equals("SPVFC") == false)
            {
                wrap.Message = "Hanya untuk supervisor";
                return wrap;
            }
            var ld = new List<UserResponseBean>();
            if (reqUser.Role!.Name!.Equals("SPVFC"))
            {

                var ls = this.ctx.Team.Where(q => q.Spv!.Id.Equals(reqUser.Id)).Include(i => i.Member!).ToList();
                if (ls != null && ls.Count > 0)
                {
                    var tm = ls[0];
                    var ttm = tm.Member!.ToList();
                    foreach (var o in ttm)
                    {
                        this.ctx.Entry(o).Reference(c => c.Member).Load();
                        var n = this.mapper.Map<UserResponseBean>(o.Member);
                        ld.Add(n);
                    }

                }
            } 
            else if (reqUser.Role!.Name!.Equals("SPVDC"))
            {

                var ls = this.ctx.User.Where(q => q.RoleId!.Equals(3)).ToList();
                if (ls != null && ls.Count > 0)
                {
                    foreach (var o in ls)
                    {
                        var n = this.mapper.Map<UserResponseBean>(o);
                        ld.Add(n);
                    }
                }
            }


            wrap.Data = ld;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserResponseBean> ListTeamAll(ListTeamBean filter)
        {
            var wrap = new GenericResponse<UserResponseBean>
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
            var ld = new List<UserResponseBean>();
            var ls = new List<User>();
            if (filter != null && filter.Rolename != null)
            {
                if (filter.Rolename.Equals("dc"))
                {
                    ls = this.ctx.User.Where(q => q.RoleId.Equals(5)).Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();
                } else
                {
                    ls = this.ctx.User.Where(q => q.RoleId.Equals(6)).Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();
                }

            }
            if (ls != null && ls.Count > 0)
            {
                foreach (var o in ls)
                {
                    //this.ctx.Entry(o).Reference(c => c.Spv).Load();
                    var n = this.mapper.Map<UserResponseBean>(o);
                    ld.Add(n);
                }

            }

            wrap.Data = ld;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserResponseBean> ListTeamByBranch()
        {
            var wrap = new GenericResponse<UserResponseBean>
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
            if (reqUser.Role!.Name!.Equals("SPVDC") == false && reqUser.Role!.Name!.Equals("SPVFC") == false)
            {
                wrap.Message = "Hanya untuk supervisor";
                return wrap;
            }

            var role = reqUser.Role;
            int urole = 3;
            if (role.Name.Equals("SPVFC"))
            {
                urole = 4;
            }

            var bid = new List<int?>();
            this.ctx.Entry(reqUser).Collection(c => c.Branch!).Load();
            foreach (var id in reqUser.Branch!)
            {
                this.ctx.Entry(id).Reference(r => r.Branch!).Load();
                bid.Add(id.Branch!.Id);
            }

            var activeBranchId = this.ctx.User.FirstOrDefault(o => o.Id == reqUser.Id).ActiveBranchId;

            var ld = new List<UserResponseBean>();
            var ls = this.ctx.UserBranch.Where(q => bid.Contains(q.Branch!.Id)).Where(q => q.User!.Role!.Id.Equals(urole))
                                        .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                                        .Include(i => i.User)
                                        .ToList();

            //var ls = this.ctx.UserBranch.Where(q => q.Branch!.Id == activeBranchId).Where(q => q.User!.Role!.Id.Equals(urole))
            //                            .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
            //                            .Include(i => i.User)
            //                            .ToList();
            if (ls != null && ls.Count > 0)
            {
                foreach (var o in ls)
                {
                    var n = this.mapper.Map<UserResponseBean>(o.User);
                    ld.Add(n);
                }

            }

            wrap.Data = ld;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserResponseBean> ListFcByBranch()
        {
            var wrap = new GenericResponse<UserResponseBean>
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

            var role = reqUser.Role;
            int urole = 4;

            var bid = new List<int?>();
            this.ctx.Entry(reqUser).Collection(c => c.Branch!).Load();
            foreach (var id in reqUser.Branch!)
            {
                this.ctx.Entry(id).Reference(r => r.Branch!).Load();
                bid.Add(id.Branch!.Id);
            }

            var ids = new Dictionary<int, int?>();

            var ld = new List<UserResponseBean>();
            var ls = this.ctx.UserBranch.Where(q => bid.Contains(q.Branch!.Id)).Where(q => q.User!.Role!.Id.Equals(urole))
                                        .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                                        .Include(i => i.User)
                                        .ToList();
            if (ls != null && ls.Count > 0)
            {
                foreach (var o in ls)
                {
                    if (ids.ContainsValue(o.User!.Id!) == false)
                    {
                        var n = this.mapper.Map<UserResponseBean>(o.User);
                        ld.Add(n);
                        ids.Add(o.User!.Id!.Value, o.User.Id);
                    } 

                }

            }

            wrap.Data = ld;

            wrap.Status = true;

            return wrap;
        }


        public GenericResponse<UserResponseBean> ListAgentByBranch()
        {
            var wrap = new GenericResponse<UserResponseBean>
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

            var role = reqUser.Role!.Id;
            var urole = new List<int?>();
            if (role == 5)
            {
                urole.Add(3);
            } 
            else if (role == 6)
            {
                urole.Add(4);
            } 
            else
            {
                urole.Add(3);
                urole.Add(4);
            }



            var bid = new List<int?>();
            this.ctx.Entry(reqUser).Collection(c => c.Branch!).Load();
            foreach (var id in reqUser.Branch!)
            {
                this.ctx.Entry(id).Reference(r => r.Branch!).Load();
                bid.Add(id.Branch!.Id);
            }

            var ids = new Dictionary<int, int?>();

            var ld = new List<UserResponseBean>();
            var qr = this.ctx.UserBranch.Where(q => urole.Contains(q.User!.Role!.Id))
                                        .Where(q => q.User!.Status!.Name!.Equals("AKTIF"));
            if (role == 7 || role == 6)
            {
                qr = qr.Where(q => bid.Contains(q.Branch!.Id));
            }

            var ls = qr.Include(i => i.User).ToList();

            if (ls != null && ls.Count > 0)
            {
                foreach (var o in ls)
                {
                    if (ids.ContainsValue(o.User!.Id!) == false)
                    {
                        var n = this.mapper.Map<UserResponseBean>(o.User);
                        ld.Add(n);
                        ids.Add(o.User!.Id!.Value, o.User.Id);
                    }

                }

            }

            wrap.Data = ld;

            wrap.Status = true;

            return wrap;
        }


        public GenericResponse<BranchResponseBean> ListMyBranch()
        {
            var wrap = new GenericResponse<BranchResponseBean>
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


            var ld = new List<BranchResponseBean>();

            var role = reqUser.RoleId;
            if (role == 3 || role == 4 || role == 6)
            {
                this.ctx.Entry(reqUser).Collection(c => c.Branch!).Load();
                foreach (var itm in reqUser.Branch!)
                {
                    this.ctx.Entry(itm).Reference(c => c.Branch!).Load();
                    var n = this.mapper.Map<BranchResponseBean>(itm.Branch);
                    ld.Add(n);
                }
            }
            else if (role == 7)
            {
                var br = this.ctx.Branch.Find(reqUser.ActiveBranchId);
                if (br != null)
                {
                    var n = this.mapper.Map<BranchResponseBean>(br);
                    ld.Add(n);
                }
            }
            else
            {
                var lb = this.ctx.Branch.Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();
                foreach (var itm in lb)
                {
                    var n = this.mapper.Map<BranchResponseBean>(itm);
                    ld.Add(n);
                }
            }

            wrap.Data = ld;
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollResponseBean> ListCollection(CollListRequestBean filter)
        {
            var wrap = new GenericResponse<CollResponseBean>
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

            if (filter.CallResultCode == null)
            {
                wrap.Message = "Call Result tidak ditemukan";
                return wrap;
            }

            IQueryable<MasterLoan> q = this.ctx.Set<MasterLoan>().Include(i => i.Customer)
                                        .Include(i => i.Product)
                                        .Include(i => i.Call!.CallBy)
                                        .Include(i => i.Call!.Branch)
                                        .Include(i => i.Call!.Branch!.Area);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<MasterLoan>();
                predicate = predicate.Or(p => p.Cif!.Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.AccNo!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.Customer!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccountNo!= null && filter.AccountNo!.Length > 0)
            {
                q = q.Where(q => q.AccNo!.Contains(filter.AccountNo));
            }

            if (filter.Dpd != null)
            {
                q = q.Where(q => q.Dpd.Equals(filter.Dpd));
            }

            if (filter.OfficerId != null && filter.OfficerId > 0)
            {
                q = q.Where(q => q.Call!.CallBy!.Id.Equals(filter.OfficerId));
            }

            if (filter.CallResultCode != null && filter.CallResultCode.Equals("PTP"))
            {
                if (filter.PtpDate != null)
                {
                    q = q.Where(q => q.Call!.CallResultDate.Equals(filter.PtpDate));
                }
            }

            q = q.Where(q => q.Call!.CallResult!.Code!.Equals(filter.CallResultCode));
            q = q.Where(q => q.Call!.CallBy!.Id.Equals(reqUser.Id));
            q = q.Where(q => q.Status.Equals(1));

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<CollResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CollResponseBean>(it);
                dto.Branch = it.Call!.Branch!.Name;
                if (it.Call != null && it.Call.Branch != null && it.Call.Branch.Area != null)
                {
                    dto.Area = it.Call!.Branch!.Area!.Name;
                }
                dto.LastActivityDate = it.Call!.CallDate!;
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollResponseBean> ListCollectionFC(CollListRequestBean filter)
        {
            var wrap = new GenericResponse<CollResponseBean>
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

            IQueryable<MasterLoan> q = this.ctx.Set<MasterLoan>().Include(i => i.Customer)
                                        .Include(i => i.Product)
                                        .Include(i => i.Call!.CallBy)
                                        .Include(i => i.Call!.Branch)
                                        .Include(i => i.Call!.Branch!.Area);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<MasterLoan>();
                predicate = predicate.Or(p => p.Cif!.Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.AccNo!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.Customer!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            if (filter.CallResultCode != null && filter.CallResultCode!.Equals("PTP"))
            {
                q = q.Where(q => q.Call!.CallResult!.Code!.Equals(filter.CallResultCode));
            } else
            {
                q = q.Where(q => q.Call!.CallResult!.Code! != "PTP");
            }

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccountNo != null && filter.AccountNo!.Length > 0)
            {
                q = q.Where(q => q.AccNo!.Contains(filter.AccountNo));
            }

            if (filter.Dpd != null)
            {
                q = q.Where(q => q.Dpd.Equals(filter.Dpd));
            }

            if (filter.BranchId != null)
            {
                q = q.Where(q => q.Call.BranchId.Equals(filter.BranchId));
            }

            if (filter.OfficerId != null && filter.OfficerId > 0)
            {
                q = q.Where(q => q.Call!.CallBy!.Id.Equals(filter.OfficerId));
            }

            q = q.Where(q => q.Status.Equals(1));

            q = q.OrderByDescending(q => q.Id);

            //sorting
            if (filter.sortasc != null && filter.sortasc == 1)
            {
                q = q.OrderBy(o => o.Customer.Name);
            }

            if (filter.sortdesc != null && filter.sortdesc == 1)
            {
                q = q.OrderByDescending(o => o.Customer.Name);
            }

            if (filter.sorttagihan != null && filter.sorttagihan == 1)
            {
                q = q.OrderByDescending(o => o.TunggakanTotal);
            }

            if (filter.sortoverduedate != null && filter.sortoverduedate == 1)
            {
                q = q.OrderByDescending(o => o.MaturityDate);
            }

            q = q.Where(q => q.Call!.CallBy!.Id.Equals(reqUser.Id) && q.Call!.CallResult!.Code.Equals("NT"));

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<CollResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CollResponseBean>(it);
                dto.Branch = it.Call!.Branch!.Name;
                if (it.Call != null && it.Call.Branch != null && it.Call.Branch.Area != null)
                {
                    dto.Area = it.Call!.Branch!.Area!.Name;
                }
                dto.LastActivityDate = it.Call!.CallDate!;

                var addrs = this.ctx.CollectionAddContact.Where(q => q.CuCif!.Equals(it.Cif)).Where(q => q.AddPhone != null).Where(q => q.Default.Equals(1)).OrderByDescending(o => o.Id).ToList();
                if (addrs != null && addrs.Count > 0)
                {
                    var add = addrs[0];
                    dto!.Lat = add.Lat;
                    dto!.Lon = add.Lon;
                }
                ldata.Add(dto);
            }

            

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }


        public GenericResponse<CollResponseBean> ListCollectionLetter(CollListRequestBean filter)
        {
            var wrap = new GenericResponse<CollResponseBean>
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

            var role = reqUser.RoleId;
            var ab = this.ctx.Branch.Find(reqUser.ActiveBranchId);


            IQueryable<MasterLoan> q = this.ctx.Set<MasterLoan>().Include(i => i.Customer)
                                        .Include(i => i.Product)
                                        .Include(i => i.Call!.CallBy)
                                        .Include(i => i.Call!.Branch)
                                        .Include(i => i.Call!.Branch!.Area);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<MasterLoan>();
                predicate = predicate.Or(p => p.Cif!.Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.AccNo!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.Customer!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            if (filter.CallResultCode != null && filter.CallResultCode!.Equals("PTP"))
            {
                q = q.Where(q => q.Call!.CallResult!.Code!.Equals(filter.CallResultCode));
            }

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccountNo != null && filter.AccountNo!.Length > 0)
            {
                q = q.Where(q => q.AccNo!.Contains(filter.AccountNo));
            }

            if (filter.Dpd != null && filter.Dpd > 0)
            {
                q = q.Where(q => q.Dpd!.Equals(filter.Dpd));
            }


            if (filter.OfficerId != null && filter.OfficerId > 0)
            {
                q = q.Where(q => q.Call!.CallBy!.Id.Equals(filter.OfficerId));
            }

            q = q.Where(q => q.ChannelBranchCode!.Equals(ab.Code)).Where(q => q.Status.Equals(1));

            q = q.Where(q => q.Dpd > 14);

            q = q.OrderBy(o => o.Dpd).OrderBy(o => o.Id);

            //sorting
            if (filter.sortasc != null && filter.sortasc == 1)
            {
                q = q.OrderBy(o => o.Customer.Name);
            }

            if (filter.sortdesc != null && filter.sortdesc == 1)
            {
                q = q.OrderByDescending(o => o.Customer.Name);
            }

            if (filter.sorttagihan != null && filter.sorttagihan == 1)
            {
                q = q.OrderByDescending(o => o.TunggakanTotal);
            }

            if (filter.sortoverduedate != null && filter.sortoverduedate == 1)
            {
                q = q.OrderByDescending(o => o.MaturityDate);
            }

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<CollResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CollResponseBean>(it);
                ldata.Add(dto);
            }



            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollResponseBean> ListCollectionFCPTP(CollListRequestBean filter)
        {
            var wrap = new GenericResponse<CollResponseBean>
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

            IQueryable<MasterLoan> q = this.ctx.Set<MasterLoan>().Include(i => i.Customer)
                                        .Include(i => i.Product)
                                        .Include(i => i.Call!.CallBy)
                                        .Include(i => i.Call!.Branch)
                                        .Include(i => i.Call!.Branch!.Area);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<MasterLoan>();
                predicate = predicate.Or(p => p.Cif!.Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.AccNo!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.Customer!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            if (filter.CallResultCode != null && filter.CallResultCode!.Equals("PTP"))
            {
                q = q.Where(q => q.Call!.CallResult!.Code!.Equals(filter.CallResultCode));
            }
            else
            {
                q = q.Where(q => q.Call!.CallResult!.Code! != "PTP");
            }

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccountNo != null && filter.AccountNo!.Length > 0)
            {
                q = q.Where(q => q.AccNo!.Contains(filter.AccountNo));
            }

            if (filter.Dpd != null)
            {
                q = q.Where(q => q.Dpd.Equals(filter.Dpd));
            }

            if (filter.BranchId != null)
            {
                q = q.Where(q => q.Call!.BranchId!.Equals(filter.BranchId));
            }

            if (filter.OfficerId != null && filter.OfficerId > 0)
            {
                q = q.Where(q => q.Call!.CallBy!.Id.Equals(filter.OfficerId));
            }

            if (filter.CallResultCode != null && filter.CallResultCode.Equals("PTP"))
            {
                if (filter.PtpDate != null)
                {
                    q = q.Where(q => q.Call!.CallResultDate.Equals(filter.PtpDate));
                }
            }

            q = q.OrderByDescending(q => q.Id);

            //sorting
            if (filter.sortasc != null && filter.sortasc == 1)
            {
                q = q.OrderBy(o => o.Customer.Name);
            }

            if (filter.sortdesc != null && filter.sortdesc == 1)
            {
                q = q.OrderByDescending(o => o.Customer.Name);
            }

            if (filter.sorttagihan != null && filter.sorttagihan == 1)
            {
                q = q.OrderByDescending(o => o.TunggakanTotal);
            }

            if (filter.sortoverduedate != null && filter.sortoverduedate == 1)
            {
                q = q.OrderByDescending(o => o.MaturityDate);
            }

            q = q.Where(q => q.Call!.CallBy!.Id.Equals(reqUser.Id));

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<CollResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CollResponseBean>(it);
                dto.Branch = it.Call!.Branch!.Name;
                if (it.Call != null && it.Call.Branch != null && it.Call.Branch.Area != null)
                {
                    dto.Area = it.Call!.Branch!.Area!.Name;
                }
                dto.LastActivityDate = it.Call!.CallDate!;
                ldata.Add(dto);
            }



            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollResponseBean> ListCollectionFCRiwayat(CollListRequestBean filter)
        {
            var wrap = new GenericResponse<CollResponseBean>
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

            IQueryable<MasterLoan> q = this.ctx.Set<MasterLoan>().Include(i => i.Customer)
                                        .Include(i => i.Product)
                                        .Include(i => i.Call!.CallBy)
                                        .Include(i => i.Call!.Branch)
                                        .Include(i => i.Call!.Branch!.Area)
                                        .Include(i => i.Call!.CallResult);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<MasterLoan>();
                predicate = predicate.Or(p => p.Cif!.Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.AccNo!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.Customer!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            if (filter.CallResultCode != null && filter.CallResultCode!.Equals("PTP"))
            {
                q = q.Where(q => q.Call!.CallResult!.Code!.Equals(filter.CallResultCode));
            }
            else
            {
                q = q.Where(q => q.Call!.CallResult!.Code! != "PTP");
            }

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccountNo != null && filter.AccountNo!.Length > 0)
            {
                q = q.Where(q => q.AccNo!.Contains(filter.AccountNo));
            }

            if (filter.Dpd != null)
            {
                q = q.Where(q => q.Dpd.Equals(filter.Dpd));
            }

            if (filter.OfficerId != null && filter.OfficerId > 0)
            {
                q = q.Where(q => q.Call!.CallBy!.Id.Equals(filter.OfficerId));
            }

            q = q.OrderByDescending(q => q.Id);

            //sorting
            if (filter.sortasc != null && filter.sortasc == 1)
            {
                q = q.OrderBy(o => o.Customer.Name);
            }

            if (filter.sortdesc != null && filter.sortdesc == 1)
            {
                q = q.OrderByDescending(o => o.Customer.Name);
            }

            if (filter.sorttagihan != null && filter.sorttagihan == 1)
            {
                q = q.OrderByDescending(o => o.TunggakanTotal);
            }

            if (filter.sortoverduedate != null && filter.sortoverduedate == 1)
            {
                q = q.OrderByDescending(o => o.MaturityDate);
            }

            q = q.Where(q => q.Call!.CallBy!.Id.Equals(reqUser.Id));
            q = q.Where(q => q.Call!.CallResult!.Code != "NT" && q.Call!.CallResult!.Code != "PTP");

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<CollResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CollResponseBean>(it);
                dto.Branch = it.Call!.Branch!.Name;
                if (it.Call != null && it.Call.Branch != null && it.Call.Branch.Area != null)
                {
                    dto.Area = it.Call!.Branch!.Area!.Name;
                }
                dto.LastActivityDate = it.Call!.CallDate!;
                ldata.Add(dto);
            }



            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollDetailResponseBean> DetailCollection(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<CollDetailResponseBean>
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Id)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var obj = this.ctx.MasterLoan.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Collection tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Call).Load();
            this.ctx.Entry(obj.Call!).Reference(r => r.CallBy).Load();
            this.ctx.Entry(obj).Reference(r => r.Customer).Load();
            this.ctx.Entry(obj).Reference(r => r.Product).Load();
            this.ctx.Entry(obj).Reference(r => r.ProductSegment).Load();

            var res = this.mapper.Map<CollDetailResponseBean>(obj);

            res.Plafond = obj.Plafond;

            var prd = this.mapper.Map<ProductLoanResponseBean>(obj.Product);
            res.Product = prd;

            var prs = this.mapper.Map<ProductSegmentResponseBean>(obj.ProductSegment);
            res.ProductSegment = prs;

            var act = new List<CollActivityLogDetailResponseBean>();
            var v = this.ctx.CollectionVisit.Where(q => q.AccNo!.Equals(obj.AccNo)).OrderByDescending(o => o.VisitDate).ToList();
            if (v != null)
            {
                foreach (var vv in v)
                {
                    var o = this.mapper.Map<CollActivityLogDetailResponseBean>(vv);
                    act.Add(o);
                }
            }
            var c = this.ctx.CollectionCall.Where(q => q.AccNo!.Equals(obj.AccNo)).OrderByDescending(o => o.CallDate).ToList();
            if (c != null)
            {
                foreach (var cc in c)
                {
                    var o = this.mapper.Map<CollActivityLogDetailResponseBean>(cc);
                    act.Add(o);
                }
            }


            var acl = new List<CollCollateralDetailResponseBean>();
            var cl = this.ctx.MasterCollateral.Where(q => q.LoanId!.Equals(obj.Id)).OrderByDescending(o => o.Id).ToList();
            if (cl != null)
            {
                foreach (var ccl in cl)
                {
                    var o = this.mapper.Map<CollCollateralDetailResponseBean>(ccl);
                    acl.Add(o);
                }
            }
            res.Collateral = acl;

            var callsc = this.ctx.CallScript.Where(q => q.AccdMin <= obj.Dpd).Where(q => q.AccdMax >= obj.Dpd).ToList();
            if (callsc != null && callsc.Count() > 0)
            {
                var cs = callsc[0];
                res.CallScript = this.GenerateCallScript(obj, cs.CsScript!);
            }

            var addrs = this.ctx.CollectionAddContact.Where(q => q.CuCif!.Equals(obj.Cif))
                            .Where(q => q.AddPhone != null).Include(i => i.Photo)
                            .OrderByDescending(o => o.Id).ToList();
            if (addrs != null)
            {
                var ladd = new List<AddressResponseBean>();
                foreach (var add in addrs)
                {
                    var nadd = this.mapper.Map<AddressResponseBean>(add);


                    ladd.Add(nadd);

                    if (add.Default != null && add.Default == 1)
                    {
                        var cust = res.Customer;
                        cust!.Lat = add.Lat;
                        cust!.Lon = add.Lon;
                        cust.Address = add.AddAddress;
                        cust.MobilePhone = add.AddPhone;

                        res.Customer= cust;
                    }
                }
                res.Address = ladd;
            }

            if (c != null)
            {
                var ladd = new List<CollActivityLogDetailResponseBean>();
                foreach (var cc in c)
                {

                    var hists = this.ctx.CollectionHistory.Where(q => q.AccNo!.Equals(cc.AccNo)).Include(i => i.Reason)
                        .Include(i => i.Result).Include(i => i.HistoryBy).Include(i => i.Photo).OrderByDescending(o => o.Id).ToList();
                    if (hists != null)
                    {
                        foreach (var add in hists)
                        {
                            var nadd = this.mapper.Map<CollActivityLogDetailResponseBean>(add);
                            ladd.Add(nadd);
                        }
                    }
                }

                res.ActivityLog = ladd;
            }

            var phs = this.ctx.PaymentHistory.Where(q => q.Loan!.Id!.Equals(obj.Id)).OrderByDescending(o => o.Tgl).ToList();
            if (phs != null)
            {
                var lphs = new List<PaymentHistoryBean>();
                foreach (var ph in phs)
                {
                    var nph = this.mapper.Map<PaymentHistoryBean>(ph);
                    lphs.Add(nph);
                }
                res.PaymentHistory = lphs;
            }

            var crq = this.ctx.CallRequest.Where(q => q.CollectionCall!.Loan!.Id!.Equals(obj.Id)).Include(i => i.CollectionCall).Include(i => i.Status)
                            .OrderByDescending(o => o.Id).ToList();
            if (crq != null)
            {
                var lphs = new List<CallBackResponseBean>();
                foreach (var it in crq)
                {
                    var dto = new CallBackResponseBean();
                    dto.Id = it.Id;
                    dto.AccNo = it.CollectionCall!.AccNo;
                    dto.CallDate = it.CreateDate;
                    dto.Result = it.Status!.Name;
                    dto.PhoneNo = it.PhoneNo;

                    lphs.Add(dto);
                }
                res.CallRequest = lphs;
            }

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<SpvCollResponseBean> ListMyTeam(SpvListCollectiontBean filter)
        {
            var wrap = new GenericResponse<SpvCollResponseBean>
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

            var team = this.ctx.Team.Where(q => q.SpvId.Equals(reqUser.Id)).Include(i => i.Member).FirstOrDefault();
            var teamk = team!.Member;
            var teaml = new List<int?>();
            foreach (var item in teamk!)
            {
                teaml.Add(item.MemberId);
            }

            IQueryable<MasterLoan> q = this.ctx.Set<MasterLoan>().Include(i => i.Customer).Include(i => i.Product).Include(i => i.Call!.CallBy);
            
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<MasterLoan>();
                predicate = predicate.Or(p => p.Cif!.Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.AccNo!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.Customer!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            if (filter.AccNo != null)
            {
                q = q.Where(q => q.AccNo!.Equals(filter.AccNo));
            }

            if (filter.AgentId != null)
            {
                q = q.Where(q => q.Call!.CallBy!.Id.Equals(filter.AgentId));
            } 

            else
            {
                if (reqUser.Role!.Name!.Equals("SPVFC"))
                {
                    q = q.Where(q => teaml.Contains(q.Call!.CallBy!.Id));
                }
                else if (reqUser.Role!.Name!.Equals("SPVDC"))
                {
                    q = q.Where(q => q.Call!.CallBy!.RoleId.Equals(3));
                }

            }

            if (filter.BranchId != null)
            {
                q = q.Where(q => q.Customer!.BranchId.Equals(filter.BranchId));
            }

                q = q.Where(q => q.Status.Equals(1));

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<SpvCollResponseBean>();
            foreach (var it in data)
            {
                this.ctx.Entry(it.Call!).Reference(r => r.Branch).Load();
                var dto = mapper.Map<SpvCollResponseBean>(it);
                var assg = mapper.Map<UserResponseBean>(it.Call!.CallBy);
                dto.Assigned = assg;
                dto.BranchName = it.Call!.Branch!.Name;

                var hist = this.ctx.CollectionHistory.Where(q => q.AccNo!.Equals(it.AccNo)).Where(q => q.CallById.Equals(assg.Id))
                                .OrderByDescending(o => o.HistoryDate).Take(1).FirstOrDefault();
                if (hist != null)
                {
                    dto.LastFollowUp = hist.HistoryDate;
                }

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ReassignBean> Reassign(ReassignWrapper filter)
        {

            var wrap = new GenericResponse<ReassignBean>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                wrap.Message = "Data tidak ditemukan";
                return wrap;
            }

            if (filter.Loan == null || filter.Loan.Count() < 1)
            {
                wrap.Message = "Data Loan tidak ditemukan";
                return wrap;
            }

            if (filter.ToMember == null)
            {
                wrap.Message = "Data tujuan ditemukan";
                return wrap;
            }

            var tom = new User();
            if (filter.ToRole!.Equals("DC"))
            {
                tom = this.ctx.User.Where(q => q.Id.Equals(filter.ToMember)).Where(q => q.Status!.Name!.Equals("AKTIF")).Where(q => q.Role!.Name!.Equals("DC")).FirstOrDefault();
                if (tom == null)
                {
                    wrap.Message = "Data tujuan (2) ditemukan";
                    return wrap;
                }
                this.ctx.Entry(tom!).Collection(c => c!.Branch!).Load();
            } else
            {
                tom = this.ctx.User.Where(q => q.Id.Equals(filter.ToMember)).Where(q => q.Status!.Name!.Equals("AKTIF")).Where(q => q.Role!.Name!.Equals("FC")).FirstOrDefault();
                if (tom == null)
                {
                    wrap.Message = "Data tujuan (2) ditemukan";
                    return wrap;
                }
                this.ctx.Entry(tom!).Collection(c => c!.Branch!).Load();
            }


            //var mym = this.ListTeamByBranch();
            var mym = this.ListTeam();
            if (mym.Status == false)
            {
                wrap.Message = mym.Message;
                return wrap;
            }

            bool found = false;
            var mymd = mym.Data.ToList();
            foreach (var i in mymd)
            {
                if (i.Id.Equals(tom!.Id))
                {
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                wrap.Message = "Data assigned tidak valid";
                return wrap;
            }

            foreach (var i in filter.Loan)
            {
                var ml = this.ctx.MasterLoan.Find(i);
                if (ml != null)
                {
                    this.ctx.Entry(ml).Reference(r => r.Call).Load();
                    var cl = ml.Call;

                    if (tom!.Id != cl!.CallById)
                    {
                        cl!.CallById = tom!.Id;
                        //cl!.BranchId = tom!.ActiveBranchId;

                        //Otomatis jadi NewTask
                        cl.CallResultId = 10;

                        this.ctx.CollectionCall.Update(cl);
                    }

                }
            }

            this.ctx.SaveChanges();

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<SaveResultBean> SaveCallResult(SaveResultBeanToDc filter)
        {

            var wrap = new GenericResponse<SaveResultBean>
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.LoanId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Pick(nameof(filter.ResultId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Pick(nameof(filter.AddId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                //.Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            if (reqUser.Role!.Name!.Equals("DC") == false)
            {
                wrap.Message = "Hanya untuk DC";
                return wrap;
            }

            var mls = this.ctx.MasterLoan.Where(q => q.Id.Equals(filter.LoanId)).Include(i => i.Call).ThenInclude(ti => ti!.CallBy).ToList();
            if (mls == null || mls.Count() < 1)
            {
                wrap.Message = "Data Loan tidak ditemukan";
                return wrap;
            }

            var ml = mls[0];
            if (ml.Call == null)
            {
                wrap.Message = "Data Loan tidak ditemukan. Collection Call failed";
                return wrap;
            }

            var call = ml.Call;
            this.ctx.Entry(call).Reference(r => r.Branch).Load();

            if (filter.ToFcId != null)
            {
                var fc = this.ctx.User.Find(filter.ToFcId);
                if (fc == null)
                {
                    wrap.Message = "Data FC tidak ditemukan di sistem";
                    return wrap;
                }
                this.ctx.Entry(fc).Reference(r => r.Status).Load();
                this.ctx.Entry(fc).Collection(c => c.Branch).Load();
                if (fc.Status!.Name!.Equals("AKTIF") == false)
                {
                    wrap.Message = "Data FC tidak aktif";
                    return wrap;
                }

                var found = false;
                if (fc.Branch == null)
                {
                    this.logger.LogError("user branch is null");
                }
                foreach (var br in fc.Branch!)
                {
                    this.ctx.Entry(br).Reference(r => r.Branch).Load();
                    this.logger.LogInformation("SaveCallResult >>> br.id = " + br.Id + " call branch: " + call.Branch!.Id);
                    if (br.Branch!.Id == call.Branch!.Id)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    wrap.Message = "Data FC (2) tidak aktif";
                    return wrap;
                }

                call.CallBy = fc;

                //Otomatis jadi NewTask
                call.CallResultId = 10;

            } else
            {
                if (call!.CallBy!.Id != reqUser.Id)
                {
                    wrap.Message = "Data Loan tidak sesuai dengan DC";
                    return wrap;
                }
            }


            var ress = this.ctx.CallResult.Where(q => q.Id.Equals(filter.ResultId)).Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();
            if (ress == null || ress.Count() < 1)
            {
                wrap.Message = "Data Result tidak valid";
                return wrap;
            }
            //var res = ress.FirstOrDefault(o => o.Code.Equals("NT"));

            if (filter.ToFcId == null)
            {
                call.CallResult = ress[0];
            }


            if (filter.ReasonId != null)
            {
                var reass = this.ctx.Reason.Where(q => q.Id.Equals(filter.ReasonId)).Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();
                if (reass == null || reass.Count() < 1)
                {
                    wrap.Message = "Data Reason tidak valid";
                    return wrap;
                }
                var reas = reass[0];
                call.Reason = reas;
            }

            var cadd = this.ctx.CollectionAddContact.Where(q => q.Id.Equals(filter.AddId)).ToList();
            if (cadd == null || cadd.Count() < 1)
            {
                wrap.Message = "Data Collection Address tidak valid";
                return wrap;
            }
            var cad = cadd[0];
            call.CollectionAddId = cad.Id;

            if (filter.ResultDate != null)
            {
                var dt = DateTime.Parse(filter.ResultDate);
                call.CallResultDate = dt;
            }

            call.CallNotes = filter.Notes;
            call.CallName = filter.Name;
            call.CallDate = DateTime.Now;
            this.ctx.CollectionCall.Update(call);
            this.ctx.SaveChanges();

            var hist = new CollectionHistory();
            hist.Call = call;
            hist.Branch = call.Branch;
            hist.AccNo = call.AccNo;
            hist.Amount = Convert.ToDouble(filter.Amount);
            hist.HistoryBy = reqUser;
            hist.Kolek = ml.Kolektibilitas.ToString();
            hist.Name = filter.Name;
            hist.Notes = filter.Notes;
            hist.Reason = call.Reason;
            hist.ResultId = call.CallResultId;
            hist.ResultDate = DateTime.Now;
            hist.HistoryDate = DateTime.Now;
            hist.Latitude = filter.Latitude;
            hist.Longitude = filter.Longitude;
            hist.CallResultHh = filter.ResultDate;
            hist.CallResultMm = filter.ResultTime;
            hist.DPD = ml.Dpd;
            hist.CallBy = call.CallBy;
            this.ctx.CollectionHistory.Add(hist);
            this.ctx.SaveChanges();

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<SaveResultBean> SaveCallResultFC(SaveResultBeanFc filter)
        {

            var wrap = new GenericResponse<SaveResultBean>
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.LoanId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Pick(nameof(filter.ResultId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                //.Pick(nameof(filter.AddId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            if (reqUser.Role!.Name!.Equals("FC") == false)
            {
                wrap.Message = "Hanya untuk FC";
                return wrap;
            }

            var mls = this.ctx.MasterLoan.Where(q => q.Id.Equals(filter.LoanId)).Include(i => i.Call).ThenInclude(ti => ti!.CallBy).ToList();
            if (mls == null || mls.Count() < 1)
            {
                wrap.Message = "Data Loan tidak ditemukan";
                return wrap;
            }

            var ml = mls[0];
            if (ml.Call == null)
            {
                wrap.Message = "Data Loan tidak ditemukan. Collection Call failed";
                return wrap;
            }

            var call = ml.Call;
            this.ctx.Entry(call).Reference(r => r.Branch).Load();

            /*
            if (filter.ToFcId != null)
            {
                var fc = this.ctx.User.Find(filter.ToFcId);
                if (fc == null)
                {
                    wrap.Message = "Data FC tidak ditemukan di sistem";
                    return wrap;
                }
                this.ctx.Entry(fc).Reference(r => r.Status).Load();
                if (fc.Status!.Name!.Equals("AKTIF") == false)
                {
                    wrap.Message = "Data FC tidak aktif";
                    return wrap;
                }

                var found = false;
                foreach (var br in fc.Branch!)
                {
                    this.ctx.Entry(br).Reference(r => r.Branch).Load();
                    this.logger.LogInformation("SaveCallResult >>> br.id = " + br.Id + " call branch: " + call.Branch!.Id);
                    if (br.Branch!.Id == call.Branch!.Id)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    wrap.Message = "Data FC (2) tidak aktif";
                    return wrap;
                }

                call.CallBy = fc;

            }
            else
            {
                if (call!.CallBy!.Id != reqUser.Id)
                {
                    wrap.Message = "Data Loan tidak sesuai dengan FC";
                    return wrap;
                }
            }
            */
            if (call!.CallBy!.Id != reqUser.Id)
            {
                wrap.Message = "Data Loan tidak sesuai dengan FC";
                return wrap;
            }

            var ress = this.ctx.CallResult.Where(q => q.Id.Equals(filter.ResultId)).Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();
            if (ress == null || ress.Count() < 1)
            {
                wrap.Message = "Data Result tidak valid";
                return wrap;
            }
            //var res = ress.FirstOrDefault(o => o.Code.Equals("NT"));
            call.CallResult = ress[0];

            if (filter.ReasonId != null)
            {
                var reass = this.ctx.Reason.Where(q => q.Id.Equals(filter.ReasonId)).Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();
                if (reass == null || reass.Count() < 1)
                {
                    wrap.Message = "Data Reason tidak valid";
                    return wrap;
                }
                var reas = reass[0];
                call.Reason = reas;
            }

            /*
            var cadd = this.ctx.CollectionAddContact.Where(q => q.Id.Equals(filter.AddId)).ToList();
            if (cadd == null || cadd.Count() < 1)
            {
                wrap.Message = "Data Collection Address tidak valid";
                return wrap;
            }
            var cad = cadd[0];
            call.CollectionAddId = cad.Id;
            */

            call.CallNotes = filter.Notes;
            call.CallName = filter.Name;
            this.ctx.CollectionCall.Update(call);
            this.ctx.SaveChanges();

            var hist = new CollectionHistory();
            hist.Call = call;
            hist.Branch = call.Branch;
            hist.AccNo = call.AccNo;
            hist.Amount = Convert.ToDouble(filter.Amount);
            hist.HistoryBy = reqUser;
            hist.Kolek = ml.Kolektibilitas.ToString();
            hist.Name = filter.Name;
            hist.Notes = filter.Notes;
            hist.Reason = call.Reason;
            hist.Result = call.CallResult;
            hist.ResultDate = DateTime.Now;
            hist.HistoryDate = DateTime.Now;
            hist.Latitude = filter.Latitude;
            hist.Longitude = filter.Longitude;
            hist.CallResultHh = filter.ResultDate;
            hist.CallResultMm = filter.ResultTime;
            hist.DPD = ml.Dpd;
            hist.CallBy = call.CallBy;
            this.ctx.CollectionHistory.Add(hist);
            this.ctx.SaveChanges();

            if (filter.PhotoId != null && filter.PhotoId.Count > 0)
            {
                foreach(var ph in filter.PhotoId)
                {
                    var pph = this.ctx.CollectionPhoto.Find(ph);
                    if (pph != null)
                    {
                        pph.CollectionHistoryId = hist.Id;
                        this.ctx.CollectionPhoto.Update(pph);   
                    }
                }
                this.ctx.SaveChanges();
            }

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<AddressResponseBean> SaveContact(SaveContactDTOBean filter)
        {

            var wrap = new GenericResponse<AddressResponseBean>
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.LoanId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Pick(nameof(filter.AddFrom)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(filter.AddPhone)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(filter.AddCity)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var loan = this.ctx.MasterLoan.Find(filter.LoanId);
            if (loan == null)
            {
                wrap.Message = "Data Loan tidak ditemukan";
                return wrap;
            }

            var cadd = new CollectionAddContact();
            IlKeiCopyObject.Instance.WithSource(filter).WithDestination(cadd)
                            .Include(nameof(filter.AddCity))
                            .Include(nameof(filter.AddFrom))
                            .Include(nameof(filter.AddPhone))
                            .Include(nameof(filter.Lat))
                            .Include(nameof(filter.Lon))
                            .Execute();

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            cadd.CuCif = loan.Cif;
            cadd.AccNo = loan.AccNo;
            cadd.AddBy = reqUser;
            cadd.AddDate = DateTime.Now;
            cadd.Default = 0;

            this.ctx.CollectionAddContact.Add(cadd);
            this.ctx.SaveChanges();

            if (filter.PhotoId != null && filter.PhotoId.Count > 0)
            {
                foreach (var ph in filter.PhotoId)
                {
                    var pph = this.ctx.CollectionContactPhoto.Find(ph);
                    if (pph != null)
                    {
                        pph.CollectionContactId = cadd.Id;
                        this.ctx.CollectionContactPhoto.Update(pph);
                    }
                }
                this.ctx.SaveChanges();
            }

            var res = this.mapper.Map<AddressResponseBean>(filter);
            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }


        public GenericResponse<UserReqApproveBean> SetAsDefault(UserReqApproveBean filter)
        {

            var wrap = new GenericResponse<UserReqApproveBean>
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Id)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var addr = this.ctx.CollectionAddContact.Find(filter.Id);
            if (addr == null)
            {
                wrap.Message = "Data Alamat tidak ditemukan";
                return wrap;
            }

            addr.Default = 1;
            this.ctx.CollectionAddContact.Update(addr); 
            this.ctx.SaveChanges();

            var prevs = this.ctx.CollectionAddContact.Where(q => q.CuCif!.Equals(addr.CuCif))
                            .Where(q => q.Id != addr.Id).ToList();

            if (prevs != null && prevs.Count > 0)
            {
                foreach (var i in prevs)
                {
                    i.Default = 0;
                    this.ctx.CollectionAddContact.Update(addr);
                }

                this.ctx.SaveChanges();
            }


            wrap.AddData(filter);
            wrap.Status = true;

            return wrap;
        }

        public IActionResult GenerateLetter()
        {

            var client = new RestClient("http://lis.healtri.com:8080/jasperserver/rest_v2/");
            client.Authenticator = new HttpBasicAuthenticator("jasperadmin", "jasperadmin");

            var request = new RestRequest("reports/collectium/surat_teguran.pdf?no_surat=1123&tgl=10 September 2022&nama_nasabah=Doni Hernandi&jumlah=1.000.000&terbilang=Satu Milyar Rupiah&tgl_bayar=10 September 2022&deputy_director=Agus Pambadio&senior_officer=Andi Hamzah", Method.Get);
            var res = client.DownloadData(request);
            MemoryStream ms = new MemoryStream(res);
            return new FileStreamResult(ms, "application/pdf");

        }

        public GenericResponse<StatementResponse> CheckPayment(string accountNo)
        {
            var wrap = new GenericResponse<StatementResponse>
            {
                Status = false,
                Message = ""
            };

            return wrap;
        }

        public GenericResponse<SpvMonResponseBean> MonitorMyTeam(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<SpvMonResponseBean>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                wrap.Message = "Tanggal Monitor adalah mandatory";
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            //var st = filter.StartDate.ToString().Trim();
            //var dst = Convert.ToDateTime(st);
            //var ed = filter.EndDate.ToString().Trim() + " 23:59:59";
            //var ded = Convert.ToDateTime(st);
            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<CollectionHistory> q = this.ctx.Set<CollectionHistory>()
                                        .Include(i => i.CollectionAdd)
                                        .Include(i => i.Result)
                                        .Include(i => i.Reason)
                                        .Include(i => i.Call!.Loan)
                                        .Include(i => i.Call!.Loan!.Product)
                                        .Include(i => i.CallBy)
                                        .Include(i => i.Call!.CallBy);


            if (filter.MyType!.Equals("SPV"))
            {
                if (filter.AgentId != null)
                {
                    q = q.Where(q => q.CallById.Equals(filter.AgentId));
                } 
                else
                {
                    var team = this.ListTeamByBranch();
                    var teamk = team.Data.ToList();
                    var teaml = new List<int?>();
                    foreach (var item in teamk)
                    {
                        teaml.Add(item.Id);
                    }

                    q = q.Where(q => teaml.Contains(q.CallBy!.Id));
                }

            } 
            else if (filter.MyType!.Equals("DCFC"))
            {
                q = q.Where(q => q.CallById.Equals(reqUser.Id));
            } 
            else if (filter.MyType!.Equals("CABANG"))
            {
                //var ls = this.ctx.UserBranch.Where(q => q.UserId.Equals(reqUser.Id)).ToList();
                //var lss = new List<int?>();
                //foreach (var item in ls)
                //{
                //    lss.Add(item.Id);
                //    q = q.Where(q => lss.Contains(q.BranchId));
                //}

                q = q.Where(q => q.BranchId.Equals(reqUser.ActiveBranchId));
            }

            q = q.Where(q => q.HistoryDate!.Value.Date >= dst && q.HistoryDate!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.Id);

            if(!string.IsNullOrEmpty(filter.AccNo))
            {
                q = q.Where(o => o.AccNo!.ToLower().Contains(filter.AccNo.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                q = q.Where(o => EF.Functions.Like(o.Call!.Loan!.Customer!.Name!.ToLower(), $"%{filter.Name}%"));
            }

            if (filter.DPDmin != null && filter.DPDmin > 0)
            {
                q = q.Where(o => o.DPD >= filter.DPDmin);

                if (filter.DPDmax != null && filter.DPDmax > 0)
                {
                    q = q.Where(o => o.DPD <= filter.DPDmax);
                }
            }

            if (filter.DPDmin > 0 && filter.DPDmax > 0)
            {
                q = q.Where(o => o.DPD >= filter.DPDmin).Where(o => o.DPD <= filter.DPDmax);
            }

            if (filter.BranchId != null)
            {
                q = q.Where(o => o.BranchId == filter.BranchId);
            }

            if (filter.ResultId != null)
            {
                q = q.Where(o => o.ResultId == filter.ResultId);
            }

            if (filter.ReasonId != null)
            {
                q = q.Where(o => o.ReasonId == filter.ReasonId);
            }

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);
            wrap.DataCount = cnt;

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<SpvMonResponseBean>();
            foreach (var it in data)
            {
                var dto = new SpvMonResponseBean();

                var cl = this.ctx.MasterLoan.Where(q => q.AccNo!.Equals(it.AccNo)).Include(i => i.Product).FirstOrDefault();
                if (cl != null)
                {
                    dto.AccNo = cl.AccNo!;
                    var cust = this.ctx.Customer.FirstOrDefault(o => o.Id == cl.CustomerId);
                    dto.Name = cust!.Name!;
                    var branch = this.ctx.Branch.FirstOrDefault(o => o.Id == cust.BranchId);
                    dto.BranchName = branch!.Name!;
                    dto.Cif = cl.Cif;
                    dto.Product = cl.Product!.Desc;
                    dto.Dpd = it.DPD;
                    dto.KewajibanTotal = cl.KewajibanTotal;
                    dto.Kolektibilitas = cl.Kolektibilitas;
                    dto.TunggakanTotal = cl.TunggakanTotal;
                    if (it.Result != null && it.Result!.Description != null)
                    {
                        dto.Result = it.Result!.Description;
                    }

                    if (it.Reason != null)
                    {
                        dto.Reason = it.Reason!.Name;
                    }

                    var ph = this.ctx.PaymentHistory.Where(q => q.LoanId.Equals(cl.Id)).OrderByDescending(o => o.CreateDate).Take(1).ToList();
                    if (ph != null && ph.Count() > 0)
                    {
                        var pph = ph[0];
                        dto.LastPayDate = pph.CreateDate;
                    }

                    dto.Janji = it.CallResultHh;

                    dto.Assigned = it.CallBy!.Username!;

                    dto.CallDate = it.ResultDate;
                }


                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }


        public ReportCounter MonitorMyTeamRes(SpvMonListBean filter)
        {
            var wrap = new ReportCounter();

            if (filter.StartDate == null || filter.EndDate == null)
            {
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<CollectionHistory> q = this.ctx.Set<CollectionHistory>();

            if (filter.MyType!.Equals("SPV"))
            {
                if (filter.AgentId != null)
                {
                    q = q.Where(q => q.CallById.Equals(filter.AgentId));
                }
                else
                {
                    var team = this.ListTeamByBranch();
                    var teamk = team.Data.ToList();
                    var teaml = new List<int?>();
                    foreach (var item in teamk)
                    {
                        teaml.Add(item.Id);
                    }

                    q = q.Where(q => teaml.Contains(q.CallBy!.Id));
                }

            }
            else if (filter.MyType!.Equals("DCFC"))
            {
                q = q.Where(q => q.CallById.Equals(reqUser.Id));
            }
            else if (filter.MyType!.Equals("CABANG"))
            {
                q = q.Where(q => q.BranchId.Equals(reqUser.ActiveBranchId));
            }

            q = q.Where(q => q.HistoryDate!.Value.Date >= dst && q.HistoryDate!.Value.Date <= ded);
            q = q.OrderByDescending(q => q.Id);

            if (!string.IsNullOrEmpty(filter.AccNo))
            {
                q = q.Where(o => o.AccNo!.ToLower().Contains(filter.AccNo.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                q = q.Where(o => EF.Functions.Like(o.Call!.Loan!.Customer!.Name!.ToLower(), $"%{filter.Name}%"));
            }

            if (filter.DPDmin != null && filter.DPDmin > 0)
            {
                q = q.Where(o => o.DPD >= filter.DPDmin);

                if (filter.DPDmax != null && filter.DPDmax > 0)
                {
                    q = q.Where(o => o.DPD <= filter.DPDmax);
                }
            }

            if (filter.DPDmin > 0 && filter.DPDmax > 0)
            {
                q = q.Where(o => o.DPD >= filter.DPDmin).Where(o => o.DPD <= filter.DPDmax);
            }

            if (filter.BranchId != null)
            {
                q = q.Where(o => o.BranchId == filter.BranchId);
            }

            if (filter.ResultId != null)
            {
                q = q.Where(o => o.ResultId == filter.ResultId);
            }

            if (filter.ReasonId != null)
            {
                q = q.Where(o => o.ReasonId == filter.ReasonId);
            }


            var a = q.Sum(s => s.Call!.Loan!.TunggakanTotal);
            var b = q.Sum(s => s.Call!.Loan!.KewajibanTotal);

            wrap.TunggakanTotal = a;
            wrap.KewajibanTotal = b;

            return wrap;
        }

        public IActionResult GenerateReportMonitor(SpvMonListBean filter)
        {
            filter.Page = 1;
            filter.PageRow = 100000;

            var res = this.MonitorMyTeam(filter);
            if (res == null || res.Status != true)
            {
                return new BadRequestResult();
            }

            var data = res.Data;

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                return new BadRequestResult();
            }

            var branch = this.ctx.Branch.Find(reqUser.ActiveBranchId);
            if (branch == null)
            {
                return new BadRequestResult();
            }

            var payload = new ReportAktivitasCabangRequest();
            payload.Cabang = branch.Name;
            payload.Start = filter.StartDate!.Value.ToString("dd-MM-yyyy");
            payload.End = filter.EndDate!.Value.ToString("dd-MM-yyyy");

            //var template = new XLTemplate("D:/tmp/bagicoll/report/Simple.xlsx");
            //template.AddVariable(payload);
            //template.Generate();

            var returnStream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {

                var wsm = "Laporan Aktivitas";
                if (filter.MyType == "SPV")
                {
                    wsm = "Laporan Aktivitas";
                }

                var worksheet = workbook.Worksheets.Add(wsm);
                worksheet.Range("A2:H3").Row(1).Merge();
                worksheet.Cell("A2").Value = wsm;

                if (filter.MyType == "CABANG")
                {
                    worksheet.Cell("A4").Value = "Cabang";
                    worksheet.Cell("A5").Value = "Tanggal";
                    worksheet.Cell("B4").Value = branch.Name;
                    worksheet.Cell("B5").Value = payload.Start;
                    worksheet.Cell("C5").Value = payload.End;
                }
                else
                {
                    worksheet.Cell("A4").Value = "Tanggal";
                    worksheet.Cell("B4").Value = payload.Start;
                    worksheet.Cell("C4").Value = payload.End;
                }

                worksheet.Cell("A7").Value = "Account No";
                worksheet.Cell("B7").Value = "Debitur";
                worksheet.Cell("C7").Value = "Branch";
                worksheet.Cell("D7").Value = "Tanggal Follow Up";
                worksheet.Cell("E7").Value = "Officer";
                worksheet.Cell("F7").Value = "Produk";
                worksheet.Cell("G7").Value = "Total Tunggakan";
                worksheet.Cell("H7").Value = "Total Kewajiban";
                worksheet.Cell("I7").Value = "DPD";
                worksheet.Cell("J7").Value = "Kolektibilitas";
                worksheet.Cell("K7").Value = "Alasan";
                worksheet.Cell("L7").Value = "Tgl Janji";
                worksheet.Cell("M7").Value = "Hasil Call";

                this.DrawTable(worksheet, "A7:M7");

                int i = 8;
                foreach (var x in data)
                {
                    worksheet.Cell("A" + i).Value = x.AccNo;
                    worksheet.Cell("B" + i).Value = x.Name;
                    worksheet.Cell("C" + i).Value = x.BranchName;
                    worksheet.Cell("D" + i).Value = x.CallDate;
                    worksheet.Cell("E" + i).Value = x.Assigned;
                    worksheet.Cell("F" + i).Value = x.Product;
                    worksheet.Cell("G" + i).Value = x.TunggakanTotal;
                    worksheet.Cell("H" + i).Value = x.KewajibanTotal;
                    worksheet.Cell("I" + i).Value = x.Dpd;
                    worksheet.Cell("J" + i).Value = x.Kolektibilitas;
                    worksheet.Cell("K" + i).Value = x.Reason;
                    worksheet.Cell("L" + i).Value = x.Janji;
                    worksheet.Cell("M" + i).Value = x.Result;

                    this.DrawTable(worksheet, "A" + i + ":M" + i);
                    i++;
                }


                workbook.SaveAs(returnStream);
            }

            returnStream.Position = 0;
            returnStream.Flush();

            //template.SaveAs(returnStream);

            return new FileStreamResult(returnStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        private void DrawTable(IXLWorksheet ws, string range)
        {
            ws.Range(range).Style.Border.TopBorder = XLBorderStyleValues.Thin; 
            ws.Range(range).Style.Border.InsideBorder = XLBorderStyleValues.Thin; 
            ws.Range(range).Style.Border.OutsideBorder = XLBorderStyleValues.Thin; 
            ws.Range(range).Style.Border.LeftBorder = XLBorderStyleValues.Thin; 
            ws.Range(range).Style.Border.RightBorder = XLBorderStyleValues.Thin; 
            ws.Range(range).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        }

        public GenericResponse<CollectionPhoto> UploadPhoto(UploadRequestBean filter)
        {

            var wrap = new GenericResponse<CollectionPhoto>
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

            if (filter.Lat == null)
            {
                wrap.Message = "Data latitude tidak ditemukan";
                return wrap;
            }

            if (filter.Lon == null)
            {
                wrap.Message = "Data longitude tidak ditemukan";
                return wrap;
            }

            if (filter.CallId == null || filter.CallId < 0)
            {
                wrap.Message = "Data Call tidak ditemukan";
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var path = conf["PhotoPath"];
            path = path + "/" + filter.CallId;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var cp = new CollectionPhoto();
            cp.Description = filter.Description;
            cp.Title    = filter.Title;
            cp.CreateDate = DateTime.Today;
            cp.Lat  = filter.Lat;
            cp.Lon  = filter.Lon;
            cp.UserId = reqUser.Id;
            cp.Mime = filter.File.ContentType.ToString();
            this.ctx.CollectionPhoto.Add(cp);
            this.ctx.SaveChanges();

            var nm = path + "/" + cp.Id.ToString();
            if (cp.Mime.Contains("jpeg") || cp.Mime.Contains("jpg"))
            {
                nm += ".jpg";
            } 
            else
            {
                nm += ".png";
            }

            using (FileStream filestream = System.IO.File.Create(nm))
            {
                filter.File.CopyTo(filestream);
                filestream.Flush();
                //  return "\\Upload\\" + objFile.files.FileName;
            }

            var url = filter.CallId + "/" + cp.Id.ToString();
            if (cp.Mime.Contains("jpeg") || cp.Mime.Contains("jpg"))
            {
                url += ".jpg";
            }
            else
            {
                url += ".png";
            }
            cp.Url = url;
            this.ctx.CollectionPhoto.Update(cp);
            this.ctx.SaveChanges(true);

            wrap.Status = true;
            wrap.AddData(cp);

            return wrap;
        }

        public GenericResponse<UserReqApproveBean> DeletePhoto(UserReqApproveBean filter)
        {

            var wrap = new GenericResponse<UserReqApproveBean>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                wrap.Message = "Data tidak ditemukan";
                return wrap;
            }

            if (filter.Id == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }

            var prev = this.ctx.CollectionPhoto.Find(filter.Id);
            if (prev == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }

            this.ctx.CollectionPhoto.Remove(prev);
            this.ctx.SaveChanges(true);

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public IActionResult ViewPhoto(UserReqApproveBean filter)
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

            var prev = this.ctx.CollectionPhoto.Find(filter.Id);
            if (prev == null)
            {
                return new BadRequestResult();
            }

            var file = conf["PhotoPath"] + prev.Url;
            if (File.Exists(file) == false)
            {
                return new BadRequestResult();
            }

            var bytes = File.ReadAllBytes(file);
            MemoryStream ms = new MemoryStream(bytes);
            return new FileStreamResult(ms, prev.Mime!);
        }

        public GenericResponse<CollectionContactPhoto> UploadContactPhoto(UploadRequestBean filter)
        {

            var wrap = new GenericResponse<CollectionContactPhoto>
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

            if (filter.Lat == null)
            {
                wrap.Message = "Data latitude tidak ditemukan";
                return wrap;
            }

            if (filter.Lon == null)
            {
                wrap.Message = "Data longitude tidak ditemukan";
                return wrap;
            }

            if (filter.CallId == null || filter.CallId < 0)
            {
                wrap.Message = "Data Call tidak ditemukan";
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var path = conf["PhotoContactPath"];
            path = path + "/" + filter.CallId;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var cp = new CollectionContactPhoto();
            cp.Description = filter.Description;
            cp.Title = filter.Title;
            cp.CreateDate = DateTime.Today;
            cp.Lat = filter.Lat;
            cp.Lon = filter.Lon;
            cp.UserId = reqUser.Id;
            cp.Mime = filter.File.ContentType.ToString();
            this.ctx.CollectionContactPhoto.Add(cp);
            this.ctx.SaveChanges();

            var nm = path + "/" + cp.Id.ToString();
            if (cp.Mime.Contains("jpeg") || cp.Mime.Contains("jpg"))
            {
                nm += ".jpg";
            }
            else
            {
                nm += ".png";
            }

            using (FileStream filestream = System.IO.File.Create(nm))
            {
                filter.File.CopyTo(filestream);
                filestream.Flush();
                //  return "\\Upload\\" + objFile.files.FileName;
            }

            var url = filter.CallId + "/" + cp.Id.ToString();
            if (cp.Mime.Contains("jpeg") || cp.Mime.Contains("jpg"))
            {
                url += ".jpg";
            }
            else
            {
                url += ".png";
            }
            cp.Url = url;
            this.ctx.CollectionContactPhoto.Update(cp);
            this.ctx.SaveChanges(true);

            wrap.Status = true;
            wrap.AddData(cp);

            return wrap;
        }

        public IActionResult ViewContactPhoto(UserReqApproveBean filter)
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

            var prev = this.ctx.CollectionContactPhoto.Find(filter.Id);
            if (prev == null)
            {
                return new BadRequestResult();
            }

            var file = conf["PhotoContactPath"] + prev.Url;
            if (File.Exists(file) == false)
            {
                return new BadRequestResult();
            }

            var bytes = File.ReadAllBytes(file);
            MemoryStream ms = new MemoryStream(bytes);
            return new FileStreamResult(ms, prev.Mime);
        }

        public GenericResponse<TrackingBean> TrackingCol(TrackingBean filter)
        {

            var wrap = new GenericResponse<TrackingBean>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                wrap.Message = "Data tidak ditemukan";
                return wrap;
            }

            if (filter.Lat == null)
            {
                wrap.Message = "Data latitude tidak ditemukan";
                return wrap;
            }

            if (filter.Lon == null)
            {
                wrap.Message = "Data longitude tidak ditemukan";
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var tfs = new TrackingFc();
            tfs.UserId = reqUser.Id;
            tfs.Lat = filter.Lat;
            tfs.Lon = filter.Lon;
            tfs.Tgl = DateTime.Now;

            this.ctx.TrackingFc.Add(tfs);
            this.ctx.SaveChanges(true);

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<TrackingHistoryResponseBean> GetTrackingHistory(SpvListCollectiontBean filter)
        {

            var wrap = new GenericResponse<TrackingHistoryResponseBean>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                wrap.Message = "Data tidak ditemukan";
                return wrap;
            }

            if (filter.AgentId == null)
            {
                wrap.Message = "Data agent ditemukan";
                return wrap;
            }

            if (filter.PtpDate == null)
            {
                wrap.Message = "Data tanggal tidak ditemukan";
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var ls = this.ctx.TrackingFc.Where(q => q.UserId.Equals(filter.AgentId))
                                .Where(q => q.Tgl!.Value.Date.Equals(filter.PtpDate.Value.Date))
                                .OrderBy(o => o.Tgl)
                                .ToList();

            var dat = new List<TrackingHistoryResponseBean>();
            var prev = DateTime.Now;
            var cnt = 0;
            foreach(var i in ls)
            {
                if (cnt < 1)
                {
                    cnt++;
                    var dto = new TrackingHistoryResponseBean();
                    dto.Lat = i.Lat;
                    dto.Lon = i.Lon;
                    dto.Time = i.Tgl!.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    dat.Add(dto);

                    prev = i.Tgl.Value;
                } 
                else
                {
                    var next30 = prev.AddMinutes(30);
                    if (next30 < i.Tgl!.Value)
                    {
                        var dto = new TrackingHistoryResponseBean();
                        dto.Lat = i.Lat;
                        dto.Lon = i.Lon;
                        dto.Time = i.Tgl!.Value.ToString("dd/MM/yyyy HH:mm:ss");
                        dat.Add(dto);

                        prev = i.Tgl.Value;
                    }
                }
            }

            wrap.Status = true;
            wrap.Data = dat;

            return wrap;
        }


        public GenericResponse<SpvCollResponseBean> ListDistribusi(SpvListCollectiontBean filter)
        {
            var wrap = new GenericResponse<SpvCollResponseBean>
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

            IQueryable<MasterLoan> q = this.ctx.Set<MasterLoan>().Include(i => i.Customer).Include(i => i.Product).Include(i => i.Call!.CallBy);

            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<MasterLoan>();
                predicate = predicate.Or(p => p.Cif!.Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.AccNo!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.Customer!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            q = q.Where(q => q.Status.Equals(1));

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<SpvCollResponseBean>();
            foreach (var it in data)
            {
                this.ctx.Entry(it.Call!).Reference(r => r.Branch).Load();
                this.ctx.Entry(it.Call!.CallBy!).Reference(r => r.Role).Load();
                var dto = mapper.Map<SpvCollResponseBean>(it);
                var assg = mapper.Map<UserResponseBean>(it.Call!.CallBy);
                dto.Assigned = assg;
                dto.BranchName = it.Call!.Branch!.Name;

                var hist = this.ctx.CollectionHistory.Where(q => q.AccNo!.Equals(it.AccNo)).Where(q => q.CallById.Equals(assg.Id))
                                .OrderByDescending(o => o.HistoryDate).Take(1).FirstOrDefault();
                if (hist != null)
                {
                    dto.LastFollowUp = hist.HistoryDate;
                }

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        private string GenerateCallScript(MasterLoan ml, string cs)
        {
            cs = cs.Replace("@nama_nasabah@", ml.Customer!.Name!);
            cs = cs.Replace("@nama_dc@", ml.Call!.CallBy!.Name!);
            cs = cs.Replace("@nama_produk@", ml.Product!.Desc!);
            if (ml.LastPayDate != null)
            {
                cs = cs.Replace("@tgl_jatuh_tempo@", ml.LastPayDate!.Value.ToString("dd MMMM yyyy"));
            } else
            {
                cs = cs.Replace("@tgl_jatuh_tempo@", DateTime.Now.ToString("dd MMMM yyyy"));
            }

            return cs;
        }
    }
}
