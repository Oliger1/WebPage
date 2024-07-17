using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSiteDocuments.Models
{
    public class IndexModel : PageModel
    {
        public string Name { get; set; } 
        public string Profession { get; set; } 
        public string Description { get; set; } 
        public string Email { get; set; } 
        public string Phone { get; set; } 
        public string LinkedIn { get; set; }
        public List<Project> Projects { get; set; }

        public List<string> PhotoPaths { get; set; } = new List<string>();



        public void OnGet()
        {
        }
    }

    public class Photo
    {
        public List<string> PhotoPaths { get; set; } 
    }



    public class Project
    {
        public string? Title { get; set;}
        public string? Description { get; set; }
        public string? TechnologiesUsed { get; set; }
        public string? GitHubLink { get; set; }
        public object PhotoPaths { get; internal set; }
    }

   

}