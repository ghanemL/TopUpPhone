using Mapster;
using MobileTopup.Contracts.Responses;
using MobileTopup.Domain.TopupOptions.Enums;

namespace MobileTopup.Api.Common.Mapping
{
    public class TopUpOptionMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TopUpOptionResponse, TopUpOption>()
                .Map(src => src, dest => dest.Options);
        }
    }
}
