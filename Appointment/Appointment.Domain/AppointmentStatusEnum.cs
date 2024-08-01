using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Domain
{
    public enum AppointmentStatusEnum
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
