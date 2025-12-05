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


        #region CRUD Operation
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

        #endregion

        #region
        // Dashboard page
        public IActionResult Dashboard()
        {
            var students = _repo.GetAllStudents();
            return View(students);
        }

        [HttpGet]
        public IActionResult _Create()
        {
            return PartialView("_CreateStudent", new Student());
        }

        [HttpPost]
        public IActionResult _Create(Student model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddStudent(model);
                return Json(new { success = true });
            }
            return PartialView("_CreateStudent", model);
        }

        [HttpGet]
        public IActionResult _Edit(int id)
        {
            var student = _repo.GetStudentById(id);
            return PartialView("_EditStudent", student);
        }

        [HttpPost]
        public IActionResult _Edit(Student model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateStudent(model);
                return Json(new { success = true });
            }
            return PartialView("_EditStudent", model);
        }

        [HttpGet]
        public IActionResult _Details(int id)
        {
            var student = _repo.GetStudentById(id);
            return PartialView("_Details", student);
        }

        [HttpGet]
        public IActionResult _Delete(int id)
        {
            var student = _repo.GetStudentById(id);
            return PartialView("_DeleteStudent", student);
        }

        [HttpPost]
        public IActionResult _DeleteConfirmed(int id)
        {
            _repo.DeleteStudent(id);
            return Json(new { success = true });
        }
    }
}
#endregion
