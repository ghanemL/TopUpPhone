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

    // 3rd approach to error handling - error endpoint
    app.UseExceptionHandler("/error"); // this is used in conjunction with the factory 

    app.UseHttpsRedirection();
    //app.UseAuthentication();
    //app.UseAuthorization();
    app.MapControllers();

    app.Run();
}

