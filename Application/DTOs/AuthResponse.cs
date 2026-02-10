using Domain.Enums;

namespace Application.DTOs
{
    public record AuthResponse
    (
        string Token,
        string Email,
        string Name,
        Guid UserId,
        Role Role,
        List<string> Permissions
    );
}
