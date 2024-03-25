using Collectium.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Collectium.Model.Helper;
using Collectium.Service;
using Collectium.Config;
using Quartz;
using Collectium.Job;
using Collectium.Model.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "CollectiumAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<CollectiumDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollectiumDatabase"));
});

builder.Services.AddDbContext<CollectiumDBStgContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("CollectiumDatabaseStg"));
});

builder.Services.AddTransient<StatusService, StatusService>();
builder.Services.AddTransient<PaginationHelper, PaginationHelper>();
builder.Services.AddTransient<RoleMenuService, RoleMenuService>();
builder.Services.AddTransient<UserService, UserService>();
builder.Services.AddTransient<InitialService, InitialService>();
builder.Services.AddTransient<ActionGroupService, ActionGroupService>();
builder.Services.AddTransient<ToolService, ToolService>();
builder.Services.AddTransient<BranchAreaService, BranchAreaService>();
builder.Services.AddTransient<CollectionService, CollectionService>();
builder.Services.AddTransient<TxMasterService, TxMasterService>();
builder.Services.AddTransient<ScriptingService, ScriptingService>();
builder.Services.AddTransient<GenerateLetterService, GenerateLetterService>();
builder.Services.AddTransient<IntegrationService, IntegrationService>();
builder.Services.AddTransient<StagingService, StagingService>();
builder.Services.AddTransient<DistribusiDataService, DistribusiDataService>();
builder.Services.AddTransient<RejigService, RejigService>();
builder.Services.AddTransient<CallTraceService, CallTraceService>();
builder.Services.AddTransient<RestructureService, RestructureService>();
builder.Services.AddTransient<AuctionService, AuctionService>();
builder.Services.AddTransient<InsuranceService, InsuranceService>();
builder.Services.AddTransient<AydaService, AydaService>();
builder.Services.AddTransient<GeneralParameterService, GeneralParameterService>();
builder.Services.AddTransient<BucketService, BucketService>();
builder.Services.AddTransient<RuleEngineService, RuleEngineService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    );

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddQuartz(q =>{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var conconcurrentJobKey = new JobKey("ConconcurrentJob");
    q.AddJob<Every5MinuteJob>(opts => opts.WithIdentity(conconcurrentJobKey));

    //q.AddTrigger(opts => opts
    //    .ForJob(conconcurrentJobKey)
    //    .WithIdentity("ConconcurrentJob-trigger")
    //    .WithSimpleSchedule(x => x
    //        .WithIntervalInSeconds(5)
    //        .RepeatForever()));

    q.AddTrigger(opts => opts
         .ForJob(conconcurrentJobKey)
         .WithIdentity("Daily7AM")
         .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(7, 1)));

});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


//builder.WebHost.UseIISIntegration();

var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseMiddleware<JWTMiddleware>();

//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("corsapp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
