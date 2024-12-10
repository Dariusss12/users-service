using users_service.Src.Models;

namespace users_service.Src.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetAll();
    }
}