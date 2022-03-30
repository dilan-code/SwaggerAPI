using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibblanAPI.DTO
{
    public class BorrowLibraryItemRequestDto
    {
        public int LibraryItemId { get; set; }
        public string Borrower { get; set; }
    }
}
