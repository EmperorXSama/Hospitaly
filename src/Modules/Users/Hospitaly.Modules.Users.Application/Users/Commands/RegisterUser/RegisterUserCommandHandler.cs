using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Users.Application.Abstractions.Data;
using Hospitaly.Modules.Users.Application.Abstractions.Identity;
using Hospitaly.Modules.Users.Domain.Users;

namespace Hospitaly.Modules.Users.Application.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IIdentityProviderService identityProviderService,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository
    ) :
    ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<ErrorOr<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var sexResult = Sex.FromName(request.Sex);
        if (sexResult.IsError)
        {
            return sexResult.Errors;
        }

        BloodType? bloodType = null;
        if (request.BloodType is not null)
        {
            var btResult = BloodType.FromName(request.BloodType);
            if (btResult.IsError)
            {
                return btResult.Errors;
            }
            bloodType = btResult.Value;
        }

        var result = await identityProviderService.RegisterUserAsync(
            new UserModel(request.Email, request.Password, request.FirstName, request.LastName),
            cancellationToken);
        if (result.IsError)
        {
            return result.Errors;
        }

        var user = User.Create(result.Value, request.Email, request.FirstName, request.LastName,
            sexResult.Value, request.DateOfBirth, bloodType);
        userRepository.Insert(user.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Value.Id;
    }
}