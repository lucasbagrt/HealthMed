using System.ComponentModel;

namespace Appointment.Domain.Enums
{
    public enum AppointmentStatus
    {
        [Description("Marcado")]
        Scheduled = 1,

        [Description("Confirmado")]
        Confirmed = 2,

        [Description("Concluído")]
        Completed = 3,

        [Description("Cancelado")]
        Cancelled = 4,

        [Description("Não compareceu")]
        NoShow = 5
    }

}
