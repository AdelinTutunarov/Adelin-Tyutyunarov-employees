using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeesPrpjects
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "yyyy’-‘MM’-‘dd")]
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
