using Microsoft.EntityFrameworkCore;
using users_service.Src.Data;
using users_service.Src.DTOs;
using users_service.Src.Models;
using users_service.Src.Repositories.Interfaces;

namespace users_service.Src.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserById(int id)
        {
            var user = await _context.Users.Where(u => u.Id == id).Include(u => u.Career).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> EditUser(int id, EditUserDto user)
        {
            var userToEdit = await _context.Users.FindAsync(id);
            if (userToEdit == null)
            {
                return false;
            }

            userToEdit.Name = user.Name ?? userToEdit.Name;
            userToEdit.FirstLastName = user.FirstLastName ?? userToEdit.FirstLastName;
            userToEdit.SecondLastName = user.SecondLastName ?? userToEdit.SecondLastName;

            _context.Users.Update(userToEdit);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<UserProgress>?> GetProgressByUser(int userId)
        {
            var userProgress = await _context.UsersProgress
                    .Where(x => x.UserId == userId)
                    .Include(x => x.Subject)
                    .ToListAsync();
            return userProgress;
        }

        public async Task<bool> AddProgress(List<UserProgress> progress)
        {
            await _context.UsersProgress.AddRangeAsync(progress);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveProgress(List<UserProgress> progress, int userId)
        {
            var subjectIdsToRemove = progress.Select(p => p.SubjectId).ToList();

            var result = await _context.UsersProgress
                .Where(u => u.UserId == userId && subjectIdsToRemove
                .Contains(u.SubjectId))
                .ExecuteDeleteAsync() > 0;

            return result;
        }
    }
}