using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookManagement.Models;
using BookManagement.Data;

using System.Threading.Tasks;
using BookManagement.Utilities;

namespace BookManagement.Pages
{
    public class CreateModel : PageModel
    {
        private readonly BookContext _context;
        private readonly S3Service _s3Service;
        private readonly LambdaService _lambdaService;
        // Constructor to inject the DbContext
        public CreateModel(BookContext context)
        {
            _context = context;
            _s3Service = new S3Service();
            _lambdaService = new LambdaService();
        }

        // Property to bind the Book data from the form
        [BindProperty]
        public Book Book { get; set; }

        [BindProperty]
        public IFormFile UploadCover { get; set; }  

        // GET handler
        public IActionResult OnGet()
        {
            return Page();
        }

        // POST handler
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return the page with the error messages
                return Page();
            }

            if(UploadCover != null)
            {
                using (var stream = new MemoryStream())
                {
                    await UploadCover.CopyToAsync(stream);
                    var s3Key = await _s3Service.UploadBookCoverAsync(stream, Book.ISBN, UploadCover.FileName);
                    Book.CoverImageUrl = s3Key;
                }
            }


            // Ensure PublishedDate is in UTC
            if (Book.PublishedDate.Kind == DateTimeKind.Unspecified)
            {
                Book.PublishedDate = DateTime.SpecifyKind(Book.PublishedDate, DateTimeKind.Utc);
            }
            else
            {
                Book.PublishedDate = Book.PublishedDate.ToUniversalTime();
            }

            // Add the new book to the database
            _context.Books.Add(Book);
            await _context.SaveChangesAsync();

            Task.Run(() => _lambdaService.InvokeLambdaFunctionAsync(Book));

            // Redirect to the Index page (or any other page you want to show after the book is created)
            return RedirectToPage("Index");
        }
    }
}
