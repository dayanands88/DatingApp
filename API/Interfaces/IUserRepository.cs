using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);
         Task<bool> SaveAllAsync();
         Task <IEnumerable<AppUser>> GetUserAsync();

         Task<AppUser> GetUserByIdAsync(int id);
         Task<AppUser> GetUserByUserName(string UserName);

         Task<IEnumerable<MemberDto>> GetMembersAsync();

         Task<MemberDto> GetMemberAsync(string UserName);


    }
}