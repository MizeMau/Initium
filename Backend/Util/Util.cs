using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Util
{
    public static class Util
    {
        public static long? GetUserID(ClaimsPrincipal user)
        {
            if (!(user.Identity?.IsAuthenticated ?? false))
                return null;

            var dbTableBackendUserService = new Database.Table.Backend.User.Service();
            var userIDString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIDString == null)
                return null;

            var userID = long.Parse(userIDString);
            var dbUser = dbTableBackendUserService.GetById(userID)!;
            if (dbUser.Deleted != null)
                return null;

            return userID;
        }
    }
}
