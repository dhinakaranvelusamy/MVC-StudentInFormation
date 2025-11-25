using Microsoft.AspNetCore.Mvc;
using Studentdata.Repo;
using Studentdata.Models;
using System;
using MVC_StudentInFormation.Models;
using Studentdata.ErorrModels;
using ErrorViewModel = Studentdata.ErorrModels.ErrorViewModel;

namespace StudentInformation.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _repo;

        public StudentController(StudentRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            try
            {
                var students = _repo.GetAllStudents();
                return View(students);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                };
                return View("Error", errorModel);
            }

        }

        public IActionResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentInfo student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.AddStudent(student);
                    return RedirectToAction(nameof(Index));
                }
                return View(student);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                };
                return View("Error", errorModel);
            }
        }


        public IActionResult Edit(int id)
        {
            try
            {
                var student = _repo.GetStudentById(id);
                if (student == null) return NotFound();
                return View(student);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                };
                return View("Error", errorModel);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentInfo student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.UpdateStudent(student);
                    return RedirectToAction(nameof(Index));
                }
                return View(student);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                };
                return View("Error", errorModel);
            }

        }
        // GET: Delete Confirmation
        public IActionResult Delete(int id)
        {
            try
            {
                var student = _repo.GetStudentById(id);
                if (student == null)
                    return NotFound();

                return View(student); // Sends student info to the Delete view
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                };
                return View("Error", errorModel);
            }

        }

        // POST: Delete Action
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _repo.DeleteStudent(id); // Calls repository to delete
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                };
                return View("Error", errorModel);
            }

        }


        public IActionResult Details(int id)
        {
            try
            {
                var student = _repo.GetStudentById(id);
                if (student == null) return NotFound();
                return View(student);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                };
                return View("Error", errorModel);
            }

        }

        [HttpPost]
        public IActionResult Search(string name)
        {
            try
            {
                var results = _repo.SearchStudentByName(name);
                return View("Index", results);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                };
                return View("Error", errorModel);
            }


        }
    }
}
