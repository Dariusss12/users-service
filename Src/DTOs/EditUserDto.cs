using System.ComponentModel.DataAnnotations;

namespace users_service.Src.DTOs
{
    public class EditUserDto
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; } = null;

        [StringLength(30, MinimumLength = 3)]
        public string? FirstLastName { get; set; } = null;

        [StringLength(30, MinimumLength = 3)]
        public string? SecondLastName { get; set; } = null;

    }
}