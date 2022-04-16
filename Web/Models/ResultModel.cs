namespace Web.Models
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}