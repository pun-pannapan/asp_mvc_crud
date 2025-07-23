using asp_mvc_crud.Data;
using asp_mvc_crud.Models;
using asp_mvc_crud.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace asp_mvc_crud.Repositories.Implementations
{
    public class MemberRepository : LibraryRepository<Member>, IMemberRepository
    {
        public MemberRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<Member?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Email == email);
        }

        public async Task<IEnumerable<Member>> GetActiveMembers()
        {
            return await _dbSet.Where(m => m.Status == "Active").ToListAsync();
        }
    }
}
