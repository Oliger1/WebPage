using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebSiteDocuments.Data;
using WebSiteDocuments.Enum;
using WebSiteDocuments.Models;

namespace WebSiteDocuments.Controllers
{
    public class ReviewController : Controller
    {
        private readonly WebDocumentDb _context;

        public ReviewController(WebDocumentDb context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview(int Id, string UserShowed, RatingEnum Rating, string Comment)
        {
            var newReview = new ReviewModel()
            {
                Id = Id,
                UserShowed = UserShowed,
                Rating = Rating,
                Comment = Comment,
                Date = DateTimeOffset.UtcNow
            };

            _context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");

        }

    }
}
