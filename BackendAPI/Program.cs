using Application;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;


public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Aggiungi i servizi al contenitore
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Registrazione dei servizi
        builder.Services.InjectApplicationServices();
        builder.Services.InjectRepositoryServices();

        // Configurazione MongoDB
        builder.Services.AddSingleton<IMongoClient>(new MongoClient("mongodb://localhost:27017"));
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

        // Aggiungi Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // Configura JWT
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

        // Configura CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("FrontendPolicy", policy =>
            {
                policy.WithOrigins("https://46dc-5-90-2-162.ngrok-free.app", "http://localhost:3000", "https://frontend-web-app-five.vercel.app")
              .AllowAnyMethod() // Permette qualsiasi metodo
              .AllowAnyHeader() // Permette qualsiasi header
              .AllowCredentials(); // Permette credenziali
            });
        });

        // Configurazione MQTT
        builder.Services.AddSingleton<MqttClientService>(provider =>
        {
            var config = builder.Configuration.GetSection("MqttSettings");
            var clientId = config["ClientId"];
            var server = config["Server"];
            var port = int.Parse(config["Port"]);
            var username = config["Username"];
            var password = config["Password"];

            var serviceScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            return new MqttClientService(clientId, server, port, username, password, serviceScopeFactory);
        });

        builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var csvFilePath = Path.Combine(baseDirectory, "PlasmacMachines.csv");

        // Servizio CSV
        builder.Services.AddScoped<CsvImportService>(provider =>
        {
            var machineRepository = provider.GetRequiredService<IDppMachineRepository>();
            var mapper = provider.GetRequiredService<IMapper>();
            return new CsvImportService(machineRepository, csvFilePath, mapper);
        });

        var app = builder.Build();

        // Configurazione della pipeline HTTP
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        // Aggiungi CORS qui prima dell'autorizzazione
        app.UseCors("FrontendPolicy");

        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.MapControllers();

        // Importazione CSV al bootstrap
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var csvImportService = services.GetRequiredService<CsvImportService>();
            await csvImportService.ImportCsvToDatabase(csvFilePath);
        }

        // Esegui il seed dei dati
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            SeedAdmin.Initialize(userManager, roleManager).Wait();
        }

        // Avvia l'app
        await app.RunAsync();
    }
       
}
