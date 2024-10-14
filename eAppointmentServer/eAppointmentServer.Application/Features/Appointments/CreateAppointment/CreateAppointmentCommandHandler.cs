﻿using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.CreateAppointment;

internal sealed class CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateAppointmentCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        DateTime startDate = Convert.ToDateTime(request.StartDate);
        DateTime endDate = Convert.ToDateTime(request.EndDate);

        bool isAppointmentDateNotAvailable =
            await appointmentRepository
            .AnyAsync(p => p.DoctorId == request.DoctorId &&
            ((p.StartDate < endDate && p.StartDate >= startDate) || //Mevcut randevunun bitişi, diğer randevunun başlangıcıyla çakışıyor
            (p.EndDate > startDate && p.EndDate <= endDate) || //Mevcut randevunun başlangıcı, diğer randevunun bitişiyle çakışıyor
            (p.StartDate >= startDate && p.StartDate <= startDate) ||//Mevcut randevu, diğer randevunun içerisinde kalıyor.
            (p.StartDate <= startDate && p.EndDate >= endDate))//Mevcut randevu, diğer randevuyu kapsıyor.
            , cancellationToken);

        if (isAppointmentDateNotAvailable)
        {
            return Result<string>.Failure("Appointment date is not available");
        }

        Patient patient = new();

        if (request.PatientId == null)
        {
            /*patient.IdentityNumber = request.IdentityNumber;
            patient.FirstName = request.FirstName;
            patient.LastName = request.LastName;
            patient.City = request.City;
            patient.Town = request.Town;
            patient.FullAddress = request.FullAddress;*/
            patient = mapper.Map<Patient>(request);

            await patientRepository.AddAsync(patient, cancellationToken);
        }

        Appointment appointment = new()
        {
            DoctorId = request.DoctorId,
            PatientId = request.PatientId ?? patient.Id,
            StartDate = Convert.ToDateTime(request.StartDate),
            EndDate = Convert.ToDateTime(request.EndDate),
            IsCompleted = false
        };

        await appointmentRepository.AddAsync(appointment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Appointment create is successfull";


    }
}
