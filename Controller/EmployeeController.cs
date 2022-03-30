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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet("employees")] // Hämtar lista av anställda
        [ProducesResponseType(typeof(List<EmployeeResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult GetEmployees()
        {
            var employees = _employeeService.GetCollectionOfEmployees();

            if (!employees.Any())
            {
                return NoContent();
            }

            var result = _mapper.Map<List<EmployeeResponseDto>>(employees);
            return Ok(result);
        }

        /// <param name="employeeRequestDto"></param>
        [HttpPost("employee")]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult CreateEmployee([FromBody] EmployeeRequestDto employeeRequestDto)
        {
            var employee = _employeeService.CreateEmployee(employeeRequestDto);

            if (employee == null)
            {
                return UnprocessableEntity();
            }

            var result = _mapper.Map<EmployeeResponseDto>(employee);
            return Ok(result);
        }

        /// <param name="managerRequestDto"></param>
        [HttpPost("manager")]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult CreateManager([FromBody] ManagerRequestDto managerRequestDto)
        {
            var manager = _employeeService.CreateManager(managerRequestDto);

            if (manager == null)
            {
                return UnprocessableEntity();
            }

            var result = _mapper.Map<EmployeeResponseDto>(manager);
            return Ok(result);
        }

        /// <param name="ceoRequestDto"></param>
        [HttpPost("ceo")]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult CreateCeo([FromBody] CeoRequestDto ceoRequestDto)
        {
            var ceo = _employeeService.CreateCeo(ceoRequestDto);

            if (ceo == null)
            {
                return UnprocessableEntity();
            }

            var result = _mapper.Map<EmployeeResponseDto>(ceo);
            return Ok(result);
        }

        /// <param name="employeeId"></param>
        /// <param name="updateEmployeeInfoRequestDto"></param>
        [HttpPut("{employeeId}")]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult UpdateEmployee([FromRoute] int employeeId, [FromBody] UpdateEmployeeInfoRequestDto updateEmployeeInfoRequestDto)
        {
            var employee = _employeeService.UpdateEmployee(employeeId, updateEmployeeInfoRequestDto);

            if (employee == null)
            {
                return UnprocessableEntity();
            }

            var result = _mapper.Map<EmployeeResponseDto>(employee);
            return Ok(result);
        }

        /// <param name="managerId"></param>
        /// <param name="updateEmployeeInfoRequestDto"></param>
        [HttpPut("{managerId}")]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult UpdateManager([FromRoute] int managerId, [FromBody] UpdateEmployeeInfoRequestDto updateEmployeeInfoRequestDto)
        {
            var manager = _employeeService.UpdateManager(managerId, updateEmployeeInfoRequestDto);

            if (manager == null)
            {
                return UnprocessableEntity();
            }

            var result = _mapper.Map<EmployeeResponseDto>(manager);
            return Ok(result);
        }

        /// <param name="ceoId"></param>
        /// <param name="updateEmployeeInfoRequestDto"></param>
        [HttpPut("{ceoId}")]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult UpdateCeo([FromRoute] int ceoId, [FromBody] UpdateEmployeeInfoRequestDto updateEmployeeInfoRequestDto)
        {
            var ceo = _employeeService.UpdateCeo(ceoId, updateEmployeeInfoRequestDto);

            if (ceo == null)
            {
                return UnprocessableEntity();
            }

            var result = _mapper.Map<EmployeeResponseDto>(ceo);
            return Ok(result);
        }

        /// <param name="employeeId"></param>
        [HttpDelete("{employeeId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        public IActionResult DeleteEmployee([FromRoute] int employeeId)
        {
            var delete = _employeeService.DeleteEmployee(employeeId);

            if (!delete)
            {
                return UnprocessableEntity();
            }

            return Ok(delete);
        }
    }
}