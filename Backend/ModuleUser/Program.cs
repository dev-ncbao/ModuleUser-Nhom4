using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ModuleUser.Data;

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
        policy.WithOrigins("http://localhost:3000").WithMethods().WithHeaders();
    });
});
builder.Services.AddDbContextPool<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
    ));
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
