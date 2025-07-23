using asp_mvc_crud.Models;

namespace asp_mvc_crud.Repositories.Interfaces
{
    public interface IMemberRepository : ILibraryRepository<Member>
    {
        Task<Member?> GetByEmailAsync(string email);
        Task<IEnumerable<Member>> GetActiveMembers();
    }
}
