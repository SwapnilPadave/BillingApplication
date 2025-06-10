namespace BA.Utility.Result
{
    public sealed record Error(string ErrorMsg)
    {
        public static readonly Error None = new(string.Empty);

        public static implicit operator Result(Error error) => Result.Failure(error);
    }
}
