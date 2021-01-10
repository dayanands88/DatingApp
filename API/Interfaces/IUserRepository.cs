using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);
         Task<bool> SaveAllAsync();
         Task <IEnumerable<AppUser>> GetUserAsync();

         Task<AppUser> GetUserByIdAsync(int id);
         Task<AppUser> GetUserByUserName(string UserName);

         Task<AppUser> GetUserByUserByIdAsync(int Id);

         Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

         Task<MemberDto> GetMemberAsync(string UserName);


    }
}