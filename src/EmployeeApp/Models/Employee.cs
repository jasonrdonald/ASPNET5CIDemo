using System.ComponentModel.DataAnnotations;
using System;

namespace EmployeeApp.Models
{
	public class Employee
	{
		public Employee()
		{
			Id = Guid.NewGuid();
			JoiningDate = DateTime.UtcNow;
		}
		public Guid Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Designation { get; set; }
		public DateTime JoiningDate { get; set; }
		public string Remarks { get; set; }
	}
}