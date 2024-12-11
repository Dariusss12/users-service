using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using users_service.Src.Repositories.Interfaces;

namespace users_service.Src.Helpers
{
    public class BlacklistInterceptor : Interceptor
    {
        private readonly IBlacklistService _blacklistService;

        public BlacklistInterceptor(IBlacklistService blacklistService)
        {
            _blacklistService = blacklistService;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var authHeader = context.RequestHeaders.FirstOrDefault(h => h.Key == "authorization")?.Value;

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Missing or invalid Authorization header"));
            }

            var token = authHeader.Replace("Bearer ", string.Empty);

            if (_blacklistService.IsBlacklisted(token))
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, "Token is blacklisted"));
            }

            return await continuation(request, context);
        }
    }
}