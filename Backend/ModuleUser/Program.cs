using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ModuleUser.Entities;
using ModuleUser.Repositories;
using ModuleUser.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(config =>
{
    config.AddPolicy("UserModelPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddSingleton<AppDbContext>();
builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<HashAlgorithm>((serviceProvider) =>
{
    var instance = HashAlgorithm.Create(HashAlgorithmName.SHA256.Name);
    return instance;
});
builder.Services.AddSingleton<EncryptorUtil>((serviceProvider) =>
{
    var hashInstance = serviceProvider.GetService<HashAlgorithm>();
    return new(hashInstance);
});

//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseCors("UserModelPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
