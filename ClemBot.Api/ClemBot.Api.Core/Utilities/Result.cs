
namespace ClemBot.Api.Core.Utilities
{
    public readonly struct Result<T, U>
    {
        public T? Value { get; }

        public U Status { get; }

        public Result(T? val, U status)
        {
            Value = val;
            Status = status;
        }
    }
}
