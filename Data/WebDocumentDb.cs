using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebSiteDocuments.Enum;
using WebSiteDocuments.Models;
using static WebSiteDocuments.Data.ResumeModel;



namespace WebSiteDocuments.Data
{
    public class WebDocumentDb : IdentityDbContext
    {
        public WebDocumentDb(DbContextOptions<WebDocumentDb> options)
            : base(options)
        {
        }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<ResumeModel> Resumes { get; set; }
        public DbSet<Community> Community { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<User> Users {  get; set; }
        public DbSet<Role> Roles { get; set; }
    }

    public class ReviewModel
    {
        public int Id { get; set; }
        public string UserShowed { get; set; }
        public RatingEnum Rating { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset Date { get; set; }
    }


    public class Community
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string? Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }

    public class ResumeModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DownloadLink { get; set; }
        public string Category { get; set; }
        public string UploadedDocumentsPathsJson { get; set; } = "";
        private List<string> _uploadedDocumentsPaths = new();

        public List<string> UploadedDocumentsPaths
        {
            get => _uploadedDocumentsPaths;
            set => _uploadedDocumentsPaths = value ?? new List<string>();
        }

        public ResumeModel()
        {
            UploadedDocumentsPaths = new List<string>();
        }

        public ResumeModel(List<string> uploadedDocumentsPaths)
        {
            UploadedDocumentsPaths = uploadedDocumentsPaths;
        }

        public class Role : IdentityRole<int>
        {
        }

        public class User : IdentityUser<int>
        {
        }
    }

}