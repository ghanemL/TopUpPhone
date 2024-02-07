using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileTopup.Application.Options.Queries;
using MobileTopup.Application.Topups.Commands.AddTopupBeneficiary;
using MobileTopup.Application.Topups.Commands.ExecuteTopup;
using MobileTopup.Application.Topups.Queries.GetTopUpBeneficiaries;
using MobileTopup.Application.Users.Commands.CreateUser;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Responses;

namespace MobileTopup.Api.Controllers
{
    [Route("api/[controller]")]
    public class TopUpController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IValidator<AddTopUpBeneficiaryCommand> _beneficiaryValidator;
        private readonly IValidator<ExecuteTopUpCommand> _topupValidator;
        

        public TopUpController(IMediator mediator, 
            IMapper mapper, 
            IValidator<AddTopUpBeneficiaryCommand> beneficiaryValidator,
            IValidator<ExecuteTopUpCommand> topupValidator)
        {
            _mediator = mediator;
            _mapper = mapper;
            _beneficiaryValidator = beneficiaryValidator;
            _topupValidator = topupValidator;

        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var command = _mapper.Map<CreateUserCommand>(request);
            var result = await _mediator.Send(command);

            return result.Match(
               user => Ok(_mapper.Map<UserResponse>(user)),
               errors => Problem(errors));
        }

        [HttpPost("addBeneficiary")]
        public async Task<IActionResult> AddBeneficiary([FromBody] AddTopUpBeneficiaryRequest request)
        {
            var command = _mapper.Map<AddTopUpBeneficiaryCommand>(request);

            var validationResult = _beneficiaryValidator.ValidateAsync(command).Result;
            if (!validationResult.IsValid)
            {
                return Problem(validationResult.Errors);
            }
                       
            var result = await _mediator.Send(command);

            return result.Match(
                user => Ok(_mapper.Map<UserResponse>(user)),
                errors => Problem(errors));
                
        }

        [HttpPost("executeTopUp")]
        public async Task<IActionResult> ExecuteTopUp([FromBody] ExecuteTopUpRequest request)
        {
            var command = _mapper.Map<ExecuteTopUpCommand>(request);

            var validationResult = _topupValidator.ValidateAsync(command).Result;
            if (!validationResult.IsValid)
            {
                return Problem(validationResult.Errors);
            }

            var result = await _mediator.Send(command);

            return result.Match(
                user => Ok(_mapper.Map<UserResponse>(user)),
                errors => Problem(errors));
        }

        [HttpGet("getBeneficiaries")]
        public async Task<IActionResult> GetBeneficiaries(Guid userId)
        {
            var query = new GetTopUpBeneficiariesQuery { UserId = userId };

            var result = await _mediator.Send(query);

            return result.IsFailed
                ? Problem()
                : Ok(result.Value);
        }

        [HttpGet("getAvailableTopUpOptions")]
        public async Task<IActionResult> GetAvailableTopUpOptions()
        {
            var query = new GetAvailableTopUpOptionsQuery();

            var result = await _mediator.Send(query);


            return result.Match(
                options => Ok(result.Value),
                errors => Problem(errors));
        }
    }
}
