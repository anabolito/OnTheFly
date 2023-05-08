using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SaleAPI.Repository;
using SaleAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SaleSettings>(builder.Configuration.GetSection("SalesSettings"));
builder.Services.AddSingleton<ISaleSettings>(s => s.GetRequiredService<IOptions<SaleSettings>>().Value);
builder.Services.AddSingleton<SaleRepository>();
builder.Services.AddSingleton<ConnectionFactory>();

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
