namespace MobileTopup.Api.Misc
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
