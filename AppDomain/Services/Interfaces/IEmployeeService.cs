using BibblanAPI.DTO;
using BibblanAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibblanAPI.AppDomain.Services.Interfaces
{
    public interface IEmployeeService
    {
        ICollection<EmployeeResponseDto> GetCollectionOfEmployees();
        Employee CreateEmployee(EmployeeRequestDto employeeRequestDto);
        Employee CreateManager(ManagerRequestDto managerRequestDto);
        Employee CreateCeo(CeoRequestDto ceoRequestDto);
        Employee UpdateEmployee(int employeeId, UpdateEmployeeInfoRequestDto updateEmployeeInfoRequestDto);
        Employee UpdateManager(int managerId, UpdateEmployeeInfoRequestDto updateEmployeeInfoRequestDto);
        Employee UpdateCeo(int managerId, UpdateEmployeeInfoRequestDto updateEmployeeInfoRequestDto);
        bool DeleteEmployee(int employeeId);
    }
}
