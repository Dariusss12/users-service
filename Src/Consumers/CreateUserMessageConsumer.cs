using MassTransit;
using Shared.Messages;
using users_service.Src.Services.Interfaces;

namespace users_service.Src.Consumers
{
    public class CreateUserMessageConsumer : IConsumer<CreateUserMessage>
    {
        private readonly IUserService _userService;

        public CreateUserMessageConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<CreateUserMessage> context)
        {
            var Messages = context.Message;
            await _userService.CreateUser(Messages.User);
        }
    }
}