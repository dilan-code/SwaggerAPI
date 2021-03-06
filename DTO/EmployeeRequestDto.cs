using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibblanAPI.DTO
{
    public class EmployeeRequestDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Rank { get; set; }
        public bool IsCeo { get; set; }
        public bool IsManager { get; set; }
        public int? ManagerId { get; set; }
    }
}
