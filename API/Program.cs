using API.Middleware;
using API.Services;
using Serilog;
using System.Reflection;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

var appsettingsFile = null as string;

#if DEBUG
    appsettingsFile = "appsettings.Development.json";
#else
    appsettingsFile= "appsettings.json";
#endif

builder.Configuration.AddJsonFile(appsettingsFile);

// Add services to the container.

builder.Services.AddControllers();

var serilogLogger = new LoggerConfiguration()
    .WriteTo.MongoDBBson(config =>
    {
        config.SetConnectionString(builder.Configuration["Logging:ConnectionString"]!);
        config.SetCollectionName("Messaging");
    })
    .CreateLogger();

builder.Services.AddLogging(configure => configure.AddSerilog(serilogLogger, dispose: true));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<APIKeyService>();

// Register the custom authentication scheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
    options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
})
.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, null);

// Register Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
