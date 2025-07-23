using asp_mvc_crud.Models;

namespace asp_mvc_crud.Repositories.Interfaces
{
    public interface IBookRepository : ILibraryRepository<Book>
    {
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
        Task<Book?> GetByISBNAsync(string isbn);
    }
}
