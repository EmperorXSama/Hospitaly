using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Users.Application.Abstractions.Data;
using Hospitaly.Modules.Users.Domain.Users;

namespace Hospitaly.Modules.Users.Application.Users.Commands.CompleteOnboarding;

internal sealed class CompleteOnboardingCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CompleteOnboardingCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(CompleteOnboardingCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserAsync(request.UserId, cancellationToken);
        if (user is null)
            return UserErrors.UserNotFound(request.UserId);

        user.CompleteOnboarding();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
