using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibblanAPI.DTO
{
    public class CeoRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Rank { get; set; }

        public bool IsCeo { get; set; }
    }
}
