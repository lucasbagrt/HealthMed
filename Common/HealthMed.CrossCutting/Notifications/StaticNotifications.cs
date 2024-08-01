namespace HealthMed.CrossCutting.Notifications;

public static class StaticNotifications
{
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
    public static Notification AppointmentAlreadyExists = new Notification("AppointmentAlreadyExists", "Consulta já agendada para esse horário.");
    public static Notification AppointmentCreated = new Notification("AppointmentCreated", "Consulta criada com sucesso!");
    public static Notification AppointmentConflict = new Notification("AppointmentConflict", "Há um conflito com outra consulta agendada.");
    public static Notification AppointmentUpdated = new Notification("AppointmentUpdated", "Consulta atualizada com sucesso!");
    public static Notification AppointmentNotFound = new Notification("AppointmentNotFound", "Consulta não encontrada!");
    public static Notification AppointmentCancelled = new Notification("AppointmentCancelled", "Consulta cancelada com sucesso!");
    public static Notification InvalidPatient = new Notification("InvalidPatient", "O paciente fornecido não corresponde ao paciente do agendamento ou houve conflito.");
    #endregion
}
