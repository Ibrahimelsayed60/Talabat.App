using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
		private readonly IGenericRepository<Employee> _employeeRepo;

		public EmployeeController(IGenericRepository<Employee> EmployeeRepo)
        {
			_employeeRepo = EmployeeRepo;
		}

        [HttpGet]
		public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var Spec = new EmployeeWithDepartmentSpecifications();
            var emplyees = await _employeeRepo.GetAllWithSpecAsync(Spec);
            return Ok(emplyees);

        }
    }
}
