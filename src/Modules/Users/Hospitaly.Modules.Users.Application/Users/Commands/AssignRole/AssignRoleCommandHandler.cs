using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Users.Application.Abstractions.Data;
using Hospitaly.Modules.Users.Domain.Users;

namespace Hospitaly.Modules.Users.Application.Users.Commands.AssignRole;

public class AssignRoleCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<AssignRoleCommand>
{
    public async Task<ErrorOr<Success>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        User? user  = await userRepository.GetUserByIdentity(request.IdentityId, cancellationToken);
        if (user is null)
        {
            return Error.NotFound("User not found");
        }
        
        var result = user.AddRole(request.Role!);
        if (result.IsError)
            return result.Errors;

        userRepository.AttachRole(request.Role!);  
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}