using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Doctors.CreateDoctor;

internal sealed class CreateDoctorCommandHandler(IDoctorRepository doctorRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateDoctorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        Doctor doctor = mapper.Map<Doctor>(request);

        /*Doctor doctor = new Doctor()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Department = DepartmentEnum.FromValue(request.Department)
        };*/

        await doctorRepository.AddAsync(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Doctor create is succesfull";


    }
}
