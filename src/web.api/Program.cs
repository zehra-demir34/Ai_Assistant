using Application.Chat;
using Infrastructure;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Chat.Commands.CreateSession;
using Infrastructure.Tools;


var builder = WebApplication.CreateBuilder(args);

//var apikey = builder.Configuration["OpenAI:apiKey"];
builder.Services.AddScoped<IChatService, MafAgentService>();

builder.Services.AddHttpClient<WeatherTool>();

// Add services to the container.

builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddMediatR(cfg =>cfg.RegisterServicesFromAssembly(typeof(CreateSessionCommand).Assembly));

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
