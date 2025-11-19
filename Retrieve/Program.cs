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

//Setup App CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        policy =>
            policy.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(builder.Configuration.GetSection("CORS:allowed").Get<string[]>())
    );
});

var app = builder.Build();

app.UseCors("AllowWebApp");

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();