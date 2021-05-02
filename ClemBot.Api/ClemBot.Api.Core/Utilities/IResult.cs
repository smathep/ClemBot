namespace ClemBot.Api.Core.Utilities
{
    public interface IResult<T>
    {
        T? Value { get; }
        ResultStatus Status { get; }
    }
}
