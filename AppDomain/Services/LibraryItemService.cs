using AutoMapper;
using BibblanAPI.AppDomain.Services.Interfaces;
using BibblanAPI.DTO;
using BibblanAPI.Repository;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibblanAPI.AppDomain.Services
{
    public class LibraryItemService : ILibraryItemsService
    {
        private readonly DbBibblanContext _dbBibblanContext;
        private readonly IMapper _mapper;

        public LibraryItemService(DbBibblanContext dbBibblanContext, IMapper mapper)
        {
            _dbBibblanContext = dbBibblanContext;
            _mapper = mapper;
        }

        public ICollection<LibraryItem> GetCollectionOfLibraryItems(bool sortByType = false)
        {
            var libraryItemCollection = _dbBibblanContext.LibraryItems.Include(x => x.Category).ToList();

            if (sortByType)
            {
                libraryItemCollection = libraryItemCollection.OrderBy(x => x.Type).ToList();
                return libraryItemCollection;
            }

            libraryItemCollection = libraryItemCollection.OrderBy(x => x.Category.CategoryName).ToList();
            return libraryItemCollection;
        }

        public LibraryItem GetLibraryItem(int itemId)
        {
            var libraryItem = _dbBibblanContext.LibraryItems.Include(x => x.Category).FirstOrDefault(x => x.Id == itemId);
            if (libraryItem == null)
            {
                return null;
            }
            return libraryItem;
        }
        public LibraryItem CreateAudioBookLibraryItem(AudioBookRequestDto audioBookLibraryItemRequestDto)
        {
            var libraryItem = _mapper.Map<LibraryItem>(audioBookLibraryItemRequestDto);
            libraryItem.Type = "Audio book";
            libraryItem.Author = ""; // Krävs att "author" står tom i databas kolumnen
            libraryItem.Borrower = ""; // Fylls automatiskt när användare lånar boken

            if (CategoryExists(audioBookLibraryItemRequestDto.CategoryId))
            {
                return SaveToDb(libraryItem);
            }
            return null; // Kategorin finns inte, går ej att spara
        }

        public LibraryItem CreateBookLibraryItem(BookRequestDto bookRequestDto)
        {
            var libraryItem = _mapper.Map<LibraryItem>(bookRequestDto);
            libraryItem.Type = "Book";
            libraryItem.Borrower = ""; // Sker när användaren lånar boken

            if (CategoryExists(bookRequestDto.CategoryId))
            {
                return SaveToDb(libraryItem);
            }
            return null; // Kategorin finns inte, går ej att spara
        }

        public LibraryItem CreateDvdLibraryItem(LibraryDvdRequestDto dvdLibraryItemRequestDto)
        {
            var libraryItem = _mapper.Map<LibraryItem>(dvdLibraryItemRequestDto);
            libraryItem.Type = "Audio Book";
            libraryItem.Author = ""; // Krävs att "author" står tom i databas kolumnen
            libraryItem.Borrower = "";

            if (CategoryExists(dvdLibraryItemRequestDto.CategoryId))
            {
                return SaveToDb(libraryItem);
            }
            return null; // Kategorin finns inte, går ej att spara
        }

        public LibraryItem CreateReferenceBookLibraryItem(ReferenceBookRequestDto referenceBookLibraryItemRequestDto)
        {
            var libraryItem = _mapper.Map<LibraryItem>(referenceBookLibraryItemRequestDto);
            libraryItem.Type = "Reference book";
            libraryItem.IsBorrowable = false;
            libraryItem.Borrower = "";

            if (CategoryExists(referenceBookLibraryItemRequestDto.CategoryId))
            {
                return SaveToDb(libraryItem);
            }
            return null; // Kategorin finns inte, går ej att spara
        }

        public bool BorrowLibraryItem(BorrowLibraryItemRequestDto borrowLibraryItemRequestDto)
        {
            var libraryItem = _dbBibblanContext.LibraryItems.FirstOrDefault(x => x.Id == borrowLibraryItemRequestDto.LibraryItemId
            && x.IsBorrowable == true && x.Type != "Reference Book");

            if (libraryItem == null)
            {
                return false;
            }

            libraryItem.IsBorrowable = false;
            libraryItem.Borrower = borrowLibraryItemRequestDto.Borrower;
            libraryItem.BorrowDate = DateTime.Now;

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return true;
            }
            return false;
        }

        public bool CheckInLibraryItem(int libraryItemId)
        {
            var libraryItem = _dbBibblanContext.LibraryItems.FirstOrDefault(x => x.Id == libraryItemId && x.IsBorrowable == false
            && x.Type != "Refernce Book");

            if (libraryItem == null)
            {
                return false; // Antingen existerar inte artikeln eller så är den redan utlånad
            }

            libraryItem.IsBorrowable = true;
            libraryItem.Borrower = "";
            libraryItem.BorrowDate = null;

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return true;
            }
            return false;

        }


        public bool DeleteLibraryItem(int libraryItemId)
        {
            var libraryItem = _dbBibblanContext.LibraryItems.FirstOrDefault(x => x.Id == libraryItemId);

            if (libraryItem == null)
            {
                return false; // Artikel som inte existerar kan inte raderas
            }
            if (!string.IsNullOrEmpty(libraryItem.Borrower) && libraryItem.BorrowDate != null)
            {
                return false;
            }

            _dbBibblanContext.LibraryItems.Remove(libraryItem);

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return true;
            }
            return false;

        }


        public LibraryItem UpdateAudioBookLibraryItem(int libraryItemId, AudioBookRequestDto audioBookLibraryItemRequestDto)
        {
            var libraryItem = _dbBibblanContext.LibraryItems.FirstOrDefault(x => x.Id == libraryItemId);

            if (libraryItem == null)
            {
                return null;
            }

            libraryItem.CategoryId = CategoryExists(audioBookLibraryItemRequestDto.CategoryId) ? audioBookLibraryItemRequestDto.CategoryId : libraryItem.CategoryId;
            libraryItem.RunTimeMinutes = audioBookLibraryItemRequestDto.RunTimeMinutes != null ? audioBookLibraryItemRequestDto.RunTimeMinutes : null;
            libraryItem.IsBorrowable = audioBookLibraryItemRequestDto.IsBorrowable ? true : false;
            libraryItem.Title = string.IsNullOrEmpty(audioBookLibraryItemRequestDto.Title) ? audioBookLibraryItemRequestDto.Title : libraryItem.Title;

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return libraryItem;
            }

            return null;
        }

        public LibraryItem UpdateBookLibraryItem(int libraryItemId, BookRequestDto bookLibraryItemRequestDto)
        {
            var libraryItem = _dbBibblanContext.LibraryItems.FirstOrDefault(x => x.Id == libraryItemId);

            if (libraryItem == null)
            {
                return null; // Existerar inte
            }

            libraryItem.CategoryId = CategoryExists(bookLibraryItemRequestDto.CategoryId) ? bookLibraryItemRequestDto.CategoryId : libraryItem.CategoryId;
            libraryItem.Author = String.IsNullOrEmpty(bookLibraryItemRequestDto.Author) ? bookLibraryItemRequestDto.Author : libraryItem.Author;
            libraryItem.Pages = bookLibraryItemRequestDto.Pages != null ? bookLibraryItemRequestDto.Pages : null;
            libraryItem.IsBorrowable = bookLibraryItemRequestDto.IsBorrowable ? true : false;
            libraryItem.Title = string.IsNullOrEmpty(bookLibraryItemRequestDto.Title) ? libraryItem.Title : bookLibraryItemRequestDto.Title;

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return libraryItem;
            }
            return null;
        }

        public LibraryItem UpdateDvdLibraryItem(int libraryItemId, LibraryDvdRequestDto dvdLibraryItemRequestDto)
        {
            var libraryItem = _dbBibblanContext.LibraryItems.FirstOrDefault(x => x.Id == libraryItemId);

            if (libraryItem == null)
            {
                return null;
            }

            libraryItem.CategoryId = CategoryExists(dvdLibraryItemRequestDto.CategoryId) ? dvdLibraryItemRequestDto.CategoryId : libraryItem.CategoryId;
            libraryItem.RunTimeMinutes = dvdLibraryItemRequestDto.RunTimeMinutes != null ? dvdLibraryItemRequestDto.RunTimeMinutes : null;
            libraryItem.IsBorrowable = dvdLibraryItemRequestDto.IsBorrowable ? true : false;
            libraryItem.Title = string.IsNullOrEmpty(dvdLibraryItemRequestDto.Title) ? dvdLibraryItemRequestDto.Title : libraryItem.Title;

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return libraryItem;
            }
            return null;
        }

        public LibraryItem UpdateReferenceBookLibraryItem(int libraryItemId, ReferenceBookRequestDto referenceBookLibraryItemRequestDto)
        {
            var libraryItem = _dbBibblanContext.LibraryItems.FirstOrDefault(x => x.Id == libraryItemId);

            if (libraryItem == null)
            {
                return null;
            }

            libraryItem.CategoryId = CategoryExists(referenceBookLibraryItemRequestDto.CategoryId) ? referenceBookLibraryItemRequestDto.CategoryId : libraryItem.CategoryId;
            libraryItem.Author = string.IsNullOrEmpty(referenceBookLibraryItemRequestDto.Author) ? referenceBookLibraryItemRequestDto.Author : libraryItem.Author;
            libraryItem.Pages = referenceBookLibraryItemRequestDto.Pages != null ? referenceBookLibraryItemRequestDto.Pages : null;
            libraryItem.Title = string.IsNullOrEmpty(referenceBookLibraryItemRequestDto.Title) ? referenceBookLibraryItemRequestDto.Title : libraryItem.Title;

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return libraryItem;
            }
            return null;
        }

        public LibraryItem SaveToDb(LibraryItem libraryItem)
        {
            _dbBibblanContext.Add(libraryItem);

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return libraryItem;
            }
            return null;
        }
        public bool CategoryExists(int id)
        {
            var category = _dbBibblanContext.Categories.FirstOrDefault(x => x.Id == id);
            if (category != null)
            {
                return true;
            }
            return false;
        }
    }
}
