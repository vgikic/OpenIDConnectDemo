using System.Collections.Generic;

namespace Client.SPA.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public long TokenRenewalTime { get; set; }
        public long TokenExpirationTime { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string SubscriptionLevel { get; set; }
    }
}
