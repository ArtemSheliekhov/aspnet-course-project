using SteamKit2.Authentication;

namespace SigmaStats.Helpers
{
    public class CredentialsAuthenticator : IAuthenticator
    {
        public string DeviceCode { get; set; }
        public string EmailCode { get; set; }
        public bool ShouldRememberPassword { get; set; }

        public Task<string> GetDeviceCodeAsync(bool previousCodeWasIncorrect)
        {
            return Task.FromResult(DeviceCode);
        }

        public Task<string> GetEmailCodeAsync(string email, bool previousCodeWasIncorrect)
        {
            return Task.FromResult(EmailCode);
        }

        public Task<bool> AcceptDeviceConfirmationAsync()
        {
            return Task.FromResult(true);
        }
    }
}
