using ProjectManagement.Data.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProjectManagement.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public State State { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Hours { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime From { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime To { get; set; }

        public IEnumerable<SolutionViewModel> Solutions { get; set; }

        public bool DateInRange(DateTime from, DateTime to)
        {
            if (From > from || To < to)
                return false;

            return true;
        }

        public double RemainingHours()
        {
            double hours = 0;
            foreach (var solution in Solutions)
            {
                hours += solution.Hours();
            }

            return Hours - hours;
        }

        public double RemainingHours(int id)
        {
            return RemainingHours() + Solutions
                .Where(s => s.Id == id)
                .FirstOrDefault().Hours();
        }

        public bool CheckIfDateRangeIsValid()
        {
            foreach (var solution in Solutions)
            {
                if (!DateInRange(solution.From, solution.To))
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckIfHoursAreValid()
        {
            return RemainingHours() > 0;
        }
    }
}