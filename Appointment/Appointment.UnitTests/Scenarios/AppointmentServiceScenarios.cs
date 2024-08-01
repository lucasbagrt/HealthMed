using Appointment.Domain.Dtos.Appointment;
using System;
using System.Collections.Generic;

namespace Appointment.UnitTests.Scenarios
{
    public static class AppointmentServiceScenarios
    {
        public static (int doctorId, DateTime date) GetScenario1()
        {
            return (1, new DateTime(2024, 8, 1));
        }

        public static List<AvailableSlotDto> ExpectedAvailableSlots()
        {
            return new List<AvailableSlotDto>
            {
                new AvailableSlotDto { Time = new TimeSpan(9, 0, 0) },
                new AvailableSlotDto { Time = new TimeSpan(13, 0, 0) }
            };
        }
    }
}
