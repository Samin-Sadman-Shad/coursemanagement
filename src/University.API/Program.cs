using University.Application.DI;
using University.Persistance.DI;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
//register swagger generation service
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "University Management API", Version = "v1" });
});

builder.Services.AddPersistanceServices();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //openapi/v1.json
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        //access swagger
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "University API"); 
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
