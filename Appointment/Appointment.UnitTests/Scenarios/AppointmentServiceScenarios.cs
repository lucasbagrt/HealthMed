using Appointment.Domain.Dtos.Appointment;
using Entities = Appointment.Domain.Entities;
using Appointment.Domain.Entities;
using Appointment.Domain.Enums;
using HealthMed.Domain.Dtos;

namespace Appointment.UnitTests.Scenarios;

public static class AppointmentServiceScenarios
{
    public static List<Entities.Appointment> Appointments = new()
    {
        new Entities.Appointment { Id = 1, AvailabilityId = 1, PatientId = 1, Status = AppointmentStatus.Scheduled, IsActive = true, Availability = new Availability { Id = 1, DoctorId = 1, Date = DateTime.Now.AddDays(1), Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0).TimeOfDay } },        
        new Entities.Appointment { Id = 3, AvailabilityId = 3, PatientId = 1, Status = AppointmentStatus.Cancelled, IsActive = false, Availability = new Availability { Id = 3, DoctorId = 1, Date = DateTime.Now.AddDays(-1), Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0).TimeOfDay } }
    };

    public static List<Availability> Availabilities = new()
    {
        new Availability { Id = 1, DoctorId = 1, Date = DateTime.Now.AddDays(1), Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0).TimeOfDay },
        new Availability { Id = 2, DoctorId = 2, Date = DateTime.Now.AddDays(2), Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0).TimeOfDay },
        new Availability { Id = 3, DoctorId = 1, Date = DateTime.Now.AddDays(-1), Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0).TimeOfDay }
    };

    public static List<UserInfoDto> Users = new()
    {
        new UserInfoDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
        new UserInfoDto { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
    };

    public static CreateAppointmentRequestDto ValidCreateAppointmentRequestDto()
    {
        return new()
        {
            AvailabilityId = 2
        };
    }

    public static CreateAppointmentRequestDto InvalidCreateAppointmentRequestDto()
    {
        return new()
        {
            AvailabilityId = 4
        };
    }

    public static UpdateAppointmentRequestDto ValidUpdateAppointmentRequestDto()
    {
        return new()
        {
            Id = 1,
            AvailabilityId = 2
        };
    }

    public static UpdateAppointmentRequestDto InvalidUpdateAppointmentRequestDto()
    {
        return new()
        {
            Id = 1,
            AvailabilityId = 0
        };
    }
}