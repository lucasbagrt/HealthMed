namespace HealthMed.CrossCutting.QueueMessenge;

public class CreateAppointment
{
    public int DoctorId { get; set; }
    public string DoctorName { get; set; }
    public string DoctorEmail { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public int AvailabilityId { get; set; }
}
