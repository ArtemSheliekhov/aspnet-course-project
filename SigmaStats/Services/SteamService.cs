/*using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SigmaStats.Models;
using SteamKit2;
using SteamKit2.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class SteamService
{
    private readonly SteamClient _steamClient;
    private readonly CallbackManager _callbackManager;
    private readonly SteamUser _steamUser;
    private readonly SteamApps _steamApps;
    private readonly Dictionary<string, bool> _authenticatedSessions = new Dictionary<string, bool>();
    private readonly Dictionary<string, SteamID> _steamIDs = new Dictionary<string, SteamID>();
    private readonly Dictionary<string, string> _accessTokens = new Dictionary<string, string>();

    public SteamService()
    {
        _steamClient = new SteamClient(SteamConfiguration.Create(b =>
        {
            b.WithProtocolTypes(ProtocolTypes.Tcp);
            b.WithCellID(0);
            b.WithConnectionTimeout(TimeSpan.FromSeconds(30));
        }));
        _callbackManager = new CallbackManager(_steamClient);
        _steamUser = _steamClient.GetHandler<SteamUser>();
        _steamApps = _steamClient.GetHandler<SteamApps>();
    }

    /// <summary>
    /// Login to Steam using username and password
    /// </summary>
    /// <param name="username">Steam account username</param>
    /// <param name="password">Steam account password</param>
    /// <param name="twoFactorCode">Two-factor authentication code (optional)</param>
    /// <param name="emailCode">Email authentication code (optional)</param>
    /// <returns>A task that completes when login is successful or fails</returns>
    public async Task<Dictionary<string, object>> LoginWithCredentialsAsync(string username, string password)
    {
        string sessionId = Guid.NewGuid().ToString("N").Substring(0, 6);
        Console.WriteLine($"Starting credential login for session: {sessionId}");

        try
        {
            if (!_steamClient.IsConnected)
            {
                Console.WriteLine("Connecting to Steam...");
                _steamClient.Connect();

                // Wait for connection to establish
                var connectionTimeout = TimeSpan.FromSeconds(30);
                var startTime = DateTime.Now;

                while (!_steamClient.IsConnected && DateTime.Now - startTime < connectionTimeout)
                {
                    await Task.Delay(500);
                }

                if (!_steamClient.IsConnected)
                {
                    Console.WriteLine("Failed to connect to Steam.");
                    return new Dictionary<string, object>
                    {
                        ["success"] = false,
                        ["error"] = "Failed to connect to Steam",
                        ["sessionId"] = sessionId
                    };
                }
            }

            Console.WriteLine($"Connected to Steam, initiating login for user: {username}");

            // Create authentication details and session
            var authSession = await _steamClient.Authentication.BeginAuthSessionViaCredentialsAsync(new AuthSessionDetails
            {
                Username = username,
                Password = password,
                IsPersistentSession = false,
                Authenticator = new UserConsoleAuthenticator()
            });

            // Poll for authentication result
            var pollResult = await authSession.PollingWaitForResultAsync();

            foreach (var prop in pollResult.GetType().GetProperties())
            {
                try
                {
                    var value = prop.GetValue(pollResult);                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving property {prop.Name}: {ex.Message}");
                }
            }

            // Inside the LoginWithCredentialsAsync method, after successful authentication:
            if (pollResult != null)
            {
                Console.WriteLine($"Login successful for user: {username}");

                // Store authentication state
                _authenticatedSessions[sessionId] = true;

                // Access tokens and account name
                var accessToken = pollResult.AccessToken;
                var refreshToken = pollResult.RefreshToken;
                var accountName = pollResult.AccountName;


                if (!string.IsNullOrEmpty(accessToken))
                {
                    _accessTokens[sessionId] = accessToken;
                }

                return new Dictionary<string, object>
                {
                    ["success"] = true,
                    ["sessionId"] = sessionId,
                    ["username"] = accountName,
                    ["accessTokenAvailable"] = !string.IsNullOrEmpty(accessToken),
                    ["refreshTokenAvailable"] = !string.IsNullOrEmpty(refreshToken)
                };
            }
            else
            {
                string errorMessage = "Authentication failed: No result returned.";
                Console.WriteLine(errorMessage);

                return new Dictionary<string, object>
                {
                    ["success"] = false,
                    ["error"] = errorMessage,
                    ["sessionId"] = sessionId,
                    ["requires2FA"] = false,
                    ["requiresEmail"] = false
                };
            }
        }
        catch (Exception ex)
        {
            string errorMessage = $"Authentication failed: {ex.Message}";
            Console.WriteLine(errorMessage);

            return new Dictionary<string, object>
            {
                ["success"] = false,
                ["error"] = errorMessage,
                ["sessionId"] = sessionId,
                ["requires2FA"] = false,
                ["requiresEmail"] = false
            };
        }
    }
   
    /// <summary>
    /// Logout from Steam session
    /// </summary>
    /// <param name="sessionId">Session ID to logout</param>
    /// <returns>Result of logout operation</returns>
    public async Task<Dictionary<string, object>> LogoutAsync(string sessionId)
    {
        if (_authenticatedSessions.TryGetValue(sessionId, out bool authenticated) && authenticated)
        {
            _authenticatedSessions.Remove(sessionId);
            _steamIDs.Remove(sessionId);
            _accessTokens.Remove(sessionId);

            return new Dictionary<string, object>
            {
                ["success"] = true,
                ["message"] = "Successfully logged out",
                ["sessionId"] = sessionId
            };
        }

        return new Dictionary<string, object>
        {
            ["success"] = false,
            ["error"] = "Session not found or not authenticated",
            ["sessionId"] = sessionId
        };
    }
}*/