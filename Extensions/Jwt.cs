using Core.Auth.Tokens;

namespace CandyOrg.Controllers.Extensions;

public static class Jwt
{
    public static TokenPayload GetPayload(this string jwt)
    {
        var tokenClaims = TokenParser.GetClaims(jwt);
        return tokenClaims;
    }
}