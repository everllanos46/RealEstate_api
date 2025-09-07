using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var mongoSettings = builder.Configuration.GetSection("MongoDb");
builder.Services.AddSingleton(new MongoDbContext(
    mongoSettings["ConnectionString"], mongoSettings["DatabaseName"]
));

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
