using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.SetClinicOperatingHours;

public class SetClinicOperatingHoursCommandHandler (
    IClinicRepository repository, 
    IUnitOfWork unitOfWork) :ICommandHandler<SetClinicOperatingHoursCommand, ErrorOr.Success>
{
    public async Task<ErrorOr<Success>> Handle(
        SetClinicOperatingHoursCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await repository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "SetClinicOperatingHours.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var errors = new List<Error>();
        var operatingHours = new List<OperatingHours>();

        foreach (var item in request.OperatingHours)
        {
            var result = OperatingHours.Create(
                item.Day,
                item.IsClosed,
                item.StartTime,
                item.EndTime,
                item.RestingStartsAt,
                item.RestingEndsAt);

            if (result.IsError)
                errors.AddRange(result.Errors);
            else
                operatingHours.Add(result.Value);
        }

        if (errors.Count != 0)
            return errors;

        var updateResult = clinic.UpdateOperatingHours(operatingHours, request.UserId, DateTime.UtcNow);
        if (updateResult.IsError)
            return updateResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}