using asp_mvc_crud.Models;
using asp_mvc_crud.Repositories.Interfaces;
using asp_mvc_crud.Services.Interfaces;

namespace asp_mvc_crud.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        public async Task<Member> CreateAsync(Member entity)
        {
            entity.JoinDate = DateTime.Now;
            entity.Status = "Active";
            return await _memberRepository.AddAsync(entity);
        }

        public async Task<Member> UpdateAsync(Member entity)
        {
            return await _memberRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _memberRepository.DeleteAsync(id);
        }

        public async Task<Member?> GetByEmailAsync(string email)
        {
            return await _memberRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<Member>> GetActiveMembersAsync()
        {
            return await _memberRepository.GetActiveMembers();
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeId = null)
        {
            var member = await _memberRepository.GetByEmailAsync(email);
            return member != null && (excludeId != null && member.MemberId == excludeId);
        }
    }
}
