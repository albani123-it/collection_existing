using AutoMapper;
using Azure.Core;
using Collectium.Model;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Bean.User;
using Collectium.Model.Entity;
using Collectium.Model.Helper;
using Collectium.Validation;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoreLinq;
using Serilog;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Collectium.Service
{
    public class UserService
    {
        private readonly CollectiumDBContext ctx;
        private IConfiguration conf;
        private readonly ILogger<UserService> logger;
        private readonly StatusService statusService;
        private readonly PaginationHelper pagination;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(ILogger<UserService> logger,
                                CollectiumDBContext ctx, 
                                IConfiguration conf,
                                PaginationHelper pagination,
                                StatusService statusService,
                                IMapper mapper,
                                IHttpContextAccessor httpContextAccessor)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.conf = conf;
            this.statusService = statusService;
            this.httpContextAccessor = httpContextAccessor;
            this.pagination = pagination;
            this.mapper = mapper;
        }

        public GenericResponse<string> Login(LoginRequestBean filter, bool isMobile)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };

            var pr = Validation.IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Username)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Password)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var maxFail = this.ctx.RfGlobal.Where(q => q.Code!.Equals("MAXP")).FirstOrDefault();
            var pw = this.GenerateMD5(filter.Password!);

            var q = this.ctx.User.Where(p => p.Username!.Equals(filter.Username)).Where(p => p.Status!.Name!.Equals("AKTIF"));
            if (isMobile == true)
            {
                //q = q.Where(q => q.RoleId.Equals(4));
            } else
            {
                //q = q.Where(q => q.RoleId != 4);
            }

            var usr = q.Include(i => i.Status).FirstOrDefault();
            if (usr == null)
            {
                wrap.Message = "Username dan atau password tidak valid";
                return wrap;
            }

            if (pw.Equals(usr.Password) == false)
            {
                if (usr.Fail == null)
                {
                    usr.Fail = 1;
                } else
                {
                    usr!.Fail += 1;
                }

                this.ctx.Update(usr);
                this.ctx.SaveChanges();

                if (maxFail != null)
                {
                    var mf = Int32.Parse(maxFail.Val!.ToString());
                    if (mf < usr.Fail)
                    {
                        usr.StatusId = 2;
                        this.ctx.Update(usr);
                        this.ctx.SaveChanges();
                    }
                }

                wrap.Message = "Username dan atau password tidak valid";
                return wrap;
            }

            var tkn = new Token();
            tkn.Users = usr;
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            tkn.Firstname = token;
            this.ctx.Token.Add(tkn);
            this.ctx.SaveChanges();

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, conf["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Name", usr.Name!),
                        new Claim("Token", token)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                conf["Jwt:Issuer"],
                conf["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: signIn);

            var ret = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var ls = new List<string>();
            ls.Add(ret);
            wrap.Data = ls;
            wrap.Status = true;

            return wrap;
        }


        public User GetUserFromToken(string src)
        {
            if (src == null)
            {
                return null;
            }

            //src = src.Substring(7);
            var tok = this.ctx.Token.Where(q => q.Firstname.ToUpper().Trim().Equals(src.ToUpper().Trim()))
                            .Where(q => q.Users!.Status!.Name.Equals("AKTIF"))
                            .Include(i => i.Users).ThenInclude(q => q.Role)
                            .FirstOrDefault();
            if (tok == null)
            {
                return null;
            }

            return tok.Users;
        }

        public User UpdateMy(UserWrapper src, User auth)
        {
            if (src == null || auth == null)
            {
                return null;
            }

            auth.Email = src.Email;
            auth.Name = src.Name;

            this.ctx.User.Update(auth);
            this.ctx.SaveChanges();

            return auth;
        }

        public GenericResponse<UserChangePasswordRequest> ChangeMyPassword(UserChangePasswordRequest src)
        {
            var wrap = new GenericResponse<UserChangePasswordRequest>();
            wrap.Status = false;

            if (src == null || src.UserId == null)
            {
                wrap.Message = "Data user tidak diketahiui";
                return wrap;
            }

            var auth = this.ctx.User.Where(q => q.Id.Equals(src.UserId)).Where(q => q.Status!.Name!.Equals("AKTIF")).FirstOrDefault();
            if (auth == null)
            {
                wrap.Message = "Data user tidak diketahiui";
                return wrap;
            }

            auth.Password = this.GenerateMD5(src.Password!);

            this.ctx.User.Update(auth);
            this.ctx.SaveChanges();

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserChangeTelRequest> ChangeMyTel(UserChangeTelRequest src)
        {
            var wrap = new GenericResponse<UserChangeTelRequest>();
            wrap.Status = false;

            if (src == null || src.UserId == null)
            {
                wrap.Message = "Data user tidak diketahiui";
                return wrap;
            }

            var auth = this.ctx.User.Where(q => q.Id.Equals(src.UserId)).Where(q => q.Status!.Name!.Equals("AKTIF")).FirstOrDefault();
            if (auth == null)
            {
                wrap.Message = "Data user tidak diketahiui";
                return wrap;
            }

            auth.TelCode = src.TelCode;
            auth.TelDevice = src.TelDevice;
            
            if (src.PassDevice != null)
            {
                auth.PassDevice = src.PassDevice;
            }

            this.ctx.User.Update(auth);
            this.ctx.SaveChanges();

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserActivateRequest> ActivateUser(UserActivateRequest src)
        {
            var wrap = new GenericResponse<UserActivateRequest>();
            wrap.Status = false;

            if (src == null)
            {
                wrap.Message = "Data tidak diketahui";
                return wrap;
            }

            var prev = this.ctx.User.Where(q => q.Id.Equals(src.UserId)).FirstOrDefault();
            if (prev == null)
            {
                wrap.Message = "Data user tidak diketahui";
                return wrap;
            }

            var stat = prev.StatusId;
            if (stat!.Value! == 1) {
                prev.StatusId = 2;
            } else
            {
                prev.StatusId = 1;
                prev.Fail = 0;
            }

            this.ctx.User.Update(prev);
            this.ctx.SaveChanges();

            if (prev.StatusId == 2)
            {
                var su = "update collection_call set call_by = null where call_by = " + prev.Id;
                this.ctx.Database.ExecuteSqlRaw(su);
            }

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<UserResponseBean> ListUser(UserListRequestBean filter)
        {
            var wrap = new GenericResponse<UserResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<User> q = this.ctx.Set<User>().Include(i => i.Role).Include(i => i.Status);
            
            if (filter.Id != null && filter.Id > 0)
            {
                q = q.Where(p => p.Id.Equals(filter.Id));
            }
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                q = q.Where(p => EF.Functions.Like(p.Name!.ToUpper().Trim(), "%" + filter.Keyword.ToUpper().Trim() + "%"));
            }
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                q = q.Where(p => EF.Functions.Like(p.Username!.ToUpper().Trim(), "%" + filter.Keyword.ToUpper().Trim() + "%"));
            }


            var cnt = q.Count();
            q = q.OrderByDescending(q => q.Id);
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            foreach (var temp in data)
            {
                this.ctx.Entry(temp).Collection(r => r.Branch!).Load();
            }

            var ldata = new List<UserResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<UserResponseBean>(it);

                var ubr = new List<BranchResponseBean>();
                var br = it.Branch;
                foreach (var idx in br!)
                {
                    this.ctx.Entry(idx).Reference(r => r.Branch!).Load();
                    var brb = new BranchResponseBean();
                    brb.Id = idx!.Branch!.Id;
                    brb.Name = idx!.Branch!.Name;
                    ubr.Add(brb);
                }
                dto.Branch = ubr;
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserCreateBean> SaveUsers(UserCreateBean filter)
        {
            var wrap = new GenericResponse<UserCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Username)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Password)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.RoleId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = mapper.Map<User>(filter);

            var pw = this.GenerateMD5(filter.Password!);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            us.Password = this.GenerateMD5(filter.Password!);

            this.ctx.User.Add(us);
            this.ctx.SaveChanges();

            filter.Id = us.Id;

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UserReqResponseBean> ListUserRequest(UserListRequestBean filter)
        {
            var wrap = new GenericResponse<UserReqResponseBean>
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

            IQueryable<UserRequest> q = this.ctx.Set<UserRequest>().Include(i => i.Role).Include(i => i.Status);
            

            if (filter.Id != null && filter.Id > 0)
            {
                q = q.Where(p => p.Id.Equals(filter.Id));
            }
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                q = q.Where(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
            }
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var temp = q.Where(p => p.Username!.ToUpper().Trim().Equals(filter.Keyword.ToUpper().Trim()));
                if(temp.Count() > 0)
                {
                    q = temp;
                }
            }

            q = q.Where(p => p.Status!.Name!.Equals("DRAFT"));

            /*
            if (role == 9)
            {

            }
            else if (role == 1) 
            {
                q = q.Where(p => p.Status!.Name!.Equals("CHECK"));
            }
            */
            /*
            var predicate = PredicateBuilder.New<UserRequest>();
            predicate = predicate.Or(o => o.Status!.Name.Equals("DRAFT"));
            predicate = predicate.Or(o => o.Status!.Name.Equals("APPROVE"));
            q = q.Where(predicate);
            */

            var cnt = q.Count();
            q = q.OrderByDescending(q => q.Id);
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            foreach (var temp in data)
            {
                this.ctx.Entry(temp).Collection(r => r.Branch!).Load();
            }

            var ldata = new List<UserReqResponseBean>();
            foreach (var it in data)
            {

                var bran = this.ctx.UserBranchRequest.Where(q => q.UserRequestId.Equals(it.Id)).ToList();

                //var dto = mapper.Map<UserReqResponseBean>(it);
                var dto = new UserReqResponseBean();
                dto.Id = it.Id;
                dto.Name = it.Name;
                dto.Username = it.Username;
                dto.Email = it.Email;
                var rl = mapper.Map<RoleResponseBean>(it.Role);
                dto.Role = rl;

                var st = mapper.Map<StatusRequestBean>(it.Status);
                dto.Status = st;

                var ubr = new List<BranchResponseBean>();
                var br = bran;
                foreach (var idx in br!)
                {
                    this.ctx.Entry(idx).Reference(r => r.Branch!).Load();
                    var brb = new BranchResponseBean();
                    brb.Id = idx!.Branch!.Id;
                    brb.Name = idx!.Branch!.Name;
                    ubr.Add(brb);
                }
                dto.Branch = ubr;
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserResponseBean> DetailListUser(UserListRequestBean filter)
        {
            var wrap = new GenericResponse<UserResponseBean>
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

            var obj = this.ctx.User.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();
            this.ctx.Entry(obj).Reference(r => r.Role).Load();
            //this.ctx.Entry(obj).Reference(r => r.Branch).Load();
            
            this.ctx.Entry(obj).Collection(r => r.Branch!).Load();
            

            if (obj.Status!.Name != "AKTIF")
            {
                wrap.Message = "User tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<UserResponseBean>(obj);

            {
                var dto = mapper.Map<UserResponseBean>(obj);

                var ubr = new List<BranchResponseBean>();
                var br = obj.Branch;
                foreach (var idx in br!)
                {
                    this.ctx.Entry(idx).Reference(r => r.Branch!).Load();
                    var brb = new BranchResponseBean();
                    brb.Id = idx!.Branch!.Id;
                    brb.Name = idx!.Branch!.Name;
                    ubr.Add(brb);
                }
                res.Branch = ubr;
            }

            var ttm = this.ctx.TeamMember.Where(q => q.MemberId.Equals(obj.Id)).Include(i => i.Team).FirstOrDefault();
            if (ttm != null)
            {
                if (ttm.Team != null)
                {
                    this.ctx.Entry(ttm.Team).Reference(r => r.Spv).Load();
                    var urs = new UserResponseBean();
                    urs.Id = ttm.Team.SpvId;
                    urs.Name = ttm.Team.Spv.Name;
                    res.Spv = urs;
                }
            }

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserReqResponseBean> DetailListUserRequest(UserListRequestBean filter)
        {
            var wrap = new GenericResponse<UserReqResponseBean>
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

            var obj = this.ctx.UserRequest.Find(filter.Id);

            if (obj == null)
            {
                wrap.Message = "User Request tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status!).Load();
            if (obj.Status!.Name!.Equals("REMOVE"))
            {
                wrap.Message = "User Request tidak ditemukan (R)";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();
            this.ctx.Entry(obj).Reference(r => r.User).Load();
            this.ctx.Entry(obj).Reference(r => r.Role).Load();
            //this.ctx.Entry(obj).Reference(r => r.Branch).Load();
            this.ctx.Entry(obj).Collection(r => r.Branch!).Load();

            if (obj.User != null)
            {
                this.ctx.Entry(obj.User!).Reference(r => r.Role!).Load();
                //this.ctx.Entry(obj.User).Reference(r => r.Branch).Load();
                this.ctx.Entry(obj.User).Collection(r => r.Branch!).Load();
            }


            if (obj.Status!.Name!.Equals("REMOVE"))
            {
                wrap.Message = "User Request tidak aktif";
                return wrap;
            }
            var bran = this.ctx.UserBranchRequest.Where(q => q.UserRequestId.Equals(obj.Id)).ToList();

            var dto = new UserReqResponseBean();
            dto.Id = obj.Id;
            dto.Name = obj.Name;
            dto.Username = obj.Username;
            dto.Email = obj.Email;
            var rl = mapper.Map<RoleResponseBean>(obj.Role);
            dto.Role = rl;

            var st = mapper.Map<StatusRequestBean>(obj.Status);
            dto.Status = st;

            var ubr = new List<BranchResponseBean>();
            var br = bran;
            foreach (var idx in br!)
            {
                this.ctx.Entry(idx).Reference(r => r.Branch!).Load();
                var brb = new BranchResponseBean();
                brb.Id = idx!.Branch!.Id;
                brb.Name = idx!.Branch!.Name;
                ubr.Add(brb);
            }
            dto.Branch = ubr;

            /*
            var res = this.mapper.Map<UserReqResponseBean>(obj);
            
            {
                var ubr = new List<BranchResponseBean>();
                var br = obj.Branch;
                foreach (var idx in br!)
                {
                    this.ctx.Entry(idx).Reference(r => r.Branch!).Load();
                    var brb = new BranchResponseBean();
                    brb.Id = idx!.Branch!.Id;
                    brb.Name = idx!.Branch!.Name;
                    ubr.Add(brb);
                }
                res.Branch = ubr;
            }

            {
                var dto = mapper.Map<UserResponseBean>(obj.User);

                var ubr = new List<BranchResponseBean>();
                var br = obj.User.Branch;
                foreach (var idx in br!)
                {
                    this.ctx.Entry(idx).Reference(r => r.Branch!).Load();
                    var brb = new BranchResponseBean();
                    brb.Id = idx!.Branch!.Id;
                    brb.Name = idx!.Branch!.Name;
                    ubr.Add(brb);
                }
                res.User.Branch = ubr;
            }
            */

            if (obj.User != null)
            {
                var dty = mapper.Map<UserResponseBean>(obj.User);

                var uby = new List<BranchResponseBean>();
                var by = obj.User.Branch;
                foreach (var idx in by!)
                {
                    this.ctx.Entry(idx).Reference(r => r.Branch!).Load();
                    var brb = new BranchResponseBean();
                    brb.Id = idx!.Branch!.Id;
                    brb.Name = idx!.Branch!.Name;
                    uby.Add(brb);
                }

                dty.Branch = uby;

                dto.User = dty;
            }

            wrap.AddData(dto);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<UserReqCreateBean> SaveNewUserRequest(UserReqCreateBean filter)
        {
            var wrap = new GenericResponse<UserReqCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Username)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Password)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.RoleId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
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

            var prevUserCnt = this.ctx.User.Where(q => q.Username!.Equals(filter.Username)).Count();
            if (prevUserCnt > 0)
            {
                wrap.Message = "Username " + filter.Username + " sudah ada di sistem";
                return wrap;
            }

            //var us = mapper.Map<UserRequest>(filter);
            var us = new UserRequest();
            us.Username = filter.Username;
            us.Password = filter.Password;
            us.Email = filter.Email;
            us.Name = filter.Name;
            us.RoleId = filter.RoleId;
            us.SpvId = filter.SpvId;

            var pw = this.GenerateMD5(filter.Password!);
            //us.Password = this.GenerateMD5(filter.Password!);
            us.Password = filter.Password!;

            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;
            us.CreateDate = DateTime.Now;
            us.RequestUserId = reqUser.Id;

            this.ctx.UserRequest.Add(us);
            this.ctx.SaveChanges();

            foreach(var tempBranch in filter.Branch)
            {
                var temp = new UserBranchRequest();
                temp.UserId = filter.UserId;
                temp.UserRequestId = us.Id;
                temp.BranchId = tempBranch.Id;
                var sg2 = this.statusService.GetStatusRequest("DRAFT");
                temp.Status = sg2;
                this.ctx.UserBranchRequest.Add(temp);
                this.ctx.SaveChanges();
            }
            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UserReqActiveBranch> SetActiveBranch(UserReqActiveBranch filter)
        {
            var wrap = new GenericResponse<UserReqActiveBranch>
            {
                Status = false,
                Message = ""
            };

            var user = this.ctx.User.FirstOrDefault(o => o.Id == filter.UserId);
            user.ActiveBranchId = filter.BranchId;
            this.ctx.User.Update(user);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UserReqCreateBean> SaveEditUserRequest(UserReqCreateBean filter)
        {
            var wrap = new GenericResponse<UserReqCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Username)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.RoleId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Pick(nameof(filter.UserId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
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

            //var us = mapper.Map<UserRequest>(filter);
            var us = new UserRequest();

            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;
            us.CreateDate = DateTime.Now;
            us.RequestUserId = reqUser.Id;
            us.UserId = filter.UserId;
            us.Name = filter.Name;
            us.RoleId = filter.RoleId;
            us.Email= filter.Email;
            us.Username = filter.Username;
            us.SpvId= filter.SpvId;
            

            this.ctx.UserRequest.Add(us);
            this.ctx.SaveChanges();

            foreach (var tempBranch in filter.Branch)
            {
                var temp = new UserBranchRequest();
                temp.UserBranchId = tempBranch.Id;
                temp.UserId = filter.UserId;
                temp.UserRequestId = us.Id;
                temp.BranchId = tempBranch.Id;
                var sg2 = this.statusService.GetStatusRequest("DRAFT");
                temp.Status = sg2;
                this.ctx.UserBranchRequest.Add(temp);
                this.ctx.SaveChanges();
            }

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        public GenericResponse<UserReqApproveBean> ApproveUserRequest(UserReqApproveBean filter)
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

            var appUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (appUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var us = this.ctx.UserRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "User Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();
            if (us.UserId != null)
            {
                this.ctx.Entry(us).Reference(r => r.User).Load();
                this.ctx.Entry(us.User!).Reference(r => r.Status).Load();

                if (us.User!.Status!.Name != "AKTIF")
                {
                    wrap.Message = "status User tidak AKTIF";
                    return wrap;
                }
            }

            if (us.Status!.Name != "DRAFT")
            {
                wrap.Message = "Status User Request tidak valid";
                return wrap;
            }

            if (us.Status!.Name == "DRAFT")
            {
                us.StatusId = 4;
                this.ctx.UserRequest.Update(us);
                this.ctx.SaveChanges();
            }


            int? usid = null;

            if (us.User == null)
            {
                var ucb = mapper.Map<UserCreateBean>(us);
                ucb.Id = null;
                ucb.Password = us.Password;
                this.SaveUsers(ucb);
                usid = ucb.Id;

            } else
            {
                var pus = us.User;
                pus.Name = us.Name;
                pus.Email = us.Email;
                pus.RoleId = us.RoleId;
                this.ctx.User.Update(pus);

                usid = pus.Id;

                var sr = this.statusService.GetStatusRequest("REMOVE");

                var prevReq = this.ctx.UserRequest.Where(q => q.Id < filter.Id).Where(q => q.UserId.Equals(pus.Id)).Where(q => q.Status!.Name.Equals("DRAFT")).ToList();
                if (prevReq != null && prevReq.Count > 0)
                {
                    foreach (var item in prevReq)
                    {
                        item.StatusId = sr.Id;
                        this.ctx.UserRequest.Update(item);
                    }
                }
            }


            var sa = this.statusService.GetStatusRequest("APPROVE");
            us.StatusId = sa.Id;
            us.ApproveUserId = appUser.Id;
            this.ctx.UserRequest.Update(us);

            this.ctx.SaveChanges();

            var ubs = this.ctx.UserBranch.Where(q => q.UserId.Equals(usid)).ToList();
            if (ubs != null && ubs.Count > 0)
            {
                foreach(var itm in ubs)
                {
                    this.ctx.UserBranch.Remove(itm);
                }

                this.ctx.SaveChanges();
            }

            var userBranchReq = this.ctx.UserBranchRequest.Where(o => o.UserRequestId == filter.Id).ToList();
            foreach (var tempBranch in userBranchReq)
            {
                var temp = new UserBranch();
                temp.UserId = usid;
                temp.BranchId = tempBranch.BranchId;
                var sg2 = this.statusService.GetStatusGeneral("AKTIF");
                temp.Status = sg2;
                this.ctx.UserBranch.Add(temp);
                this.ctx.SaveChanges();

                var sr = this.statusService.GetStatusRequest("APPROVE");
                tempBranch.Status = sr;
                this.ctx.UserBranchRequest.Update(tempBranch);
                this.ctx.SaveChanges();
            }

            var prevBranchReq = this.ctx.UserBranchRequest.Where(q => q.Id < filter.Id).Where(q => q.UserId.Equals(us.Id)).Where(q => q.Status!.Name.Equals("DRAFT")).ToList();
            if (prevBranchReq != null && prevBranchReq.Count > 0)
            {
                var sr = this.statusService.GetStatusRequest("REMOVE");
                foreach (var item in prevBranchReq)
                {
                    item.StatusId = sr.Id;
                    this.ctx.UserBranchRequest.Update(item);
                }
            }

            if (us.SpvId != null)
            {
                var prevTeam = this.ctx.TeamMember.Where(q => q.MemberId.Equals(usid)).ToList();
                if (prevTeam != null)
                {
                    foreach(var id in prevTeam)
                    {
                        this.ctx.TeamMember.Remove(id);
                        this.ctx.SaveChanges();
                    }

                }

                var tm = this.ctx.Team.Where(q => q.SpvId.Equals(us.SpvId)).FirstOrDefault();
                if (tm != null)
                {
                    var ttm = new TeamMember();
                    ttm.TeamId = tm.Id;
                    ttm.MemberId = usid;
                    this.ctx.TeamMember.Add(ttm);
                    this.ctx.SaveChanges();
                }
                else
                {
                    var ntxm = new Team();
                    ntxm.SpvId = us.SpvId;
                    this.ctx.Add(ntxm);
                    this.ctx.SaveChanges();

                    var ttm = new TeamMember();
                    ttm.TeamId = ntxm.Id;
                    ttm.MemberId = usid;
                    this.ctx.TeamMember.Add(ttm);
                    this.ctx.SaveChanges();
                }
            }

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UserReqApproveBean> RejectUserRequest(UserReqApproveBean filter)
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

            var appUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (appUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var us = this.ctx.UserRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "User Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();
            if (us.UserId != null)
            {
                this.ctx.Entry(us).Reference(r => r.User).Load();
                this.ctx.Entry(us.User!).Reference(r => r.Status).Load();

                if (us.User!.Status!.Name != "AKTIF")
                {
                    wrap.Message = "status User tidak AKTIF";
                    return wrap;
                }
            }

            if (us.Status!.Name != "DRAFT")
            {
                wrap.Message = "Status User Request tidak valid";
                return wrap;
            }

            us.StatusId = 6;
            this.ctx.UserRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UserCreateBean> UpdateUsers(UserCreateBean filter)
        {
            var wrap = new GenericResponse<UserCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Username)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Password)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.RoleId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Pick(nameof(filter.Id)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = this.ctx.User.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "User tidak ditemukan dalam sistem";
                return wrap;
            }

            us.Email = filter.Email;
            us.Name = filter.Name;
            us.Username = filter.Username;
            this.ctx.User.Update(us);
            this.ctx.SaveChanges();


            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<User> DeleteUsers(UserWrapper filter)
        {
            GenericResponse<User> wrap = new GenericResponse<User>
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

            var us = this.ctx.User.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "User tidak ditemukan dalam sistem";
                return wrap;
            }

            //us.Status = 0;
            this.ctx.User.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            var l = new List<User>();
            l.Add(us);
            wrap.Data = l;

            return wrap;
        }


        public GenericResponse<MyProfileBean> GetMyProfile()
        {

            var wrap = new GenericResponse<MyProfileBean>
            {
                Status = false,
                Message = ""
            };

            var user = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (user == null)
            {
                return wrap;
            }

            var jq = this.ctx.User.Where(q => q.Id.Equals(user.Id)).Where(q => q.Status!.Name.Equals("AKTIF"));

            jq = jq.Include(i => i.Role).ThenInclude(t => t.RolePermission).ThenInclude(t => t.Permission);

            var data = jq.ToList();
            if (data == null || data.Count() < 1)
            {
                return wrap;
            }

            var us = data[0];

            var profile = mapper.Map<MyProfileBean>(us);
            profile.Menu = this.GenerateMenu(us);

            //var ub = this.ctx.UserBranch.Where(q => q.User!.Id!.Equals(us.Id)).FirstOrDefault();
            //if (ub != null)
            //{
            //    this.ctx.Entry(ub).Reference(r => r.Branch).Load();
            //    profile.Branch = ub.Branch!.Name;
            //}

           
            profile.UserBranch = new List<BranchResponseBean>();
            var tempUserBranch = this.ctx.UserBranch.Where(q => q.User!.Id!.Equals(us.Id)).ToList();

            if(tempUserBranch.Count() > 0)
            {
                if (us.ActiveBranchId == null)
                {
                    foreach (var it in tempUserBranch)
                    {
                        this.ctx.Entry(it).Reference(r => r.Branch).Load();
                    }

                    profile.Branch = tempUserBranch.FirstOrDefault().Branch;

                    var updateUser = this.ctx.User.FirstOrDefault(o => o.Id == us.Id);
                    updateUser.ActiveBranchId = profile.Branch.Id;
                    this.ctx.User.Update(updateUser);
                    this.ctx.SaveChanges();
                }
                else
                {
                    var temp = tempUserBranch.FirstOrDefault(o => o.BranchId == us.ActiveBranchId);
                    if (temp != null && temp.BranchId != null)
                    {
                        this.ctx.Entry(temp).Reference(r => r.Branch).Load();
                        profile.Branch = temp.Branch;
                    }

                }

                foreach (var it in tempUserBranch)
                {
                    this.ctx.Entry(it).Reference(r => r.Branch).Load();
                    var ubr = new BranchResponseBean();
                    ubr.Id = it!.Branch!.Id;
                    ubr.Name = it!.Branch!.Name;

                    profile.UserBranch.Add(ubr);
                }
            }

            var brip = this.ctx.RfGlobal.Where(q => q.Code!.Equals("BRKIP")).FirstOrDefault();
            if (brip != null)
            {
                profile.TelIp = brip.Val;
            }

            if (us.PassDevice == null)
            {
                var brsec = this.ctx.RfGlobal.Where(q => q.Code!.Equals("BRKSEC")).FirstOrDefault();
                if (brsec != null)
                {
                    profile.TelSecret = brsec.Val;
                }
            } 
            else
            {
                profile.TelSecret = us.PassDevice;
            }


            wrap.Status = true;
            wrap.AddData(profile);

            return wrap;
        }


        public ICollection<Menu> GenerateMenu(User us)
        {
            var menu = new List<Menu>();

            var menuHome = new Menu();
            menuHome.Title = "Home";
            menuHome.Icon = "HomeIcon";
            menuHome.Route = "dashboards";
            menu.Add(menuHome);

            if (us.Role!.Name!.Equals("ADMIN") || us.Role!.Name!.Equals("SUPERUSER") || us.Role!.Name!.Equals("ADMIN2") || us.Role!.Name!.Equals("SISKON") || us.Role!.Name!.Equals("SISKON2"))
            {
                var masterMgmt = new Menu();
                masterMgmt.Title = "Master Management";
                masterMgmt.Icon = "InboxIcon";

                if (us.Role!.Name!.Equals("SISKON") || us.Role!.Name!.Equals("SISKON2"))
                {
                    //User
                    var menuUser = new Menu();
                    menuUser.Title = "User";

                    var menuListUser = new Menu();
                    menuListUser.Title = "List User";
                    menuListUser.Route = "user/list";
                    menuUser.AddChildren(menuListUser);

                    if (us.Role!.Name!.Equals("SISKON"))
                    {
                        var menuListUserRequest = new Menu();
                        menuListUserRequest.Title = "List User Request";
                        menuListUserRequest.Route = "user/request/list";
                        menuUser.AddChildren(menuListUserRequest);
                    }




                    masterMgmt.AddChildren(menuUser);
                    menu.Add(masterMgmt);

                } 
                else
                {

                    //SMS Content
                    var menuAccountSMSContent = new Menu();
                    menuAccountSMSContent.Title = "SMS Content";

                    var menuAccountSMSContentList = new Menu();
                    menuAccountSMSContentList.Title = "List SMS Content";
                    menuAccountSMSContentList.Route = "smscontent/list";
                    menuAccountSMSContent.AddChildren(menuAccountSMSContentList);

                    if (us.Role!.Name!.Equals("SUPERUSER") || us.Role!.Name!.Equals("ADMIN2"))
                    {
                        var menuAccountSMSContentListRequest = new Menu();
                        menuAccountSMSContentListRequest.Title = "List SMS Content Request";
                        menuAccountSMSContentListRequest.Route = "smscontent/request/list";
                        menuAccountSMSContent.AddChildren(menuAccountSMSContentListRequest);
                    }


                    //Call Script
                    var menuAccountCallScript = new Menu();
                    menuAccountCallScript.Title = "Call Script";

                    var menuAccountCallScriptList = new Menu();
                    menuAccountCallScriptList.Title = "List Call Script";
                    menuAccountCallScriptList.Route = "callscript/list";
                    menuAccountCallScript.AddChildren(menuAccountCallScriptList);

                    if (us.Role!.Name!.Equals("SUPERUSER") || us.Role!.Name!.Equals("ADMIN2"))
                    {
                        var menuAccountCallScriptListRequest = new Menu();
                        menuAccountCallScriptListRequest.Title = "List Call Script Request";
                        menuAccountCallScriptListRequest.Route = "callscript/request/list";
                        menuAccountCallScript.AddChildren(menuAccountCallScriptListRequest);
                    }

                    //Reason
                    var menuReason = new Menu();
                    menuReason.Title = "Alasan";

                    var menuReasonList = new Menu();
                    menuReasonList.Title = "List Alasan";
                    menuReasonList.Route = "reason/list";
                    menuReason.AddChildren(menuReasonList);

                    if (us.Role!.Name!.Equals("SUPERUSER") || us.Role!.Name!.Equals("ADMIN2"))
                    {
                        var menuReasonRequest = new Menu();
                        menuReasonRequest.Title = "List Alasan Request";
                        menuReasonRequest.Route = "reason/request/list";
                        menuReason.AddChildren(menuReasonRequest);
                    }


                    var menuReport = new Menu();
                    menuReport.Title = "Report";
                    menuReport.Route = "monitor/admin";
                    menuReport.Icon = "MonitorIcon";

                    masterMgmt.AddChildren(menuAccountSMSContent);
                    masterMgmt.AddChildren(menuAccountCallScript);
                    masterMgmt.AddChildren(menuReason);
                    menu.Add(masterMgmt);

                    if (us.Role!.Name!.Equals("SUPERUSER") || us.Role!.Name!.Equals("ADMIN") || us.Role!.Name!.Equals("ADMIN2"))
                    {

                        var dist = new Menu();
                        dist.Title = "Distribusi";
                        dist.Icon = "NavigationIcon";

                        var distList = new Menu();
                        distList.Title = "List Rule";
                        distList.Route = "distribution/rules";
                        dist.AddChildren(distList);

                        var bucketList = new Menu();
                        bucketList.Title = "List Bucket";
                        bucketList.Route = "bucket_lists";
                        dist.AddChildren(bucketList);

                        menu.Add(dist);

                        var genParam = new Menu();
                        genParam.Title = "General Parameter";
                        genParam.Icon = "SettingsIcon";
                        menu.Add(genParam);

                        var branchList = new Menu();
                        branchList.Title = "List Cabang";
                        branchList.Route = "param/branch";
                        genParam.AddChildren(branchList);

                        var settingList = new Menu();
                        settingList.Title = "Setting";
                        settingList.Route = "param/setting";
                        genParam.AddChildren(settingList);

                        var settingDc = new Menu();
                        settingDc.Title = "Setting DC";
                        settingDc.Route = "param/setting/dc";
                        genParam.AddChildren(settingDc);

                        var settingFc = new Menu();
                        settingFc.Title = "Setting FC";
                        settingFc.Route = "param/setting/fc";
                        genParam.AddChildren(settingFc);
                    }

                    var menuMenu = new Menu();
                    menuMenu.Title = "Menu Management";
                    menuMenu.Route = "master/menu-management";
                    masterMgmt.AddChildren(menuMenu);

                    var menuDoc = new Menu();
                    menuDoc.Title = "Document";
                    menuDoc.Route = "master/document";
                    masterMgmt.AddChildren(menuDoc);


                }

            }

            if (us.Role!.Name!.Equals("CABANG"))
            {
                var menuReport = new Menu();
                menuReport.Title = "Report";
                //menuReport.Route = "monitor/cabang";
                menuReport.Icon = "MonitorIcon";

                var menuListNasabahCbg = new Menu();
                menuListNasabahCbg.Title = "Aktivitas";
                menuListNasabahCbg.Route = "monitor/cabang";
                menuReport.AddChildren(menuListNasabahCbg);

                var menuListNasabahFU = new Menu();
                menuListNasabahFU.Title = "Performance";
                menuListNasabahFU.Route = "monitor/cabang/performance";
                menuReport.AddChildren(menuListNasabahFU);

                var menuListNasabahNonFu = new Menu();
                menuListNasabahNonFu.Title = "Belum Follow Up";
                menuListNasabahNonFu.Route = "monitor/cabang/nonfollowup";
                //menuReport.AddChildren(menuListNasabahNonFu);

                //Generate Letter
                var generateLetterMenu = new Menu();
                generateLetterMenu.Title = "Generate Letter";
                generateLetterMenu.Icon = "BookmarkIcon";

                var generateLetterList = new Menu();
                generateLetterList.Title = "Generate Letter";
                generateLetterList.Route = "collection/generate-letter";
                generateLetterMenu.AddChildren(generateLetterList);

                var generateLetterHistory = new Menu();
                generateLetterHistory.Title = "Generate Letter History";
                generateLetterHistory.Route = "collection/generate-letter-history";
                generateLetterMenu.AddChildren(generateLetterHistory);

                menu.Add(generateLetterMenu);

                menu.Add(menuReport);
            }

            if (us.Role!.Name!.Equals("MANAJEMEN") || us.Role!.Name!.Equals("SUPERUSER") || us.Role!.Name!.Equals("ADMIN2") || us.Role!.Name!.Equals("ADMIN"))
            {

                //Generate Letter
                var generateLetterMenu = new Menu();
                generateLetterMenu.Title = "Generate Letter";
                generateLetterMenu.Icon = "BookmarkIcon";

                var generateLetterHistory = new Menu();
                generateLetterHistory.Title = "Generate Letter History";
                generateLetterHistory.Route = "collection/generate-letter-history";
                generateLetterMenu.AddChildren(generateLetterHistory);

                menu.Add(generateLetterMenu);

                var menuReport = new Menu();
                menuReport.Title = "Report";
                menuReport.Route = "monitor/admin";
                menuReport.Icon = "MonitorIcon";


                var menuListNasabahCbg = new Menu();
                menuListNasabahCbg.Title = "Distribusi";
                menuListNasabahCbg.Route = "monitor/dc/admin/distribusi";
                //menuReport.AddChildren(menuListNasabahCbg);

                var menuListNasabahFU = new Menu();
                menuListNasabahFU.Title = "Follow Up";
                menuListNasabahFU.Route = "monitor/dc/admin/followup";
                //menuReport.AddChildren(menuListNasabahFU);

                var menuListNasabahNonFu = new Menu();
                menuListNasabahNonFu.Title = "Belum Follow Up";
                menuListNasabahNonFu.Route = "monitor/admin/nonfollowup";
                //menuReport.AddChildren(menuListNasabahNonFu);

                var traceMenu = new Menu();
                traceMenu.Title = "Distribusi History";
                traceMenu.Route = "monitor/trace";
                menuReport.AddChildren(traceMenu);

                var payRecord = new Menu();
                payRecord.Title = "Pembayaran Record";
                payRecord.Route = "monitor/payrecord";
                menuReport.AddChildren(payRecord);

                var menuRp = new Menu();
                menuRp.Title = "Pelunasan";
                menuRp.Route = "report/payment";
                menuReport.AddChildren(menuRp);


                var geo = new Menu();
                geo.Title = "Data Geolocation";
                geo.Route = "monitor/geo";
                menuReport.AddChildren(geo);

                var daily = new Menu();
                daily.Title = "Data Nasabah Baru";
                daily.Route = "monitor/newdaily";
                menuReport.AddChildren(daily);

                menu.Add(menuReport);

                var menuTeam = new Menu();
                menuTeam.Title = "Struktur Team";
                menuTeam.Icon = "UsersIcon";

                var menuTeamFC = new Menu();
                menuTeamFC.Title = "Team FC";
                menuTeamFC.Route = "user/team/spvfc";
                menuTeam.AddChildren(menuTeamFC);

                var menuTeamCabang = new Menu();
                menuTeamCabang.Title = "Cabang FC";
                menuTeamCabang.Route = "user/team/branchfc";
                menuTeam.AddChildren(menuTeamCabang);

                menu.Add(menuTeam);

                
                var menuRecovery = new Menu();
                menuRecovery.Title = "Recovery";
                menuRecovery.Icon = "ToolIcon";

                var menuRestruk = new Menu();
                menuRestruk.Title = "Restruktur";
                //menuRestruk.Route = "recovery/restruktur";

                var menuRestrukMon = new Menu();
                menuRestrukMon.Title = "Monitoring Restruktur";
                menuRestrukMon.Route = "recovery/restruktur/monitoring/list";
                menuRestruk.AddChildren(menuRestrukMon);

                var menuRestrukReq = new Menu();
                menuRestrukReq.Title = "Tugas Saya";
                menuRestrukReq.Route = "recovery/restruktur/pengajuan/list";
                menuRestruk.AddChildren(menuRestrukReq);


                menuRecovery.AddChildren(menuRestruk);

                var menuLelang= new Menu();
                menuLelang.Title = "Lelang";
                //menuLelang.Route = "recovery/lelang";

                var menuLelangMon = new Menu();
                menuLelangMon.Title = "Monitoring Lelang";
                menuLelangMon.Route = "recovery/lelang/monitoring/list";
                menuLelang.AddChildren(menuLelangMon);

                var menuLelangReq = new Menu();
                menuLelangReq.Title = "Tugas Saya";
                menuLelangReq.Route = "recovery/lelang/pengajuan/list";
                menuLelang.AddChildren(menuLelangReq);


                menuRecovery.AddChildren(menuLelang);

                var menuAsuransi = new Menu();
                menuAsuransi.Title = "Asuransi";
                //menuAsuransi.Route = "recovery/asuransi";
                //menuRecovery.AddChildren(menuAsuransi);

                var menuAsuransiMon = new Menu();
                menuAsuransiMon.Title = "Monitoring Asuransi";
                menuAsuransiMon.Route = "recovery/asuransi/monitoring/list";
                menuAsuransi.AddChildren(menuAsuransiMon);

                var menuAsuransiReq = new Menu();
                menuAsuransiReq.Title = "Tugas Saya";
                menuAsuransiReq.Route = "recovery/asuransi/pengajuan/list";
                menuAsuransi.AddChildren(menuAsuransiReq);

                menuRecovery.AddChildren(menuAsuransi);

                var menuAyda = new Menu();
                menuAyda.Title = "AYDA";
                var menuAydaMon = new Menu();
                menuAydaMon.Title = "Monitoring AYDA";
                menuAydaMon.Route = "recovery/ayda/monitoring/list";
                menuAyda.AddChildren(menuAydaMon);

                var menuAydaMonReq = new Menu();
                menuAydaMonReq.Title = "Tugas Saya";
                menuAydaMonReq.Route = "recovery/ayda/pengajuan/list";
                menuAyda.AddChildren(menuAydaMonReq);

                menuRecovery.AddChildren(menuAyda);

                var menuWo = new Menu();
                menuWo.Title = "Write Off";
                var menuWoMon = new Menu();
                menuWoMon.Title = "Monitoring Write Off";
                menuWoMon.Route = "recovery/writeoff/monitoring/list";
                menuWo.AddChildren(menuWoMon);

                var menuWoMonReq = new Menu();
                menuWoMonReq.Title = "Tugas Saya";
                menuWoMonReq.Route = "recovery/writeoff/pengajuan/list";
                menuWo.AddChildren(menuWoMonReq);

                menuRecovery.AddChildren(menuWo);

                menu.Add(menuRecovery);
            }

            if (us.Role!.Name!.Equals("DC") || us.Role!.Name!.Equals("SPVDC"))
            {
                var deskCallMenu = new Menu();
                deskCallMenu.Title = "Desk Call Management";
                deskCallMenu.Icon = "UsersIcon";

                var menuInputHasilCall = new Menu();
                menuInputHasilCall.Title = "Tasklist Collection";
                menuInputHasilCall.Route = "hasilcall/list";
                menuInputHasilCall.Icon = "PhoneIcon";


                if (us.Role.Name.Equals("SPVDC"))
                {
                    var menuMonitorDC = new Menu();
                    menuMonitorDC.Title = "Report SPV DC";
                    //menuMonitorDC.Route = "monitor/dc/spv";
                    menuMonitorDC.Icon = "MonitorIcon";

                    var menuListNasabahSPVDC = new Menu();
                    menuListNasabahSPVDC.Title = "Aktivitas";
                    menuListNasabahSPVDC.Route = "monitor/dc/spv";
                    menuMonitorDC.AddChildren(menuListNasabahSPVDC);

                    var menuListNasabahFSPVDC = new Menu();
                    menuListNasabahFSPVDC.Title = "Performance";
                    menuListNasabahFSPVDC.Route = "monitor/dc/spv/performance";
                    menuMonitorDC.AddChildren(menuListNasabahFSPVDC);

                    var menuListNasabahNonFuSPVDC = new Menu();
                    menuListNasabahNonFuSPVDC.Title = "Belum Follow Up";
                    menuListNasabahNonFuSPVDC.Route = "monitor/dc/spv/nonfollowup";
                    //menuMonitorDC.AddChildren(menuListNasabahNonFuSPVDC);

                    var dist = new Menu();
                    dist.Title = "Distribusi";
                    dist.Icon = "NavigationIcon";

                    var menuReassignDC = new Menu();
                    menuReassignDC.Title = "Reassign DC";
                    menuReassignDC.Route = "reassign/dc/list";

                    dist.AddChildren(menuReassignDC);

                    menu.Add(dist);

                    menu.Add(menuMonitorDC);
                }

                if (us.Role.Name.Equals("DC"))
                {
                    var menuMonitorDC = new Menu();
                    menuMonitorDC.Title = "Report DC";
                    menuMonitorDC.Route = "monitor/dc";
                    menuMonitorDC.Icon = "MonitorIcon";

                    var menuListNasabahDC = new Menu();
                    menuListNasabahDC.Title = "Distribusi";
                    menuListNasabahDC.Route = "monitor/dc/distribusi";
                    //menuMonitorDC.AddChildren(menuListNasabahDC);

                    var menuListNasabahFuDC = new Menu();
                    menuListNasabahFuDC.Title = "Follow Up";
                    menuListNasabahFuDC.Route = "monitor/dc/followup";
                    //menuMonitorDC.AddChildren(menuListNasabahFuDC);

                    var menuListNasabahNonFuDC = new Menu();
                    menuListNasabahNonFuDC.Title = "Belum Follow Up";
                    menuListNasabahNonFuDC.Route = "monitor/dc/nonfollowup";
                    //menuMonitorDC.AddChildren(menuListNasabahNonFuDC);


                    menu.Add(menuInputHasilCall);
                    menu.Add(menuMonitorDC);
                }

            }


            if (us.Role!.Name!.Equals("FC") || us.Role!.Name!.Equals("SPVFC"))
            {
                var fcMenu = new Menu();
                fcMenu.Title = "Field Coll Management";
                fcMenu.Icon = "UsersIcon";

                var menuInputHasilCall = new Menu();
                menuInputHasilCall.Title = "Tasks - Individu";
                menuInputHasilCall.Route = "hasilcallfc/list";

                

                if (us.Role.Name.Equals("SPVFC"))
                {
                    var menuMonitorFC = new Menu();
                    menuMonitorFC.Title = "Report SPV FC";
                    //menuMonitorFC.Route = "monitor/fc/spv";
                    menuMonitorFC.Icon = "MonitorIcon";

                    var menuListNasabahSPVFC = new Menu();
                    menuListNasabahSPVFC.Title = "Aktivitas";
                    menuListNasabahSPVFC.Route = "monitor/fc/spv";
                    menuMonitorFC.AddChildren(menuListNasabahSPVFC);

                    var menuListNasabahFSPVFC = new Menu();
                    menuListNasabahFSPVFC.Title = "Performance";
                    menuListNasabahFSPVFC.Route = "monitor/fc/spv/performance";
                    menuMonitorFC.AddChildren(menuListNasabahFSPVFC);

                    var menuListNasabahNonFuSPVFC = new Menu();
                    menuListNasabahNonFuSPVFC.Title = "Belum Follow Up";
                    menuListNasabahNonFuSPVFC.Route = "monitor/fc/spv/nonfollowup";
                    //menuMonitorFC.AddChildren(menuListNasabahNonFuSPVFC);

                    var menuTrackingFC = new Menu();
                    menuTrackingFC.Title = "Tracking FC";
                    menuTrackingFC.Route = "tracking-fc";
                    menuMonitorFC.AddChildren(menuTrackingFC);

                    var dist = new Menu();
                    dist.Title = "Distribusi";
                    dist.Icon = "NavigationIcon";


                    var menuReassignFC = new Menu();
                    menuReassignFC.Title = "Reassign FC";
                    menuReassignFC.Route = "reassign/fc/list";

                    dist.AddChildren(menuReassignFC);

                    menu.Add(dist);
                    menu.Add(menuMonitorFC);
                }

                if (us.Role.Name.Equals("FC"))
                {
                    var menuMonitorFC = new Menu();
                    menuMonitorFC.Title = "Report FC";
                    menuMonitorFC.Route = "monitor/fc";
                    menuMonitorFC.Icon = "MonitorIcon";

                    var menuListNasabahFC = new Menu();
                    menuListNasabahFC.Title = "Distribusi";
                    menuListNasabahFC.Route = "monitor/fc/distribusi";
                    //menuMonitorFC.AddChildren(menuListNasabahFC);

                    var menuListNasabahFuFC = new Menu();
                    menuListNasabahFuFC.Title = "Follow Up";
                    menuListNasabahFuFC.Route = "monitor/fc/followup";
                    //menuMonitorFC.AddChildren(menuListNasabahFuFC);

                    var menuListNasabahNonFuFC = new Menu();
                    menuListNasabahNonFuFC.Title = "Belum Follow Up";
                    menuListNasabahNonFuFC.Route = "monitor/fc/nonfollowup";
                    //menuMonitorFC.AddChildren(menuListNasabahNonFuFC);

                    menu.Add(menuInputHasilCall);
                    menu.Add(menuMonitorFC);
                }

            }

            var menuPass = new Menu();
            menuPass.Title = "Ganti Password";
            menuPass.Icon = "HomeIcon";
            menuPass.Route = "mypass";
            menu.Add(menuPass);

            return menu;
        }


        public GenericResponse<RoleResponseBean> ListRole(RoleListRequestBean filter)
        {
            var wrap = new GenericResponse<RoleResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<Role> q = this.ctx.Set<Role>().Include(i => i.RolePermission!).ThenInclude(i => i.Permission).Include(i => i.Status);
            if (filter.Id != null && filter.Id > 0)
            {
                q = q.Where(p => p.Id.Equals(filter.Id));
            }
            //if (filter.Keyword != null && filter.Keyword.Length > 0)
            //{
            //    q = q.Where(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
            //}
            //var cnt = q.Count();
            //wrap.DataCount = cnt;
            //wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            //var pagination = this.pagination.getPagination(filter);
            var data = q.OrderByDescending(q => q.Id).ToList();

            var ldata = new List<RoleResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<RoleResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }


        public GenericResponse<RoleResponseBean> DetailRole(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<RoleResponseBean>
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

            var obj = this.ctx.Role.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Role tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();

            if (obj.Status!.Name != "AKTIF")
            {
                wrap.Message = "Role tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<RoleResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<RoleCreateBean> SaveRole(RoleCreateBean filter)
        {
            var wrap = new GenericResponse<RoleCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            //if (filter.Permission == null || filter.Permission.Count() < 1)
            //{
            //    wrap.Message = "Permission adalah wajib";
            //    return wrap;
            //}

            var us = mapper.Map<Role>(filter);
            this.ctx.Role.Add(us);
            this.ctx.SaveChanges();

            //foreach (var i in filter.Permission)
            //{
            //    var rp = new RolePermission();
            //    rp.RoleId = us.Id;
            //    rp.PermissionId = i;
            //    this.ctx.RolePermission.Add(rp);
            //    this.ctx.SaveChanges();
            //}

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        //public GenericResponse<RoleReqResponseBean> ListRoleRequest(RoleReqListRequestBean filter)
        //{
        //    var wrap = new GenericResponse<RoleReqResponseBean>
        //    {
        //        Status = false,
        //        Message = ""
        //    };

        //    IQueryable<RoleRequest> q = this.ctx.Set<RoleRequest>().Include(i => i.Role).Include(i => i.RolePermissionRequest).Include(i => i.Status);
        //    //if (filter.Keyword != null && filter.Keyword.Length > 0)
        //    //{
        //    //    var predicatek = PredicateBuilder.New<RoleRequest>();
        //    //    predicatek = predicatek.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
        //    //    q = q.Where(predicatek);
        //    //}

        //    var predicate = PredicateBuilder.New<RoleRequest>();
        //    predicate = predicate.Or(o => o.Status!.Name!.Equals("DRAFT"));
        //    predicate = predicate.Or(o => o.Status!.Name!.Equals("APPROVE"));
        //    q = q.Where(predicate);
        //    q = q.OrderByDescending(q => q.Id);

        //    //var cnt = q.Count();
        //    //wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

        //    //var pagination = this.pagination.getPagination(filter);
        //    var data = q.ToList();

        //    var ldata = new List<RoleReqResponseBean>();
        //    foreach (var it in data)
        //    {
        //        var dto = mapper.Map<RoleReqResponseBean>(it);
        //        ldata.Add(dto);
        //    }

        //    wrap.Data = ldata;

        //    wrap.Status = true;

        //    return wrap;
        //}

        public GenericResponse<DashboardWrapper> Dashboard(DashboardBeanV2 filter)
        {
            var wrap = new GenericResponse<DashboardWrapper>
            {
                Status = true,
                Message = ""
            };

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }
            if (reqUser.Role!.Name!.Equals("DC") || reqUser.Role!.Name!.Equals("FC"))
            {
                wrap.AddData(this.DashboardDCFC(reqUser, filter));
            }
            else if (reqUser.Role!.Name!.Equals("SPVFC") || reqUser.Role!.Name!.Equals("SPVDC"))
            {
                wrap.AddData(this.DashboardSPV(reqUser, filter));
            }
            else if (reqUser.Role!.Name!.Equals("CABANG"))
            {
                wrap.AddData(this.DashboardCabang(reqUser, filter));
            }
            else
            {
                wrap.AddData(this.DashboardAll(reqUser, filter));
            }


            return wrap;
        }

        private DashboardWrapper DashboardDCFC(User user, DashboardBeanV2 filter)
        {
            var w = new DashboardWrapper();
            var now = DateTime.Now;

            if (filter.PeriodId == 1)
            {
                var today = from b in this.ctx.CollectionTrace
                            where b.CallById == user.Id
                            where b.TraceDate!.Value.Date == now.Date
                            group b by b.Result!.Code into g
                            select new
                            {
                                Key = g.Key,
                                Num = g.Count()
                            };

                var result = today.ToList();
                var dtd = new DashboardBean();
                foreach (var i in result)
                {

                    if (i.Key.ToLower().Equals("fuim"))
                    {
                        dtd.Fuim = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("ll"))
                    {
                        dtd.Ll = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("mess"))
                    {
                        dtd.Mess = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas"))
                    {
                        dtd.Noas = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas1"))
                    {
                        dtd.Noas1 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas2"))
                    {
                        dtd.Noas2 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas3"))
                    {
                        dtd.Noas3 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("pay"))
                    {
                        dtd.Pay = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("ptp"))
                    {
                        dtd.Ptp = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("vt"))
                    {
                        dtd.Vt = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("re"))
                    {
                        dtd.Rem = i.Num;
                    }
                }
                w.Data = dtd;
            }


            DateTime mondayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);
            DateTime sundayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 6);

            if (filter.PeriodId == 2)
            {
                var thisWeek = from b in this.ctx.CollectionTrace
                               where b.CallById == user.Id
                               where b.TraceDate!.Value.Date >= mondayDate.Date
                               where b.TraceDate!.Value.Date <= sundayDate.Date
                               group b by b.Result!.Code into g
                               select new
                               {
                                   Key = g.Key,
                                   Num = g.Count()
                               };

                var result = thisWeek.ToList();
                var dtw = new DashboardBean();
                foreach (var i in result)
                {

                    if (i.Key.ToLower().Equals("fuim"))
                    {
                        dtw.Fuim = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("ll"))
                    {
                        dtw.Ll = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("mess"))
                    {
                        dtw.Mess = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas"))
                    {
                        dtw.Noas = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas1"))
                    {
                        dtw.Noas1 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas2"))
                    {
                        dtw.Noas2 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas3"))
                    {
                        dtw.Noas3 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("pay"))
                    {
                        dtw.Pay = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("ptp"))
                    {
                        dtw.Ptp = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("vt"))
                    {
                        dtw.Vt = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("re"))
                    {
                        dtw.Rem = i.Num;
                    }
                }

                w.Data = dtw;
            }

            if (filter.PeriodId == 3)
            {
                var firstDate = new DateTime(now.Year, now.Month, 1);
                var LastDate = firstDate.AddMonths(1).AddDays(-1);

                var thisMonth = from b in this.ctx.CollectionTrace
                                where b.CallById == user.Id
                                where b.TraceDate!.Value.Date >= mondayDate.Date
                                where b.TraceDate!.Value.Date <= sundayDate.Date
                                group b by b.Result!.Code into g
                                select new
                                {
                                    Key = g.Key,
                                    Num = g.Count()
                                };

                var result = thisMonth.ToList();
                var dtm = new DashboardBean();
                foreach (var i in result)
                {

                    if (i.Key.ToLower().Equals("fuim"))
                    {
                        dtm.Fuim = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("ll"))
                    {
                        dtm.Ll = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("mess"))
                    {
                        dtm.Mess = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas"))
                    {
                        dtm.Noas = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas1"))
                    {
                        dtm.Noas1 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas2"))
                    {
                        dtm.Noas2 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas3"))
                    {
                        dtm.Noas3 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("pay"))
                    {
                        dtm.Pay = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("ptp"))
                    {
                        dtm.Ptp = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("vt"))
                    {
                        dtm.Vt = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("re"))
                    {
                        dtm.Rem = i.Num;
                    }
                }

                w.Data = dtm;
            }


            Console.WriteLine(mondayDate.ToString());
            Console.WriteLine(sundayDate.ToString());


            return w;
        }


        private DashboardWrapper DashboardSPV(User user, DashboardBeanV2 filter)
        {
            var w = new DashboardWrapper();
            var now = DateTime.Now;
            var today = from b in this.ctx.CollectionTrace
                        join xx in this.ctx.TeamMember on b.CallById equals xx.MemberId
                        where xx.Team!.SpvId == user.Id
                        where b.TraceDate!.Value.Date == now.Date
                        group b by b.Result!.Code into g
                        select new
                        {
                            Key = g.Key,
                            Num = g.Count()
                        };

            var result = today.ToList();
            var dtd = new DashboardBean();
            foreach (var i in result)
            {

                if (i.Key.ToLower().Equals("fuim"))
                {
                    dtd.Fuim = i.Num;
                }
                else if (i.Key.ToLower().Equals("ll"))
                {
                    dtd.Ll = i.Num;
                }
                else if (i.Key.ToLower().Equals("mess"))
                {
                    dtd.Mess = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas"))
                {
                    dtd.Noas = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas1"))
                {
                    dtd.Noas1 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas2"))
                {
                    dtd.Noas2 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas3"))
                {
                    dtd.Noas3 = i.Num;
                }
                else if (i.Key.ToLower().Equals("pay"))
                {
                    dtd.Pay = i.Num;
                }
                else if (i.Key.ToLower().Equals("ptp"))
                {
                    dtd.Ptp = i.Num;
                }
                else if (i.Key.ToLower().Equals("vt"))
                {
                    dtd.Vt = i.Num;
                }
                else if (i.Key.ToLower().Equals("re"))
                {
                    dtd.Rem = i.Num;
                }
            }

            w.Data = dtd;



            DateTime mondayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);
            DateTime sundayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 6);

            var thisWeek = from b in this.ctx.CollectionTrace
                           join xx in this.ctx.TeamMember on b.CallById equals xx.MemberId
                           where xx.Team!.SpvId == user.Id
                           where b.TraceDate!.Value.Date >= mondayDate.Date
                           where b.TraceDate!.Value.Date <= sundayDate.Date
                           group b by b.Result!.Code into g
                           select new
                           {
                               Key = g.Key,
                               Num = g.Count()
                           };

            result = thisWeek.ToList();
            var dtw = new DashboardBean();
            foreach (var i in result)
            {

                if (i.Key.ToLower().Equals("fuim"))
                {
                    dtw.Fuim = i.Num;
                }
                else if (i.Key.ToLower().Equals("ll"))
                {
                    dtw.Ll = i.Num;
                }
                else if (i.Key.ToLower().Equals("mess"))
                {
                    dtw.Mess = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas"))
                {
                    dtw.Noas = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas1"))
                {
                    dtw.Noas1 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas2"))
                {
                    dtw.Noas2 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas3"))
                {
                    dtw.Noas3 = i.Num;
                }
                else if (i.Key.ToLower().Equals("pay"))
                {
                    dtw.Pay = i.Num;
                }
                else if (i.Key.ToLower().Equals("ptp"))
                {
                    dtw.Ptp = i.Num;
                }
                else if (i.Key.ToLower().Equals("vt"))
                {
                    dtw.Vt = i.Num;
                }
                else if (i.Key.ToLower().Equals("re"))
                {
                    dtw.Rem = i.Num;
                }
            }

            w.Data = dtw;

            var firstDate = new DateTime(now.Year, now.Month, 1);
            var LastDate = firstDate.AddMonths(1).AddDays(-1);

            var thisMonth = from b in this.ctx.CollectionTrace
                            join xx in this.ctx.TeamMember on b.CallById equals xx.MemberId
                            where xx.Team!.SpvId == user.Id
                            where b.TraceDate!.Value.Date >= mondayDate.Date
                            where b.TraceDate!.Value.Date <= sundayDate.Date
                            group b by b.Result!.Code into g
                            select new
                            {
                                Key = g.Key,
                                Num = g.Count()
                            };

            result = thisMonth.ToList();
            var dtm = new DashboardBean();
            foreach (var i in result)
            {

                if (i.Key.ToLower().Equals("fuim"))
                {
                    dtm.Fuim = i.Num;
                }
                else if (i.Key.ToLower().Equals("ll"))
                {
                    dtm.Ll = i.Num;
                }
                else if (i.Key.ToLower().Equals("mess"))
                {
                    dtm.Mess = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas"))
                {
                    dtm.Noas = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas1"))
                {
                    dtm.Noas1 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas2"))
                {
                    dtm.Noas2 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas3"))
                {
                    dtm.Noas3 = i.Num;
                }
                else if (i.Key.ToLower().Equals("pay"))
                {
                    dtm.Pay = i.Num;
                }
                else if (i.Key.ToLower().Equals("ptp"))
                {
                    dtm.Ptp = i.Num;
                }
                else if (i.Key.ToLower().Equals("vt"))
                {
                    dtm.Vt = i.Num;
                }
                else if (i.Key.ToLower().Equals("re"))
                {
                    dtm.Rem = i.Num;
                }
            }

            w.Data = dtm;

            Console.WriteLine(mondayDate.ToString());
            Console.WriteLine(sundayDate.ToString());


            return w;
        }


        private DashboardWrapper DashboardCabang(User user, DashboardBeanV2 filter)
        {
            var w = new DashboardWrapper();
            var now = DateTime.Now;

            var today = from b in this.ctx.CollectionTrace
                        join xx in this.ctx.UserBranch on b.CallById equals xx.UserId
                        where xx.BranchId! == user.ActiveBranchId
                        where b.TraceDate!.Value.Date == now.Date
                        group b by b.Result!.Code into g
                        select new
                        {
                            Key = g.Key,
                            Num = g.Count()
                        };

            var result = today.ToList();
            var dtd = new DashboardBean();
            foreach (var i in result)
            {

                if (i.Key.ToLower().Equals("fuim"))
                {
                    dtd.Fuim = i.Num;
                }
                else if (i.Key.ToLower().Equals("ll"))
                {
                    dtd.Ll = i.Num;
                }
                else if (i.Key.ToLower().Equals("mess"))
                {
                    dtd.Mess = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas"))
                {
                    dtd.Noas = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas1"))
                {
                    dtd.Noas1 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas2"))
                {
                    dtd.Noas2 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas3"))
                {
                    dtd.Noas3 = i.Num;
                }
                else if (i.Key.ToLower().Equals("pay"))
                {
                    dtd.Pay = i.Num;
                }
                else if (i.Key.ToLower().Equals("ptp"))
                {
                    dtd.Ptp = i.Num;
                }
                else if (i.Key.ToLower().Equals("vt"))
                {
                    dtd.Vt = i.Num;
                }
                else if (i.Key.ToLower().Equals("re"))
                {
                    dtd.Rem = i.Num;
                }
            }

            w.Data = dtd;



            DateTime mondayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);
            DateTime sundayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 6);

            var thisWeek = from b in this.ctx.CollectionTrace
                           join xx in this.ctx.UserBranch on b.CallById equals xx.UserId
                           where xx.BranchId! == user.ActiveBranchId
                           where b.TraceDate!.Value.Date >= mondayDate.Date
                           where b.TraceDate!.Value.Date <= sundayDate.Date
                           group b by b.Result!.Code into g
                           select new
                           {
                               Key = g.Key,
                               Num = g.Count()
                           };

            result = thisWeek.ToList();
            var dtw = new DashboardBean();
            foreach (var i in result)
            {

                if (i.Key.ToLower().Equals("fuim"))
                {
                    dtw.Fuim = i.Num;
                }
                else if (i.Key.ToLower().Equals("ll"))
                {
                    dtw.Ll = i.Num;
                }
                else if (i.Key.ToLower().Equals("mess"))
                {
                    dtw.Mess = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas"))
                {
                    dtw.Noas = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas1"))
                {
                    dtw.Noas1 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas2"))
                {
                    dtw.Noas2 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas3"))
                {
                    dtw.Noas3 = i.Num;
                }
                else if (i.Key.ToLower().Equals("pay"))
                {
                    dtw.Pay = i.Num;
                }
                else if (i.Key.ToLower().Equals("ptp"))
                {
                    dtw.Ptp = i.Num;
                }
                else if (i.Key.ToLower().Equals("vt"))
                {
                    dtw.Vt = i.Num;
                }
                else if (i.Key.ToLower().Equals("re"))
                {
                    dtw.Rem = i.Num;
                }
            }

            w.Data = dtw;

            var firstDate = new DateTime(now.Year, now.Month, 1);
            var LastDate = firstDate.AddMonths(1).AddDays(-1);

            var thisMonth = from b in this.ctx.CollectionTrace
                            join xx in this.ctx.UserBranch on b.CallById equals xx.UserId
                            where xx.BranchId! == user.ActiveBranchId
                            where b.TraceDate!.Value.Date >= mondayDate.Date
                            where b.TraceDate!.Value.Date <= sundayDate.Date
                            group b by b.Result!.Code into g
                            select new
                            {
                                Key = g.Key,
                                Num = g.Count()
                            };

            result = thisMonth.ToList();
            var dtm = new DashboardBean();
            foreach (var i in result)
            {

                if (i.Key.ToLower().Equals("fuim"))
                {
                    dtm.Fuim = i.Num;
                }
                else if (i.Key.ToLower().Equals("ll"))
                {
                    dtm.Ll = i.Num;
                }
                else if (i.Key.ToLower().Equals("mess"))
                {
                    dtm.Mess = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas"))
                {
                    dtm.Noas = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas1"))
                {
                    dtm.Noas1 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas2"))
                {
                    dtm.Noas2 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas3"))
                {
                    dtm.Noas3 = i.Num;
                }
                else if (i.Key.ToLower().Equals("pay"))
                {
                    dtm.Pay = i.Num;
                }
                else if (i.Key.ToLower().Equals("ptp"))
                {
                    dtm.Ptp = i.Num;
                }
                else if (i.Key.ToLower().Equals("vt"))
                {
                    dtm.Vt = i.Num;
                }
                else if (i.Key.ToLower().Equals("re"))
                {
                    dtm.Rem = i.Num;
                }
            }

            w.Data = dtm;

            Console.WriteLine(mondayDate.ToString());
            Console.WriteLine(sundayDate.ToString());


            return w;
        }

        private DashboardWrapper DashboardAll(User user, DashboardBeanV2 filter)
        {
            var w = new DashboardWrapper();
            var now = DateTime.Now;
            DateTime mondayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);
            DateTime sundayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 6);
            var firstDate = new DateTime(now.Year, now.Month, 1);
            var LastDate = firstDate.AddMonths(1).AddDays(-1);

            /*
            var today = from b in this.ctx.CollectionHistory
                        where b.HistoryDate!.Value.Date == now.Date
                        group b by b.Result!.Code into g
                        select new
                        {
                            Key = g.Key,
                            Num = g.Count()
                        };
            */

            var today = this.ctx.CollectionTrace.Where(q => q.CallId != null);

            if (filter.PeriodId != null)
            {
                if (filter.PeriodId == 1)
                {
                    today = today.Where(q => q.TraceDate! == now.Date);
                }
                else if (filter.PeriodId == 2)
                {
                    today = today.Where(q => q.TraceDate! >= mondayDate.Date).Where(q => q.TraceDate! <= sundayDate.Date);
                }
                else if (filter.PeriodId == 3)
                {
                    today = today.Where(q => q.TraceDate! >= firstDate.Date).Where(q => q.TraceDate! <= LastDate.Date);
                }
            }

            if (filter != null)
            {
                if (filter.BranchId != null)
                {
                    today = today.Where(q => q.Call!.BranchId == filter.BranchId);
                }

                if (filter.SpvId != null)
                {
                    var team = from b in this.ctx.Team
                               join x in this.ctx.TeamMember on b.Id equals x.TeamId
                               where b.SpvId == filter.SpvId
                               select x.Member!.Id;
                    var tmp = team.ToList();

                    if (filter.AgentId != null)
                    {
                        today = today.Where(q => q.CallById == filter.AgentId);
                    }
                    else
                    {
                        today = today.Where(q => tmp.Contains(q.CallById));
                    }

                }
            }

            var today1 = today.GroupBy(g => g.Result!.Code).Select(s => new { Key = s.Key, Num = s.Count() });

            var result = today1.ToList();
            var dtd = new DashboardBean();
            foreach (var i in result)
            {

                if (i.Key.ToLower().Equals("fuim"))
                {
                    dtd.Fuim = i.Num;
                }
                else if (i.Key.ToLower().Equals("ll"))
                {
                    dtd.Ll = i.Num;
                }
                else if (i.Key.ToLower().Equals("mess"))
                {
                    dtd.Mess = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas"))
                {
                    dtd.Noas = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas1"))
                {
                    dtd.Noas1 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas2"))
                {
                    dtd.Noas2 = i.Num;
                }
                else if (i.Key.ToLower().Equals("noas3"))
                {
                    dtd.Noas3 = i.Num;
                }
                else if (i.Key.ToLower().Equals("pay"))
                {
                    dtd.Pay = i.Num;
                }
                else if (i.Key.ToLower().Equals("ptp"))
                {
                    dtd.Ptp = i.Num;
                }
                else if (i.Key.ToLower().Equals("vt"))
                {
                    dtd.Vt = i.Num;
                }
                else if (i.Key.ToLower().Equals("re"))
                {
                    dtd.Rem = i.Num;
                }
            }

            var tu = this.getTunggakan(user, filter!);
            dtd.KewajibanAmount = tu;

            var py = this.getPayment(user, filter!);
            dtd.PayAmount = py;

            w.Data = dtd;

            /*
            DateTime mondayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);
            DateTime sundayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 6);

            if (filter!.PeriodId == 2)
            {
                var today = this.ctx.CollectionHistory.Where(q => q.HistoryDate! >= mondayDate.Date).Where(q => q.HistoryDate! <= sundayDate.Date);

                if (filter != null)
                {
                    if (filter.BranchId != null)
                    {
                        today = today.Where(q => q.BranchId == filter.BranchId);
                    }

                    if (filter.SpvId != null)
                    {
                        var team = from b in this.ctx.Team
                                   join x in this.ctx.TeamMember on b.Id equals x.TeamId
                                   where b.SpvId == filter.SpvId
                                   select x.Member!.Id;
                        var tmp = team.ToList();

                        if (filter.AgentId != null)
                        {
                            today = today.Where(q => q.CallById == filter.AgentId);
                        }
                        else
                        {
                            today = today.Where(q => tmp.Contains(q.CallById));
                        }

                    }
                }

                var today1 = today.GroupBy(g => g.Result!.Code).Select(s => new { Key = s.Key, Num = s.Count() });

                var result = today1.ToList();
                var dtw = new DashboardBean();
                foreach (var i in result)
                {

                    if (i.Key.ToLower().Equals("fuim"))
                    {
                        dtw.Fuim = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("ll"))
                    {
                        dtw.Ll = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("mess"))
                    {
                        dtw.Mess = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas"))
                    {
                        dtw.Noas = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas1"))
                    {
                        dtw.Noas1 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas2"))
                    {
                        dtw.Noas2 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas3"))
                    {
                        dtw.Noas3 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("pay"))
                    {
                        dtw.Pay = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("ptp"))
                    {
                        dtw.Ptp = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("vt"))
                    {
                        dtw.Vt = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("re"))
                    {
                        dtw.Rem = i.Num;
                    }
                }

                var tu = this.getTunggakan(user, filter!);
                dtw.KewajibanAmount = tu;

                w.Data = dtw;
            }
            

            
            var thisWeek = from b in this.ctx.CollectionHistory
                           where b.HistoryDate!.Value.Date >= mondayDate.Date
                           where b.HistoryDate!.Value.Date <= sundayDate.Date
                           group b by b.Result!.Code into g
                           select new
                           {
                               Key = g.Key,
                               Num = g.Count()
                           };

            result = thisWeek.ToList();
            

            if (filter!.PeriodId == 3)
            {
                var firstDate = new DateTime(now.Year, now.Month, 1);
                var LastDate = firstDate.AddMonths(1).AddDays(-1);

                var today = this.ctx.CollectionHistory.Where(q => q.HistoryDate! >= firstDate.Date).Where(q => q.HistoryDate! <= LastDate.Date);

                if (filter != null)
                {
                    if (filter.BranchId != null)
                    {
                        today = today.Where(q => q.BranchId == filter.BranchId);
                    }

                    if (filter.SpvId != null)
                    {
                        var team = from b in this.ctx.Team
                                   join x in this.ctx.TeamMember on b.Id equals x.TeamId
                                   where b.SpvId == filter.SpvId
                                   select x.Member!.Id;
                        var tmp = team.ToList();

                        if (filter.AgentId != null)
                        {
                            today = today.Where(q => q.CallById == filter.AgentId);
                        }
                        else
                        {
                            today = today.Where(q => tmp.Contains(q.CallById));
                        }

                    }
                }

                var today1 = today.GroupBy(g => g.Result!.Code).Select(s => new { Key = s.Key, Num = s.Count() });

                var result = today1.ToList();
                var dtw = new DashboardBean();
                foreach (var i in result)
                {

                    if (i.Key.ToLower().Equals("fuim"))
                    {
                        dtw.Fuim = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("ll"))
                    {
                        dtw.Ll = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("mess"))
                    {
                        dtw.Mess = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas"))
                    {
                        dtw.Noas = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas1"))
                    {
                        dtw.Noas1 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas2"))
                    {
                        dtw.Noas2 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("noas3"))
                    {
                        dtw.Noas3 = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("pay"))
                    {
                        dtw.Pay = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("ptp"))
                    {
                        dtw.Ptp = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("vt"))
                    {
                        dtw.Vt = i.Num;
                    }
                    else if (i.Key.ToLower().Equals("re"))
                    {
                        dtw.Rem = i.Num;
                    }
                }

                var tu = this.getTunggakan(user, filter!);
                dtw.KewajibanAmount = tu;

                w.Data = dtw;
            }
            */


            return w;
        }

        public GenericResponse<UserResponseBean> ListDashboardSpv()
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

            var td = new List<UserResponseBean>();

            if (role == 8 || role == 1 || role == 9 || role == 2)
            {
                var arr = new List<int>();
                arr.Add(6);
                var tmp = this.ctx.User.Where(q => arr.Contains(q.RoleId!.Value)).Where(q => q.Role!.Status!.Name!.Equals("AKTIF"))
                                    .OrderBy(o => o.Name).ToList();

                foreach(var item in tmp)
                {
                    var dto = this.mapper.Map<UserResponseBean>(item);
                    td.Add(dto);
                }
            }
            else if (role == 3 || role == 4 || role == 5 || role == 6)
            {
                var dto = this.mapper.Map<UserResponseBean>(reqUser);
                td.Add(dto);
            }
            else if (role == 7)
            {
                var ls = this.ctx.UserBranch.Where(q => q.Branch!.Id == reqUser.ActiveBranchId).Where(q => q.User!.Role!.Id.Equals(4))
                                            .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                                            .Select(s => s.User)
                                            .Distinct()
                                            .ToList();
                if (ls != null && ls.Count > 0)
                {
                    foreach (var o in ls)
                    {
                        var n = this.mapper.Map<UserResponseBean>(o);
                        td.Add(n);
                    }

                }
            }

            wrap.Data = td;
            return wrap;
        }

        public GenericResponse<UserResponseBean> ListDashboardAgentBySpv(int spvId)
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

            var td = new List<UserResponseBean>();

            var ls = this.ctx.Team.Where(q => q.Spv!.Id.Equals(spvId)).Include(i => i.Member!).ToList();
            if (ls != null && ls.Count > 0)
            {
                var tm = ls[0];
                var ttm = tm.Member!.ToList();
                foreach (var o in ttm)
                {
                    this.ctx.Entry(o).Reference(c => c.Member).Load();
                    var n = this.mapper.Map<UserResponseBean>(o.Member);
                    td.Add(n);
                }

            }

            wrap.Data = td;
            return wrap;
        }

        public GenericResponse<BranchResponseBean> ListDashboardBranch()
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

            var role = reqUser.Role!.Id;

            var td = new List<BranchResponseBean>();

            if (role == 8 || role == 1 || role == 9 || role == 2 || role == 5 || role == 3)
            {
                var tmp = this.ctx.Branch.Where(q => q.Status!.Name!.Equals("AKTIF"))
                                    .OrderBy(o => o.Name).ToList();

                foreach (var item in tmp)
                {
                    var dto = this.mapper.Map<BranchResponseBean>(item);
                    td.Add(dto);
                }
            }
            else if (role == 4 || role == 6)
            {
                var dto = this.mapper.Map<BranchResponseBean>(reqUser);
                td.Add(dto);
            }
            else if (role == 7)
            {
                var ls = this.ctx.UserBranch.Where(q => q.Branch!.Id == reqUser.ActiveBranchId).Where(q => q.User!.Role!.Id.Equals(4))
                                            .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                                            .Select(s => s.User)
                                            .Distinct()
                                            .ToList();
                if (ls != null && ls.Count > 0)
                {
                    foreach (var o in ls)
                    {
                        var n = this.mapper.Map<BranchResponseBean>(o);
                        td.Add(n);
                    }

                }
            }

            wrap.Data = td;
            return wrap;
        }

        private string GenerateMD5(string src)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(src));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private double getTunggakan(User user, DashboardBeanV2 filter)
        {
            var now = DateTime.Now;
            DateTime mondayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);
            DateTime sundayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 6);
            var firstDate = new DateTime(now.Year, now.Month, 1);
            var LastDate = firstDate.AddMonths(1).AddDays(-1);

            var today = this.ctx.CollectionTrace.Where(q => q.CallId != null);

            if (filter.PeriodId != null)
            {
                if (filter.PeriodId == 1)
                {
                    today = today.Where(q => q.TraceDate! == now.Date);
                } 
                else if (filter.PeriodId == 2)
                {
                    today = today.Where(q => q.TraceDate! >= mondayDate.Date).Where(q => q.TraceDate! <= sundayDate.Date);
                }
                else if (filter.PeriodId == 3)
                {
                    today = today.Where(q => q.TraceDate! >= firstDate.Date).Where(q => q.TraceDate! <= LastDate.Date);
                }
            }


            if (filter != null)
            {
                if (filter.BranchId != null)
                {
                    today = today.Where(q => q.Call!.BranchId == filter.BranchId);
                }

                if (filter.SpvId != null)
                {
                    var team = from b in this.ctx.Team
                               join x in this.ctx.TeamMember on b.Id equals x.TeamId
                               where b.SpvId == filter.SpvId
                               select x.Member!.Id;
                    var tmp = team.ToList();

                    if (filter.AgentId != null)
                    {
                        today = today.Where(q => q.CallById == filter.AgentId);
                    }
                    else
                    {
                        today = today.Where(q => tmp.Contains(q.CallById));
                    }

                }
            }

            var today1 = today.GroupBy(g => g.CallId!).Select(s => new { Num = s.Sum(q => q.Amount) });
            var ret = today1.ToList();
            if (ret != null && ret.Count() > 0)
            {
                return ret![0].Num!.Value;
            }
            else
            {
                return 0;
            }

        }

        private double getPayment(User user, DashboardBeanV2 filter)
        {
            var now = DateTime.Now;
            DateTime mondayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);
            DateTime sundayDate = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 6);
            var firstDate = new DateTime(now.Year, now.Month, 1);
            var LastDate = firstDate.AddMonths(1).AddDays(-1);

            var today = this.ctx.CollectionHistory.Where(q => q.ResultId == 8);

            if (filter.PeriodId != null)
            {
                if (filter.PeriodId == 1)
                {
                    today = today.Where(q => q.HistoryDate! == now.Date);
                }
                else if (filter.PeriodId == 2)
                {
                    today = today.Where(q => q.HistoryDate! >= mondayDate.Date).Where(q => q.HistoryDate! <= sundayDate.Date);
                }
                else if (filter.PeriodId == 3)
                {
                    today = today.Where(q => q.HistoryDate! >= firstDate.Date).Where(q => q.HistoryDate! <= LastDate.Date);
                }
            }


            if (filter != null)
            {
                if (filter.BranchId != null)
                {
                    today = today.Where(q => q.BranchId == filter.BranchId);
                }

                if (filter.SpvId != null)
                {
                    var team = from b in this.ctx.Team
                               join x in this.ctx.TeamMember on b.Id equals x.TeamId
                               where b.SpvId == filter.SpvId
                               select x.Member!.Id;
                    var tmp = team.ToList();

                    if (filter.AgentId != null)
                    {
                        today = today.Where(q => q.CallById == filter.AgentId);
                    }
                    else
                    {
                        today = today.Where(q => tmp.Contains(q.CallById));
                    }

                }
            }

            var today1 = today.GroupBy(g => g.CallId!).Select(s => new { Num = s.Sum(q => q.Amount) });
            var ret = today1.ToList();
            if (ret != null && ret.Count() > 0)
            {
                return ret![0].Num!.Value;
            }
            else
            {
                return 0;
            }

        }

        public GenericResponse<DashboardTreeSpv> SpvTree()
        {
            var wrap = new GenericResponse<DashboardTreeSpv>
            {
                Status = true,
                Message = ""
            };


            var today = this.ctx.Team.Where(q => q.Spv!.Status!.Name!.Equals("AKTIF"))
                                .Where(q => q.Spv!.Role!.Name!.Equals("SPVFC"))
                                .Include(i => i.Member!).ThenInclude(j => j.Member)
                                .Include(i => i.Spv)
                                .OrderBy(o => o.Spv!.Name)
                                .ToList();
            var ls = today.ToList();
            var ld = new List<DashboardTreeSpv>();

            foreach(var i in ls)
            {
                var p = new DashboardTreeSpv();
                p.Name = i.Spv!.Name;
                var tt = new List<DashboardTreeSpv>();

                foreach (var x in i.Member!)
                {
                    var pp = new DashboardTreeSpv();
                    pp.Name = x.Member!.Name!;
                    tt.Add(pp);
                }
                p.Agent = tt;

                ld.Add(p);
            }

            wrap.Data = ld;

            return wrap;
        }

        public GenericResponse<DashboardTreeSpv> BranchTree()
        {
            var wrap = new GenericResponse<DashboardTreeSpv>
            {
                Status = true,
                Message = ""
            };


            var today = this.ctx.Branch.Where(q => q.Status!.Name!.Equals("AKTIF"))
                                .OrderBy(o => o.Name)
                                .ToList();

            var ls = today.ToList();
            var ld = new List<DashboardTreeSpv>();

            foreach (var i in ls)
            {
                var p = new DashboardTreeSpv();
                p.Name = i.Name;
                var tt = new List<DashboardTreeSpv>();

                var tmp = this.ctx.UserBranch.Where(q => q.BranchId == i.Id)
                            .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                            .Where(q => q.User!.Role!.Name!.Equals("FC"))
                            .Include(i => i.User!)
                            .ToList();
                foreach (var x in tmp!)
                {
                    var pp = new DashboardTreeSpv();
                    pp.Name = x.User!.Name!;
                    tt.Add(pp);
                }
                p.Agent = tt;

                ld.Add(p);
            }

            wrap.Data = ld;

            return wrap;
        }

        public GenericResponse<UserSelfPasswordRequest> ChangeSelfPassword(UserSelfPasswordRequest src)
        {
            var wrap = new GenericResponse<UserSelfPasswordRequest>();
            wrap.Status = false;

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            if (src.Password!.Equals(src.Password1!) == false)
            {
                wrap.Message = "Password dan password konfirmasi tidak sama";
                return wrap;
            }

            var auth = this.ctx.User.Where(q => q.Id.Equals(reqUser.Id)).Where(q => q.Status!.Name!.Equals("AKTIF")).FirstOrDefault();
            if (auth == null)
            {
                wrap.Message = "Data user tidak diketahiui";
                return wrap;
            }

            auth.Password = this.GenerateMD5(src.Password!);

            this.ctx.User.Update(auth);
            this.ctx.SaveChanges();

            wrap.Status = true;

            return wrap;
        }
    }
}
