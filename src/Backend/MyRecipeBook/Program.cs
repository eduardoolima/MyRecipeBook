using MyRecipeBook.Filters;
using MyRecipeBook.Middleware;
using MyRecipeBook.Application;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Infrastructure.Migrations;
using MyRecipeBook.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


MigrateDatabase();

app.Run();

void MigrateDatabase()
{
    if(builder.Configuration.IsUnitTestEnvironment())
        return;
    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    DatabaseMigration.Migrate(builder.Configuration.ConnectionString(), serviceScope.ServiceProvider);
}

public partial class Program
{
    protected Program() { }
    // This partial class is used to allow for the Program class to be extended in other files.
    // This is useful for testing purposes or when you want to separate concerns in your application.
}