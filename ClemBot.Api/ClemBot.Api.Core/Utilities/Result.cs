namespace ClemBot.Api.Core.Utilities
{
    public class Result<T> : IResult<T>
    {
        public T? Value { get; }

        public ResultStatus Status { get; }

        private Result(T? val, ResultStatus status)
        {
            Value = val;
            Status = status;
        }

        public static IResult<T> Success(T val)
            => new Result<T>(val, ResultStatus.Success);

        public static IResult<T> NotFound()
            => new Result<T>(default, ResultStatus.NotFound);

        public static IResult<T> Conflict()
            => new Result<T>(default, ResultStatus.Conflict);
    }
}
