namespace MobileTopUp.Web.Converter
{
    public interface IJsonConverter
    {
        T Deserialize<T>(Stream responseContent);
    }
}
