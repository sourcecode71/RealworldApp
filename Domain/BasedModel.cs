using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class BasedModel
    {
        public BasedModel()
        {
            SetDate = DateTime.Now;
        }

        [MaxLength(200)]
        public string SetUser { get; set; }
        public DateTime SetDate { get; set; } 
    }
}
