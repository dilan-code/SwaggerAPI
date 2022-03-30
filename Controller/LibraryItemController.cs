using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BibblanAPI.AppDomain.Services.Interfaces;
using BibblanAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibblanAPI.Controller
{
    [ApiController]
    [Route("v1/[controller]")]
    public class LibraryItemController : ControllerBase
    {
        private readonly ILibraryItemsService _libraryItemService;
        private readonly IMapper _mapper;

        public LibraryItemController(ILibraryItemsService libraryItemsService, IMapper mapper)
        {
            _libraryItemService = libraryItemsService;
            _mapper = mapper;
        }

        [HttpGet] // Get collection of library items
        [ProducesResponseType(typeof(List<BorrowLibraryItemResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult GetCollectionOfLibraryItem([FromQuery] bool sortByType)
        {
            var collection = _libraryItemService.GetCollectionOfLibraryItems(sortByType);

            if (!collection.Any())
            {
                return NoContent();
            }

            var result = _mapper.Map<List<BorrowLibraryItemResponseDto>>(collection);
            return Ok(result);
        }

        /// <param name="libraryItemId"></param>
        [HttpGet("{libraryItemId}")]
        [ProducesResponseType(typeof(BorrowLibraryItemResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetItem([FromRoute] int libraryItemId)
        {
            var item = _libraryItemService.GetLibraryItem(libraryItemId);

            if (item == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<BorrowLibraryItemResponseDto>(item);
            return Ok(result);
        }

        /// <param name="bookRequestDto"></param>
        [HttpPost("book")]
        [ProducesResponseType(typeof(BorrowLibraryItemResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult CreateBookItem([FromBody] BookRequestDto bookRequestDto)
        {
            var item = _libraryItemService.CreateBookLibraryItem(bookRequestDto);
            if (item == null)
            {
                return UnprocessableEntity();
            }

            var result = _mapper.Map<BookResponseDto>(item);
            return Ok(result);
        }

        [HttpPost("dvd")]
        [ProducesResponseType(typeof(LibraryDvdResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult CreateDvdItem([FromBody] LibraryDvdRequestDto libraryDvdRequestDto)
        {
            var item = _libraryItemService.CreateDvdLibraryItem(libraryDvdRequestDto);

            if (item == null)
            {
                return UnprocessableEntity();
            }

            var result = _mapper.Map<LibraryDvdResponseDto>(item);

            return Ok(result);
        }


        /// <param name="audioBookRequestDto"></param>
        [HttpPost("audiobook")]
        [ProducesResponseType(typeof(AudioBookResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult CreateAudioBookItem([FromBody] AudioBookRequestDto audioBookRequestDto)
        {
            var item = _libraryItemService.CreateAudioBookLibraryItem(audioBookRequestDto);

            if (item == null)
            {
                return UnprocessableEntity();
            }

            var result = _mapper.Map<AudioBookResponseDto>(item);

            return Ok(result);
        }


        /// <param name="referenceBookRequestDto"></param>
        [HttpPost("referencebook")]
        [ProducesResponseType(typeof(ReferenceBookResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult CreateReferenceBookItem([FromBody] ReferenceBookRequestDto referenceBookRequestDto)
        {
            var item = _libraryItemService.CreateReferenceBookLibraryItem(referenceBookRequestDto);

            if (item == null)
            {
                return UnprocessableEntity();
            }

            var result = _mapper.Map<ReferenceBookResponseDto>(item);

            return Ok(result);
        }

        [HttpPut("borrow")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult BorrowItem([FromBody] BorrowLibraryItemRequestDto borrowLibraryItemRequestDto)
        {
            var borrow = _libraryItemService.BorrowLibraryItem(borrowLibraryItemRequestDto);
            if (!borrow)
            {
                return UnprocessableEntity();
            }
            return Ok(borrow);
        }

        /// <param name="libraryItemId"></param>
        [HttpPut("checkin/{libraryItemId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult CheckInItem([FromRoute] int libraryItemId)
        {
            var checkIn = _libraryItemService.CheckInLibraryItem(libraryItemId);

            if (!checkIn)
            {
                return UnprocessableEntity();
            }
            return Ok(checkIn);
        }

        /// <param name="libraryItemId"></param>
        /// <param name="bookRequestDto"></param>
        [HttpPut("book/{libraryItemId}")]
        [ProducesResponseType(typeof(BookResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult EditBookItem([FromRoute] int libraryItemId, [FromBody] BookRequestDto bookRequestDto)
        {
            var edit = _libraryItemService.UpdateBookLibraryItem(libraryItemId, bookRequestDto);

            if (edit == null)
            {
                return UnprocessableEntity();
            }
            var result = _mapper.Map<BookResponseDto>(edit);
            return Ok(result);
        }

        /// <param name="libraryItemId"></param>
        /// <param name="libraryDvdRequestDto"></param>
        [HttpPut("dvd/{libraryItemId}")]
        [ProducesResponseType(typeof(LibraryDvdRequestDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult EditDvdItem([FromRoute] int libraryItemId, [FromBody] LibraryDvdRequestDto libraryDvdRequestDto)
        {
            var edit = _libraryItemService.UpdateDvdLibraryItem(libraryItemId, libraryDvdRequestDto);

            if (edit == null)
            {
                return UnprocessableEntity();
            }
            var result = _mapper.Map<LibraryDvdResponseDto>(edit);
            return Ok(result);
        }


        /// <param name="libraryItemId"></param>
        /// <param name="audioBookRequestDto"></param>
        [HttpPut("audio/{libraryItemId}")]
        [ProducesResponseType(typeof(AudioBookResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult EditAudioItem([FromRoute] int libraryItemId, [FromBody] AudioBookRequestDto audioBookRequestDto)
        {
            var edit = _libraryItemService.UpdateAudioBookLibraryItem(libraryItemId, audioBookRequestDto);

            if (edit == null)
            {
                return UnprocessableEntity();
            }
            var result = _mapper.Map<AudioBookResponseDto>(edit);
            return Ok(result);
        }

        /// <param name="libraryItemId"></param>
        /// <param name="referenceBookRequestDto"></param>
        [HttpPut("referencebook/{libraryItemId}")]
        [ProducesResponseType(typeof(AudioBookResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult EditReferenceItem([FromRoute] int libraryItemId, [FromBody] ReferenceBookRequestDto referenceBookRequestDto)
        {
            var edit = _libraryItemService.UpdateReferenceBookLibraryItem(libraryItemId, referenceBookRequestDto);

            if (edit == null)
            {
                return UnprocessableEntity();
            }
            var result = _mapper.Map<ReferenceBookResponseDto>(edit);
            return Ok(result);
        }

        /// <param name="libraryItemId"></param>
        [HttpDelete("{libraryItemId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult DeleteItem([FromRoute] int libraryItemId)
        {
            var delete = _libraryItemService.DeleteLibraryItem(libraryItemId);

            if (!delete)
            {
                return UnprocessableEntity();
            }
            return Ok(delete);
        }



    }
}