using AzureAccess.Services;
using Data.Access.Abstractions.Interfaces;
using Data.Access.DAL;
using Data.Access.Services;
using Data.Models;
using Intake.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
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