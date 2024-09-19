namespace Gifty.Application.Responses
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        // Static helper methods for ease of use
        public static ServiceResponse<T> SuccessResponse(T data, string message = "")
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ServiceResponse<T> FailureResponse(string message)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}