using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Doctors.UpdateDoctorById;

internal sealed class UpdateDoctorCommandHandler(IDoctorRepository doctorRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateDoctorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        Doctor doctor = await doctorRepository.GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);
        if (doctor == null)
        {
            return Result<string>.Failure("Doctor is not found");
        }

        doctor = mapper.Map<Doctor>(request);

        doctorRepository.Update(doctor);
        await unitOfWork.SaveChangesAsync();

        return $"{doctor.FullName} informations are successfully updated.";
    }
}