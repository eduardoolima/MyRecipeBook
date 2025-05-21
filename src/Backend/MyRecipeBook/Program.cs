using MyRecipeBook.Filters;
using MyRecipeBook.Middleware;
using MyRecipeBook.Application;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Application.UseCases.User.Register;

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

using (var scope = app.Services.CreateScope())
{
    try
    {
        var test = scope.ServiceProvider.GetRequiredService<IRegisterUserUseCase>();
        Console.WriteLine("Servi�o resolvido com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao resolver servi�o:");
        Console.WriteLine(ex);
        throw; // dispara a exce��o para stacktrace completo
    }
}

app.Run();
