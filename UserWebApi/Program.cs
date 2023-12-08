using Microsoft.OpenApi.Models;
using UserWebApi.Services;
using UserWebApi.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Web API", Description = "User Web API", Version = "v1" });
});
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());
    });
builder.Services.AddControllers();
builder.Services.AddScoped<IJsonDataService, JsonFileDataService>();
builder.Services.AddScoped<IValidator, UserValidator>();
builder.Services.AddScoped<IDataRepository<User>>(serviceProvider =>
     {
         var logger = serviceProvider.GetRequiredService<ILogger<UserRepository>>();
         var jsonFileData = serviceProvider.GetRequiredService<IJsonDataService>();
         var validator = serviceProvider.GetRequiredService<IValidator>();

         return new UserRepository(logger, jsonFileData, validator);
     });
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
    });

}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();
app.Run();