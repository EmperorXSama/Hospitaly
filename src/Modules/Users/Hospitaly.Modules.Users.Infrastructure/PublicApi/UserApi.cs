using ErrorOr;
using Hospitaly.Modules.Users.Application.Users.Commands.AssignRole;
using Hospitaly.Modules.Users.Application.Users.Queries.GetUserInfo;
using Hospitaly.Modules.Users.Application.Users.Queries.SearchUsersByEmail;
using Hospitaly.Modules.Users.Domain.Users;
using MediatR;
using PublicApi;

namespace Hospitaly.Modules.Users.Infrastructure.PublicApi;

public class UserApi(ISender sender) : IUserApi
{
    public async Task<UserResponseDto?> GetUserDataByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetUserInfoQuery(identityId), cancellationToken);
        if (result.IsError)
        {
            return null;
        }
        return new UserResponseDto()
        {
            UserId = result.Value.UserId,
            IdentityId = result.Value.IdentityId,
            FirstName = result.Value.FirstName,
            LastName = result.Value.LastName,
            Email = result.Value.Email,
            Sex = result.Value.Sex,
            DateOfBirth = result.Value.DateOfBirth,
            CreatedOnUtc = result.Value.CreatedOnUtc
        };
    }

    public async Task<List<UserResponseDto>> SearchUsersByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new SearchUsersByEmailQuery(email), cancellationToken);
        if (result.IsError)
        {
            return [];
        }
        return result.Value.Select(u => new UserResponseDto()
        {
            UserId = u.UserId,
            IdentityId = u.IdentityId,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Sex = u.Sex,
            DateOfBirth = u.DateOfBirth,
            CreatedOnUtc = u.CreatedOnUtc
        }).ToList();
    }

    public async Task AddClinicOwnerRole(string identityId, CancellationToken cancellationToken = default)
    {
        var user = await sender.Send(new AssignRoleCommand(identityId, Role.HospitalAdministrator), cancellationToken);
        if (user.IsError)
        {
            // todo : log error or throw 
        }
    }

    public async  Task AddDoctorRole(string identityId, CancellationToken cancellationToken = default)
    {
        var user = await sender.Send(new AssignRoleCommand(identityId, Role.Doctor), cancellationToken);
        if (user.IsError)
        {
            // todo : log error or throw 
        }
    }
}