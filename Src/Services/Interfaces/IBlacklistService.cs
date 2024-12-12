namespace users_service.Src.Services.Interfaces
{
    public interface IBlacklistService
    {
        void AddToBlacklist(string token);
        bool IsBlacklisted(string token);
    }
}