using HotelManagementAPI.Data;
using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration().MinimumLevel.Error().WriteTo.File("Log/log.txt", rollingInterval:RollingInterval.Month).CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddDbContext<HotelManagementContext>(options => options.UseSqlServer("Server=.;Database=HotelManagement;Trusted_Connection=true;TrustServerCertificate=True;MultipleActiveResultSets=true"));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Registering stores
builder.Services.AddScoped<IAccountTypeStore, AccountTypeStore>();
builder.Services.AddScoped<IBookingStore, BookingStore>();
builder.Services.AddScoped<ICurrencyStore, CurrencyStore>();
builder.Services.AddScoped<IHotelCodeStatusStore, HotelCodeStatusStore>();
builder.Services.AddScoped<IHotelCodeStore, HotelCodeStore>();
builder.Services.AddScoped<IHotelStore, HotelStore>();
builder.Services.AddScoped<IRoomStore, RoomStore>();
builder.Services.AddScoped<IUserHotelStore, UserHotelStore>();
builder.Services.AddScoped<IUserStore, UserStore>();
builder.Services.AddScoped<IColorStore, ColorStore>();
builder.Services.AddScoped<IBedStore, BedStore>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Authorization header using the bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme
    ).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
