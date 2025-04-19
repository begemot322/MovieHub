using MovieHub.Application;
using MovieHub.Infrastructure;
using MovieHub.Infrastructure.Data;
using MovieHub.WebApi;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices()
    .AddWebServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   
    app.UseSwaggerUI();
    await app.InitialiseDatabaseAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();