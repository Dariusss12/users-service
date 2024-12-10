using Grpc.Core;
using UsersServiceProto;
using users_service.Src.Services.Interfaces;
using users_service.Src.DTOs;


public class UserServiceGrpc : UserService.UserServiceBase
{
    private readonly IUserService _userService;

    public UserServiceGrpc(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        var user = await _userService.GetById(request.Id);
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

    public override async Task<EditUserResponse> EditUser(EditUserRequest request, ServerCallContext context)
    {
        var success = await _userService.EditUser(request.Id, new EditUserDto
        {
            Name = request.Name,
            FirstLastName = request.FirstLastName,
            SecondLastName = request.SecondLastName
        });
        return new EditUserResponse { Success = success };
    }

    public override async Task<GetProgressByUserResponse> GetProgressByUser(GetProgressByUserRequest request, ServerCallContext context)
    {
        var progress = await _userService.GetProgressByUser(request.UserId);
        var response = new GetProgressByUserResponse();
        response.Progress.AddRange(progress.Select(p => new UserProgress
        {
            Id = p.Id,
            SubjectCode = p.SubjectCode
        }));
        return response;
    }

    public override async Task<SetUserProgressResponse> SetUserProgress(SetUserProgressRequest request, ServerCallContext context)
    {
        await _userService.SetUserProgress(new UpdateUserProgressDto
        {
            AddSubjects = request.AddSubjects.ToList(),
            DeleteSubjects = request.DeleteSubjects.ToList()
        }, request.UserId);

        return new SetUserProgressResponse { Success = true };
    }
}