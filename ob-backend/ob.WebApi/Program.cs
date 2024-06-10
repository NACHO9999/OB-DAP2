using ob.WebApi.Filters;
using ob.ServicesFactory;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine(DateTime.Now);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddScoped<AuthenticationFilter>();

var servicesFactory = new ServicesFactory();
servicesFactory.RegistrateServices(builder.Services);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();
app.UseCors();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ob.DataAccess.AppContext>();
        await context.InitializeAsync(); // Inicializa el administrador base
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during migration: {ex.Message}");
    }
}

app.Run();

