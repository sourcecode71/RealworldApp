using Domain.Enums;

namespace Application.DTOs
{
    public class ProjectCorrentStatusDTO
    {
        public string ProjectId { get; set; }
        public string ProjectNo { get; set; }
        public int Status { get; set; }
        public string Comments { get; set; }
        public string SetUser { get; set; }
    }
}
