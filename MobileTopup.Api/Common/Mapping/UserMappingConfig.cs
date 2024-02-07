using Mapster;
using MobileTopup.Application.Topups.Commands.AddTopupBeneficiary;
using MobileTopup.Application.Users.Commands.CreateUser;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Responses;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.Domain.UserAggregate.Entities;

namespace MobileTopup.Api.Common.Mapping
{
    public class UserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateUserRequest, CreateUserCommand>()
            .Map(dest => dest, src => src);

            config.NewConfig<AddTopUpBeneficiaryRequest, AddTopUpBeneficiaryCommand>()
            .Map(dest => dest, src => src);

            config.NewConfig<User, UserResponse>()
            .Map(dest => dest.Id, src => src.UserId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Beneficiaries, src => src.Beneficiaries);

            config.NewConfig<TopUpBeneficiary, TopupBeneficiaryResponse>()
            .Map(dest => dest.Id, src => src.BeneficiaryId)
            .Map(dest => dest.Transactions, src => src.Transactions);

            config.NewConfig<Transaction, TransactionResponse>()
            .Map(dest => dest.Id, src => src.TransactionId);



        }
    }
}
