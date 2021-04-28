using ProjectManagement.Data.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.ViewModels
{
    public class SolutionViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime From { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime To { get; set; }
        [Required]
        public State State { get; set; }

        public double Hours()
        {
            if(To != null && From != null)
            {
                return (To - From).TotalHours;
            }

            return 0;
        }
    }
}