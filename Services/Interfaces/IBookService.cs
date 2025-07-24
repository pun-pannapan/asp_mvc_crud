using asp_mvc_crud.Models;

namespace asp_mvc_crud.Services.Interfaces
{
    public interface IBookService : ILibraryService<Book>
    {
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<bool> IsISBNExistsAsync(string isbn, int? excludeId = null);
    }
}
