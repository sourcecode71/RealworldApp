using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class BasedModel
    {
        public BasedModel()
        {
            SetDate = DateTime.Now;
            IsDeleted = false;
        }

        [MaxLength(200)]

        public bool IsDeleted { get; set; }
        public string SetUser { get; set; }
        public DateTime SetDate { get; set; }
    }
}
