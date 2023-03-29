using Microsoft.EntityFrameworkCore;
using Persistence;
using Logic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AnnouncementdbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'LocatorBackEndContext' not found."), 
    sqlServerOptions => {
        sqlServerOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
                );
    })
);

// Add services to the container.

builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try 
{
    var _context = services.GetRequiredService<AnnouncementdbContext>();
    _context.Database.Migrate();
}
catch (Exception ex) {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error happened during a migration");

}

app.Run();
