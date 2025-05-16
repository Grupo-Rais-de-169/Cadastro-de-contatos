namespace TechChallenge.DAO.Api.Utils
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Result()
        {
            
        }
        private Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string message) => new Result(false, message);
    }
}
