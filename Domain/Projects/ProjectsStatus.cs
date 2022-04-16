using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Projects
{
    public class ProjectsStatus : BasedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public ProjectStatus Status { get; set; }
        [MaxLength(250)]
        public string Comments { get; set; }
        public DateTime SatusSetDate { get; set; }


    }
}
