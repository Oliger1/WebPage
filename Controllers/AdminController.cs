using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebSiteDocuments.Data;
using WebSiteDocuments.Models;

namespace WebSiteDocuments.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly WebDocumentDb _context;


        public AdminController(WebDocumentDb context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

     
        public IActionResult ManageResumes()
        {
            List<ResumeModel> resumes = _context.Resumes.ToList();
            return View(resumes);
        }


        [HttpGet]
        public IActionResult EditResume(int id)
        {
            ResumeModel resume = _context.Resumes.Find(id);
            if (resume == null)
            {
                return NotFound();
            }
            return View(resume);
        }

        [HttpPost]
        public IActionResult EditResume(ResumeModel resume)
        {
            if (ModelState.IsValid)
            {
                _context.Update(resume);
                _context.SaveChanges();
                return RedirectToAction(nameof(ManageResumes));
            }
            return View(resume);
        }

        [HttpPost]
        public IActionResult DeleteResume(int id)
        {
            ResumeModel resume = _context.Resumes.Find(id);
            if (resume == null)
            {
                return NotFound();
            }

            _context.Resumes.Remove(resume);
            _context.SaveChanges();

            // Drejto përdoruesin në faqen për njoftimet
            return RedirectToAction("Notification", "Admin");
        }

    }
}
