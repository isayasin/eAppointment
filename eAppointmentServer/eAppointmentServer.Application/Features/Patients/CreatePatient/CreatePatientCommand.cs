using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Patients.CreatePatient;
public sealed record CreatePatientCommand(
    string FirstName,
    string LastName,
    string City,
    string Town,
    string FullAddress,
    string IdentityNumber
    ) : IRequest<Result<string>>;
