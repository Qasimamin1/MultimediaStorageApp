using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared;
using Serilog;
using Repository.Interface;
using Repository;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);


IConfiguration _configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                             .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                             .Build();
//essential for monitoring, debugging, and error tracking in production environments.
Serilog.Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
.WriteTo.File($"{builder.Environment.WebRootPath}" + @"\\logs\\log.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();


#region DbContext

builder.Services.AddDbContext<ApplicationDBContext>(options =>
   options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"), x =>
   {
       x.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
   }));

#endregion
//This is a common practice for handling configurations like API keys, feature toggles, or any custom settings your application might need.
#region Metadata

AppSettings _settings = new();
builder.Configuration.GetSection("Settings").Bind(_settings, c => c.BindNonPublicProperties = true);
Static.Settings = _settings;

#endregion

#region Services

builder.Services.AddLogging(configuration => configuration.ClearProviders());

builder.Services.AddScoped<Response>();
builder.Services.AddScoped<IMultimediaStorage, MultimediaStorageRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
      "CorsPolicy",
      builder => builder.WithOrigins(_settings.CorsUrl)
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());
});

#endregion

builder.Services.AddSwaggerGen();

//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

//void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//{
//    app.UseCors(options => options.WithOrigins("http://localhost:3000")
//.AllowAnyMethod()
//.AllowAnyHeader());
//}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region App

//app.UseMiddleware(typeof(OAuth));
app.UseCors(options => options.WithOrigins("http://localhost:3000")
.AllowAnyMethod()
.AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

#endregion
