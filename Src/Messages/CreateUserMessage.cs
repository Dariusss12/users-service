using users_service.Src.DTOs;

namespace Shared.Messages
{
    public class CreateUserMessage
    {
        public string Sender { get; set; } = string.Empty;

        public CreateUserDto User { get; set; } = null!;
    }
}