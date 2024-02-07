using ErrorOr;

namespace MobileTopup.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error UserNotFound => Error.NotFound(code: "User.UserId", description: "User Not Found");
            public static Error UserAlreadyExist => Error.Conflict(code: "User.UserId", description: "User Already Exist");
            public static Error UserMaximumBeneficiaryAttempt => Error.Custom(491, code: "User.Beneficiaries", description: "Maximum Beneficiarries Attemp per user");
            public static Error BeneficiaryAlreadyExists => Error.Conflict(code: "User.Beneficiary.NickName", description: "Benefeciary is already existing");
        }

        public static class ExecuteTopUp
        {
            public static Error InsufficientBalance => Error.Custom(490, code: "Balance", description: "Insufficient balance or monthly limit exceeded.");
            public static Error BalanceServiceUnavailable => Error.Failure(code: "BalanceHttpService", description: "External Balance Http Service is Unavailable.");
            public static Error DebitFailure => Error.Failure(code: "BalanceHttpService", description: "Fail To process Debit in Balance Service");
            public static Error MaximumCapacityExceedVerifiedUser => Error.Failure(code: "ExecuteTopUpLimitExceeded", description: "Maximum Monthly Capacity Exceed for Verified User");
            public static Error MaximumCapacityExceedUnverifiedUser => Error.Failure(code: "ExecuteTopUpLimitExceededUnverfiedUser", description: "Maximum Monthly Capacity Exceed for UNVerified User");


        }
    }
}
