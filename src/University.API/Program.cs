using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Compact;
using University.API.Services;
using University.API.Utils;
using University.Application.Contracts.API;
using University.Application.DI;
using University.Identity.DI;
using University.Persistance.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Application", "University.Api")
    //.WriteTo.Console(new CompactJsonFormatter()));  => this one is for production
    .WriteTo.Console
    (
        outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"  
        // changed to this to make log clean for by debug purpose
    ));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
//register swagger generation service
//builder.Services.AddSwaggerGen(option =>
//{
//    option.SwaggerDoc("v1", new OpenApiInfo { Title = "University Management API", Version = "v1" });
//});
builder.Services.ConfigSwagger();

builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    await app.ApplyMigrationAsync();
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("TraceId", httpContext.TraceIdentifier);
    };
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    //openapi/v1.json
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        //access swagger
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "UniversityManagement API"); 
    });
}

//app.UseHttpsRedirection();
if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
