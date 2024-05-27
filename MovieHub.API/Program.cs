using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"] ?? throw new ArgumentNullException(nameof(builder.Configuration)),
        ValidAudience = builder.Configuration["Authentication:Audience"] ?? throw new ArgumentNullException(nameof(builder.Configuration)),
        IssuerSigningKey = new SymmetricSecurityKey(
            Convert.FromBase64String(builder.Configuration["Authentication:SecretKey"] ?? throw new ArgumentNullException(nameof(builder.Configuration)))
        )
    };
});

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1,0);
}).AddMvc().AddApiExplorer(setupAction =>
{
    setupAction.SubstituteApiVersionInUrl = true;
});

var apiVersionDescriptionProvider =
    builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

builder.Services.AddSwaggerGen(setupAction =>
{
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        setupAction.SwaggerDoc(description.GroupName, new()
        {
            Title = "Movie Hub API",
            Version = description.ApiVersion.ToString(),
            Description = "Through this API, you can get access to movies and reviews in MovieHub"
        });
    }
    
    setupAction.AddSecurityDefinition("MovieHubAPIBearerAuth", new()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to the access the API"
    });
    
    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "MovieHubAPIBearerAuth"
                }
            },
            new List<string>()
        }
    });
});

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
    app.UseSwaggerUI(setupAction =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            setupAction.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
        setupAction.EnableTryItOutByDefault();
    });
}
else app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

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

    if (!File.Exists(sqlFile)) throw new FileNotFoundException(sqlFile);
    
    var sql = File.ReadAllText(sqlFile);
    context.Database.ExecuteSqlRaw(sql);
}