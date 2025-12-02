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
                return View("Error", new ErrorViewModel { ErrorMessage = ex.Message, RequestId = HttpContext.TraceIdentifier });
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student s)
        {
            if (!ModelState.IsValid)
            {
                return View(s);
            }

            int result = _repo.AddStudent(s);

            if (result == -1)
            {
                ModelState.AddModelError("", "Email or Mobile already exists.");
                return View(s);
            }

            return RedirectToAction("Index");
        }

        // EDIT GET
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
                return View("Error", new ErrorViewModel { ErrorMessage = ex.Message, RequestId = HttpContext.TraceIdentifier });
            }
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student s)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int status = _repo.UpdateStudent(s);
                    if (status == -1)
                    {
                        ModelState.AddModelError("", "Email or Mobile number already exists!");
                        return View(s);
                    }
                    return RedirectToAction("Index");
                }
                return View(s);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ex.Message, RequestId = HttpContext.TraceIdentifier });
            }
        }

        // DELETE GET
        public IActionResult Delete(int id)
        {
            var student = _repo.GetStudentById(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.DeleteStudent(id);
            return RedirectToAction("Index");
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
            var results = _repo.SearchStudentByName(name);
            return View("Index", results);
        }
    }
}
