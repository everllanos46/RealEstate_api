using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Domain.Interfaces;
using RealEstate.Application.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using RealEstate.Infrastructure.Services;
using RealEstate.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var mongoSettings = builder.Configuration.GetSection("MongoDb");
builder.Services.AddSingleton(new MongoDbContext(
    mongoSettings["ConnectionString"]!, mongoSettings["DatabaseName"]!
));
MongoMapping.Configure();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
builder.Services.AddScoped<IFileStorageRepository, FirebaseStorageRepository>();
builder.Services.AddAutoMapper(typeof(PropertyProfile));
builder.Services.AddScoped<PropertyService>();
builder.Services.AddScoped<PropertyImageService>();


var firebaseKeyPath = builder.Configuration["Firebase:CredentialsPath"];
if (FirebaseApp.DefaultInstance == null)
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(firebaseKeyPath)
    });
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();
