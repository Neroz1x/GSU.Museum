using System;

namespace GSU.Museum.Web.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTimeOffset ExpirationTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }

            RefreshToken refreshToken = (RefreshToken)obj;
            return Token.Equals(refreshToken.Token);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
