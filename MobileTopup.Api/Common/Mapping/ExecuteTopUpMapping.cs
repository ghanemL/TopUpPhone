using Mapster;
using MobileTopup.Application.Topups.Commands.ExecuteTopup;
using MobileTopup.Contracts.Requests;
using MobileTopup.Domain.TopupOptions.Enums;

namespace MobileTopup.Api.Common.Mapping
{
    public class ExecuteTopUpMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ExecuteTopUpRequest, ExecuteTopUpCommand>()
            .Map(dest => dest.Beneficiaries, src => src.TopUpRequests);

            config.NewConfig<TopUpRequest, TopUpBeneficiaryCommand>()
                .Map(dest => dest.BeneficiaryId, src => src.BeneficiaryId)
                .Map(dest => dest.TopUpOption, src => Convert.ToInt64(src.TopUpOption));
        }
    }
}
