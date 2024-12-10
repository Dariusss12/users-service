namespace users_service.Src.Models
{
    public class UserProgress
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
        public int UserId { get; set; }
    }
}