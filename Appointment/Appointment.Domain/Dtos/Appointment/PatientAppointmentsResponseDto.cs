using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Domain.Dtos.Appointment
{
    public class PatientAppointmentsResponseDto
    {
        public int PatientId { get; set; }
        public List<AppointmentDto> Appointments { get; set; }
    }
}
