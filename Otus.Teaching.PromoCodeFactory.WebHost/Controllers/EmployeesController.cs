using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController
        : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {

            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать нового сотрудника
        /// </summary>
        /// <param name="createEmployee"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync([FromBody] EmployeeShortRequest employeeRequest)
        {
            // Создаем объект класса сотрудник        
            Employee employee = new()
            {
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                AppliedPromocodesCount = 5,
                Roles = new List<Role>()
            };

            // Этот сотрудник пусть будет администаротором
            employee.Roles.Add(new Role()
            {
                Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                Name = "Admin",
                Description = "Администратор",
            });

            // Создаем его в репозитарии
            var createdEmployee = await _employeeRepository.CreateAsync(employee);
            if (createdEmployee == null)
                return BadRequest();

            // Возвращаем созданного сотрудника
            var employeeModel = new EmployeeResponse()
            {
                Id = createdEmployee.Id,
                Email = createdEmployee.Email,
                Roles = createdEmployee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = createdEmployee.FullName,
                AppliedPromocodesCount = createdEmployee.AppliedPromocodesCount
            };
            return employeeModel;
        }

        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <param name="createEmployee"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> Delete(Guid id)
        {
            var employee = await _employeeRepository.Delete(id);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };
            return Ok(employee);
        }

        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        /// <param name="UpdateEmployee"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<EmployeeResponse>> Update(Guid id, [FromBody] EmployeeShortRequest employeeRequest)
        {
            var findedEmployee = await _employeeRepository.GetByIdAsync(id);
            if (findedEmployee == null)
            {
                return NotFound();
            }

            //employee.FirstName = employeeRequest.FirstName;
            //employee.LastName = employeeRequest.LastName;
            //employee.Email = employeeRequest.Email;

            Employee employee = new()
            {
                Id = findedEmployee.Id,
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                AppliedPromocodesCount = findedEmployee.AppliedPromocodesCount,
                Roles = findedEmployee.Roles
            };

            var updatedEmployee = await _employeeRepository.Update(employee);
            // Возвращаем измененного  сотрудника
            var employeeModel = new EmployeeResponse()
            {
                Id = updatedEmployee.Id,
                Email = updatedEmployee.Email,
                Roles = updatedEmployee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = updatedEmployee.FullName,
                AppliedPromocodesCount = updatedEmployee.AppliedPromocodesCount
            };
            return Ok(employeeModel);
        }
    }
}