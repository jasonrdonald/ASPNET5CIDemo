using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EmployeeApp.Models
{
	public interface IEmployeeRepository
	{
		Task<Employee> Get(Guid? id);
		Task Save(Employee employee);
		Task Delete(Employee employee);
		Task Update(Employee employee);
		Task<IEnumerable<Employee>> FindAll();
	}
}