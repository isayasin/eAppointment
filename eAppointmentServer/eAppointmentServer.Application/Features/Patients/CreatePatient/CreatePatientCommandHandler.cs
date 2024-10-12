using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Patients.CreatePatient;
internal sealed class CreatePatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreatePatientCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        Patient? patient = await patientRepository.GetByExpressionAsync(p => p.IdentityNumber == request.IdentityNumber);

        if (patient == null)
        {
            patient = mapper.Map<Patient>(request);

            await patientRepository.AddAsync(patient);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Patient successfuly created.";
        }

        return Result<string>.Failure("This Identity Number is already use");
    }
}
