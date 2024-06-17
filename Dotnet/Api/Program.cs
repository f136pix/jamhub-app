using System.Configuration;
using System.Net.Mime;
using System.Reflection;
using DemoLibrary;
using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.CrossCutting;
using DemoLibrary.CrossCutting.Queues;
using DemoLibrary.CrossCutting.Queues.Configuration;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.Context;
using DemoLibrary.Infraestructure.Messaging._Mail;
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
        // opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

        opt.JsonSerializerOptions.MaxDepth = 32;
    });

var isDevelopment = builder.Environment.IsDevelopment();
var isProduction = builder.Environment.IsProduction();


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
}

if (isProduction)
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger();
    Console.WriteLine("--> Is Prod mode");
    Console.WriteLine("--> K8S PGSQL DB");

    // local dockerized db
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"--> Connection string: {connectionString}");


    // local installed db
    // var connectionString = "Host=localhost;Database=jamhub;Username=root;Password=Filipeco123!";

    // db context
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Api")));

    // run pending migrations 


    Console.WriteLine("--> Prod Connection");
}


// Http client
builder.Services.AddHttpPublisher();

// injection of the data access
builder.Services.AddRepositories();
builder.Services.AddUnitOfWork();

// mediatr -> use the assembly of the Core solution
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(DemoLibraryMediatREntrypoint).GetTypeInfo().Assembly));

// automapper
builder.Services.AddAutoMapper(typeof(DemoLibraryMediatREntrypoint).Assembly);

// services
// builder.Services.AddScoped<IAsyncProcessorService, AsyncProcessorService>();
builder.Services.AddSingleton<IAsyncProcessorService, AsyncProcessorService>();

// mailer sender

// rabbimq connection
var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ").Get<BusConfigData>();
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
Console.WriteLine("--> " + rabbitMqConfig!.Host);
builder.Services.AddRabbitConnection(rabbitMqConfig);
// services.addScoped<IConnection, RabbitMQConnection>();

// creates queue if not exists
var queues = new List<string> { "dotnet.rails", "rails.dotnet" };
builder.Services.InitializeRabbitMQQueues(queues);

// rabbitmq publisher and consumer
builder.Services.AddRabbitMqMessagePublisher();
builder.Services.AddRabbitMqMessageConsumer();

// email service
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddEmailService();


var app = builder.Build();

// configure jwt authentication middleware
app.AddJwtMiddleware();

var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

// orquestrates the consumer
app.RunRabbitMqMessageConsumer();

// run pending migrations
// using (var scope = serviceScopeFactory.CreateScope())
// {
//     // Get the DataContext
//     var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//         
//     // Check if any migrations have been applied (i.e., tables have been created)
//     var appliedMigrations = context.Database.GetAppliedMigrations().ToList();
//
//     if (!appliedMigrations.Any())
//     {
//             
//         Console.WriteLine("--> Applying migrations");
//         // If no migrations have been applied, apply the migrations
//         context.Database.Migrate();
//         Console.WriteLine("--> Migrations applied");
//     }
// }


app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


if (app.Environment.IsProduction())
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