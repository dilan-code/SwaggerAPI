using System;
using System.Collections.Generic;

#nullable disable

namespace BibblanAPI.Repository
{
    public partial class Category
    {
        public Category()
        {
            LibraryItems = new HashSet<LibraryItem>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<LibraryItem> LibraryItems { get; set; }
    }
}
