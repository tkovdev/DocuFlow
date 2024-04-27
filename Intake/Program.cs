using DataAccess.DAL;
using Intake.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddScoped<IMongoDb, MongoDb>();

var app = builder.Build();
IWebHostEnvironment env = app.Environment;


// Configure the HTTP request pipeline.
app.MapGrpcService<DocumentService>();

if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();