using AutoMapper;
using BibblanAPI.AppDomain.Services.Interfaces;
using BibblanAPI.DTO;
using BibblanAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibblanAPI.AppDomain.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DbBibblanContext _dbBibblanContext;
        private readonly IMapper _mapper;

        public EmployeeService(DbBibblanContext dbBibblanContext, IMapper mapper)
        {
            _dbBibblanContext = dbBibblanContext;
            _mapper = mapper;
        }

        public ICollection<EmployeeResponseDto> GetCollectionOfEmployees()
        {
            var employeeList = _dbBibblanContext.Employees.ToList();

            if (employeeList.Any())
            {
                var mappedEmployeeList = _mapper.Map<List<EmployeeResponseDto>>(employeeList);
                foreach (var employee in mappedEmployeeList)
                {
                    if (employee.IsManager == false && employee.IsCeo == false)
                    {
                        employee.Role = "Employee";
                        continue;
                    }
                    if (employee.IsManager)
                    {
                        employee.Role = "Manager";
                        continue;
                    }
                    if (employee.IsCeo)
                    {
                        employee.Role = "Ceo";
                        continue;
                    }
                }

                mappedEmployeeList = mappedEmployeeList.OrderBy(x => x.Role).ToList();
                return mappedEmployeeList;
            }

            return new List<EmployeeResponseDto>();
        }

        public Employee CreateEmployee(EmployeeRequestDto employeeRequestDto)
        {

            var manager = _dbBibblanContext.Employees.FirstOrDefault(x => x.Id == employeeRequestDto.ManagerId
            && x.IsManager == true);
            if (manager != null) // Kontrollerar att det finns en manager som den anställda kan ha
            {
                var newEmployee = _mapper.Map<Employee>(employeeRequestDto);

                newEmployee.FirstName = employeeRequestDto.FirstName;
                newEmployee.LastName = employeeRequestDto.LastName;
                newEmployee.IsManager = false;
                newEmployee.IsCeo = false;
                newEmployee.ManagerId = employeeRequestDto.ManagerId;
                newEmployee.Salary = (decimal)Calculate.CalculateSalary(employeeRequestDto.Rank, "Employee");

                _dbBibblanContext.Add(newEmployee);
                return SaveToDb(newEmployee);
            }
            return null;
        }

        public Employee CreateManager(ManagerRequestDto managerRequestDto)
        {
            var manager = _dbBibblanContext.Employees.FirstOrDefault(x => x.Id == managerRequestDto.ManagerId
            && (x.IsManager == true || x.IsCeo == true)); // Manager måste rapportera till existerande manager

            if (manager != null)
            {
                var newManager = _mapper.Map<Employee>(managerRequestDto);
                newManager.FirstName = managerRequestDto.FirstName;
                newManager.LastName = managerRequestDto.LastName;
                newManager.ManagerId = managerRequestDto.ManagerId;
                newManager.IsManager = true;
                newManager.IsCeo = false;
                newManager.Salary = (decimal)Calculate.CalculateSalary(managerRequestDto.Rank, "Manager");
                return SaveToDb(newManager);

            }
            return null;
        }

        public Employee CreateCeo(CeoRequestDto ceoRequestDto)
        {
            var employeeIsCeo = _dbBibblanContext.Employees.FirstOrDefault(x => x.IsCeo == true);

            if (employeeIsCeo != null) // Får bara vara 1 ceo i taget
            {
                return null;
            }

            if (employeeIsCeo == null)
            {
                var ceo = _mapper.Map<Employee>(ceoRequestDto);
                ceo.FirstName = ceoRequestDto.FirstName;
                ceo.LastName = ceoRequestDto.LastName;
                ceo.IsManager = false;
                ceo.ManagerId = null;
                ceo.IsCeo = true;
                ceo.Salary = (decimal)Calculate.CalculateSalary(ceoRequestDto.Rank, "Ceo");
                return SaveToDb(ceo);
            }
            return null;
        }

        public bool DeleteEmployee(int employeeId)
        {
            var employee = _dbBibblanContext.Employees.FirstOrDefault(x => x.Id == employeeId);

            if (employee == null)
            {
                return false; // anställd hittades ej
            }

            if (employee.IsManager == true || employee.IsCeo == true) // kontroll om anställda är manager eller ceo
            {
                if (managerOverOtherEmployee(employeeId))
                {
                    return false; // går ej att ta bort manager eller ceo som har anställda
                }
                _dbBibblanContext.Remove(employee);
                _dbBibblanContext.SaveChanges();
                return true;
            }
            _dbBibblanContext.Remove(employee);
            _dbBibblanContext.SaveChanges();
            return true;
        }


        public Employee UpdateEmployee(int employeeId, UpdateEmployeeInfoRequestDto updateEmployeeInfoRequestDto)
        {
            var employee = _dbBibblanContext.Employees.FirstOrDefault(x => x.Id == employeeId);
            if (employee == null)
            {
                return null; //anställd hittades inte
            }
            employee.FirstName = string.IsNullOrEmpty(updateEmployeeInfoRequestDto.FirstName) ? updateEmployeeInfoRequestDto.FirstName : employee.FirstName;
            employee.LastName = string.IsNullOrEmpty(updateEmployeeInfoRequestDto.LastName) ? updateEmployeeInfoRequestDto.LastName : employee.LastName;
            employee.Salary = updateEmployeeInfoRequestDto.Rank == 0 ? employee.Salary : (decimal)Calculate.CalculateSalary(updateEmployeeInfoRequestDto.Rank, "employee");
            _dbBibblanContext.SaveChanges();
            return employee;
        }

        public Employee UpdateManager(int managerId, UpdateEmployeeInfoRequestDto updateEmployeeInfoRequestDto)
        {
            var manager = _dbBibblanContext.Employees.FirstOrDefault(x => x.ManagerId == managerId);
            var managerEmployee = _dbBibblanContext.Employees.FirstOrDefault(x => x.Id == updateEmployeeInfoRequestDto.ManagerId);
            if (manager == null)
            {
                return null; // manager hittades inte
            }
            if (manager.IsManager == true && (managerEmployee.IsManager == true || managerEmployee.IsCeo == true)) // kontroll ifall det är en manager som har en annan manager eller ceo att rapportera till
            {
                manager.FirstName = string.IsNullOrEmpty(updateEmployeeInfoRequestDto.FirstName) ? updateEmployeeInfoRequestDto.FirstName : manager.FirstName;
                manager.LastName = string.IsNullOrEmpty(updateEmployeeInfoRequestDto.LastName) ? updateEmployeeInfoRequestDto.LastName : manager.LastName;
                manager.Salary = updateEmployeeInfoRequestDto.Rank == 1.725 ? manager.Salary : (decimal)Calculate.CalculateSalary(updateEmployeeInfoRequestDto.Rank, "manager");
                _dbBibblanContext.SaveChanges();
            }
            return manager;
        }

        public Employee UpdateCeo(int managerId, UpdateEmployeeInfoRequestDto updateEmployeeInfoRequestDto)
        {
            var isCeo = _dbBibblanContext.Employees.FirstOrDefault(x => x.IsCeo == true);
            if (updateEmployeeInfoRequestDto.ManagerId == managerId) // Kontroll om det är ceo
            {

                isCeo.FirstName = string.IsNullOrEmpty(updateEmployeeInfoRequestDto.FirstName) ? updateEmployeeInfoRequestDto.FirstName : isCeo.FirstName;
                isCeo.LastName = string.IsNullOrEmpty(updateEmployeeInfoRequestDto.LastName) ? updateEmployeeInfoRequestDto.LastName : isCeo.LastName;
                isCeo.Salary = updateEmployeeInfoRequestDto.Rank == 2.725 ? isCeo.Salary : (decimal)Calculate.CalculateSalary(updateEmployeeInfoRequestDto.Rank, "ceo");
                _dbBibblanContext.SaveChanges();
            }

            return isCeo;
        }

        public bool managerOverOtherEmployee(int employeeId)
        {
            var managerOverEmployee = _dbBibblanContext.Employees.Any(x => x.ManagerId == employeeId);
            return managerOverEmployee;
        }

        public Employee SaveToDb(Employee employee)
        {
            _dbBibblanContext.Add(employee);

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return employee;
            }
            return null;
        }

    }
}
