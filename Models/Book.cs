// Models/Book.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookManagement.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        public string? CoverImageUrl { get; set; } 

    }
}
