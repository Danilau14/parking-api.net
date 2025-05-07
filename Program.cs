using System.Configuration;
using System.Reflection;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime.SharedInterfaces;
using Amazon.S3;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers( options =>
{
   //options.Filters.Add<GlobalExceptionFilter>();
}
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.SwaggerWithBearerToken();

builder.Services.ConfigureDbContext(builder.Configuration);


//builder.Services.Configure<DynamoDbSettings>(builder.Configuration.GetSection("DynamoDbSettings"));
//builder.Services.AddDynamoDb(builder.Configuration);

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddSingleton<IDynamoDBContext>(sp => new DynamoDBContext(sp.GetRequiredService<IAmazonDynamoDB>()));

builder.Services.AddAWSService<IAmazonS3>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRepositoryDynamo, UserRepositoryDynamo>();
builder.Services.AddScoped<IRevokedTokenRepository, RevokedTokenRepository>();
builder.Services.AddScoped<IParkingLotRepository, ParkingLotRepository>();
builder.Services.AddScoped<IParkingHistoryRepository, ParkingHistoryRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();


builder.Services.AddScoped<IRabbitMQMessageBuilder, RabbitMQMessageBuilder>();
builder.Services.AddScoped<IRabbitMQSendMail, RabbitMQSendMail>();
builder.Services.AddScoped<IS3Service, S3Service>();


//builder.Services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
//builder.Services.AddScoped<IDomainEventHandler<VehicleRegisteredEvent>, VehicleRegisteredEventHandler>();

builder.Services.AddAuthorizationPolicies();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddAutoMapper(static cfg => {
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
