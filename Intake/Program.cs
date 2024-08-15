using AzureAccess.Interfaces;
using AzureAccess.Services;
using Data;
using DataAccess.DAL;
using DataAccess.Interfaces;
using DataAccess.Services;
using Intake.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddScoped<IMongoDb, MongoDb>();

builder.Services.AddScoped<IEntityService<Document>, DocumentEntityService>();
builder.Services.AddScoped<IFileService, AzureFileService>();

var app = builder.Build();
IWebHostEnvironment env = app.Environment;


// Configure the HTTP request pipeline.
app.MapGrpcService<DocumentService>();

if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();