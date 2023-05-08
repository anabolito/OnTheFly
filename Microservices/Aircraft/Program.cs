using AircraftAPI.Config;
using AircraftAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AircraftAPISettings>(builder.Configuration.GetSection("AircraftAPISettings"));
builder.Services.AddSingleton<IAircraftAPISettings>(s => s.GetRequiredService<IOptions<AircraftAPISettings>>().Value);
builder.Services.AddSingleton<AircraftAPIRepository>();
builder.Services.AddSingleton<CompanyService>();

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
