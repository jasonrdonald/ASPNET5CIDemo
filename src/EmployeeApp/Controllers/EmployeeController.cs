using System.Linq;
using Microsoft.AspNet.Mvc;
using EmployeeApp.Models;
using System;
using System.Threading.Tasks;

namespace EmployeeApp.Controllers
{
	public class EmployeeController : Controller
	{
		private IEmployeeRepository _employeeRepository;

		public EmployeeController(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		// GET: Employee
        public async Task<IActionResult> Index()
		{
			var employees = await _employeeRepository.FindAll();
			return View("Index", employees.ToList());
		}

		// GET: Employee/Details/5
        public async Task<IActionResult> Details(System.Guid? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(404);
			}


			Employee employee = await _employeeRepository.Get(id);
			if (employee == null)
			{
				return new HttpStatusCodeResult(404);
			}

			return View("Details",employee);
		}

		// GET: Employee/Create
        public IActionResult Create()
		{
			return View("Create", new Employee());
		}

		// POST: Employee/Create
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Employee employee)
		{
			if (ModelState.IsValid)
			{
				_employeeRepository.Save(employee);
				return RedirectToAction("Index");
			}

			return View("Create", employee);
		}

		// GET: Employee/Edit/5
        public async Task<IActionResult> Edit(System.Guid? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(404);
			}

			Employee employee = await _employeeRepository.Get(id);
			if (employee == null)
			{
				return new HttpStatusCodeResult(404);
			}

			return View("Edit", employee);
		}

		// POST: Employee/Edit/5
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Employee employee)
		{
			if (ModelState.IsValid)
			{
				_employeeRepository.Update(employee);
				return RedirectToAction("Index");
			}

			return View("Edit", employee);
		}

		// GET: Employee/Delete/5
        [ActionName("Delete")]
		public async Task<IActionResult> Delete(System.Guid? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(404);
			}

			Employee employee = await _employeeRepository.Get(id);
			if (employee == null)
			{
				return new HttpStatusCodeResult(404);
			}

			return View("Delete", employee);
		}

		// POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(System.Guid id)
		{
			Employee employee = await _employeeRepository.Get(id);
			await _employeeRepository.Delete(employee);

            return RedirectToAction("Index");
        }
    }
}
