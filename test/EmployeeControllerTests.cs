using Xunit;
using EmployeeApp.Controllers;
using EmployeeApp.Models;
using Moq;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Tests
{
	public class EmployeeControllerTests
	{
		[Fact]
		public async Task VerifyIndexActionReturnsIndexView()
		{
			var employeeRepository = new Mock<IEmployeeRepository>();
			var employeeController = new EmployeeController(employeeRepository.Object);
			var actionResult = await employeeController.Index();
			var result = actionResult as ViewResult;
        	Assert.NotNull(result);
			Assert.Equal("Index", result.ViewName);
		}
		
		[Fact]
		public async Task VerifyDetailsActionReturns404IfIdIsNull()
		{
			var employeeRepository = new Mock<IEmployeeRepository>();
			var employeeController = new EmployeeController(employeeRepository.Object);
			var actionResult = await employeeController.Details(null);
			var result = actionResult as HttpStatusCodeResult;
        	Assert.NotNull(result);
			Assert.Equal(404, result.StatusCode);
		}
		
		[Fact]
		public async Task VerifyDetailsActionReturns404IfIdNotExists()
		{
			var employeeRepository = new Mock<IEmployeeRepository>();
			employeeRepository.Setup(rep => rep.Get(It.IsAny<Guid?>()))
				.Returns(Task.FromResult(default(Employee)));
			var employeeController = new EmployeeController(employeeRepository.Object);
			var actionResult = await employeeController.Details(Guid.Empty);
			var result = actionResult as HttpStatusCodeResult;
        	Assert.NotNull(result);
			Assert.Equal(404, result.StatusCode);
		}
		
		[Fact]
		public void VerifyCreateActionReturnsCreateView()
		{
			var employeeRepository = new Mock<IEmployeeRepository>();
			var employeeController = new EmployeeController(employeeRepository.Object);
			var actionResult = employeeController.Create();
			var result = actionResult as ViewResult;
        	Assert.NotNull(result);
			Assert.Equal("Create", result.ViewName);
		}
		
		[Fact]
		public void VerifyCreateActionSavesTheRecordAndRedirects()
		{
			var employeeId = Guid.NewGuid();
			var employee = new Employee();
			employee.Id = employeeId;
			employee.Name = "Employee";
			employee.Designation = "Lead";
			employee.JoiningDate = DateTime.UtcNow;
			employee.Remarks = "Remarks for employee";
			var employeeRepository = new Mock<IEmployeeRepository>();
			employeeRepository.Setup(rep => rep.Save(It.IsAny<Employee>())).Verifiable();
			var employeeController = new EmployeeController(employeeRepository.Object);
			var actionResult = employeeController.Create(employee);
			var result = actionResult as RedirectToActionResult;
        	Assert.NotNull(result);
			Assert.Equal("Index", result.ActionName);
			employeeRepository.Verify();
		}
		
		[Fact]
		public void VerifyCreateActionRedirectsToErrorIfModelStateNotValid()
		{
			var employee = new Employee();
			var employeeRepository = new Mock<IEmployeeRepository>();
			employeeRepository.Setup(rep => rep.Save(It.IsAny<Employee>())).Verifiable();
			var employeeController = new EmployeeController(employeeRepository.Object);
			//Mocking the employeeController.ModelState.IsValid = false
			employeeController.ModelState.AddModelError("Error", "Name is Required");
			var actionResult = employeeController.Create(employee);
			var result = actionResult as ViewResult;
        	Assert.NotNull(result);
			Assert.Equal("Create", result.ViewName);
		}
		
		[Fact]
		public async Task VerifyEditActionReturnsViewIfIdIsNull()
		{
			var employeeRepository = new Mock<IEmployeeRepository>();
			var employeeController = new EmployeeController(employeeRepository.Object);
			var httpStatusCodeResult = await employeeController.Edit(Guid.Empty) as HttpStatusCodeResult;
			
			Assert.NotNull(httpStatusCodeResult);
			Assert.Equal(404, httpStatusCodeResult.StatusCode);
		}
		
		[Fact]
		public async Task VerifyEditActionReturns404IfEmployeeNotFound()
		{
			var employeeRepository = new Mock<IEmployeeRepository>();
			employeeRepository.Setup(x => x.Get(It.IsAny<Guid?>())).Returns(Task.FromResult(default(Employee)));
			var employeeController = new EmployeeController(employeeRepository.Object);
			var httpStatusCodeResult = await employeeController.Edit(Guid.NewGuid()) as HttpStatusCodeResult;
			
			Assert.NotNull(httpStatusCodeResult);
			Assert.Equal(404, httpStatusCodeResult.StatusCode);
		}
		
		[Fact]
		public async Task VerifyEditActionReturnsViewWithModel()
		{
			var employeeRepository = new Mock<IEmployeeRepository>();
			var employeeController = new EmployeeController(employeeRepository.Object);
			var employee = new Employee() { Id = Guid.NewGuid(), Name = "Employee 1" };
			employeeRepository.Setup(x => x.Get(It.IsAny<Guid?>())).Returns(Task.FromResult(employee));
			var employeeResult = await employeeController.Edit(Guid.NewGuid());
			var viewResult = employeeResult as ViewResult;
			var employeeModel = viewResult.ViewData.Model as Employee;
			Assert.Equal("Edit",viewResult.ViewName);
			Assert.NotNull(employeeModel);
		}
		
		[Fact]
		public void VerifyEditActionRedirectsToErrorIfModelStateNotValid()
		{
			var employeeRepository = new Mock<IEmployeeRepository>();
			var employeeController = new EmployeeController(employeeRepository.Object);
			var employee = new Employee() { Id = Guid.NewGuid() };
			//Mocking the employeeController.ModelState.IsValid = false
			employeeController.ModelState.AddModelError("Error", "Name is Required");
			var actionResult =  employeeController.Edit(employee);
			var result = actionResult as ViewResult;
        	Assert.NotNull(result);
			Assert.Equal("Edit", result.ViewName);
		}
				
		[Fact]
		public async Task VerifyIndexPageReturnsListOfEmployees()
		{
			var employeeRepository = new Mock<IEmployeeRepository>();
			IEnumerable<Employee> employees = new List<Employee>()
			{
				new Employee() { Id = Guid.NewGuid(), Name = "Employee 1" },
				new Employee() { Id = Guid.NewGuid(), Name = "Employee 2" },
				new Employee() { Id = Guid.NewGuid(), Name = "Employee 3" }
			};
			employeeRepository.Setup(x => x.FindAll())
			.Returns(Task.FromResult(employees));
			var employeeController = new EmployeeController(employeeRepository.Object);
			var actionResult = await employeeController.Index();
			var result = actionResult as ViewResult;
			var model = result.ViewData.Model as List<Employee>;
        	Assert.NotNull(result);
			Assert.Equal("Index", result.ViewName);
			Assert.NotNull(model);
			Assert.Equal(3, model.Count);
			Assert.Equal("Employee 3", model[2].Name);
		}		
		
		[Fact]
		public async Task VerifyDetailsActionReturnsEmployee()
		{
			var employeeId = Guid.NewGuid();
			var employee = new Employee();
			employee.Id = employeeId;
			employee.Name = "Employee";
			employee.Designation = "Lead";
			employee.JoiningDate = DateTime.UtcNow;
			employee.Remarks = "Remarks for employee";
			
			var employeeRepository = new Mock<IEmployeeRepository>();
			employeeRepository.Setup(rep => rep.Get(It.IsAny<Guid?>()))
				.Returns(Task.FromResult(employee));
			var employeeController = new EmployeeController(employeeRepository.Object);
			var actionResult = await employeeController.Details(employeeId);
			var result = actionResult as ViewResult;
        	Assert.NotNull(result);
			var model = result.ViewData.Model as Employee;
			Assert.Equal(employee.Name, model.Name);
			Assert.Equal(employee.Designation, model.Designation);
			Assert.Equal(employee.JoiningDate, model.JoiningDate);
			Assert.Equal(employee.Remarks, model.Remarks);
		}
		
		[Fact]
		public async Task VerifyDeleteActionReturnsViewIfIdIsNull()
		{
			Guid? employee2Delete = null;
			var employeeRepository = new Mock<IEmployeeRepository>();
			var employeeController = new EmployeeController(employeeRepository.Object);
			var httpStatusCodeResult = await employeeController.Delete(employee2Delete) as HttpStatusCodeResult;
			
			Assert.NotNull(httpStatusCodeResult);
			Assert.Equal(404, httpStatusCodeResult.StatusCode);
		}
		
		[Fact]
		public async Task VerifyDeleteActionReturnsViewIfEmployeeNotFound()
		{
			Guid? employee2Delete = Guid.NewGuid();
			var employeeRepository = new Mock<IEmployeeRepository>();
			employeeRepository.Setup(x => x.Get(It.IsAny<Guid?>())).Returns(Task.FromResult(default(Employee)));
			var employeeController = new EmployeeController(employeeRepository.Object);
			var httpStatusCodeResult = await employeeController.Delete(employee2Delete) as HttpStatusCodeResult;
			
			Assert.NotNull(httpStatusCodeResult);
			Assert.Equal(404, httpStatusCodeResult.StatusCode);
		}
		
		[Fact]
		public async Task VerifyDeleteActionReturnsViewIfEmployeeFound()
		{
			var employeeId = Guid.NewGuid();
			var employee = new Employee();
			employee.Id = employeeId;
			employee.Name = "Employee";
			employee.Designation = "Lead";
			employee.JoiningDate = DateTime.UtcNow;
			employee.Remarks = "Remarks for employee";
			var employeeRepository = new Mock<IEmployeeRepository>();
			employeeRepository.Setup(x => x.Get(It.IsAny<Guid?>())).Returns(Task.FromResult(employee));
			var employeeController = new EmployeeController(employeeRepository.Object);
			var ViewResult = await employeeController.Delete(employeeId) as ViewResult;
			
			Assert.NotNull(ViewResult);
			Assert.Equal("Delete", ViewResult.ViewName);
		}
	}
}