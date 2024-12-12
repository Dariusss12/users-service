using MassTransit;
using Shared.Messages;
using users_service.Src.Services.Interfaces;


namespace users_service.Src.Consumers
{
    public class TokenToBlacklistConsumer : IConsumer<TokenToBlacklistMessage>
    {
        private readonly IBlacklistService _blacklistService;

        public TokenToBlacklistConsumer(IBlacklistService blacklistService)
        {
            _blacklistService = blacklistService;
        }

        public Task Consume(ConsumeContext<TokenToBlacklistMessage> context)
        {
            var Messages = context.Message;
            Console.WriteLine($"Adding token to blacklist: {Messages.Token}");
            _blacklistService.AddToBlacklist(Messages.Token);
            return Task.CompletedTask;
        }
    }
    
}