using users_service.Src.DTOs;
using users_service.Src.Models;

namespace users_service.Src.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetById(int id);
        public Task<bool> EditUser(int id, EditUserDto user);
        public Task<List<UserProgressDto>> GetProgressByUser(int userId);
        public Task SetUserProgress(UpdateUserProgressDto subjects, int userId);
        
    }
}