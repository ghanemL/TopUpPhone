using MobileTopup.Api;
using MobileTopup.Application;
using MobileTopup.Infrastructure;
using MobileTopUp.Web;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure()
        .AddPersistence()
        .AddWeb();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseStaticFiles();
    app.UseHttpsRedirection();
    app.MapControllers();

    app.Run();
}

public partial class Program { }

