using Domain.Enums;

namespace Application.DTOs
{
    public record RegisterRequest
    (
        string Name,
        string Email,
        string Password,
        Role Role
    );
}
