using asp_mvc_crud.Data;
using asp_mvc_crud.Models;
using asp_mvc_crud.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace asp_mvc_crud.Repositories.Implementations
{
    public class BorrowingRepository : LibraryRepository<Borrowing>, IBorrowingRepository
    {
        public BorrowingRepository(LibraryDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Borrowing>> GetAllAsync()
        {
            return await _dbSet
                .Include(b => b.Member)
                .Include(b => b.Book)
                .ThenInclude(book => book.Author)
                .ToListAsync();
        }

        public override async Task<Borrowing?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(b => b.Member)
                .Include(b => b.Book)
                .ThenInclude(book => book.Author)
                .FirstOrDefaultAsync(b => b.BorrowingId == id);
        }

        public async Task<IEnumerable<Borrowing>> GetOverdueBorrowingsAsync()
        {
            return await _dbSet
                .Include(b => b.Member)
                .Include(b => b.Book)
                .Where(b => b.ReturnDate == null && b.DueDate < DateTime.Now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Borrowing>> GetBorrowingsByMemberAsync(int memberId)
        {
            return await _dbSet
                .Include(b => b.Book)
                .ThenInclude(book => book.Author)
                .Where(b => b.MemberId == memberId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Borrowing>> GetActiveBorrowingsAsync()
        {
            return await _dbSet
                .Include(b => b.Member)
                .Include(b => b.Book)
                .Where(b => b.Status == "Borrowed")
                .ToListAsync();
        }
    }
}
