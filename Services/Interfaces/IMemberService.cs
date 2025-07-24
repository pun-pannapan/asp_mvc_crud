using asp_mvc_crud.Models;

namespace asp_mvc_crud.Services.Interfaces
{
    public interface IMemberService : ILibraryService<Member>
    {
        Task<Member?> GetByEmailAsync(string email);
        Task<IEnumerable<Member>> GetActiveMembersAsync();
        Task<bool> IsEmailExistsAsync(string email, int? excludeId = null);
    }
}
