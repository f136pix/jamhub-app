using System.Configuration;
using System.Reflection;
using DemoLibrary;
using DemoLibrary.CrossCutting;
using DemoLibrary.CrossCutting.Queues;
using DemoLibrary.CrossCutting.Queues.Configuration;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        opt.JsonSerializerOptions.MaxDepth = 32;
    });

var isDevelopment = builder.Environment.IsDevelopment();
if (isDevelopment)
{
    //serilog logger
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger();
    Console.WriteLine("--> Is Dev mode");
    Console.WriteLine("--> Local PGSQL DB");

    // local dockerized db
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"--> Connection string: {connectionString}");

    // local installed db
    // var connectionString = "Host=localhost;Database=jamhub;Username=root;Password=Filipeco123!";

    // db context
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Api")));

    Console.WriteLine("--> Dev RabbitMq Connection");

    // rabbimq connection
    var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ").Get<BusConfigData>();
    Console.WriteLine("--> " + rabbitMqConfig!.Host);
    builder.Services.AddRabbitConnection(rabbitMqConfig);
    // services.addScoped<IConnection, RabbitMQConnection>();

    builder.Services.AddRabbitMqMessagePublisher();
}

// injection of the data access
builder.Services.AddRepositories();
builder.Services.AddUnitOfWork();

// data context
// builder.Services.AddSingleton<IDataAccess, DemoDataAccess>();

// mediatr -> use the assembly of the Core solution
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(DemoLibraryMediatREntrypoint).GetTypeInfo().Assembly));

// builder.Services.AddAutoMapper(typeof(DemoLibraryMediatREntrypoint).Assembly);
builder.Services.AddAutoMapper(typeof(DemoLibraryMediatREntrypoint).Assembly);

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}