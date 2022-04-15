using System;
using System.Diagnostics;

using Newtonsoft.Json;

namespace xFordPassLite.net.Models
{
    public class TokenResponseModel
    {
        [JsonIgnore]
        private int expiresIn;

        public TokenResponseModel()
        {
            ExpireDateTime = DateTime.Now.AddHours(-1);
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("grant_id")]
        public string GrantId { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn
        {
            get
            {
                _counter++;
                return expiresIn;
            }
            set
            {
                expiresIn = value;

                DateTime now = DateTime.Now;
                ExpireDateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond, now.Kind).AddSeconds(value);
                _counter = 0;
            }
        }

        [JsonIgnore]
        public DateTime ExpireDateTime { get; set; }

        public bool IsTokenValid()
        {
            _counter++;

            if (_counter > 4)
            {
                Debug.WriteLine(">>>>>>>>>>> _counter=" + _counter.ToString());
                _counter = 0;
                return false;
            }
            DateTime now = DateTime.Now;
            return ExpireDateTime > new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Kind);
        }

        [JsonIgnore]
        private int _counter = 0;

        public override string ToString()
        {
            string s = "AccessToken:" + AccessToken + ", RefreshToken: " + RefreshToken + ", GrantId: " + GrantId + ", ExpiresIn: " + ExpiresIn.ToString() + ", ExpireDateTime: " + ExpireDateTime.ToLocalTime();
            return s;
        }
    }
}
