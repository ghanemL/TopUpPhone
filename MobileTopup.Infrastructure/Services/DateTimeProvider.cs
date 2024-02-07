using MobileTopup.Application.Common.Interfaces.Services;

namespace MobileTopup.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public bool IsInCurrentMonth(DateTime inputDate)
        {
            return (inputDate.Year == UtcNow.Year && inputDate.Month == UtcNow.Month);
        }
    }
}
