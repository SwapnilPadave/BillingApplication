namespace BA.Utility.Result
{
    public class Result
    {
        private Result(bool isSuccess, Error error, object? data = null)
        {
            if (isSuccess && error != Error.None
                || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Data = data;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public object? Data { get; set; }
        public Error Error { get; }

        public static Result Success() => new(true, Error.None);
        public static Result Success(object data) => new(true, Error.None, data);
        public static Result Failure(Error error) => new(false, error);
    }
}
