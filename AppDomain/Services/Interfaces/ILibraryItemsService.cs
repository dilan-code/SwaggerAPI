using BibblanAPI.DTO;
using BibblanAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibblanAPI.AppDomain.Services.Interfaces
{
    public interface ILibraryItemsService
    {
        ICollection<LibraryItem> GetCollectionOfLibraryItems(bool sortByType = false);
        LibraryItem GetLibraryItem(int libraryItemId);
        LibraryItem CreateBookLibraryItem(BookRequestDto bookRequestDto);
        LibraryItem CreateDvdLibraryItem(LibraryDvdRequestDto dvdLibraryItemRequestDto);
        LibraryItem CreateAudioBookLibraryItem(AudioBookRequestDto audioBookLibraryItemRequestDto);
        LibraryItem CreateReferenceBookLibraryItem(ReferenceBookRequestDto referenceBookLibraryItemRequestDto);
        bool BorrowLibraryItem(BorrowLibraryItemRequestDto borrowLibraryItemRequestDto);
        bool CheckInLibraryItem(int libraryItemId);
        LibraryItem UpdateBookLibraryItem(int libraryItemId, BookRequestDto bookLibraryItemRequestDto);
        LibraryItem UpdateDvdLibraryItem(int libraryItemId, LibraryDvdRequestDto dvdLibraryItemRequestDto);
        LibraryItem UpdateAudioBookLibraryItem(int libraryItemId, AudioBookRequestDto audioBookLibraryItemRequestDto);
        LibraryItem UpdateReferenceBookLibraryItem(int libraryItemId, ReferenceBookRequestDto referenceBookLibraryItemRequestDto);
        bool DeleteLibraryItem(int libraryItemId);
    }
}
