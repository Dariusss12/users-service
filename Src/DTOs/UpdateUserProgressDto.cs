using System.ComponentModel.DataAnnotations;

namespace users_service.Src.DTOs
{
    public class UpdateUserProgressDto
    {
        public List<string> AddSubjects { get; set; } = [];

        public List<string> DeleteSubjects { get; set; } = [];
    }
}