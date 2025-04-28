using Microsoft.Extensions.Options;
using ParkingApi.Core.Global;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers( options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
}
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.SwaggerWithBearerToken();

builder.Services.ConfigureDbContext(builder.Configuration);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserServiceInterface, UserService>();
builder.Services.AddScoped<IRevokedTokenRepository, RevokedTokenRepository>();
builder.Services.AddScoped<IParkingLotRepository, ParkingLotRepository>();
builder.Services.AddScoped<IParkingLotService, ParkingLotService>();
builder.Services.AddScoped<IParkingHistoryRepository, ParkingHistoryRepository>();
builder.Services.AddScoped<IParkingHistoryService, ParkingHistoryService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();


builder.Services.AddAuthorizationPolicies();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddJwtAuthentication(builder.Configuration);

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
