using users_service.Src.DTOs;
using users_service.Src.Models;
using users_service.Src.Repositories.Interfaces;
using users_service.Src.Services.Interfaces;

namespace users_service.Src.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubjectRepository _subjectRepository;

        public UserService(IUserRepository userRepository, ISubjectRepository subjectRepository)
        {
            _userRepository = userRepository;
            _subjectRepository = subjectRepository;
        }
        public async Task<UserDto> GetById(int id)
        {
            var user = await _userRepository.GetUserById(id) ?? throw new Exception("User not found");
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                FirstLastName = user.FirstLastName,
                SecondLastName = user.SecondLastName,
                Rut = user.Rut,
                Email = user.Email,
                Career = new CareerDto
                {
                    Id = user.Career.Id,
                    Name = user.Career.Name
                }
            };
        }

        public async Task<bool> EditUser(int id, EditUserDto user)
        {
            return await _userRepository.EditUser(id, user);
        }

        public async Task<List<UserProgressDto>> GetProgressByUser(int userId)
        {
            var userProgress = await _userRepository.GetProgressByUser(userId) ?? throw new Exception("User progress not found");
            return userProgress.Select(progress => new UserProgressDto
            {
                Id = progress.Id,
                SubjectCode = progress.Subject.Code,
            }).ToList();
        }

        public async Task SetUserProgress(UpdateUserProgressDto subjects, int userId)
        {
            var validSubjects = await _subjectRepository.GetAll();
            var subjectsToAdd = subjects.AddSubjects.Select(code =>
            {
                var subject = validSubjects.FirstOrDefault(s => s.Code == code) ?? throw new Exception($"Subject with code {code} not found");
                return subject.Id;
            }).ToList();

            var subjectsToDelete = subjects.DeleteSubjects.Select(code =>
            {
                var subject = validSubjects.FirstOrDefault(s => s.Code == code) ?? throw new Exception($"Subject with code {code} not found");
                return subject.Id;
            }).ToList();

            var userProgress = await _userRepository.GetProgressByUser(userId);

            var progressToAdd = subjectsToAdd.Select(subjectId =>
            {
                var foundUserProgress = userProgress?.FirstOrDefault(up => up.Subject.Id == subjectId);

                if (foundUserProgress is not null)
                    throw new Exception($"Subject with Code: {foundUserProgress.Subject.Code} already exists");

                return new UserProgress()
                {
                    SubjectId = subjectId,
                    UserId = userId,
                };
            }).ToList();

            var progressToRemove = subjectsToDelete.Select(subjectId =>
            {
                if (userProgress?.FirstOrDefault(up => up.SubjectId == subjectId) is null)
                    throw new Exception($"Subject with ID: {subjectId} not found");

                return new UserProgress()
                {
                    SubjectId = subjectId,
                    UserId = userId,
                };
            }).ToList();

            var addResult = await _userRepository.AddProgress(progressToAdd);
            var removeResult = await _userRepository.RemoveProgress(progressToRemove, userId);
            if (!removeResult && !addResult)
                throw new Exception("Cannot update user progress");
            
        }


    }
}