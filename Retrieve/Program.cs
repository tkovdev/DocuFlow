using AzureAccess.Services;
using Data.Access.Abstractions.Interfaces;
using Data.Access.DAL;
using Data.Access.Services;
using Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IMongoDb, MongoDb>();

builder.Services.AddScoped<IEntityService<Document>, DocumentEntityService>();
builder.Services.AddScoped<IFileService, AzureFileService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();