using asp_mvc_crud.Models;

namespace asp_mvc_crud.Services.Interfaces
{
    public interface IBorrowingService : ILibraryService<Borrowing>
    {
        Task<IEnumerable<Borrowing>> GetOverdueBorrowingsAsync();
        Task<IEnumerable<Borrowing>> GetBorrowingsByMemberAsync(int memberId);
        Task<IEnumerable<Borrowing>> GetActiveBorrowingsAsync();
        Task<bool> BorrowBookAsync(int memberId, int bookId, int days = 14);
        Task<bool> ReturnBookAsync(int borrowingId);
        Task<decimal> CalculateFineAsync(int borrowingId);
    }
}
