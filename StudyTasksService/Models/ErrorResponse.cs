namespace StudyTasksService.Models
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }

        public string Message { get; set; }

        public string RequestId { get; set; }
    }
}