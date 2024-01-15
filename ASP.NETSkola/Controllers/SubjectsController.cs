using ASP.NETSkola.Models;
using ASP.NETSkola.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETSkola.Controllers
{
    public class SubjectsController : Controller
    {
        public SubjectService service;

        public SubjectsController(SubjectService subjectService)
        {
            this.service = subjectService;
        }

        public async Task<IActionResult> SubjectsAsync()
        {
            var allSubjects = await service.GetAllAsync();
            return View(allSubjects);
        }
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult CreateSubject()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSubjectAsync(Subject subject)
        {
            await service.CreateSubjectAsync(subject);
            return RedirectToAction("Subjects");
        }
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> EditSubject(int id)
        {
            var subjectToEdit = await service.GetById(id);
            if (subjectToEdit == null)
            {
                return View("NotFound");
            }
            return View(subjectToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> EditSubjectAsync(int id, [Bind("Id, Name")] Subject subject)
        {
            await service.UpdateAsync(subject);
            return RedirectToAction("Subjects");
        }
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> DeleteSubjectAsync(int id)
        {
            var subjectToDelete = await service.GetById(id);
            if (subjectToDelete == null)
            {
                return View("NotFound");
            }
            await service.DeleteSubjectAsync(subjectToDelete);
            return RedirectToAction("Subjects");
        }
    }
}
