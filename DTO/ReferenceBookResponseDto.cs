using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibblanAPI.DTO
{
    public class ReferenceBookResponseDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int? Pages { get; set; }
        public bool IsBorrowable { get; set; }
        public string Type { get; set; }
    }
}
