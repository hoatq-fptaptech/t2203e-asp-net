﻿using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// add Cors
builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy=>
            {
                //policy.WithOrigins("https://24h.com.vn");
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            }
            );
    }
);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<T2203E_API.Entities.T2203eApiContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("T2203E_API"))
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

