using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using WebSiteDocuments.Data;
using WebSiteDocuments.Models;
using WebSiteDocuments.Service;

namespace WebSiteDocuments.Controllers
{
    public class HomeController : Controller
    {


        private readonly INotificationService _notificationService;
        private readonly ILogger<HomeController> _logger;
        private static readonly List<ResumeModel> Resumes = new();
        private readonly WebDocumentDb _context;
        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, WebDocumentDb context, IMemoryCache memoryCache, INotificationService notificationService)
        {
            _logger = logger;
            _context = context;
            _cache = memoryCache;
            _notificationService = notificationService;
        }

        public IActionResult Notification()
        {
            var notifications = _notificationService.GetNotifications();
            return View(notifications);
        }


        public IActionResult Index()
        {
            _logger.LogInformation("Index action executed.");
            var model = new IndexModel
            {
                Name = "Oliger Shehi",
                Profession = "Software Developer",
                Description = "I'm Oliger Shehi, a passionate software developer with experience in web development.",
                Email = "oliger@example.com",
                Phone = "+1234567890",
                LinkedIn = "https://www.linkedin.com/in/oliger-shehi",
                Projects = new List<Project>
                {
                    new() { Title = "Portfolio Website", Description = "A personal portfolio website showcasing my skills and projects.", TechnologiesUsed = "HTML, CSS, ASP.NET Core", GitHubLink = "https://github.com/yourusername/portfolio-website" },
                    new() { Title = "E-commerce Website", Description = "Developed an e-commerce website using HTML, CSS, and JavaScript.", TechnologiesUsed = "HTML, CSS, JavaScript, ASP.NET Core", GitHubLink = "https://github.com/yourusername/ecommerce-website" }
                }
            };

            var notification = new Notification
            {
                Message = "Welcome to the application!",
                Date = DateTime.Now,
            };
            _notificationService.AddNotification(notification);
            var notifications = _notificationService.GetNotifications();


            return View(model);


        }

        public IActionResult Review()
        {
            return View();
        }


        public IActionResult Resume(int page = 1)
        {
            int pageSize = 8;
            int totalResumes = _context.Resumes.Count();

            // Përpiquni të merrni rezultatin nga cache
            if (!_cache.TryGetValue("Resumes_" + page, out List<ResumeModel> resumes))
            {
                resumes = _context.Resumes
                    .OrderByDescending(r => r.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                _cache.Set("Resumes_" + page, resumes, TimeSpan.FromSeconds(30));
            }

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalResumes / pageSize);
            ViewBag.CurrentPage = page;

            return View(resumes);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditDeleteCV(int resumeId, string title, string description, string action, string category)
        {
            var resume = _context.Resumes.FirstOrDefault(r => r.Id == resumeId);
            if (resume != null)
            {
                if (action == "edit")
                {
                    resume.Title = title;
                    resume.Description = description;
                    resume.Category = category;
                    _context.SaveChanges();

                }
                else if (action == "delete")
                {
                    string deletedTitle = resume.Title; // Ruaj emrin e CV-së që do të fshihet për njoftim
                    _context.Resumes.Remove(resume);
                    _context.SaveChanges();


                }
            }

            return RedirectToAction("Resume");
        }

        [HttpGet]
        public IActionResult Community()
        {
            var model = _context.Community
                .Where(c => !string.IsNullOrEmpty(c.UserName) && c.Message != null)
                .ToList();

            return View(model);
        }


        private static ResumeModel? LastAddedResume = null;

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UploadDocument(IFormFile file, string description, string title, int Id, string category)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var newResume = new ResumeModel
                {
                    Id = Id,
                    Title = title,
                    Category = category,
                    Description = description,
                    DownloadLink = filePath
                };

                ViewBag.Message = "File-u u ngarkua me sukses.";


                _context.Resumes.Add(newResume);
                _context.SaveChanges();

                return RedirectToAction("Resume");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Download(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/octet-stream", Path.GetFileName(filePath));
            }
            return NotFound();
        }

        public IActionResult ResumeList()
        {
            var model = _context.Resumes.ToList();
            return View(model);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Privacy()
        {
            _logger.LogInformation("This is an information message");
            _logger.LogWarning("This is a warning message");
            _logger.LogError("This is an error message");

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            var model = Resumes;
            return View(model);          
        }
    }
}
