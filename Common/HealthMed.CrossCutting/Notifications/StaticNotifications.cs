namespace HealthMed.CrossCutting.Notifications;

public static class StaticNotifications
{
	#region [Availability]
	public static Notification AvailabilityCreated = new("AvailabilityCreated", "Horário criado com sucesso!");
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
}
