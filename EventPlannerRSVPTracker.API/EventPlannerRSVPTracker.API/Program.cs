

using EventPlannerRSVPTracker.API.Middlewares;
using EventPlannerRSVPTracker.Database;
using EventPlannerRSVPTracker.Database.DbContext;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    // Configure Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    // Configure DbContext, EF Core, Repositories and Services
    builder.Services
        .AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection")!)
        .AddRepositories()
        .AddServices();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configure global exception handling middleware
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    builder.Services.AddProblemDetails();

    // Configure the CORS policy.
    builder.Services.AddCors(options =>
    {
        // Define a new policy with the name we specified above.
        options.AddPolicy(name: "defaultCORSPolicy",
                          policy =>
                          {
                              // IMPORTANT: Use WithOrigins() to specify the allowed frontend URLs.
                              // These must EXACTLY match the URL(s) where your React app is running,
                              // including protocol (http/https) and port.
                              policy.WithOrigins("http://localhost:5173",    // Common for Vite development
                                                 "https://localhost:5173",   // Common for Vite development with HTTPS
                                                 "http://localhost:3000",    // Common for Create React App development
                                                 "https://localhost:3000")   // Common for Create React App with HTTPS
                                                                             // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.).
                                    .AllowAnyMethod()
                                    // Allow all headers to be sent by the client.
                                    .AllowAnyHeader()
                                    // IMPORTANT: Allow credentials (like cookies or authorization headers).
                                    // This is crucial if your frontend sends authentication tokens (e.g., JWT in Authorization header)
                                    // or relies on cookies for session management.
                                    // Note: WithAllowCredentials() means you CANNOT use AllowAnyOrigin() ("*").
                                    .AllowCredentials();
                          });
    });

    var app = builder.Build();

    // Use exception handler
    app.UseExceptionHandler();

    // Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();

        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        Seeder.SeedData(appDbContext, logger);

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("defaultCORSPolicy"); // Apply the CORS policy

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();

    Log.Information("Application has stopped.");
}
