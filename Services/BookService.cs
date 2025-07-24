using asp_mvc_crud.Models;
using asp_mvc_crud.Repositories.Interfaces;
using asp_mvc_crud.Services.Interfaces;

namespace asp_mvc_crud.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<Book> CreateAsync(Book entity)
        {
            entity.Status = "Available";
            entity.AvailableCopies = entity.TotalCopies;
            return await _bookRepository.AddAsync(entity);
        }

        public async Task<Book> UpdateAsync(Book entity)
        {
            return await _bookRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _bookRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _bookRepository.GetAvailableBooksAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId)
        {
            return await _bookRepository.GetBooksByAuthorAsync(authorId);
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _bookRepository.GetBooksByCategoryAsync(categoryId);
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _bookRepository.GetByISBNAsync(isbn);
        }

        public async Task<bool> IsISBNExistsAsync(string isbn, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(isbn)) return false;

            var book = await _bookRepository.GetByISBNAsync(isbn);
            return book != null && (excludeId == null || book.BookId != excludeId);
        }
    }
}
