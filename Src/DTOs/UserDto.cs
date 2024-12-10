namespace users_service.Src.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;

        public string FirstLastName { get; set; } = null!;

        public string SecondLastName { get; set; } = null!;

        public string Rut { get; set; } = null!;

        public string Email { get; set; } = null!;

        public CareerDto Career { get; set; } = null!;
    }
}