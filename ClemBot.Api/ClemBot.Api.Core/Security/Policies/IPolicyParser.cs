namespace ClemBot.Api.Core.Security.Policies
{
    public interface IPolicyParser<T>
    {
        public string Serialize(T? t);
        public T? Deserialize(string val);
    }
}
