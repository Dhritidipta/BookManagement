using Microsoft.AspNetCore.Mvc.RazorPages;
using BookManagement.Models;
using System.Collections.Generic;
using BookManagement.Data;

namespace BookManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BookContext _context;

        public IndexModel(BookContext context)
        {
            _context = context;
        }
        public List<Book> Book { get; set; }


        public void OnGet()
        {
            // Fetch the list of books from the database
            Book = _context.Books.ToList();  
        }
    }
}
