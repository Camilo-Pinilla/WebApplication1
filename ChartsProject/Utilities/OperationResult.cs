namespace ChartsProject.Utilities
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage  { get; set; }
        public Exception? Exception { get; set; }

        public T? Data { get; set; }

        public static OperationResult<T> Resolve(T data)
        {
            return new OperationResult<T> { Data = data, IsSuccess = true };
        }

        public static OperationResult<T> Reject(string errorMessage, Exception exception)
        {
            return new OperationResult<T> { IsSuccess = false, ErrorMessage = errorMessage, Exception = exception };
        }
    }
}
