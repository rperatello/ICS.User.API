namespace ICS.User.Application.DTOs;

public static class ErrorsResponseDTO
{
    public static Object InformError(string messageError)
    {
        return new { Errors = messageError };
    }
}
