using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Entity;
using System.Threading.Tasks;
using System;

namespace EmployeeApp.Models
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private EmployeeContext _employeeContext;
		public EmployeeRepository()
		{
			_employeeContext = new EmployeeContext();
		}
		public async Task<Employee> Get(Guid? id)
		{
			return await _employeeContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
		}
		
		public async Task Save(Employee employee)
		{
			 _employeeContext.Employees.Add(employee);
			 await _employeeContext.SaveChangesAsync();
		}
		
		public async Task Delete(Employee employee)
		{
			_employeeContext.Employees.Remove(employee);
			 await _employeeContext.SaveChangesAsync();
		}
		
		public async Task Update(Employee employee)
		{
			_employeeContext.Employees.Update(employee);
			await _employeeContext.SaveChangesAsync();
		}
		
		public async Task<IEnumerable<Employee>> FindAll()
		{
			return await _employeeContext.Employees.ToListAsync();
		}
	}
}