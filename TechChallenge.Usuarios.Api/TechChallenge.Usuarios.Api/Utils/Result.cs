namespace TechChallenge.Usuarios.Api.Utils
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        private Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string message) => new Result(false, message);
    }
}
