namespace Presentation.Models
{
    public class ServiceResponse<T> : BaseResponse<T>
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public string? Error { get; set; }

    }
}
