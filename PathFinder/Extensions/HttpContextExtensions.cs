using System.Security.Claims;

public static class HttpContextExtensions
{
    public static long GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        return long.TryParse(userIdClaim?.Value, out var id) ? id : throw new UnauthorizedAccessException();
    }
}
