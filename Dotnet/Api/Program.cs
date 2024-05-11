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
builder.Services.AddControllers();

var isDevelopment = builder.Environment.IsDevelopment();
if (isDevelopment)
{
    Console.WriteLine("--> Is Dev mode");
    Console.WriteLine("--> Local PGSQL DB");

    // local dockerized
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"--> Connection string: {connectionString}");

    // local installed
    // var connectionString = "Host=localhost;Database=jamhub;Username=root;Password=Filipeco123!";

    // db context
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Api")));

    Console.WriteLine("--> Dev RabbitMq Connection");
    
    // rabbimq queue
    var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ").Get<BusConfigData>();
    Console.WriteLine("--> " + rabbitMqConfig!.Host);
    builder.Services.AddRabbitConnection(rabbitMqConfig);
    // services.addScoped<IConnection, RabbitMQConnection>();
    
    //serilog logger
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger();
}

// injection of the data access
builder.Services.AddRepositories();
builder.Services.AddUnitOfWork();

// data context
// builder.Services.AddSingleton<IDataAccess, DemoDataAccess>();

// mediatr -> use the assembly of the Core solution
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(DemoLibraryMediatREntrypoint).GetTypeInfo().Assembly));



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