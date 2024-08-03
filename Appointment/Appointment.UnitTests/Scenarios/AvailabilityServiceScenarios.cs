using Appointment.Domain.Dtos.Availability;
using Appointment.Domain.Entities;

namespace Appointment.UnitTests.Scenarios;

public static class AvailabilityServiceScenarios
{
    public static List<Availability> Availabilities = new List<Availability>
    {
        new Availability { Id = 1, DoctorId = 1, Date = DateTime.Now.AddDays(1), Time = TimeSpan.FromHours(10) },
        new Availability { Id = 2, DoctorId = 1, Date = DateTime.Now.AddDays(2), Time = TimeSpan.FromHours(14) },
        new Availability { Id = 3, DoctorId = 2, Date = DateTime.Now.AddDays(3), Time = TimeSpan.FromHours(9) }
    };

    public static List<Domain.Entities.Appointment> Appointments = new ()
    {
        new Domain.Entities.Appointment { Id = 1, AvailabilityId = 1, PatientId = 1 },
        new Domain.Entities.Appointment { Id = 2, AvailabilityId = 2, PatientId = 2 }
    };

    public static CreateAvailabilityDto ValidCreateAvailabilityDto()
    {
        return new()
        {
            Date = DateTime.Now.AddDays(4),
            Time = TimeSpan.FromHours(11)
        };
    }

    public static UpdateAvailabilityDto ValidUpdateAvailabilityDto()
    {
        return new()
        {
            Id = 1,
            Date = DateTime.Now.AddDays(5),
            Time = TimeSpan.FromHours(13)
        };
    }
}
