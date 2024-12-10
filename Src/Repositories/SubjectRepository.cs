using Microsoft.EntityFrameworkCore;
using users_service.Src.Data;
using users_service.Src.Models;
using users_service.Src.Repositories.Interfaces;

namespace users_service.Src.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DataContext _context;

        public SubjectRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subject>> GetAll()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return subjects;
        }

    }
}