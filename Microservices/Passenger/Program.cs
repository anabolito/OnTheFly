using Microsoft.Extensions.Options;
using PassengerAPI.Repositories;
using PassengerAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<PassengerSettings>(builder.Configuration.GetSection("PassengerSettings"));
builder.Services.AddSingleton<IPassengerSettings>(sp => sp.GetRequiredService<IOptions<PassengerSettings>>().Value);
builder.Services.AddSingleton<PassengerService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
