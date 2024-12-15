using users_service.Src.DTOs;

namespace Shared.Messages
{
    public class CreateUserMessage
    {
        public CreateUserDto User { get; set; } = null!;
    }
}