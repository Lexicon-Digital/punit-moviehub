using Microsoft.EntityFrameworkCore;
using MovieHub.DbContexts;
using MovieHub.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/movieHubInfo.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MovieHubContext>(dbContextOptions => 
    dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:MovieHubConnectionString"])
);

builder.Services.AddScoped<IMovieHubRepository, MovieHubRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IPrincesTheatreService, PrincesTheatreService>();

builder.Services.AddHttpClient();

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

SeedDatabase(app);

app.Run();

return;


static void SeedDatabase(IHost app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MovieHubContext>();
    context.Database.EnsureCreated();

    if (context.Movies.Any() || context.Cinemas.Any() || context.MovieCinemas.Any()) return;

    var sqlFile = Path.Combine(
        Directory.GetCurrentDirectory(),
        "Scripts",
        "moviehub-db-data-seed.sql"
    );

    if (!File.Exists(sqlFile)) return;
    
    var sql = File.ReadAllText(sqlFile);
    context.Database.ExecuteSqlRaw(sql);
}