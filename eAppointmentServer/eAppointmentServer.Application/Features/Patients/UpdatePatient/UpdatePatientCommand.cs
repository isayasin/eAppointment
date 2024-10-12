using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Patients.UpdatePatient;
public sealed record UpdatePatientCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string City,
    string Town,
    string FullAddress,
    string IdentityNumber
    ) : IRequest<Result<string>>;
