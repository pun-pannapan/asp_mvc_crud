using asp_mvc_crud.Models;

namespace asp_mvc_crud.Repositories.Interfaces
{
    public interface IBorrowingRepository : ILibraryRepository<Borrowing>
    {
        Task<IEnumerable<Borrowing>> GetOverdueBorrowingsAsync();
        Task<IEnumerable<Borrowing>> GetBorrowingsByMemberAsync(int memberId);
        Task<IEnumerable<Borrowing>> GetActiveBorrowingsAsync();
    }
}
