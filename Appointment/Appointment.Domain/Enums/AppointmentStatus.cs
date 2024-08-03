using System.ComponentModel;

namespace Appointment.Domain.Enums
{
    public enum AppointmentStatus
    {
        [Description("Marcado")]
        Scheduled = 1,

        [Description("Cancelado")]
        Cancelled = 2,
    }

}
