using demo.Data;
using demo.Models;
using demo.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace demo.Controllers
{
    public class EmpController : Controller
    {
        private readonly MvcDemoDbContents mvcDemoDbContent;
        public EmpController(MvcDemoDbContents mvcDemoDbContent)
        {
            this.mvcDemoDbContent = mvcDemoDbContent;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
             var employees=    await  mvcDemoDbContent.Emp.ToListAsync();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Emp()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateofBirth = addEmployeeRequest.DateofBirth,
                Department = addEmployeeRequest.Department
            };
          await  mvcDemoDbContent.Emp.AddAsync(employee);
           await mvcDemoDbContent.SaveChangesAsync();

            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
         var employee= await mvcDemoDbContent.Emp.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateofBirth = employee.DateofBirth,
                    Department = employee.Department
                };
                return await Task.Run(()=> View("View",viewModel));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContent.Emp.FindAsync(model.Id);
            if(employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateofBirth = model.DateofBirth;
                employee.Department = model.Department;

             await  mvcDemoDbContent.SaveChangesAsync();

                return RedirectToAction("Index");


            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model) {

            var employee = await mvcDemoDbContent.Emp.FindAsync(model.Id);

            if(employee != null)
            {
                mvcDemoDbContent.Emp.Remove(employee);
                await mvcDemoDbContent.SaveChangesAsync();

                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }
    }
}
