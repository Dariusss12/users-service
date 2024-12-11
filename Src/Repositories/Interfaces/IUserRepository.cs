using users_service.Src.DTOs;
using users_service.Src.Models;

namespace users_service.Src.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(int id);

        Task CreateUser(User user);

        Task<bool> EditUser(int id, EditUserDto user);

        public Task<List<UserProgress>?> GetProgressByUser(int userId);

        public Task<bool> AddProgress(List<UserProgress> progress); 
        
        public Task<bool> RemoveProgress(List<UserProgress> progress, int userId); 
    }
}