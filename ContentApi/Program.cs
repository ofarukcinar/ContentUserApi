using System;
using ContentApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ContentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ContentDb")));
builder.Services.AddTransient<IContentService, ContentService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ApiClient>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty; 
});
app.UseAuthorization();
app.MapControllers();

app.Run();