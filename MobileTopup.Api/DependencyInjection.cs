using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MobileTopup.Api.Common.Errors;
using MobileTopup.Api.Common.Mapping;
using MobileTopup.Application.Topups.Commands.AddTopupBeneficiary;
using MobileTopup.Application.Topups.Commands.ExecuteTopup;

namespace MobileTopup.Api;
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<AddTopUpBeneficiaryCommand>, AddTopUpBeneficiaryCommandValidator>();
        services.AddScoped<IValidator<ExecuteTopUpCommand>, ExecuteTopUpCommandValidator>();
        
        services.AddSingleton<ProblemDetailsFactory, TopUpProblemDetailsFactory>();
        services.AddMappings();
        return services;
    }
}
