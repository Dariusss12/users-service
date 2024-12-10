using System.ComponentModel.DataAnnotations;

namespace users_service.Src.DTOs
{
    public class UpdateUserProgressDto
    {
        [RegularExpression(@"^[A-Za-z]{3}-\d{3}$", ErrorMessage = "Invalid format. It should be LLL-NNN.")]
        public List<string> AddSubjects { get; set; } = [];

        [RegularExpression(@"^[A-Za-z]{3}-\d{3}$", ErrorMessage = "Invalid format. It should be LLL-NNN.")]
        public List<string> DeleteSubjects { get; set; } = [];
    }
}