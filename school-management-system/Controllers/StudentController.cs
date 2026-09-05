using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using school_management_system.Services.Interfaces;
using System.Security.Claims;

namespace school_management_system.Controllers
{
    public class StudentController : Controller
    {
        private readonly IProfileService _profileService;

        public StudentController (IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: StudentController
        public IActionResult Index()
        {
            return View();
        }

        // GET: StudentController/Details/5
        public async Task<IActionResult> Profile()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var profile = await _profileService.GetStudentProfileAsync(userId);
            if(profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
