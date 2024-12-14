using Grpc.Core;
using UsersServiceProto;
using users_service.Src.Services.Interfaces;
using users_service.Src.DTOs;
using users_service.Src.Helpers;
using Microsoft.AspNetCore.Authorization;
using Sprache;


public class UserServiceGrpc : UserService.UserServiceBase
{
    private readonly IUserService _userService;

    public UserServiceGrpc(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    public override async Task<GetUserByIdResponse> GetUserById(Empty request, ServerCallContext context)
    {
        try
        {
            var userIdClaim = context.GetHttpContext().User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "User ID not found in token"));
            }

            var userId = int.Parse(userIdClaim);
            var user = await _userService.GetById(userId);

            return new GetUserByIdResponse
            {
                Id = user.Id,
                Name = user.Name,
                FirstLastName = user.FirstLastName,
                SecondLastName = user.SecondLastName,
                Rut = user.Rut,
                Email = user.Email,
                Career = new Career
                {
                    Id = user.Career.Id,
                    Name = user.Career.Name
                }
            };
        }
        catch (NotFoundException notFound)
        {
            throw new RpcException(new Status(StatusCode.NotFound, notFound.Message));
        }

    }

    [Authorize]
    public override async Task<EditUserResponse> EditUser(EditUserRequest request, ServerCallContext context)
    {

        var userIdClaim = context.GetHttpContext().User.FindFirst("Id")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "User ID not found in token"));
        }

        var userId = int.Parse(userIdClaim);

        var errors = ValidateEditUserRequest(request);

        if (errors.Count > 0)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(" ", errors)));
        }

        var success = await _userService.EditUser(userId, new EditUserDto
        {
            Name = request.Name,
            FirstLastName = request.FirstLastName,
            SecondLastName = request.SecondLastName
        });

        if(!success)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }

        return new EditUserResponse { Success = success };
    }

    [Authorize]
    public override async Task<GetProgressByUserResponse> GetProgressByUser(Empty request, ServerCallContext context)
    {
        try{
            var userIdClaim = context.GetHttpContext().User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "User ID not found in token"));
            }

            var userId = int.Parse(userIdClaim);
            var progress = await _userService.GetProgressByUser(userId);
            var response = new GetProgressByUserResponse();
            response.Progress.AddRange(progress.Select(p => new UserProgress
            {
                Id = p.Id,
                SubjectCode = p.Subject.Code,
            }));
            return response;
        }
        catch (NotFoundException notFound)
        {
            throw new RpcException(new Status(StatusCode.NotFound, notFound.Message));
        }

    }

    [Authorize]
    public override async Task<SetUserProgressResponse> SetUserProgress(SetUserProgressRequest request, ServerCallContext context)
    {
        try{
            var userIdClaim = context.GetHttpContext().User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "User ID not found in token"));
            }

            var userId = int.Parse(userIdClaim);
            await _userService.SetUserProgress(new UpdateUserProgressDto
            {
                AddSubjects = [.. request.AddSubjects],
                DeleteSubjects = [.. request.DeleteSubjects]
            }, userId);

            return new SetUserProgressResponse { Success = true };

        }
        catch (BadRequestException badRequest)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, badRequest.Message));
        }
        catch (NotFoundException notFound)
        {
            throw new RpcException(new Status(StatusCode.NotFound, notFound.Message));
        }

        
    }

    private static List<string> ValidateEditUserRequest(EditUserRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length < 3 || request.Name.Length > 50)
        {
            errors.Add("Name is required and must be between 3 and 50 characters.");
        }

        if (string.IsNullOrWhiteSpace(request.FirstLastName) || request.FirstLastName.Length < 3 || request.FirstLastName.Length > 30)
        {
            errors.Add("FirstLastName is required and must be between 3 and 30 characters.");
        }

        if (string.IsNullOrWhiteSpace(request.SecondLastName) || request.SecondLastName.Length < 3 || request.SecondLastName.Length > 30)
        {
            errors.Add("SecondLastName is required and must be between 3 and 30 characters.");
        }

        return errors;
    }
}