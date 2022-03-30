using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BibblanAPI.Repository;

namespace BibblanAPI.DTO.Configurations
{
    public class MapConfiguration : Profile
    {
        public MapConfiguration()
        {
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<LibraryItem, BorrowLibraryItemResponseDto>().ForMember(x => x.Category, y => y.MapFrom(z => z.Category));

            CreateMap<BookRequestDto, LibraryItem>();
            CreateMap<LibraryItem, BookResponseDto>();

            CreateMap<LibraryDvdRequestDto, LibraryItem>();
            CreateMap<LibraryItem, LibraryDvdResponseDto>();

            CreateMap<AudioBookRequestDto, LibraryItem>();
            CreateMap<LibraryItem, AudioBookResponseDto>();

            CreateMap<ReferenceBookRequestDto, LibraryItem>();
            CreateMap<LibraryItem, ReferenceBookResponseDto>();

            CreateMap<EmployeeRequestDto, Employee>().ForSourceMember(x => x.Rank, y => y.DoNotValidate());
            CreateMap<Employee, EmployeeResponseDto>();

            CreateMap<ManagerRequestDto, Employee>().ForSourceMember(x => x.Rank, y => y.DoNotValidate());
            CreateMap<Employee, EmployeeResponseDto>();

            CreateMap<CeoRequestDto, Employee>().ForSourceMember(x => x.Rank, y => y.DoNotValidate());
            CreateMap<Employee, EmployeeResponseDto>();

        }
    }
}
