namespace HealthMed.CrossCutting.Notifications;

public static class StaticNotifications
{
	#region [Availability]
	public static Notification AvailabilityAlreadyExists = new("AvailabilityAlreadyExists", "Horários já cadastrado!");
	public static Notification AvailabilityCreated = new("AvailabilityCreated", "Horário criado com sucesso!");
	public static Notification AvailabilityChanged = new("AvailabilityChanged", "Horário editado com sucesso!");
	public static Notification AvailabilityNotAvailable = new("AvailabilityNotAvailable", "Horario indisponivel!");
	public static Notification AvailabilityNotFound = new("AvailabilityNotFound", "Horario não encontrado!");
	public static Notification AvailabilityDoesNotBelongToDoctor = new("AvailabilityDoesNotBelongToDoctor", "Horario não pertence ao medico logado!");
	public static Notification AvailabilityHasAppointment = new("AvailabilityHasAppointment", "Existem agendamentos para o horario!");
	public static Notification AvailabilityDeleted = new("AvailabilityDeleted", "Horario deletado com sucesso!");
	#endregion

	#region [Users]
	public static Notification InvalidCredentials = new("InvalidCredentials", "Credenciais invalidas!");
    public static Notification UserAlreadyExists = new("UserAlreadyExists", "Usuario já cadastrado!");
    public static Notification UserNotFound = new("InvalidUser", "Usuario não encontrado!");
    public static Notification RevokeToken = new("RevokeToken", "Token revogado com sucesso!");
    public static Notification InvalidToken = new("InvalidToken", "Token invalido!");
    public static Notification UserCreated = new ("UserCreated", "Usuario criado com sucesso!");
    public static Notification UsernameAlreadyExists = new("UsernameAlreadyExists", "Username já está sendo utilizado!");
    public static Notification UserEdited = new("UserEdited", "Usuario editado com sucesso!");
    public static Notification PasswordChanged = new("PasswordChanged", "Senha alterada com sucesso!");   
    public static Notification UserDeleted = new("UserDeleted", "Usuario removido com sucesso!");
    public static Notification UserActivated = new("UserActivated", "Ativação de usuário alterada com sucesso!");
    public static Notification UserUnauthorized = new("UserUnauthorized", "Usuarios administradores podem ser criados apenas por outros administradores!");
    #endregion

    #region [Appointment]
    public static readonly Notification AppointmentAlreadyExists = new ("AppointmentAlreadyExists", "Consulta já agendada para esse horário.");
    public static readonly Notification AppointmentCreated = new ("AppointmentCreated", "Consulta criada com sucesso!");
    public static readonly Notification AppointmentConflict = new ("AppointmentConflict", "Há um conflito com outra consulta agendada.");
    public static readonly Notification AppointmentUpdated = new ("AppointmentUpdated", "Consulta atualizada com sucesso!");
    public static readonly Notification AppointmentNotFound = new ("AppointmentNotFound", "Consulta não encontrada!");
    public static readonly Notification AppointmentCancelled = new ("AppointmentCancelled", "Consulta cancelada com sucesso!");
    public static readonly Notification InvalidPatient = new ("InvalidPatient", "O paciente fornecido não corresponde ao paciente do agendamento ou houve conflito.");
    #endregion

}
