using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ParkingApi.Data;
using ParkingApi.Interfaces;
using ParkingApi.Models.Enums;
using ParkingApi.Repositories;
using ParkingApi.Services.cs;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using ParkingApi.Mappings.Users;
using ParkingApi.Mappings.ParkingsLot;
using ParkingApi.Mappings.ParkingHistories;
using ParkingApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Por favor, ingrese el token JWT como 'Bearer <token>' en la cabecera."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserServiceInterface, UserService>();
builder.Services.AddScoped<IRevokedTokenRepository, RevokedTokenRepository>();
builder.Services.AddScoped<IParkingLotRepository, ParkingLotRepository>();
builder.Services.AddScoped<IParkingLotService, ParkingLotService>();
builder.Services.AddScoped<IParkingHistoryRepository, ParkingHistoryRepository>();
builder.Services.AddScoped<IParkingHistoryService, ParkingHistoryService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, UserRole.ADMIN.ToString()));

    options.AddPolicy("PartnerOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, UserRole.PARTNER.ToString()));
});

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var token = context.SecurityToken as JwtSecurityToken;
                var revokedRepo = context.HttpContext.RequestServices.GetRequiredService<IRevokedTokenRepository>();

                if (token != null && await revokedRepo.IsTokenRevoked(token.RawData))
                {
                    context.Fail("Token has been revoked");
                }
            }
        };
    });

builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<CreateUserMapping>();
    cfg.AddProfile<CreateParkingLotMapping>();
    cfg.AddProfile<ParkingLotMapping>();
    cfg.AddProfile<ParkingHistoryMapping>();
    cfg.AddProfile<UpdateParkingLotMapping>();
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
