namespace users_service.Src.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string FirstLastName { get; set; } = string.Empty;

        public string SecondLastName { get; set; } = string.Empty;

        public string Rut { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int CareerId { get; set; }

        public Career Career { get; set; } = null!;
        
    }
}