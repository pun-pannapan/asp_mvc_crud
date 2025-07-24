using asp_mvc_crud.Models;
using asp_mvc_crud.Repositories.Interfaces;
using asp_mvc_crud.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asp_mvc_crud.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILibraryRepository<Author> _authorRepository;
        private readonly ILibraryRepository<Category> _categoryRepository;

        public BooksController(
            IBookService bookService,
            ILibraryRepository<Author> authorRepository,
            ILibraryRepository<Category> categoryRepository)
        {
            _bookService = bookService;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllAsync();
            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _bookService.GetByIdAsync(id.Value);
            if (book == null) return NotFound();

            return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,ISBN,AuthorId,CategoryId,PublicationYear,Publisher,TotalCopies")] Book book)
        {
            if (ModelState.IsValid)
            {
                // Check if ISBN already exists
                if (!string.IsNullOrWhiteSpace(book.ISBN) && await _bookService.IsISBNExistsAsync(book.ISBN))
                {
                    ModelState.AddModelError("ISBN", "ISBN already exists");
                }
                else
                {
                    await _bookService.CreateAsync(book);
                    TempData["Success"] = "Book created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }

            await PopulateDropdowns(book.AuthorId, book.CategoryId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _bookService.GetByIdAsync(id.Value);
            if (book == null) return NotFound();

            await PopulateDropdowns(book.AuthorId, book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,ISBN,AuthorId,CategoryId,PublicationYear,Publisher,TotalCopies,AvailableCopies,Status")] Book book)
        {
            if (id != book.BookId) return NotFound();

            if (ModelState.IsValid)
            {
                // Check if ISBN already exists for other books
                if (!string.IsNullOrWhiteSpace(book.ISBN) && await _bookService.IsISBNExistsAsync(book.ISBN, book.BookId))
                {
                    ModelState.AddModelError("ISBN", "ISBN already exists");
                }
                else
                {
                    await _bookService.UpdateAsync(book);
                    TempData["Success"] = "Book updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }

            await PopulateDropdowns(book.AuthorId, book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _bookService.GetByIdAsync(id.Value);
            if (book == null) return NotFound();

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _bookService.DeleteAsync(id);
            if (result)
            {
                TempData["Success"] = "Book deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete book!";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdowns(int? selectedAuthorId = null, int? selectedCategoryId = null)
        {
            var authors = await _authorRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            ViewBag.AuthorId = new SelectList(
                authors.Select(a => new { Value = a.AuthorId, Text = a.FullName }),
                "Value", "Text", selectedAuthorId);

            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName", selectedCategoryId);

            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Available", Text = "Available" },
                new { Value = "Unavailable", Text = "Unavailable" },
                new { Value = "Maintenance", Text = "Maintenance" }
            }, "Value", "Text");
        }
    }
}
