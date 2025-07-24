using asp_mvc_crud.Models;
using asp_mvc_crud.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asp_mvc_crud.Controllers
{
    public class BorrowingsController : Controller
    {
        private readonly IBorrowingService _borrowingService;
        private readonly IMemberService _memberService;
        private readonly IBookService _bookService;

        public BorrowingsController(
            IBorrowingService borrowingService,
            IMemberService memberService,
            IBookService bookService)
        {
            _borrowingService = borrowingService;
            _memberService = memberService;
            _bookService = bookService;
        }

        // GET: Borrowings
        public async Task<IActionResult> Index()
        {
            var borrowings = await _borrowingService.GetAllAsync();
            return View(borrowings);
        }

        // GET: Borrowings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var borrowing = await _borrowingService.GetByIdAsync(id.Value);
            if (borrowing == null) return NotFound();

            return View(borrowing);
        }

        // GET: Borrowings/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: Borrowings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,BookId,DueDate")] Borrowing borrowing)
        {
            if (ModelState.IsValid)
            {
                borrowing.BorrowDate = DateTime.Now;
                borrowing.Status = "Borrowed";

                var days = (borrowing.DueDate - borrowing.BorrowDate).Days;
                var result = await _borrowingService.BorrowBookAsync(borrowing.MemberId, borrowing.BookId, days);

                if (result)
                {
                    TempData["Success"] = "Book borrowed successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Book is not available for borrowing");
                }
            }

            await PopulateDropdowns(borrowing.MemberId, borrowing.BookId);
            return View(borrowing);
        }

        // GET: Borrowings/Return/5
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null) return NotFound();

            var borrowing = await _borrowingService.GetByIdAsync(id.Value);
            if (borrowing == null) return NotFound();

            if (borrowing.ReturnDate.HasValue)
            {
                TempData["Error"] = "Book has already been returned!";
                return RedirectToAction(nameof(Index));
            }

            // Calculate potential fine
            ViewBag.PotentialFine = await _borrowingService.CalculateFineAsync(id.Value);

            return View(borrowing);
        }

        // POST: Borrowings/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnConfirmed(int id)
        {
            var result = await _borrowingService.ReturnBookAsync(id);
            if (result)
            {
                TempData["Success"] = "Book returned successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to return book!";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Borrowings/Overdue
        public async Task<IActionResult> Overdue()
        {
            var overdueBorrowings = await _borrowingService.GetOverdueBorrowingsAsync();
            return View(overdueBorrowings);
        }

        private async Task PopulateDropdowns(int? selectedMemberId = null, int? selectedBookId = null)
        {
            var activeMembers = await _memberService.GetActiveMembersAsync();
            var availableBooks = await _bookService.GetAvailableBooksAsync();

            ViewBag.MemberId = new SelectList(
                activeMembers.Select(m => new { Value = m.MemberId, Text = m.FullName }),
                "Value", "Text", selectedMemberId);

            ViewBag.BookId = new SelectList(
                availableBooks.Select(b => new { Value = b.BookId, Text = $"{b.Title} - {b.Author.FullName}" }),
                "Value", "Text", selectedBookId);
        }
    }
}
