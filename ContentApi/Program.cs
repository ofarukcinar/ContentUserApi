using ContentApi.Helper;
using ContentApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment.EnvironmentName;

// Add configuration files
builder.Configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true);

// Database and Services
builder.Services.AddDbContext<ContentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ContentDb")));
builder.Services.AddTransient<IContentService, ContentService>();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ApiClient>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add Controllers
builder.Services.AddControllers();

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build Application
var app = builder.Build();

// Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Content API"); });
}


app.UseCors("AllowAll");
// Middleware Configuration
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Run Application
app.Run();