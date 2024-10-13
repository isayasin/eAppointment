using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.UpdateAppointment;

internal sealed class UpdateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateAppointmentCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        Appointment? appointment = await appointmentRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);
        if (appointment is null)
        {
            return Result<string>.Failure("Appointment is not found");
        }

        if (appointment.StartDate < DateTime.Now)
        {
            return Result<string>.Failure("Appointment can not updated. Because Appointment date is already passed.");
        }

        if (Convert.ToDateTime(request.StartDate) < DateTime.Now)
        {
            return Result<string>.Failure("Appointment can not updated. Because Appointment date is already passed.");
        }

        appointment.StartDate = Convert.ToDateTime(request.StartDate);
        appointment.EndDate = Convert.ToDateTime(request.EndDate);

        appointmentRepository.Update(appointment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Appointment is updated successfully";
    }
}
