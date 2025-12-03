using Microsoft.AspNetCore.Mvc;
using StudentCRUD.Models;
using StudentCRUD.Repo;
using System;

namespace StudentCRUD.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _repo;

        public StudentController(IStudentRepository repo)
        {
            _repo = repo;
        }

        // INDEX
        public IActionResult Index()
        {
            try
            {
                var data = _repo.GetAllStudents();
                return View(data);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student s)
        {
            try
            {
                if (!ModelState.IsValid) return View(s);

                int result = _repo.AddStudent(s);

                if (result == -1)
                {
                    ModelState.AddModelError("", "Email or Mobile Number already exists!");
                    return View(s);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(s);
            }
        }

        // EDIT GET
        public IActionResult Edit(int id)
        {
            var student = _repo.GetStudentById(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student s)
        {
            try
            {
                if (!ModelState.IsValid) return View(s);

                int result = _repo.UpdateStudent(s);

                if (result == -1)
                {
                    ModelState.AddModelError("", "Email or Mobile Number already exists!");
                    return View(s);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(s);
            }
        }

        // DELETE GET
        public IActionResult Delete(int id)
        {
            var student = _repo.GetStudentById(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // DELETE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _repo.DeleteStudent(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            var student = _repo.GetStudentById(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // SEARCH
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
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }
    }
}
