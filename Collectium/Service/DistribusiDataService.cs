using AutoMapper;
using Collectium.Model;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Model.Helper;
using Collectium.Validation;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using static Collectium.Model.Bean.Request.IntegrationRequestBean;

namespace Collectium.Service
{
    public class DistribusiDataService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<DistribusiDataService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly IntegrationService intService;
        private readonly GenerateLetterService generateLetterService;
        private readonly IMapper mapper;

        public DistribusiDataService(CollectiumDBContext ctx, 
                                ILogger<DistribusiDataService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                GenerateLetterService generateLetterService,
                                IntegrationService intService,
                                IMapper mapper)
        {
            this.ctx = ctx; 
            this.logger = logger;
            this.pagination = pagination;
            this.statusService = statusService;
            this.mapper = mapper;
            this.intService = intService;
            this.generateLetterService = generateLetterService;
        }


        public GenericResponse<DistributionResponseBean> Distribution()
        {
            this.logger.LogInformation("Distribution >>> started");
            var wrap = new GenericResponse<DistributionResponseBean>()
            {
                Status = false,
                Message = ""
            };

            var dateProcess = DateTime.Today;
            //Head: Copy Data dari STG Data Lake ke STG Collection
            //-----------BEGIN COPY STG----------

            //Open Connection Data Lake (Postgres)

            //Retrieve data n insert into table Staging Collection with mark STG_DATE


            //-----------END COPY STG----------


            //Head: Insert/Update Data dari STG Collection ke Transaction Collection
            //check if data already exist using ACC_NO as key
            //if exist update data n STG_DATE
            //if not exist insert new data
            //-----------BEGIN Insert/Update----------

            this.ctx.Database.ExecuteSqlRaw("UPDATE MASTER_LOAN SET STATUS = 2 WHERE STATUS = 1");

            var dataSTGBranch = this.ctx.STGBranch.Where(o => o.STG_DATE == dateProcess).ToList();
            var dataSTGCustomer = this.ctx.STGCustomer.Where(o => o.STG_DATE == dateProcess).ToList();
            var dataSTGKredit = this.ctx.STGDataKredit.Where(o => o.STG_DATE == dateProcess).ToList();
            var dataSTGLoanDetail = this.ctx.STGLoanDetail.Where(o => o.STG_DATE == dateProcess).ToList();
            var dataSTGLoanBiayaLain = this.ctx.STGDataLoanBiayaLain.Where(o => o.STG_DATE == dateProcess).ToList();
            var dataSTGLoanKodeAO = this.ctx.STGDataLoanKodeAO.Where(o => o.STG_DATE == dateProcess).ToList();
            var dataSTGLoanKomiteKredit = this.ctx.STGDataLoanKomiteKredit.Where(o => o.STG_DATE == dateProcess).ToList();
            var dataSTGLoanKSL = this.ctx.STGDataLoanKSL.Where(o => o.STG_DATE == dateProcess).ToList();
            var dataSTGLoanPK = this.ctx.STGDataLoanPK.Where(o => o.STG_DATE == dateProcess).ToList();
            var dataSTGLoanTagihanLain = this.ctx.STGDataLoanTagihanLain.Where(o => o.STG_DATE == dateProcess).ToList();
            //var dataSTGJaminam = this.ctx.STGDataJaminan.Where(o => o.STG_DATE == dateProcess).ToList();
            var dataPhone = this.ctx.STGCustomerPhone.Where(o => o.STG_DATE == dateProcess).ToList();

            //1: Branch
            foreach (var data in dataSTGBranch)
            {
                var check = this.ctx.Branch.FirstOrDefault(o => o.Code.ToLower().Equals(data.COMPANY_CODE.ToLower()));
                if(check == null)
                {
                    var newData = new Branch();
                    newData.Code = data.COMPANY_CODE;
                    newData.Name = data.COMPANY_NAME;
                    newData.Addr1 = data.NAME_ADDRESS;
                    newData.STG_DATE = data.STG_DATE;

                    this.ctx.Branch.Add(newData);
                    this.ctx.SaveChanges();
                }
                else
                {
                    //check.Code = data.COMPANY_CODE;
                    //check.Name = data.COMPANY_NAME;
                    //check.Addr1 = data.NAME_ADDRESS;
                    check.STG_DATE = data.STG_DATE;

                    this.ctx.Branch.Update(check);
                    this.ctx.SaveChanges();
                }
            }


            //var query = from kr in this.ctx.Set<STGDataKredit>()
            //            join ns in this.ctx.Set<STGCustomer>() on kr.ACC_NO equals ns.NO_REKENING
            //            join ld in this.ctx.Set<STGLoanDetail>() on kr.ACC_NO equals ld.ACC_NO
            //            where ns.BRANCH_CODE!.Equals("345335")
            //            select new { kr, ns, ld };

            //foreach(var t in query)
            //{
            //    var kre = t.kr;
            //}

            //2: Customer / Nasabah
            //foreach (var data in dataSTGCustomer)
            //{
            //    var check = this.ctx.Customer.FirstOrDefault(o => o.Cif.ToLower().Equals(data.CU_CIF.ToLower()));
            //    if (check == null)
            //    {
            //        var newData = new Customer();
            //        newData.Cif = data.CU_CIF;
            //        newData.Name = data.CU_FIRSTNAME;
            //        newData.BornPlace = data.CU_POB;
            //        newData.BornDate = data.CU_DOB;

            //        if(data.CU_IDTYPE.ToLower().Equals("ktp"))
            //        {
            //            newData.IdTypeId = 1;
            //        }
            //        else if (data.CU_IDTYPE.ToLower().Equals("sim"))
            //        {
            //            newData.IdTypeId = 2;
            //        }
            //        else if (data.CU_IDTYPE.ToLower().Equals("passport"))
            //        {
            //            newData.IdTypeId = 3;
            //        }
            //        else if (data.CU_IDTYPE.ToLower().Equals("kitas"))
            //        {
            //            newData.IdTypeId = 4;
            //        }

            //        newData.Idnumber = data.CU_IDNUM;

            //        if (data.CU_GENDER.ToLower().Equals("male"))
            //        {
            //            newData.GenderId = 1;
            //        }
            //        else if (data.CU_GENDER.ToLower().Equals("female"))
            //        {
            //            newData.GenderId = 2;
            //        }

            //        if (data.CU_MARITAL_STATUS.ToLower().Equals("married"))
            //        {
            //            newData.MaritalStatusId = 1;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("single"))
            //        {
            //            newData.MaritalStatusId = 2;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("partner"))
            //        {
            //            newData.MaritalStatusId = 3;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("widowed"))
            //        {
            //            newData.MaritalStatusId = 4;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("other"))
            //        {
            //            newData.MaritalStatusId = 5;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("divorced"))
            //        {
            //            newData.MaritalStatusId = 6;
            //        }


            //        if (data.CU_NATIONALITY.ToLower().Equals("id"))
            //        {
            //            newData.NationalityId = 1;
            //        }
            //        else
            //        {
            //            newData.NationalityId = 2;
            //        }

            //        if (data.CU_INCOMETYPE.ToLower().Equals("gaji"))
            //        {
            //            newData.IncomeTypeId = 1;
            //        }
            //        else if (data.CU_INCOMETYPE.ToLower().Equals("hasil usaha"))
            //        {
            //            newData.IncomeTypeId = 2;
            //        }
            //        else if (data.CU_INCOMETYPE.ToLower().Equals("komisi"))
            //        {
            //            newData.IncomeTypeId = 3;
            //        }
            //        else if (data.CU_INCOMETYPE.ToLower().Equals("laba"))
            //        {
            //            newData.IncomeTypeId = 4;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("lainnya"))
            //        {
            //            newData.IncomeTypeId = 5;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("modal"))
            //        {
            //            newData.IncomeTypeId = 6;
            //        }

            //        newData.CuIncome = data.CU_INCOME;

            //        if (data.CU_TYPE.ToLower().Equals("r"))
            //        {
            //            newData.CustomerTypeId = 1;
            //        }
            //        else if (data.CU_TYPE.ToLower().Equals("c"))
            //        {
            //            newData.CustomerTypeId = 2;
            //        }

            //        newData.Pekerjaan = data.PEKERJAAN;
            //        newData.Jabatan = data.CU_OCCUPATION;
            //        newData.Company = data.CU_COMPANYNAME;
            //        newData.Email = data.CU_EMAIL;
            //        newData.Address = data.CU_ADDR1;
            //        newData.Rt = data.CU_RT;
            //        newData.Rw = data.CU_RW;
            //        newData.KelurahanData = data.CU_KEL;
            //        newData.KecamatanData = data.CU_KEC;
            //        newData.CityData = data.CU_CITY;
            //        newData.ProvinsiData = data.CU_PROVINSI;
            //        newData.ZipCode = data.CU_ZIP_CODE;
            //        newData.HmPhone = data.CU_PHNNUM;
            //        newData.MobilePhone = data.CU_HPNUM;
            //        newData.ProvinsiData = data.CU_PROVINSI;
            //        newData.ProvinsiData = data.CU_PROVINSI;
            //        newData.ProvinsiData = data.CU_PROVINSI;

            //        newData.BranchId = this.ctx.Branch.FirstOrDefault(o => o.Code.ToLower().Equals(data.BRANCH_CODE.ToLower())).Id;
            //        newData.STG_DATE = data.STG_DATE;
            //        this.ctx.Customer.Add(newData);
            //        this.ctx.SaveChanges();
            //    }
            //    else
            //    {
            //        check.Cif = data.CU_CIF;
            //        check.Name = data.CU_FIRSTNAME;
            //        check.BornPlace = data.CU_POB;
            //        check.BornDate = data.CU_DOB;

            //        if (data.CU_IDTYPE.ToLower().Equals("ktp"))
            //        {
            //            check.IdTypeId = 1;
            //        }
            //        else if (data.CU_IDTYPE.ToLower().Equals("sim"))
            //        {
            //            check.IdTypeId = 2;
            //        }
            //        else if (data.CU_IDTYPE.ToLower().Equals("passport"))
            //        {
            //            check.IdTypeId = 3;
            //        }
            //        else if (data.CU_IDTYPE.ToLower().Equals("kitas"))
            //        {
            //            check.IdTypeId = 4;
            //        }

            //        check.Idnumber = data.CU_IDNUM;

            //        if (data.CU_GENDER.ToLower().Equals("male"))
            //        {
            //            check.GenderId = 1;
            //        }
            //        else if (data.CU_GENDER.ToLower().Equals("female"))
            //        {
            //            check.GenderId = 2;
            //        }

            //        if (data.CU_MARITAL_STATUS.ToLower().Equals("married"))
            //        {
            //            check.MaritalStatusId = 1;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("single"))
            //        {
            //            check.MaritalStatusId = 2;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("partner"))
            //        {
            //            check.MaritalStatusId = 3;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("widowed"))
            //        {
            //            check.MaritalStatusId = 4;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("other"))
            //        {
            //            check.MaritalStatusId = 5;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("divorced"))
            //        {
            //            check.MaritalStatusId = 6;
            //        }

            //        if (data.CU_NATIONALITY.ToLower().Equals("id"))
            //        {
            //            check.NationalityId = 1;
            //        }
            //        else
            //        {
            //            check.NationalityId = 2;
            //        }

            //        if (data.CU_INCOMETYPE.ToLower().Equals("gaji"))
            //        {
            //            check.IncomeTypeId = 1;
            //        }
            //        else if (data.CU_INCOMETYPE.ToLower().Equals("hasil usaha"))
            //        {
            //            check.IncomeTypeId = 2;
            //        }
            //        else if (data.CU_INCOMETYPE.ToLower().Equals("komisi"))
            //        {
            //            check.IncomeTypeId = 3;
            //        }
            //        else if (data.CU_INCOMETYPE.ToLower().Equals("laba"))
            //        {
            //            check.IncomeTypeId = 4;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("lainnya"))
            //        {
            //            check.IncomeTypeId = 5;
            //        }
            //        else if (data.CU_MARITAL_STATUS.ToLower().Equals("modal"))
            //        {
            //            check.IncomeTypeId = 6;
            //        }

            //        check.CuIncome = data.CU_INCOME;

            //        if (data.CU_TYPE.ToLower().Equals("r"))
            //        {
            //            check.CustomerTypeId = 1;
            //        }
            //        else if (data.CU_TYPE.ToLower().Equals("c"))
            //        {
            //            check.CustomerTypeId = 2;
            //        }

            //        check.Pekerjaan = data.PEKERJAAN;
            //        check.Jabatan = data.CU_OCCUPATION;
            //        check.Company = data.CU_COMPANYNAME;
            //        check.Email = data.CU_EMAIL;
            //        check.Address = data.CU_ADDR1;
            //        check.Rt = data.CU_RT;
            //        check.Rw = data.CU_RW;
            //        check.KelurahanData = data.CU_KEL;
            //        check.KecamatanData = data.CU_KEC;
            //        check.CityData = data.CU_CITY;
            //        check.ProvinsiData = data.CU_PROVINSI;
            //        check.ZipCode = data.CU_ZIP_CODE;
            //        check.HmPhone = data.CU_PHNNUM;
            //        check.MobilePhone = data.CU_HPNUM;
            //        check.ProvinsiData = data.CU_PROVINSI;
            //        check.ProvinsiData = data.CU_PROVINSI;
            //        check.ProvinsiData = data.CU_PROVINSI;

            //        check.BranchId = this.ctx.Branch.FirstOrDefault(o => o.Code.ToLower().Equals(data.BRANCH_CODE.ToLower())).Id;
            //        check.STG_DATE = data.STG_DATE;
            //        this.ctx.Customer.Update(check);
            //        this.ctx.SaveChanges();
            //    }
            //}

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var cnt = dataSTGKredit.Count;
            var i = 0;

            this.logger.LogInformation("Distribution >>> num of stgkredit = " + cnt);

            //3: Data Kredit & Relasi nya
            foreach (var data in dataSTGKredit)
            {
                logger.LogInformation("Distribution >>> process master loan acc = " + data.ACC_NO + " count: " + i + " of " + cnt);
                var check = this.ctx.MasterLoan.FirstOrDefault(o => o.AccNo!.ToLower().Equals(data.ACC_NO!.ToLower()));
                if (check == null)
                {
                    logger.LogInformation("Distribution >>> process master loan acc = " + data.ACC_NO + " is null, create new");
                    //Master Loan
                    var newData = new MasterLoan();

                    logger.LogInformation("Distribution >>> process loan detail acc = " + data.ACC_NO);
                    var stgloanDetail = dataSTGLoanDetail.Where(o => o.ACC_NO!.ToLower().Equals(data.ACC_NO!.ToLower())).FirstOrDefault();
                    if (stgloanDetail == null)
                    {
                        logger.LogInformation("Distribution >>> process loan detail acc = " + data.ACC_NO + " is null continue");
                        continue;
                    }

                    logger.LogInformation("Distribution >>> process find staging customer acc = " + data.ACC_NO);
                    var dataCust = dataSTGCustomer.FirstOrDefault(o => o.CU_CIF!.ToLower().Equals(data.CU_CIF!.ToLower()));
                    {
                        if (dataCust != null && dataCust.CU_CIF != null)
                        {
                            logger.LogInformation("Distribution >>> process find customer cif = " + dataCust.CU_CIF);
                            var checkCust = this.ctx.Customer.FirstOrDefault(o => o.Cif.ToLower().Equals(dataCust.CU_CIF.ToLower()));
                            if (checkCust == null)
                            {
                                logger.LogInformation("Distribution >>> process find customer cif = " + dataCust.CU_CIF + " is null create new");

                                var newDataCust = new Customer();
                                newDataCust.Cif = dataCust.CU_CIF;
                                newDataCust.Name = dataCust.CU_FIRSTNAME;
                                newDataCust.BornPlace = dataCust.CU_POB;
                                newDataCust.BornDate = dataCust.CU_DOB;

                                if (dataCust.CU_IDTYPE.ToLower().Equals("ktp"))
                                {
                                    newDataCust.IdTypeId = 1;
                                }
                                else if (dataCust.CU_IDTYPE.ToLower().Equals("sim"))
                                {
                                    newDataCust.IdTypeId = 2;
                                }
                                else if (dataCust.CU_IDTYPE.ToLower().Equals("passport"))
                                {
                                    newDataCust.IdTypeId = 3;
                                }
                                else if (dataCust.CU_IDTYPE.ToLower().Equals("kitas"))
                                {
                                    newDataCust.IdTypeId = 4;
                                }

                                newDataCust.Idnumber = dataCust.CU_IDNUM;

                                if (dataCust.CU_GENDER != null)
                                {
                                    if (dataCust.CU_GENDER.ToLower().Equals("male"))
                                    {
                                        newDataCust.GenderId = 1;
                                    }
                                    else if (dataCust.CU_GENDER.ToLower().Equals("female"))
                                    {
                                        newDataCust.GenderId = 2;
                                    }
                                }

                                if (dataCust.CU_MARITAL_STATUS != null)
                                {
                                    if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("married"))
                                    {
                                        newDataCust.MaritalStatusId = 1;
                                    }
                                    else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("single"))
                                    {
                                        newDataCust.MaritalStatusId = 2;
                                    }
                                    else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("partner"))
                                    {
                                        newDataCust.MaritalStatusId = 3;
                                    }
                                    else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("widowed"))
                                    {
                                        newDataCust.MaritalStatusId = 4;
                                    }
                                    else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("other"))
                                    {
                                        newDataCust.MaritalStatusId = 5;
                                    }
                                    else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("divorced"))
                                    {
                                        newDataCust.MaritalStatusId = 6;
                                    }
                                }


