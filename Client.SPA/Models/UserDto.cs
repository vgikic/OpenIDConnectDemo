using System;

namespace Client.SPA.DTO
{
    public class UserDto
    {
        public string Token { get; set; }
        public long TokenExpirationTime { get; set; }
    }
}
