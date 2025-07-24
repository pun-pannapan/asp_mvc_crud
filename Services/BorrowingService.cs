using asp_mvc_crud.Models;
using asp_mvc_crud.Repositories.Interfaces;
using asp_mvc_crud.Services.Interfaces;

namespace asp_mvc_crud.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;

        public BorrowingService(IBorrowingRepository borrowingRepository, IBookRepository bookRepository)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Borrowing>> GetAllAsync()
        {
            return await _borrowingRepository.GetAllAsync();
        }

        public async Task<Borrowing?> GetByIdAsync(int id)
        {
            return await _borrowingRepository.GetByIdAsync(id);
        }

        public async Task<Borrowing> CreateAsync(Borrowing entity)
        {
            return await _borrowingRepository.AddAsync(entity);
        }

        public async Task<Borrowing> UpdateAsync(Borrowing entity)
        {
            return await _borrowingRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _borrowingRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Borrowing>> GetOverdueBorrowingsAsync()
        {
            return await _borrowingRepository.GetOverdueBorrowingsAsync();
        }

        public async Task<IEnumerable<Borrowing>> GetBorrowingsByMemberAsync(int memberId)
        {
            return await _borrowingRepository.GetBorrowingsByMemberAsync(memberId);
        }

        public async Task<IEnumerable<Borrowing>> GetActiveBorrowingsAsync()
        {
            return await _borrowingRepository.GetActiveBorrowingsAsync();
        }

        public async Task<bool> BorrowBookAsync(int memberId, int bookId, int days = 14)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null || book.AvailableCopies <= 0) return false;

            var borrowing = new Borrowing
            {
                MemberId = memberId,
                BookId = bookId,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(days),
                Status = "Borrowed"
            };

            await _borrowingRepository.AddAsync(borrowing);

            // Update book availability
            book.AvailableCopies--;
            await _bookRepository.UpdateAsync(book);

            return true;
        }

        public async Task<bool> ReturnBookAsync(int borrowingId)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(borrowingId);
            if (borrowing == null || borrowing.ReturnDate.HasValue) return false;

            borrowing.ReturnDate = DateTime.Now;
            borrowing.Status = "Returned";

            // Calculate fine if overdue
            if (borrowing.IsOverdue)
            {
                borrowing.FineAmount = await CalculateFineAsync(borrowingId);
            }

            await _borrowingRepository.UpdateAsync(borrowing);

            // Update book availability
            var book = await _bookRepository.GetByIdAsync(borrowing.BookId);
            if (book != null)
            {
                book.AvailableCopies++;
                await _bookRepository.UpdateAsync(book);
            }

            return true;
        }

        public async Task<decimal> CalculateFineAsync(int borrowingId)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(borrowingId);
            if (borrowing == null || !borrowing.IsOverdue) return 0;

            decimal finePerDay = 5.0m; // 5 บาทต่อวัน
            return borrowing.DaysOverdue * finePerDay;
        }
    }
}
