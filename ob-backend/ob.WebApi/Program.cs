using ob.WebApi.Filters;
using ob.ServicesFactory;
using ob.IBusinessLogic;
using ob.BusinessLogic;
using ob.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddScoped<AuthenticationFilter>();

var servicesFactory = new ServicesFactory();
servicesFactory.RegistrateServices(builder.Services);


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

app.Run();