                                if (dataCust.CU_NATIONALITY != null)
                                {
                                    if (dataCust.CU_NATIONALITY.ToLower().Equals("id"))
                                    {
                                        newDataCust.NationalityId = 1;
                                    }
                                    else
                                    {
                                        newDataCust.NationalityId = 2;
                                    }
                                }

                                if (dataCust.CU_INCOMETYPE != null)
                                {
                                    if (dataCust.CU_INCOMETYPE != null)
                                    {
                                        if (dataCust.CU_INCOMETYPE.ToLower().Equals("gaji"))
                                        {
                                            newDataCust.IncomeTypeId = 1;
                                        }
                                        else if (dataCust.CU_INCOMETYPE.ToLower().Equals("hasil usaha"))
                                        {
                                            newDataCust.IncomeTypeId = 2;
                                        }
                                        else if (dataCust.CU_INCOMETYPE.ToLower().Equals("komisi"))
                                        {
                                            newDataCust.IncomeTypeId = 3;
                                        }
                                        else if (dataCust.CU_INCOMETYPE.ToLower().Equals("laba"))
                                        {
                                            newDataCust.IncomeTypeId = 4;
                                        } else
                                        {
                                            if (dataCust.CU_MARITAL_STATUS != null)
                                            {
                                                if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("lainnya"))
                                                {
                                                    newDataCust.IncomeTypeId = 5;
                                                }
                                                else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("modal"))
                                                {
                                                    newDataCust.IncomeTypeId = 6;
                                                }
                                            }

                                        }

                                    }
                                }




                                newDataCust.CuIncome = dataCust.CU_INCOME;

                                if (dataCust.CU_TYPE.ToLower().Equals("r"))
                                {
                                    newDataCust.CustomerTypeId = 1;
                                }
                                else if (dataCust.CU_TYPE.ToLower().Equals("c"))
                                {
                                    newDataCust.CustomerTypeId = 2;
                                }

                                newDataCust.Pekerjaan = dataCust.PEKERJAAN;
                                newDataCust.Jabatan = dataCust.CU_OCCUPATION;
                                newDataCust.Company = dataCust.CU_COMPANYNAME;
                                newDataCust.Email = dataCust.CU_EMAIL;
                                newDataCust.Address = dataCust.CU_ADDR1;
                                newDataCust.Rt = dataCust.CU_RT;
                                newDataCust.Rw = dataCust.CU_RW;
                                newDataCust.KelurahanData = dataCust.CU_KEL;
                                newDataCust.KecamatanData = dataCust.CU_KEC;
                                newDataCust.CityData = dataCust.CU_CITY;
                                newDataCust.ProvinsiData = dataCust.CU_PROVINSI;
                                newDataCust.ZipCode = dataCust.CU_ZIP_CODE;
                                newDataCust.HmPhone = dataCust.CU_PHNNUM;
                                newDataCust.MobilePhone = dataCust.CU_HPNUM;
                                newDataCust.ProvinsiData = dataCust.CU_PROVINSI;
                                newDataCust.ProvinsiData = dataCust.CU_PROVINSI;
                                newDataCust.ProvinsiData = dataCust.CU_PROVINSI;

                                newDataCust.BranchId = this.ctx.Branch.FirstOrDefault(o => o.Code.ToLower().Equals(dataCust.BRANCH_CODE.ToLower())).Id;
                                newDataCust.STG_DATE = dataCust.STG_DATE;

                                logger.LogInformation("Distribution >>> process find customer cif = " + dataCust.CU_CIF + " to save");
                                var cma = this.mapper.Map<CustomerDetailResponseBean>(newDataCust);
                                string jsona = JsonSerializer.Serialize(cma, serializeOptions);
                                logger.LogInformation("Distribution >>> " + jsona);

                                this.ctx.Customer.Add(newDataCust);
                                this.ctx.SaveChanges();
                            }
                            else
                            {
                                checkCust.Cif = dataCust.CU_CIF;
                                checkCust.Name = dataCust.CU_FIRSTNAME;
                                checkCust.BornPlace = dataCust.CU_POB;
                                checkCust.BornDate = dataCust.CU_DOB;

                                if (dataCust.CU_IDTYPE.ToLower().Equals("ktp"))
                                {
                                    checkCust.IdTypeId = 1;
                                }
                                else if (dataCust.CU_IDTYPE.ToLower().Equals("sim"))
                                {
                                    checkCust.IdTypeId = 2;
                                }
                                else if (dataCust.CU_IDTYPE.ToLower().Equals("passport"))
                                {
                                    checkCust.IdTypeId = 3;
                                }
                                else if (dataCust.CU_IDTYPE.ToLower().Equals("kitas"))
                                {
                                    checkCust.IdTypeId = 4;
                                }

                                checkCust.Idnumber = dataCust.CU_IDNUM;

                                if (dataCust.CU_GENDER != null)
                                {
                                    if (dataCust.CU_GENDER.ToLower().Equals("male"))
                                    {
                                        checkCust.GenderId = 1;
                                    }
                                    else if (dataCust.CU_GENDER.ToLower().Equals("female"))
                                    {
                                        checkCust.GenderId = 2;
                                    }
                                }

                                if (dataCust.CU_MARITAL_STATUS != null)
                                {
                                    if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("married"))
                                    {
                                        checkCust.MaritalStatusId = 1;
                                    }
                                    else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("single"))
                                    {
                                        checkCust.MaritalStatusId = 2;
                                    }
                                    else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("partner"))
                                    {
                                        checkCust.MaritalStatusId = 3;
                                    }
                                    else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("widowed"))
                                    {
                                        checkCust.MaritalStatusId = 4;
                                    }
                                    else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("other"))
                                    {
                                        checkCust.MaritalStatusId = 5;
                                    }
                                    else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("divorced"))
                                    {
                                        checkCust.MaritalStatusId = 6;
                                    }
                                }

                                if (dataCust.CU_NATIONALITY != null)
                                {
                                    if (dataCust.CU_NATIONALITY.ToLower().Equals("id"))
                                    {
                                        checkCust.NationalityId = 1;
                                    }
                                    else
                                    {
                                        checkCust.NationalityId = 2;
                                    }
                                }

                                if (dataCust.CU_INCOMETYPE != null)
                                {
                                    if (dataCust.CU_INCOMETYPE != null)
                                    {
                                        if (dataCust.CU_INCOMETYPE.ToLower().Equals("gaji"))
                                        {
                                            checkCust.IncomeTypeId = 1;
                                        }
                                        else if (dataCust.CU_INCOMETYPE.ToLower().Equals("hasil usaha"))
                                        {
                                            checkCust.IncomeTypeId = 2;
                                        }
                                        else if (dataCust.CU_INCOMETYPE.ToLower().Equals("komisi"))
                                        {
                                            checkCust.IncomeTypeId = 3;
                                        }
                                        else if (dataCust.CU_INCOMETYPE.ToLower().Equals("laba"))
                                        {
                                            checkCust.IncomeTypeId = 4;
                                        }
                                        else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("lainnya"))
                                        {
                                            checkCust.IncomeTypeId = 5;
                                        }
                                        else if (dataCust.CU_MARITAL_STATUS.ToLower().Equals("modal"))
                                        {
                                            checkCust.IncomeTypeId = 6;
                                        }
                                    }
                                    checkCust.CuIncome = dataCust.CU_INCOME;
                                }


                                if (dataCust.CU_TYPE != null)
                                {
                                    if (dataCust.CU_TYPE.ToLower().Equals("r"))
                                    {
                                        checkCust.CustomerTypeId = 1;
                                    }
                                    else if (dataCust.CU_TYPE.ToLower().Equals("c"))
                                    {
                                        checkCust.CustomerTypeId = 2;
                                    }
                                }


                                checkCust.Pekerjaan = dataCust.PEKERJAAN;
                                checkCust.Jabatan = dataCust.CU_OCCUPATION;
                                checkCust.Company = dataCust.CU_COMPANYNAME;
                                checkCust.Email = dataCust.CU_EMAIL;
                                checkCust.Address = dataCust.CU_ADDR1;
                                checkCust.Rt = dataCust.CU_RT;
                                checkCust.Rw = dataCust.CU_RW;
                                checkCust.KelurahanData = dataCust.CU_KEL;
                                checkCust.KecamatanData = dataCust.CU_KEC;
                                checkCust.CityData = dataCust.CU_CITY;
                                checkCust.ProvinsiData = dataCust.CU_PROVINSI;
                                checkCust.ZipCode = dataCust.CU_ZIP_CODE;
                                checkCust.HmPhone = dataCust.CU_PHNNUM;
                                checkCust.MobilePhone = dataCust.CU_HPNUM;
                                checkCust.ProvinsiData = dataCust.CU_PROVINSI;
                                checkCust.ProvinsiData = dataCust.CU_PROVINSI;
                                checkCust.ProvinsiData = dataCust.CU_PROVINSI;

                                checkCust.BranchId = this.ctx.Branch.FirstOrDefault(o => o.Code.ToLower().Equals(dataCust.BRANCH_CODE.ToLower())).Id;
                                checkCust.STG_DATE = dataCust.STG_DATE;

                                logger.LogInformation("Distribution >>> process find customer cif = " + dataCust.CU_CIF + " to update");
                                var cmb = this.mapper.Map<CustomerDetailResponseBean>(checkCust);
                                string jsonb = JsonSerializer.Serialize(cmb, serializeOptions);
                                logger.LogInformation("Distribution >>> " + jsonb);

                                this.ctx.Customer.Update(checkCust);
                                this.ctx.SaveChanges();
                            }

                        }
                    }

                    var cust = this.ctx.Customer.FirstOrDefault(o => o.Cif.ToLower().Equals(data.CU_CIF.ToLower()));
                    this.ctx.Entry(cust).Reference(r => r.Branch).Load();
                    if (cust != null)
                    {
                        newData.CustomerId = cust.Id;
                    }
                    
                    newData.Cif = data.CU_CIF;
                    //newData.ChannelBranchCode = data.CABANG_ASAL_DEBITUR;
                    newData.ChannelBranchCode = cust!.Branch!.Code;
                    newData.AccNo = data.ACC_NO;
                    newData.Ccy = data.ISO_CURRENCY;
                    newData.Fasilitas = data.FASILITAS;
                    newData.Plafond = data.PLAFON;
                    newData.MaturityDate = data.MATURITY_DATE;
                    newData.StartDate = data.TANGGAL_MULAI_MENUNGGAK;
                    newData.Tenor = data.TENOR;
                    newData.InstallmentPokok = (data.PRINCIPAL_IDR != null ? data.PRINCIPAL_IDR : data.PRINCIPAL_USD);

                    if (stgloanDetail != null)
                    {
                        newData.InterestRate = stgloanDetail.SUKU_BUNGA;
                        newData.Installment = stgloanDetail.INSTALLMENT;
                        newData.TunggakanPokok = stgloanDetail.PRINCIPAL_DUE;
                        newData.TunggakanBunga = stgloanDetail.INTEREST_DUE;
                        newData.TunggakanDenda = stgloanDetail.PENALTY_DENDA;
                        newData.TunggakanTotal = stgloanDetail.SUB_TOTAL;
                        newData.KewajibanTotal = stgloanDetail.TOTAL_KEWAJIBAN;
                        newData.LastPayDate = stgloanDetail.LAST_PAYMENT_DATE;
                        newData.Outstanding = stgloanDetail.OUTSTANDING;
                        newData.Dpd = stgloanDetail.DPD;
                        newData.Kolektibilitas = stgloanDetail.KOLEKTIBILITY;

                        newData.Plafond = stgloanDetail.Plafond;
                        newData.MaturityDate = stgloanDetail.MaturityDate;
                        newData.PayInAccount = stgloanDetail.PayInAccount;
                        newData.StartDate = stgloanDetail.StartDate;
                        newData.FileDate = stgloanDetail.FileDate;

                        if (stgloanDetail.Segment != null)
                        {
                            var segment = this.ctx.ProductSegment.Where(q => q.Desc!.Trim().Equals(stgloanDetail.Segment!.Trim())).FirstOrDefault();
                            if (segment == null)
                            {
                                segment = new ProductSegment();
                                segment.StatusId = 1;
                                segment.Desc = stgloanDetail.Segment!;
                                segment.CreateDate = DateTime.Now;

                                this.ctx.ProductSegment.Add(segment);
                                this.ctx.SaveChanges();
                            }

                            newData.ProductSegment = segment;
                        }
                        else
                        {
                            var segment = this.ctx.ProductSegment.Find(1);
                            newData.ProductSegment = segment;
                        }

                        //Product diambil dari fasilitas
                        if (stgloanDetail.ProductLoan != null)
                        {
                            var product = this.ctx.Product.Where(q => q.Desc!.Trim().Equals(stgloanDetail.ProductLoan!.Trim())).FirstOrDefault();
                            if (product == null)
                            {
                                product = new Product();
                                product.StatusId = 1;
                                product.ProductSegment = newData.ProductSegment;
                                product.Desc = stgloanDetail.ProductLoan!.Trim();
                                product.CreateDate = DateTime.Now;

                                this.ctx.Product.Add(product);
                                this.ctx.SaveChanges();
                            }

                            newData.Product = product;
                        }
                        else
                        {
                            var product = this.ctx.Product.Find(1);
                            newData.Product = product;
                        }

                    }

                    newData.STG_DATE = data.STG_DATE;

                    logger.LogInformation("Distribution >>> process find master loan cif = " + dataCust.CU_CIF + " to save");
                    var cm = this.mapper.Map<CollResponseBean>(newData);
                    string json = JsonSerializer.Serialize(cm, serializeOptions);
                    logger.LogInformation("Distribution >>> " + json);

                    this.ctx.MasterLoan.Add(newData);
                    this.ctx.SaveChanges();


                    newData.Status = 1;
                    this.ctx.MasterLoan.Update(newData);
                    this.ctx.SaveChanges();

                    //Data Loan Biaya Lain
                    var stgBiayaLain = dataSTGLoanBiayaLain.FirstOrDefault(o => o.ACC_NO!.ToLower().Equals(data.ACC_NO!.ToLower()));
                    if (stgBiayaLain != null)
                    {
                        var newBiayaLain = new LoanBiayaLain();
                        newBiayaLain.loan_id = newData.Id;
                        newBiayaLain.ACC_NO = stgBiayaLain.ACC_NO;
                        newBiayaLain.Tanggal_Biaya_Lain = stgBiayaLain.Tanggal_Biaya_Lain;
                        newBiayaLain.NAMA_Biaya_Lain = stgBiayaLain.NAMA_Biaya_Lain;
                        newBiayaLain.Nominal_Biaya_Lain = stgBiayaLain.Nominal_Biaya_Lain;
                        newBiayaLain.STG_DATE = stgBiayaLain.STG_DATE;
                        this.ctx.LoanBiayaLain.Add(newBiayaLain);
                        this.ctx.SaveChanges();
                    }

                    //Data Loan Kode AO
                    var stgKodeAO = dataSTGLoanKodeAO.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgKodeAO != null)
                    {
                        var newKodeAO = new LoanKodeAO();
                        newKodeAO.loan_id = newData.Id;
                        newKodeAO.ACC_NO = stgKodeAO.ACC_NO;
                        newKodeAO.KODE_AO = stgKodeAO.KODE_AO;
                        newKodeAO.TANGGAL_AO = stgKodeAO.TANGGAL_AO;
                        newKodeAO.STG_DATE = stgKodeAO.STG_DATE;
                        this.ctx.LoanKodeAO.Add(newKodeAO);
                        this.ctx.SaveChanges();
                    }

                    //Data Loan Komite Kredit
                    var stgKomiteKredit = dataSTGLoanKomiteKredit.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgKomiteKredit != null)
                    {
                        var newKomiteKredit = new LoanKomiteKredit();
                        newKomiteKredit.loan_id = newData.Id;
                        newKomiteKredit.ACC_NO = stgKomiteKredit.ACC_NO;
                        newKomiteKredit.NOMOR_PK = stgKomiteKredit.NOMOR_PK;
                        newKomiteKredit.TANGGAL_PK = stgKomiteKredit.TANGGAL_PK;
                        newKomiteKredit.KOMITE01 = stgKomiteKredit.KOMITE01;
                        newKomiteKredit.KOMITE02 = stgKomiteKredit.KOMITE02;
                        newKomiteKredit.KOMITE03 = stgKomiteKredit.KOMITE03;
                        newKomiteKredit.KOMITE04 = stgKomiteKredit.KOMITE04;
                        newKomiteKredit.KOMITE05 = stgKomiteKredit.KOMITE05;
                        newKomiteKredit.KOMITE06 = stgKomiteKredit.KOMITE06;
                        newKomiteKredit.STG_DATE = stgKomiteKredit.STG_DATE;
                        this.ctx.LoanKomiteKredit.Add(newKomiteKredit);
                        this.ctx.SaveChanges();
                    }

                    //Data Loan KSL
                    var stgKSL = dataSTGLoanKSL.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgKSL != null)
                    {
                        var newKSL = new LoanKSL();
                        newKSL.loan_id = newData.Id;
                        newKSL.ACC_NO = stgKSL.ACC_NO;
                        newKSL.Tanggal_KSL = stgKSL.Tanggal_KSL;
                        newKSL.NAMA_KSL = stgKSL.NAMA_KSL;
                        newKSL.Saldo_KSL = stgKSL.Saldo_KSL;
                        newKSL.STG_DATE = stgKSL.STG_DATE;
                        this.ctx.LoanKSL.Add(newKSL);
                        this.ctx.SaveChanges();
                    }

                    //Data Loan PK
                    var stgPK = dataSTGLoanPK.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgPK != null)
                    {
                        var newPK = new LoanPK();
                        newPK.loan_id = newData.Id;
                        newPK.ACC_NO = stgPK.ACC_NO;
                        newPK.TANGGAL_PK = stgPK.TANGGAL_PK;
                        newPK.NOMOR_PK = stgPK.NOMOR_PK;
                        newPK.NAMA_NOTARIS = stgPK.NAMA_NOTARIS;
                        newPK.NAMA_LEGAL = stgPK.NAMA_LEGAL;
                        newPK.STG_DATE = stgBiayaLain.STG_DATE;
                        this.ctx.LoanPK.Add(newPK);
                        this.ctx.SaveChanges();
                    }

                    //Data Loan Tagihan Lainnya
                    var stgTagihanLainnya = dataSTGLoanTagihanLain.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgTagihanLainnya != null)
                    {
                        var newTagihanLainnya = new LoanTagihanLain();
                        newTagihanLainnya.loan_id = newData.Id;
                        newTagihanLainnya.ACC_NO = stgTagihanLainnya.ACC_NO;
                        newTagihanLainnya.Tanggal_TL = stgTagihanLainnya.Tanggal_TL;
                        newTagihanLainnya.NAMA_TL = stgTagihanLainnya.NAMA_TL;
                        newTagihanLainnya.Nominal_TL = stgTagihanLainnya.Nominal_TL;
                        newTagihanLainnya.STG_DATE = stgBiayaLain.STG_DATE;
                        this.ctx.LoanTagihanLain.Add(newTagihanLainnya);
                        this.ctx.SaveChanges();
                    }
                }
                else
                {
                    var cust = this.ctx.Customer.FirstOrDefault(o => o.Cif.ToLower().Equals(data.CU_CIF.ToLower()));
                    this.ctx.Entry(cust).Reference(r => r.Branch).Load();
                    if (cust != null)
                    {
                        check.CustomerId = cust.Id;
                    }

                    var stgloanDetail = dataSTGLoanDetail.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgloanDetail == null)
                    {
                        logger.LogInformation("Distribution >>> process loan detail acc = " + data.ACC_NO + " is null continue");
                        continue;
                    }

                    check.Cif = data.CU_CIF;
                    //check.ChannelBranchCode = data.CABANG_ASAL_DEBITUR;
                    check.ChannelBranchCode = cust.Branch.Code;
                    check.AccNo = data.ACC_NO;
                    check.Ccy = data.ISO_CURRENCY;
                    check.Fasilitas = data.FASILITAS;
                    check.Plafond = data.PLAFON;
                    check.MaturityDate = data.MATURITY_DATE;
                    check.StartDate = data.TANGGAL_MULAI_MENUNGGAK;
                    check.Tenor = data.TENOR;
                    check.Status = 1;
                    check.InstallmentPokok = (data.PRINCIPAL_IDR != null ? data.PRINCIPAL_IDR : data.PRINCIPAL_USD);

                    if (stgloanDetail != null)
                    {
                        check.InterestRate = stgloanDetail.SUKU_BUNGA;
                        check.Installment = stgloanDetail.INSTALLMENT;
                        check.TunggakanPokok = stgloanDetail.PRINCIPAL_DUE;
                        check.TunggakanBunga = stgloanDetail.INTEREST_DUE;
                        check.TunggakanDenda = stgloanDetail.PENALTY_DENDA;
                        check.TunggakanTotal = stgloanDetail.SUB_TOTAL;
                        check.KewajibanTotal = stgloanDetail.TOTAL_KEWAJIBAN;
                        check.LastPayDate = stgloanDetail.LAST_PAYMENT_DATE;
                        check.Outstanding = stgloanDetail.OUTSTANDING;
                        check.Dpd = stgloanDetail.DPD;
                        check.Kolektibilitas = stgloanDetail.KOLEKTIBILITY;

                        check.Plafond = stgloanDetail.Plafond;
                        check.MaturityDate = stgloanDetail.MaturityDate;
                        check.PayInAccount = stgloanDetail.PayInAccount;
                        check.StartDate = stgloanDetail.StartDate;
                        check.FileDate = stgloanDetail.FileDate;

                        if (stgloanDetail.Segment != null)
                        {
                            var segment = this.ctx.ProductSegment.Where(q => q.Desc!.Trim().Equals(stgloanDetail.Segment!.Trim())).FirstOrDefault();
                            if (segment == null)
                            {
                                segment = new ProductSegment();
                                segment.StatusId = 1;
                                segment.Desc = stgloanDetail.Segment!;
                                segment.CreateDate = DateTime.Now;

                                this.ctx.ProductSegment.Add(segment);
                                this.ctx.SaveChanges();
                            }

                            check.ProductSegment = segment;
                        }
                        else
                        {
                            var segment = this.ctx.ProductSegment.Find(1);
                            check.ProductSegment = segment;
                        }

                        //Product diambil dari fasilitas
                        if (stgloanDetail.ProductLoan != null)
                        {
                            var product = this.ctx.Product.Where(q => q.Desc!.Trim().Equals(stgloanDetail.ProductLoan!.Trim())).FirstOrDefault();
                            if (product == null)
                            {
                                product = new Product();
                                product.StatusId = 1;
                                product.ProductSegment = check.ProductSegment;
                                product.Desc = stgloanDetail.ProductLoan!.Trim();
                                product.CreateDate = DateTime.Now;

                                this.ctx.Product.Add(product);
                                this.ctx.SaveChanges();
                            }

                            check.Product = product;
                        }
                        else
                        {
                            var product = this.ctx.Product.Find(1);
                            check.Product = product;
                        }

                    }

                    check.STG_DATE = data.STG_DATE;

                    logger.LogInformation("Distribution >>> process find master loan cif = " + data.CU_CIF + " to save");
                    var cm = this.mapper.Map<CollResponseBean>(check);
                    string json = JsonSerializer.Serialize(cm, serializeOptions);
                    logger.LogInformation("Distribution >>> " + json);

                    this.ctx.MasterLoan.Update(check);
                    this.ctx.SaveChanges();

                    //Data Loan Biaya Lain
                    var stgBiayaLain = dataSTGLoanBiayaLain.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgBiayaLain != null)
                    {
                        var newBiayaLain = this.ctx.LoanBiayaLain.FirstOrDefault(o => o.loan_id == check.Id);
                        newBiayaLain.ACC_NO = stgBiayaLain.ACC_NO;
                        newBiayaLain.Tanggal_Biaya_Lain = stgBiayaLain.Tanggal_Biaya_Lain;
                        newBiayaLain.NAMA_Biaya_Lain = stgBiayaLain.NAMA_Biaya_Lain;
                        newBiayaLain.Nominal_Biaya_Lain = stgBiayaLain.Nominal_Biaya_Lain;
                        newBiayaLain.STG_DATE = stgBiayaLain.STG_DATE;
                        this.ctx.LoanBiayaLain.Update(newBiayaLain);
                        this.ctx.SaveChanges();
                    }

                    //Data Loan Kode AO
                    var stgKodeAO = dataSTGLoanKodeAO.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgKodeAO != null)
                    {
                        var newKodeAO = this.ctx.LoanKodeAO.FirstOrDefault(o => o.loan_id == check.Id);
                        newKodeAO.ACC_NO = stgKodeAO.ACC_NO;
                        newKodeAO.KODE_AO = stgKodeAO.KODE_AO;
                        newKodeAO.TANGGAL_AO = stgKodeAO.TANGGAL_AO;
                        newKodeAO.STG_DATE = stgKodeAO.STG_DATE;
                        this.ctx.LoanKodeAO.Update(newKodeAO);
                        this.ctx.SaveChanges();
                    }

                    //Data Loan Komite Kredit
                    var stgKomiteKredit = dataSTGLoanKomiteKredit.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgKomiteKredit != null)
                    {
                        var newKomiteKredit = this.ctx.LoanKomiteKredit.FirstOrDefault(o => o.loan_id == check.Id);
                        newKomiteKredit.ACC_NO = stgKomiteKredit.ACC_NO;
                        newKomiteKredit.NOMOR_PK = stgKomiteKredit.NOMOR_PK;
                        newKomiteKredit.TANGGAL_PK = stgKomiteKredit.TANGGAL_PK;
                        newKomiteKredit.KOMITE01 = stgKomiteKredit.KOMITE01;
                        newKomiteKredit.KOMITE02 = stgKomiteKredit.KOMITE02;
                        newKomiteKredit.KOMITE03 = stgKomiteKredit.KOMITE03;
                        newKomiteKredit.KOMITE04 = stgKomiteKredit.KOMITE04;
                        newKomiteKredit.KOMITE05 = stgKomiteKredit.KOMITE05;
                        newKomiteKredit.KOMITE06 = stgKomiteKredit.KOMITE06;
                        newKomiteKredit.STG_DATE = stgKomiteKredit.STG_DATE;
                        this.ctx.LoanKomiteKredit.Update(newKomiteKredit);
                        this.ctx.SaveChanges();
                    }

                    //Data Loan KSL
                    var stgKSL = dataSTGLoanKSL.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgKSL != null)
                    {
                        var newKSL = this.ctx.LoanKSL.FirstOrDefault(o => o.loan_id == check.Id);
                        newKSL.ACC_NO = stgKSL.ACC_NO;
                        newKSL.Tanggal_KSL = stgKSL.Tanggal_KSL;
                        newKSL.NAMA_KSL = stgKSL.NAMA_KSL;
                        newKSL.Saldo_KSL = stgKSL.Saldo_KSL;
                        newKSL.STG_DATE = stgKSL.STG_DATE;
                        this.ctx.LoanKSL.Update(newKSL);
                        this.ctx.SaveChanges();
                    }

                    //Data Loan PK
                    var stgPK = dataSTGLoanPK.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgPK != null)
                    {
                        var newPK = this.ctx.LoanPK.FirstOrDefault(o => o.loan_id == check.Id);
                        newPK.ACC_NO = stgPK.ACC_NO;
                        newPK.TANGGAL_PK = stgPK.TANGGAL_PK;
                        newPK.NOMOR_PK = stgPK.NOMOR_PK;
                        newPK.NAMA_NOTARIS = stgPK.NAMA_NOTARIS;
                        newPK.NAMA_LEGAL = stgPK.NAMA_LEGAL;
                        newPK.STG_DATE = stgBiayaLain.STG_DATE;
                        this.ctx.LoanPK.Update(newPK);
                        this.ctx.SaveChanges();
                    }

                    //Data Loan Tagihan Lainnya
                    var stgTagihanLainnya = dataSTGLoanTagihanLain.FirstOrDefault(o => o.ACC_NO.ToLower().Equals(data.ACC_NO.ToLower()));
                    if (stgTagihanLainnya != null)
                    {
                        var newTagihanLainnya = this.ctx.LoanTagihanLain.FirstOrDefault(o => o.loan_id == check.Id);
                        newTagihanLainnya.ACC_NO = stgTagihanLainnya.ACC_NO;
                        newTagihanLainnya.Tanggal_TL = stgTagihanLainnya.Tanggal_TL;
                        newTagihanLainnya.NAMA_TL = stgTagihanLainnya.NAMA_TL;
                        newTagihanLainnya.Nominal_TL = stgTagihanLainnya.Nominal_TL;
                        newTagihanLainnya.STG_DATE = stgBiayaLain.STG_DATE;
                        this.ctx.LoanTagihanLain.Update(newTagihanLainnya);
                        this.ctx.SaveChanges();
                    }
                }

                cnt++;
            }

            //Customer Phone

            foreach (var phitem in dataPhone)
            {
                var eph = this.ctx.CollectionAddContact.Where(q => q.CuCif!.Equals(phitem.CIF)).Where(q => q.AddPhone!.Equals(phitem.PHONE)).Count();
                if (eph < 1)
                {
                    var cac = new CollectionAddContact();
                    cac.CuCif = phitem.CIF;
                    cac.AddPhone = phitem.PHONE;
                    cac.AddFrom = "CORE";
                    cac.AddDate = DateTime.Now;
                    this.ctx.CollectionAddContact.Add(cac);
                    this.ctx.SaveChanges();
                }
            }

            //-----------END Insert/Update----------

            //this.ctx.Database.ExecuteSqlRaw("UPDATE MASTER_LOAN SET STATUS = 0 WHERE STATUS = 2");
            var paid = this.ctx.MasterLoan.Where(q => q.Status == 2).ToList();
            var cntx = paid.Count;
            var y = 0;

            this.logger.LogInformation("Distribution >>> num of loan status 2 = " + cntx);
            if (paid != null && paid.Count > 0)
            {
                foreach(var item in paid)
                {
                    this.logger.LogInformation("Distribution >>> process loanid with acc = " + item.AccNo + " with num " + y + " of " + cntx);

                    item.Status = 0;
                    this.ctx.MasterLoan.Update(item);

                    var pay = new PaymentHistory();
                    //pay.Loan = item;
                    pay.Denda = item.TunggakanDenda;
                    pay.TotalBayar = item.KewajibanTotal;
                    pay.PokokCicilan = item.TunggakanPokok;
                    pay.AccNo = item.AccNo;
                    pay.Bunga = item.TunggakanBunga;
                    pay.Tgl = DateTime.Now;
                    pay.CreateDate = DateTime.Now;

                    //Add paymentrecord
                    var payr = new PaymentRecord();

                    var cc = this.ctx.CollectionCall.Where(q => q.AccNo!.Equals(item.AccNo)).OrderByDescending(o => o.Id).FirstOrDefault();
                    if (cc != null)
                    {
                        pay.CallById = cc.CallById;
                        payr.CallById = cc.CallById;
                        payr.Call = cc;
                    }

                    //var tdy = dateProcess.ToString("yyyy-MM-dd");
                    //var stv2 = this.GetPaymentFromT24(item.AccNo!, item.LoanNumber!, tdy);
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
                    var prv = this.ctx.PaymentHistory.Where(q => q.AccNo!.Equals(item.AccNo))
                                            .Where(q => q.Tgl!.Value.Date.Equals(nw)).FirstOrDefault();
                    if (prv == null)
                    {
                        this.ctx.PaymentHistory.Add(pay);
                        this.ctx.SaveChanges();

                        this.logger.LogInformation("Distribution >>> payment history >>> acc: " + pay.AccNo);
                    }

                    //Add paymentrecord
                    payr.AccNo = item.AccNo;
                    payr.RecordDate = DateTime.Now;
                    payr.Amount = item.KewajibanTotal;
                    this.ctx.PaymentRecord.Add(payr);
                    this.ctx.SaveChanges();
                    this.logger.LogInformation("Distribution >>> payment record >>> acc: " + payr.AccNo);

                    // update generate letter
                    //var gll = this.ctx.GenerateLetter.Where(q => q.LoanId.Equals(item.Id)).Where(q => q.StatusId.Equals(1)).ToList();
                    //if (gll != null && gll.Count() > 0)
                    //{
                    //    var gl = gll[0];
                    //    gl.StatusId = 2;
                    //    this.ctx.GenerateLetter.Update(gl);
                    //    this.ctx.SaveChanges();
                    //}
                }
            }

            this.logger.LogInformation("Distribution >>> finished");

            var res = new DistributionResponseBean();
            res.Result = "Success on " + DateTime.Today;
            var output = new List<DistributionResponseBean>();
            output.Add(res);
            wrap.Data = output;
            wrap.Status = true;
            return wrap;
        }

        public void DistributinData()
        {

            this.logger.LogInformation("DistributionData >>> started");
            var dateProcess = DateTime.Today;
            var stoday = dateProcess.ToString("yyyy-MM-dd");
            this.ctx.Database.ExecuteSqlRaw("UPDATE MASTER_LOAN SET STATUS = 0 WHERE STG_DATE != '" + stoday + "'");

            this.ctx.Database.ExecuteSqlRaw("DELETE FROM master_loan WHERE id NOT IN (SELECT id FROM (SELECT MIN(id) AS id FROM master_loan GROUP BY acc_no HAVING COUNT(*) > 1) AS a) AND id NOT IN ( (SELECT ids FROM (SELECT MIN(id)AS ids FROM master_loan GROUP BY acc_no HAVING COUNT(*) = 1) AS a1))");
            this.ctx.Database.ExecuteSqlRaw("DELETE FROM collection_call WHERE id NOT IN(SELECT id FROM (SELECT MIN(id)AS id FROM collection_call GROUP BY acc_no HAVING COUNT(*) > 1)AS a ) AND id NOT IN((SELECT ids FROM (SELECT MIN(id)AS ids FROM collection_call GROUP BY acc_no HAVING COUNT(*) =1)AS a1))");

            this.DistributinDataDC();
            this.DistributinDataFC();
            //this.GenerateLetter();
            //this.CopyLoanNumber();

            this.logger.LogInformation("DistributionData >>> finished");
        }


        public void DistributinDataOld()
        {

            var dateProcess = DateTime.Today;
            //Head: Distribusi Assignment Collection ke DC dan FC
            //data nasabah di cabang X
            //select DC FC di cabang X
            //cek brp data harus ke DC, brp harus ke FC
            //-----------BEGIN ASSIGN----------
            //var oldData = this.ctx.MasterLoan.Where(o => o.STG_DATE != dateProcess).ToList();//data lama yg sudah tidak turun lagi

            //foreach (var data in oldData)
            //{
            //    data.Status = 0;
            //    this.ctx.MasterLoan.Update(data);
            //    this.ctx.SaveChanges();
            //}

            var stoday = dateProcess.ToString("yyyy-MM-dd");
            this.ctx.Database.ExecuteSqlRaw("UPDATE MASTER_LOAN SET STATUS = 0 WHERE STG_DATE != '" + stoday + "'");

            var todayData = this.ctx.MasterLoan.Where(o => o.STG_DATE == dateProcess);//data hari ini

            //var allBranch = this.ctx.Branch.Where(q => q.Id.Equals(1)).ToList();
            var allBranch = this.ctx.Branch.ToList();

            foreach (var cabang in allBranch)
            {
                this.logger.LogInformation("DistributinData >>> process cabang: " + cabang.Code);
                var dataCabang = todayData.Where(o => o.ChannelBranchCode!.ToLower().Equals(cabang!.Code!.ToLower())).OrderBy(o => o.Cif).ToList();
                //var dataCabang = todayData.Where(q => q.Cif.Equals("1000171471")).OrderBy(o => o.Cif).ToList();

                if (dataCabang.Count > 0 ) 
                {
                    this.logger.LogInformation("DistributinData >>> process cabang: " + cabang.Code + " di process");

                    //var dcCabang = this.ctx.UserBranch.Where(q => q.Branch!.Id == cabang.Id).Where(q => q.User!.Role!.Id == 3)
                    //    .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                    //    .Include(i => i.User)
                    //    .ToList();

                    //DC allcabang

                    var dcCabang = this.ctx.User.Where(q => q.RoleId == 3).Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();

                    var fcCabang = this.ctx.UserBranch.Where(q => q.Branch!.Id == cabang.Id).Where(q => q.User!.Role!.Id == 4)
                                           .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                                           .Include(i => i.User)
                                           .ToList();

                    //var dcCabang = this.ctx.UserBranch.Where(q => q.Branch!.Id == 1).Where(q => q.User!.Role!.Id == 3)
                    //    .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                    //    .Include(i => i.User)
                    //    .ToList();

                    //var fcCabang = this.ctx.UserBranch.Where(q => q.Branch!.Id == cabang.Id).Where(q => q.User!.Role!.Id == 4)
                    //                       .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                    //                       .Include(i => i.User)
                    //                       .ToList();

                    int countDC = dcCabang.Count;
                    int countFC = fcCabang.Count;
                    int counterDC = 0;
                    int counterFC = 0;
                    int counterExistingDC = 0; int prevCounterDCId = 0;
                    int counterExistingFC = 0; int prevCounterFCId = 0;

                    foreach (var data in dataCabang)
                    {
                        this.logger.LogInformation("DistributinData >>> process loanid: " + data.Id);
                        var checkAssign = this.ctx.CollectionCall.FirstOrDefault(o => o.LoanId == data.Id);
                        if (checkAssign != null) //sudah pernah di assign, data sudah pernah turun
                        {
                            this.logger.LogInformation("DistributinData >>> process loanid: " + data.Id + " existing");
                            if (data.Dpd > 14 && data.Dpd <= 90)
                            {
                                this.logger.LogInformation("DistributinData >>> process loanid: " + data.Id + " " + data.AccNo + " dpd = " + data.Dpd + " existing");

                                this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                                var role = checkAssign.CallBy!.RoleId;

                                if (role == 3)
                                {
                                    var FCReady = this.ctx.UserBranch.Where(o => o.User!.RoleId!.Equals(4))
                                                           .Where(q => q.BranchId.Equals(checkAssign.BranchId))
                                                           .OrderBy(o => o.UserId).ToList();

                                    if (FCReady != null && FCReady.Count() > 0)
                                    {

                                        if (counterExistingFC == prevCounterFCId)
                                        {
                                            if (counterExistingFC != 0 && prevCounterFCId != 0)
                                            {
                                                counterExistingFC += 1;
                                            }

                                        }
                                        checkAssign.CallById = FCReady[counterExistingFC].UserId;
                                        checkAssign.CallResultId = 10;
                                        prevCounterFCId = counterExistingFC;
                                        this.ctx.CollectionCall.Update(checkAssign);
                                        this.ctx.SaveChanges();

                                        this.logger.LogInformation("DistributinData >>> process loanid: " + data.Id + " " + data.AccNo + " dpd = " + data.Dpd + " existing assigned to " + checkAssign.CallById);

                                        counterExistingFC += 1;
                                        if (counterExistingFC == countFC - 1)
                                        {
                                            counterExistingFC = 0;
                                        }
                                    }
                                } 
                                else
                                {
                                    var firstAssign = this.ctx.MasterLoanHistory
                                        .Where(q => q.LoanId.Equals(checkAssign.LoanId))
                                        .Where(q => q.CallById.Equals(checkAssign.CallById))
                                        .OrderBy(o => o.STG_DATE)
                                        .Take(1)
                                        .FirstOrDefault();

                                    var assignDate = firstAssign!.STG_DATE;
                                    var lastSevenDay = DateTime.Now.AddDays(-7);
                                    if (assignDate < lastSevenDay)
                                    {
                                        if (role == 4)
                                        {
                                            var FCReady = this.ctx.UserBranch.Where(o => o.UserId != checkAssign.CallById)
                                                                  .Where(o => o.User!.RoleId!.Equals(role))
                                                                  .Where(q => q.BranchId.Equals(checkAssign.BranchId))
                                                                  .OrderBy(o => o.UserId).ToList();

                                            if (FCReady != null && FCReady.Count() > 0)
                                            {
                                                if (counterExistingFC == prevCounterFCId)
                                                {
                                                    if (counterExistingFC != 0 && prevCounterFCId != 0)
                                                    {
                                                        counterExistingFC += 1;
                                                    }

                                                }
                                                checkAssign.CallById = FCReady[counterExistingFC].UserId;
                                                checkAssign.CallResultId = 10;
                                                prevCounterFCId = counterExistingFC;
                                                this.ctx.CollectionCall.Update(checkAssign);
                                                this.ctx.SaveChanges();


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
                            else
                            {
                                var firstAssign = this.ctx.MasterLoanHistory
                                    .Where(q => q.LoanId.Equals(checkAssign.LoanId))
                                    .Where(q => q.CallById.Equals(checkAssign.CallById))
                                    .OrderBy(o => o.STG_DATE)
                                    .Take(1)
                                    .FirstOrDefault();

                                if (firstAssign != null)
                                {
                                    var assignDate = firstAssign!.STG_DATE;
                                    var lastSevenDay = DateTime.Now.AddDays(-7);
                                    if (assignDate < lastSevenDay)
                                    {
                                        this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                                        var role = checkAssign.CallBy!.RoleId;

                                        //Ini DC
                                        if (role == 3)
                                        {
                                            var DCReady = this.ctx.UserBranch.Where(o => o.UserId != checkAssign.CallById)
                                                                  .Where(q => q.BranchId.Equals(checkAssign.BranchId))
                                                                  .Where(o => o.User!.RoleId!.Equals(role))
                                                                  .OrderBy(o => o.UserId)
                                                                  .ToList();
                                            if (DCReady != null && DCReady.Count() > 0)
                                            {
                                                if (counterExistingDC == prevCounterDCId)
                                                {
                                                    if (counterExistingDC != 0 && prevCounterDCId != 0)
                                                    {
                                                        counterExistingDC += 1;
                                                    }

                                                }
                                                checkAssign.CallById = DCReady[counterExistingDC].UserId;
                                                checkAssign.CallResultId = 10;
                                                prevCounterDCId = counterExistingDC;
                                                this.ctx.CollectionCall.Update(checkAssign);
                                                this.ctx.SaveChanges();
                                                counterExistingDC += 1;
                                                if (counterExistingDC == countDC - 1)
                                                {
                                                    counterExistingFC = 0;
                                                }
                                            }
                                        }
                                        //ELSE FC
                                        else if (role == 4)
                                        {
                                            var FCReady = this.ctx.UserBranch.Where(o => o.UserId != checkAssign.CallById)
                                                                  .Where(o => o.User!.RoleId!.Equals(role))
                                                                  .OrderBy(o => o.UserId).ToList();

                                            if (FCReady != null && FCReady.Count() > 0)
                                            {
                                                if (counterExistingFC == prevCounterFCId)
                                                {
                                                    counterExistingFC += 1;
                                                }
                                                checkAssign.CallById = FCReady[counterExistingFC].UserId;
                                                checkAssign.CallResultId = 10;
                                                prevCounterFCId = counterExistingFC;
                                                this.ctx.CollectionCall.Update(checkAssign);
                                                this.ctx.SaveChanges();
                                                counterExistingFC += 1;
                                                if (counterExistingFC == countFC - 1)
                                                {
                                                    counterExistingFC = 0;
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                                        var role = checkAssign.CallBy!.RoleId;

                                        if (role == 3)
                                        {
                                            var cnt = this.ctx.CollectionHistory.Where(q => q.AccNo!.Equals(checkAssign.AccNo))
                                                                      .Where(q => q.CallById.Equals(checkAssign.CallById))
                                                                      .Count();
                                            if (cnt > 2)
                                            {
                                                var DCReady = this.ctx.UserBranch.Where(o => o.UserId != checkAssign.CallById)
                                                                      .Where(o => o.User!.RoleId!.Equals(role))
                                                                      .OrderBy(o => o.UserId)
                                                                      .ToList();
                                                if (DCReady != null && DCReady.Count() > 0)
                                                {
                                                    if (counterExistingDC == prevCounterDCId)
                                                    {
                                                        counterExistingDC += 1;
                                                    }
                                                    checkAssign.CallById = DCReady[counterExistingDC].UserId;
                                                    checkAssign.CallResultId = 10;
                                                    prevCounterDCId = counterExistingDC;
                                                    this.ctx.CollectionCall.Update(checkAssign);
                                                    this.ctx.SaveChanges();
                                                    counterExistingDC += 1;
                                                    if (counterExistingDC == countDC - 1)
                                                    {
                                                        counterExistingFC = 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            var mlh = new MasterLoanHistory();
                            mlh.LoanId = data.Id;
                            mlh.CallById = checkAssign.CallById;
                            mlh.STG_DATE = DateTime.Now;
                            this.CopyLoanHistory(data, mlh);
                            this.ctx.MasterLoanHistory.Add(mlh);
                            this.ctx.SaveChanges();
                        }
                        else //data baru
                        {
                            //FC: DPD > 14 & <= 90 OR Tidak bisa dihubungi 7 hari
                            //DPD > 14 & <= 90
                            if (data.Dpd > 14 && data.Dpd <= 90)
                            {
                                if (fcCabang.Count < 1)
                                {
                                    continue;
                                }

                                var callFC = new CollectionCall();
                                callFC.LoanId = data.Id;
                                callFC.BranchId = cabang.Id;
                                callFC.AccNo = data.AccNo;
                                callFC.CallById = fcCabang[counterFC].UserId;
                                callFC.CallResultId = 10;
                                this.ctx.CollectionCall.Add(callFC);
                                this.ctx.SaveChanges();
                                counterFC += 1;
                                if (counterFC == countFC)
                                {
                                    counterFC = 0;
                                }

                                var mlh = new MasterLoanHistory();
                                mlh.LoanId = data.Id;
                                mlh.CallById = callFC.CallById;
                                mlh.STG_DATE = DateTime.Now;
                                this.CopyLoanHistory(data, mlh);
                                this.ctx.MasterLoanHistory.Add(mlh);
                                this.ctx.SaveChanges();
                            }
                            else
                            {
                                if (dcCabang.Count < 1)
                                {
                                    continue;
                                }
                                //DC
                                var callDC = new CollectionCall();
                                callDC.LoanId = data.Id;
                                callDC.BranchId = cabang.Id;
                                callDC.AccNo = data.AccNo;
                                callDC.CallById = dcCabang[counterDC].Id;
                                callDC.CallResultId = 10;
                                this.ctx.CollectionCall.Add(callDC);
                                this.ctx.SaveChanges();
                                counterDC += 1;
                                if (counterDC == countDC)
                                {
                                    counterDC = 0;
                                }

                                var mlh = new MasterLoanHistory();
                                mlh.LoanId = data.Id;
                                mlh.CallById = callDC.CallById;
                                mlh.STG_DATE = DateTime.Now;
                                this.CopyLoanHistory(data, mlh);
                                this.ctx.MasterLoanHistory.Add(mlh);
                                this.ctx.SaveChanges();
                            }
                        }
                    }
                }

            }
        }

        public void DistributinReset()
        {

            var dateProcess = DateTime.Today;
            var stoday = dateProcess.ToString("yyyy-MM-dd");
            this.ctx.Database.ExecuteSqlRaw("UPDATE MASTER_LOAN SET STATUS = 0 WHERE STG_DATE != '" + stoday + "'");

        }

        public void DistributinDataDC()
        {
            this.logger.LogInformation("DistributionDataDC >>> started");

            var dateProcess = DateTime.Today;
            var todayData = this.ctx.MasterLoan.Where(o => o.STG_DATE == dateProcess)
                                                .Where(o => o.Dpd < 15)
                                                .ToList();//data hari ini

            var dcCabang = this.ctx.User.Where(q => q.RoleId == 3).Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();

            this.logger.LogInformation("DistributionDataDC >>> num of data: " + todayData.Count);

            if (todayData.Count > 0)
            {
                int countDC = dcCabang.Count;
                int counterDC = 0;
                int counterExistingDC = 0; 
                int prevCounterDCId = 0;

                foreach (var data in todayData)
                {
                    this.ctx.Entry(data).Reference(r => r.Customer).Load();

                    this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id);
                    var checkAssign = this.ctx.CollectionCall.Where(o => o.LoanId == data.Id)
                                            .Include(i => i.CallBy)
                                            .FirstOrDefault();
                    if (checkAssign != null) //sudah pernah di assign, data sudah pernah turun
                    {
                        this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id + " existing");

                        this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id + " check existing role");
                        this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                        if (checkAssign.CallBy!.RoleId! == 3)
                        {
                            this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id + " check existing role is DC usual route");

                            var lastSevenDay = DateTime.Now.AddDays(-7);
                            var firstAssign = this.ctx.MasterLoanHistory
                                .Where(q => q.LoanId.Equals(checkAssign.LoanId))
                                .Where(q => q.CallById.Equals(checkAssign.CallById))
                                .Where(q => q.STG_DATE!.Value.Date >= lastSevenDay.Date)
                                .Count();

                            this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id + " count first assign: " + firstAssign);

                            if (firstAssign > 0)
                            {
                                if (firstAssign >= 7)
                                {
                                    this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id + " count first assign: " + firstAssign + " rotate");
                                    this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                                    var role = checkAssign.CallBy!.RoleId;

                                    var DCReady = this.ctx.User.Where(o => o.Id != checkAssign.CallById)
                                                          .Where(o => o.RoleId!.Equals(3))
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
                                        counterExistingDC += 1;
                                        if (counterExistingDC == countDC - 1)
                                        {
                                            counterExistingDC = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id + " count first assign: " + firstAssign + " still same");
                                    this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                                    var role = checkAssign.CallBy!.RoleId;

                                    var cnt = this.ctx.CollectionHistory.Where(q => q.AccNo!.Equals(checkAssign.AccNo))
                                                              .Where(q => q.CallById.Equals(checkAssign.CallById))
                                                              .Count();
                                    if (cnt > 2)
                                    {
                                        var DCReady = this.ctx.User.Where(o => o.Id != checkAssign.CallById)
                                                              .Where(o => o.RoleId!.Equals(3))
                                                              .OrderBy(o => o.Id)
                                                              .ToList();
                                        if (DCReady != null && DCReady.Count() > 0)
                                        {
                                            var xcnt = DCReady.Count();

                                            if (counterExistingDC == prevCounterDCId)
                                            {
                                                counterExistingDC += 1;
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
                                            counterExistingDC += 1;
                                            if (counterExistingDC == countDC - 1)
                                            {
                                                counterExistingDC = 0;
                                            }
                                        }
                                    }
                                }
                            }

                            var mlh = new MasterLoanHistory();
                            mlh.LoanId = data.Id;
                            mlh.CallById = checkAssign.CallById;
                            mlh.STG_DATE = DateTime.Now;
                            this.CopyLoanHistory(data, mlh);
                            this.ctx.MasterLoanHistory.Add(mlh);
                            this.ctx.SaveChanges();
                        }
                        else
                        {
                            this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id + " check existing role is FC, treat as new");

                            this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                            var role = checkAssign.CallBy!.RoleId;

                            var DCReady = this.ctx.User.Where(o => o.Id != checkAssign.CallById)
                                                  .Where(o => o.RoleId!.Equals(3))
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

                                var prevCaller = checkAssign.CallById;

                                checkAssign.CallById = DCReady[counterExistingDC].Id;
                                checkAssign.CallResultId = 10;
                                prevCounterDCId = counterExistingDC;
                                this.ctx.CollectionCall.Update(checkAssign);
                                this.ctx.SaveChanges();

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

                                //var stv2 = this.GetPaymentFromT24(data.AccNo!, data.LoanNumber!, tdy);
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

                                //var nw = pay.Tgl.Value.Date;
                                //var prv = this.ctx.PaymentHistory.Where(q => q.AccNo!.Equals(data.AccNo))
                                //                        .Where(q => q.Tgl!.Value.Date.Equals(nw)).FirstOrDefault();
                                //if (prv == null)
                                //{
                                //    this.ctx.PaymentHistory.Add(pay);
                                //    this.ctx.SaveChanges();
                                //}


                                // update generate letter
                                //var gl = this.ctx.GenerateLetter.Where(q => q.LoanId.Equals(data.Id)).Where(q => q.StatusId.Equals(1)).FirstOrDefault();
                                //if (gl != null)
                                //{
                                //    gl.StatusId = 2;
                                //    this.ctx.GenerateLetter.Update(gl);
                                //    this.ctx.SaveChanges();
                                //}

                                counterExistingDC += 1;
                                if (counterExistingDC == countDC - 1)
                                {
                                    counterExistingDC = 0;
                                }
                            }
                        }

                    }
                    else //data baru
                    {
                        this.logger.LogInformation("DistributionDataDC >>> process loanid: " + data.Id);
                        if (dcCabang.Count < 1)
                        {
                            continue;
                        }
                        //DC
                        var callDC = new CollectionCall();
                        callDC.LoanId = data.Id;
                        callDC.BranchId = data.Customer!.BranchId!;
                        callDC.AccNo = data.AccNo;
                        callDC.CallById = dcCabang[counterDC].Id;
                        callDC.CallResultId = 10;
                        this.ctx.CollectionCall.Add(callDC);
                        this.ctx.SaveChanges();
                        counterDC += 1;
                        if (counterDC == countDC)
                        {
                            counterDC = 0;
                        }

                        var mlh = new MasterLoanHistory();
                        mlh.LoanId = data.Id;
                        mlh.CallById = callDC.CallById;
                        mlh.STG_DATE = DateTime.Now;
                        this.CopyLoanHistory(data, mlh);
                        this.ctx.MasterLoanHistory.Add(mlh);
                        this.ctx.SaveChanges();

                    }

                    data.Status = 1;
                    this.ctx.MasterLoan.Update(data);
                    this.ctx.SaveChanges();
                }
            }

            this.logger.LogInformation("DistributionDataDC >>> finished");
        }

        public void DistributinDataFC()
        {
            this.logger.LogInformation("DistributionDataFC >>> started");
            var dateProcess = DateTime.Today;
            var todayData = this.ctx.MasterLoan.Where(o => o.STG_DATE == dateProcess)
                                                .Where(o => o.Dpd > 14);

            //var allBranch = this.ctx.Branch.Where(q => q.Id.Equals(1)).ToList();
            var allBranch = this.ctx.Branch.OrderBy(o => o.Id).ToList();

            foreach (var cabang in allBranch)
            {

                this.logger.LogInformation("DistributionDataFC >>> process cabang: " + cabang.Code);
                var dataCabang = todayData.Where(o => o.ChannelBranchCode!.ToLower().Equals(cabang!.Code!.ToLower())).OrderBy(o => o.Cif).ToList();
                //var dataCabang = todayData.Where(q => q.Cif.Equals("1000171471")).OrderBy(o => o.Cif).ToList();

                if (dataCabang.Count > 0)
                {
                    this.logger.LogInformation("DistributionDataFC >>> process cabang: " + cabang.Code + " di process");

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
                        this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id);
                        var checkAssign = this.ctx.CollectionCall.FirstOrDefault(o => o.LoanId == data.Id);
                        if (checkAssign != null) //sudah pernah di assign, data sudah pernah turun
                        {
                            this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id + " " + data.AccNo + " dpd = " + data.Dpd + " existing");

                            this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                            var role = checkAssign.CallBy!.RoleId;

                            if (role == 3)
                            {
                                this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id + " prev is dc");
                                var FCReady = this.ctx.UserBranch.Where(o => o.User!.RoleId!.Equals(4))
                                                       .Where(q => q.BranchId.Equals(cabang.Id))
                                                       .OrderBy(o => o.UserId).ToList();

                                this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id + " fc count: " + FCReady.Count());
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

                                    this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id + " " + data.AccNo + " dpd = " + data.Dpd + " existing assigned to " + checkAssign.CallById);

                                    counterExistingFC += 1;
                                    if (counterExistingFC == countFC - 1)
                                    {
                                        counterExistingFC = 0;
                                    }
                                }
                            }
                            else
                            {
                                this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id + " check last 7 day");

                                var lastSevenDay = DateTime.Now.AddDays(-25);
                                var firstAssign = this.ctx.MasterLoanHistory
                                    .Where(q => q.LoanId.Equals(checkAssign.LoanId))
                                    .Where(q => q.CallById.Equals(checkAssign.CallById))
                                    .Where(q => q.STG_DATE!.Value.Date >= lastSevenDay.Date)
                                    .Count();

                                if (firstAssign > 0)
                                {
                                    this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id + " last 7 day not found");

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
                                        this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id + " assign to " + checkAssign.CallById);

                                        counterExistingFC += 1;


                                        if (counterExistingFC == countFC - 1)
                                        {
                                            counterExistingFC = 0;
                                        }

                                    }
                                }
                                else
                                {
                                    this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id + " last 7 day found reassign");
                                    var FCReady = this.ctx.UserBranch.Where(o => o.UserId != checkAssign.CallById)
                                                          .Where(o => o.User!.RoleId!.Equals(role))
                                                          .Where(q => q.BranchId.Equals(checkAssign.BranchId))
                                                          .OrderBy(o => o.UserId).ToList();

                                    if (FCReady != null && FCReady.Count() > 0)
                                    {
                                        if (counterExistingFC == prevCounterFCId)
                                        {
                                            if (counterExistingFC != 0 && prevCounterFCId != 0)
                                            {
                                                counterExistingFC += 1;
                                            }

                                        }

                                        if (FCReady.Count() <= counterExistingFC)
                                        {
                                            counterExistingFC = 0;
                                        }
                                        checkAssign.CallById = FCReady[counterExistingFC].UserId;
                                        checkAssign.CallResultId = 10;
                                        prevCounterFCId = counterExistingFC;
                                        this.ctx.CollectionCall.Update(checkAssign);
                                        this.ctx.SaveChanges();
                                        this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id + " assign to " + checkAssign.CallById);

                                        counterExistingFC += 1;
                                        if (counterExistingFC == countFC - 1)
                                        {
                                            counterExistingFC = 0;
                                        }

                                    }
                                }
                            }

                            var mlh = new MasterLoanHistory();
                            mlh.LoanId = data.Id;
                            mlh.CallById = checkAssign.CallById;
                            mlh.STG_DATE = DateTime.Now;
                            this.CopyLoanHistory(data, mlh);
                            this.ctx.MasterLoanHistory.Add(mlh);
                            this.ctx.SaveChanges();
                        }
                        else //data baru
                        {

                            if (fcCabang.Count < 1)
                            {
                                continue;
                            }

                            this.logger.LogInformation("DistributionDataFC >>> process loanid: " + data.Id + " " + data.AccNo + " dpd = " + data.Dpd + " new");
                            var callFC = new CollectionCall();
                            callFC.LoanId = data.Id;
                            callFC.BranchId = cabang.Id;
                            callFC.AccNo = data.AccNo;
                            callFC.CallById = fcCabang[counterFC].UserId;
                            callFC.CallResultId = 10;
                            this.ctx.CollectionCall.Add(callFC);
                            this.ctx.SaveChanges();
                            counterFC += 1;
                            if (counterFC == countFC)
                            {
                                counterFC = 0;
                            }

                            var mlh = new MasterLoanHistory();
                            mlh.LoanId = data.Id;
                            mlh.CallById = callFC.CallById;
                            mlh.STG_DATE = DateTime.Now;
                            this.CopyLoanHistory(data, mlh);
                            this.ctx.MasterLoanHistory.Add(mlh);
                            this.ctx.SaveChanges();
                        }

                        data.Status = 1;
                        this.ctx.MasterLoan.Update(data);
                        this.ctx.SaveChanges();
                    }
                }

            }
            this.logger.LogInformation("DistributionDataFC >>> finished");
        }


        private async Task<bool> ProcessByBranch(int id, string code)
        {

            this.logger.LogInformation("DistributinData >>> process cabang: " + code + " di process");

            var dateProcess = DateTime.Today;

            var dcCabang = this.ctx.UserBranch.Where(q => q.Branch!.Id == id).Where(q => q.User!.Role!.Id == 3)
                                    .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                                    .Include(i => i.User)
                                    .Select(s => s.User)
                                    .ToList();

            var fcCabang = this.ctx.UserBranch.Where(q => q.Branch!.Id == id).Where(q => q.User!.Role!.Id == 4)
                                   .Where(q => q.User!.Status!.Name!.Equals("AKTIF"))
                                   .Include(i => i.User)
                                   .Select(s => s.User)
                                   .ToList();



            //Ambil Data DC
            var dataDC = this.ctx.MasterLoan.Where(o => o.STG_DATE == dateProcess)
                                            .Where(o => o.ChannelBranchCode!.Equals(code))
                                            .Where(o => o.Dpd >= 1)
                                            .Where(o => o.Dpd <= 14)
                                            .OrderBy(o => o.Cif)
                                            .ToList();//data hari ini

            if (dataDC.Count < 1) 
            {
                return false;
            }

            int counterDC = 0;
            int countDC = dcCabang.Count;
            int lastDC = 0;
            string lastCif = "";

            foreach (var item in dataDC)
            {

                var checkAssign = this.ctx.CollectionCall.Where(o => o.LoanId == item.Id).FirstOrDefault();
                if (checkAssign != null) //sudah pernah di assign, data sudah pernah turun
                {
                    this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                    var role = checkAssign.CallBy!.RoleId;
                    var dpd = item.Dpd;

                    //Sudah lewat 7 hari
                    if (dpd > 7)
                    {
                        if (lastDC ==  checkAssign.CallById)
                        {
                            counterDC++;
                            checkAssign.CallById = dcCabang![counterDC].Id.Value;
                            checkAssign.CallResultId = 10;
                            this.ctx.CollectionCall.Update(checkAssign);
                            this.ctx.SaveChanges();
                            counterDC += 1;
                            lastDC = checkAssign.CallById!.Value;
                            lastCif = item.Cif!;
                            if (counterDC == countDC)
                            {
                                counterDC = 0;
                            }

                            var mlh = new MasterLoanHistory();
                            mlh.LoanId = item.Id;
                            mlh.CallById = checkAssign.CallById;
                            mlh.STG_DATE = DateTime.Now;
                            this.CopyLoanHistory(item, mlh);
                            this.ctx.MasterLoanHistory.Add(mlh);
                            this.ctx.SaveChanges();

                        } 
                        else
                        {
                            var hist = this.ctx.MasterLoanHistory
                                                .Where(q => q.LoanId.Equals(checkAssign.LoanId))
                                                .Where(q => q.CallById.Equals(checkAssign.CallById))
                                                .OrderBy(o => o.STG_DATE)
                                                .Take(1)
                                                .ToList();

                            if (hist != null && hist.Count() > 0)
                            {
                                var hi = hist[0];
                                var pd = hi.STG_DATE;
                                var lastSevenDay = DateTime.Now.AddDays(-7);
                                if (lastSevenDay < pd)
                                {
                                    var c0 = dataDC[counterDC].Id;
                                    if (c0 == lastDC)
                                    {
                                        counterDC++;
                                    }
                                    checkAssign.CallById = dcCabang[counterDC].Id.Value;
                                    checkAssign.CallResultId = 10;
                                    this.ctx.CollectionCall.Update(checkAssign);
                                    this.ctx.SaveChanges();
                                    counterDC += 1;
                                    lastDC = checkAssign.CallById!.Value;
                                    lastCif = item.Cif!;
                                    if (counterDC == countDC)
                                    {
                                        counterDC = 0;
                                    }

                                    var mlh = new MasterLoanHistory();
                                    mlh.LoanId = item.Id;
                                    mlh.CallById = checkAssign.CallById;
                                    mlh.STG_DATE = DateTime.Now;
                                    this.CopyLoanHistory(item, mlh);
                                    this.ctx.MasterLoanHistory.Add(mlh);
                                    this.ctx.SaveChanges();
                                }

                            } 
                            else
                            {
                                checkAssign.CallById = dcCabang[counterDC].Id.Value;
                                checkAssign.CallResultId = 10;
                                this.ctx.CollectionCall.Update(checkAssign);
                                this.ctx.SaveChanges();
                                counterDC += 1;
                                lastDC = checkAssign.CallById!.Value;
                                lastCif = item.Cif!;
                                if (counterDC == countDC)
                                {
                                    counterDC = 0;
                                }

                                var mlh = new MasterLoanHistory();
                                mlh.LoanId = item.Id;
                                mlh.CallById = checkAssign.CallById;
                                mlh.STG_DATE = DateTime.Now;
                                this.CopyLoanHistory(item, mlh);
                                this.ctx.MasterLoanHistory.Add(mlh);
                                this.ctx.SaveChanges();
                            }
                        }

                    }
                }
                else
                {
                    //DC
                    var callDC = new CollectionCall();
                    callDC.LoanId = item.Id;
                    callDC.BranchId = item.Id;
                    callDC.AccNo = item.AccNo;
                    callDC.CallById = dcCabang[counterDC].Id.Value;
                    callDC.CallResultId = 10;
                    this.ctx.CollectionCall.Add(callDC);
                    this.ctx.SaveChanges();
                    counterDC += 1;
                    lastDC = callDC.CallById!.Value;
                    lastCif = item.Cif!;
                    if (counterDC == countDC)
                    {
                        counterDC = 0;
                    }

                    var mlh = new MasterLoanHistory();
                    mlh.LoanId = item.Id;
                    mlh.CallById = callDC.CallById;
                    mlh.STG_DATE = DateTime.Now;
                    this.CopyLoanHistory(item, mlh);
                    this.ctx.MasterLoanHistory.Add(mlh);
                    this.ctx.SaveChanges();
                }
            }

            //Ambil Data FC
            var dataFC = this.ctx.MasterLoan.Where(o => o.STG_DATE == dateProcess)
                                            .Where(o => o.ChannelBranchCode!.Equals(code))
                                            .Where(o => o.Dpd > 14)
                                            .OrderBy(o => o.Cif)
                                            .ToList();//data hari ini

            if (dataFC.Count < 1)
            {
                return false;
            }

            int counterFC = 0;
            int countFC = fcCabang.Count;
            int lastFC = 0;
            lastCif = "";

            foreach (var item in dataFC)
            {

                var checkAssign = this.ctx.CollectionCall.Where(o => o.LoanId == item.Id).FirstOrDefault();
                if (checkAssign != null) //sudah pernah di assign, data sudah pernah turun
                {
                    this.ctx.Entry(checkAssign).Reference(r => r.CallBy).Load();
                    var role = checkAssign.CallBy!.RoleId;
                    var dpd = item.Dpd;

                    var hist = this.ctx.MasterLoanHistory
                                        .Where(q => q.LoanId.Equals(checkAssign.LoanId))
                                        .Where(q => q.CallById.Equals(checkAssign.CallById))
                                        .OrderBy(o => o.STG_DATE)
                                        .Take(1)
                                        .ToList();

                    if (hist != null && hist.Count() > 0)
                    {
                        var hi = hist[0];
                        var pd = hi.STG_DATE;
                        var lastSevenDay = DateTime.Now.AddDays(-7);
                        if (lastSevenDay < pd)
                        {
                            var c0 = dataFC[counterFC].Id;
                            if (c0 == lastFC)
                            {
                                counterFC++;
                            }
                            checkAssign.CallById = fcCabang[counterFC].Id.Value;
                            checkAssign.CallResultId = 10;
                            this.ctx.CollectionCall.Update(checkAssign);
                            this.ctx.SaveChanges();
                            counterDC += 1;
                            lastDC = checkAssign.CallById!.Value;
                            lastCif = item.Cif!;
                            if (counterFC == countFC)
                            {
                                counterFC = 0;
                            }

                            var mlh = new MasterLoanHistory();
                            mlh.LoanId = item.Id;
                            mlh.CallById = checkAssign.CallById;
                            mlh.STG_DATE = DateTime.Now;
                            this.CopyLoanHistory(item, mlh);
                            this.ctx.MasterLoanHistory.Add(mlh);
                            this.ctx.SaveChanges();
                        }

                    }
                    else
                    {
                        checkAssign.CallById = fcCabang[counterFC].Id.Value;
                        checkAssign.CallResultId = 10;
                        this.ctx.CollectionCall.Update(checkAssign);
                        this.ctx.SaveChanges();
                        counterFC += 1;
                        lastDC = checkAssign.CallById!.Value;
                        lastCif = item.Cif!;
                        if (counterFC == countFC)
                        {
                            counterFC = 0;
                        }

                        var mlh = new MasterLoanHistory();
                        mlh.LoanId = item.Id;
                        mlh.CallById = checkAssign.CallById;
                        mlh.STG_DATE = DateTime.Now;
                        this.CopyLoanHistory(item, mlh);
                        this.ctx.MasterLoanHistory.Add(mlh);
                        this.ctx.SaveChanges();
                    }
                }
                else
                {
                    var callFC = new CollectionCall();
                    callFC.LoanId = item.Id;
                    callFC.BranchId = item.Id;
                    callFC.AccNo = item.AccNo;
                    callFC.CallById = fcCabang[counterFC].Id.Value;
                    callFC.CallResultId = 10;
                    this.ctx.CollectionCall.Add(callFC);
                    this.ctx.SaveChanges();
                    counterFC += 1;
                    lastFC = callFC.CallById!.Value;
                    lastCif = item.Cif!;
                    if (counterFC == countFC)
                    {
                        counterFC = 0;
                    }

                    var mlh = new MasterLoanHistory();
                    mlh.LoanId = item.Id;
                    mlh.CallById = callFC.CallById;
                    mlh.STG_DATE = DateTime.Now;
                    this.CopyLoanHistory(item, mlh);
                    this.ctx.MasterLoanHistory.Add(mlh);
                    this.ctx.SaveChanges();
                }
            }

            return true;

        }

        public void SMSReminder()
        {
            this.logger.LogInformation("SMSReminder >>> start");
            var dateProcess = DateTime.Today;
            var sms = this.ctx.NotifContent.Where(q => q.Status!.Name!.Equals("AKTIF")).OrderBy(o => o.Day).ToList();
            if (sms != null && sms.Count> 0)
            {
                foreach(var i in sms)
                {
                    var data = this.ctx.STGSmsReminder.Where(o => o.STG_DATE == dateProcess).Where(o => o.DAY.Equals(i.Day)).ToList();
                    if (data != null && data.Count > 0)
                    {
                        foreach(var y in data)
                        {
                            var msg = i.Content;
                            var nm = y.NAMA;
                            var dt = y.DUE_DATE!.Value.ToString("d");

                            msg = msg.Replace("@1", nm);
                            msg = msg.Replace("@2", dt);

                            this.logger.LogInformation("SMSReminder >>> prepare to send sms");

                            this.intService.SendSms("081280498835", msg);
                            this.intService.SendSms("085263821007", msg);
                        }
                    }
                }
            }
        }

        public void GenerateLetter()
        {
            var ml = this.ctx.MasterLoan.Where(q => q.Status.Equals(1)).ToList();
            foreach(var itm in ml)
            {
                var par = new GenerateLetterRequestBean();
                par.LoanId = itm.Id;
                //this.generateLetterService.ListGenerateLetter(par);
            }
        }

        public void CopyLoanNumber()
        {
            var stg = this.ctx.STGDataKredit.ToList();
            if (stg.Count > 0)
            {
                var cnt = 0;
                foreach (var itm in stg)
                {
                    var ml = this.ctx.MasterLoan.Where(q => q.AccNo == itm.ACC_NO).Where(q => q.Cif == itm.CU_CIF).Where(q => q.LoanNumber == null).ToList();
                    if (ml.Count > 0)
                    {
                        var lm = ml[0];
                        lm.LoanNumber = itm.LOAN_NUMBER;
                        this.ctx.MasterLoan.Update(lm);
                        cnt++;
                    }

                    if (cnt > 100)
                    {
                        this.ctx.SaveChanges();
                        cnt = 0;
                    }
                }

            }

        }

        public void UpdateLoanNumber()
        {
            var ml = this.ctx.MasterLoan.Where(q => q.LoanNumber == null).ToList();
            var cnt = 0;
            if (ml.Count > 0)
            {
                foreach (var itx in ml)
                {
                    var stg = this.ctx.STGDataKredit.Where(q => q.ACC_NO == itx.AccNo).Where(q => q.CU_CIF == itx.Cif).ToList();
                    if (stg.Count > 0)
                    {
                        var lm = stg[0];
                        itx.LoanNumber = lm.LOAN_NUMBER;
                        this.ctx.MasterLoan.Update(itx);
                        cnt++;
                    }

                    if (cnt > 100)
                    {
                        this.ctx.SaveChanges();
                        cnt = 0;
                    }
                }
            }
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

        public StatementsBeanV2 GetPaymentFromT24Remote(string accNo, string date)
        {
            var client = new RestClient("https://collectionsys.ag.co.id/");

            string vars = "backend/api/Collection/checkdailypayment?AccountNo="+accNo+"&Date=" + date;
            this.logger.LogInformation("url: " + vars);

            var request = new RestRequest(vars, Method.Get);
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJDb2xsZWN0aXVtSldUU2VydmljZUFjY2Vzc1Rva2VuIiwianRpIjoiNmIyMGFlNGUtOGFjZi00YThjLWI5MzEtZmZhOTU2MmE4ZGUwIiwiaWF0IjoiMDMvMDMvMjAyMyAxNC4yMy4yNCIsIk5hbWUiOiJTaXNkdXIiLCJUb2tlbiI6InBkVHlRa0liTEVLU3JYZXpqZTJSenc9PSIsImV4cCI6MTY3ODcxNzQwNCwiaXNzIjoiQ29sbGVjdGl1bUpXVEF1dGhlbnRpY2F0aW9uU2VydmVyIiwiYXVkIjoiQ29sbGVjdGl1bUpXVFNlcnZpY2VQb3N0bWFuQ2xpZW50In0.sCgv1DunGV9w1P0FK8T3UkS4SDbewZy4PbVyma1ntfo");
            var res = client.Execute(request);
            var tmp = res.Content!.ToString();
            this.logger.LogInformation("response from core: " + tmp);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var obj = JsonSerializer.Deserialize<GenericResponse<PaymentResponseBean>>(tmp, options);
            if (obj!.Status! == true && obj!.Data != null)
            {
                var pays = obj.Data.ToList()[0].Payment;
                foreach(var pay in pays!)
                {
                    if (pay!.Status!.ToLower().Equals("settled"))
                    {
                        return pay;
                    }
                }
            }

            return null;
        }

        public StatementsBeanV2 GetPaymentFromT24(string accNo, string loanNumber, string date)
        {

            var obj = this.intService.CheckDailyPayment(accNo, loanNumber, date, date);
            if (obj!.Status! == true && obj!.Data != null)
            {
                var pays = obj.Data.ToList()[0].Payment;
                foreach (var pay in pays!)
                {
                    if (pay!.Status!.ToLower().Equals("settled"))
                    {
                        return pay;
                    }
                }
            }

            return null;
        }
    }

}
