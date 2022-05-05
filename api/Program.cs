global using Serilog;
global using Serilog.AspNetCore;

using data;
using data.Repositories;

using Microsoft.Data.Sqlite;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
// builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var connectionString = builder.Configuration.GetConnectionString("InTouch");
//Log.Information($"Using connection string \"{connectionString}\"");
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddTransient<IContactRepository, ContactRepository>();
builder.Services.AddTransient<IContactMethodRepository, ContactMethodRepository>();
builder.Services.AddTransient<ICommunicationRepository, CommunicationRepository>();
builder.Services.AddTransient<ICorrespondenceRepository, CorrespondenceRepository>();


builder.Services.AddControllers()
    .AddJsonOptions(options => {
      options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Swagger/OpenAPI https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS step 1
builder.Services.AddCors(p => p.AddPolicy("devcors", builder => {
  builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.MapSwagger();
  app.UseSwaggerUI();
}

// CORS part 2
app.UseCors("devcors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try {
  app.Run();
} catch (Exception ex) {
  Log.Fatal(ex, "Host terminated unexpectedly.");
} finally {
  Log.CloseAndFlush();
}
