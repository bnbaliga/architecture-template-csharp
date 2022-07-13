using Vertical.Product.Service.Manager;
using Core.Logging.Serilog;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Vertical.Product.Service.Contract.AppSettings;
using Vertical.Product.Service.Data.Dependency;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
                },
                new List<string>()
            } });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd", "Bearer", true);
builder.Services.AddAuthorization();

builder.Services.AddCors(options => options.AddPolicy("allowAny", o => o.AllowAnyOrigin()));

Configurations(builder);

//register MediatR
ManagerDependency.Register(builder.Services, builder.Configuration);
//Register Serilog
SeriLogDependency.Register(builder);
//Add Distributed Cache
//AddDistributedCache(builder);

//builder.Services.AddDataContext(builder.Configuration.GetSection(DataConfiguration.DBConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();

//app.Use(async (context, next) =>
//{
//    if (!context.User.Identity?.IsAuthenticated ?? false)
//    {
//        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
//        await context.Response.WriteAsync("Not authenticated");
//    }
//    else await next();
//});

app.MapDefaultControllerRoute();
app.MapControllers();
//HttpClient.DefaultProxy = new WebProxy(new Uri("http://localhost:8866"));

app.Run();

//static void AddDistributedCache(WebApplicationBuilder builder)
//{
//    //optional
//    builder.Services.AddDistributedMemoryCache(options =>
//    {
//        options.SizeLimit = 2000 * 1024 * 1024;
//    });
//    //Register for IOptionsMonitor usage
//    builder.Services.Configure<DistributedCacheOptions>(builder.Configuration.GetSection(DistributedCacheOptions.DistributedCache));
//    builder.Services.Configure<AzureAD>(builder.Configuration.GetSection(AzureAD.ADName));
//}

static void Configurations(WebApplicationBuilder builder)
{
    builder.Services.Configure<AzureAD>(builder.Configuration.GetSection(AzureAD.ADName));
    builder.Services.Configure<DataConfiguration>(builder.Configuration.GetSection(DataConfiguration.DBConnectionString));
}