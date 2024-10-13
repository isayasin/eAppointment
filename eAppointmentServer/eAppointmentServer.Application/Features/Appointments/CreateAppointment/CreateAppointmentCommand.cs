using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.CreateAppointment;
public sealed record CreateAppointmentCommand(
    string StartDate,
    string EndDate,
    Guid DoctorId,
    Guid? PatientId,
    string IdentityNumber,
    string FirstName,
    string LastName,
    string City,
    string Town,
    string FullAddress
    ) : IRequest<Result<string>>;
