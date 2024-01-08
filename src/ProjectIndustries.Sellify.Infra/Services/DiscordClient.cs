// using System;
// using System.Collections.Generic;
// using System.Net.Http;
// using System.Net.Http.Headers;
// using System.Text;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;
// using ProjectIndustries.Dashboards.App;
// using ProjectIndustries.Dashboards.App.Model.Discord;
// using ProjectIndustries.Dashboards.App.Services.Discord;
// using ProjectIndustries.Dashboards.Core.Products;
//
// namespace ProjectIndustries.Dashboards.Infra.Services
// {
//   public class DiscordClient : IDiscordClient
//   {
//     private readonly HttpClient _httpClient;
//     private readonly ILogger<DiscordClient> _logger;
//
//     public DiscordClient(IHttpClientFactory clientFactory, ILogger<DiscordClient> logger)
//     {
//       _logger = logger;
//       _httpClient = clientFactory.CreateClient(NamedHttpClients.DiscordClient);
//     }
//
//     public ValueTask<DiscordSecurityToken?> AuthenticateAsync(DiscordOAuthConfig config, string code,
//       CancellationToken ct = default)
//     {
//       return AuthenticateAsync(config, payload =>
//       {
//         payload["grant_type"] = "authorization_code";
//         payload["code"] = code;
//       }, ct);
//     }
//
//     public ValueTask<DiscordSecurityToken?> ReauthenticateAsync(DiscordOAuthConfig config, string refreshToken,
//       CancellationToken ct = default)
//     {
//       return AuthenticateAsync(config, payload =>
//       {
//         payload["grant_type"] = "refresh_token";
//         payload["refresh_token"] = refreshToken;
//       }, ct);
//     }
//
//     public async ValueTask<DiscordUser?> GetProfileAsync(string accessToken, CancellationToken ct = default)
//     {
//       var profileMessage = new HttpRequestMessage(HttpMethod.Get, new Uri("https://discord.com/api/v6/users/@me"));
//       profileMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
//       var response = await _httpClient.SendAsync(profileMessage, ct);
//       var raw = await response.Content.ReadAsStringAsync(ct);
//
//       return JsonConvert.DeserializeObject<DiscordUser>(raw);
//     }
//
//     public async ValueTask<bool> JoinGuildAsync(DiscordConfig config, string accessToken, DiscordUser user,
//       CancellationToken ct = default)
//     {
//       var profileMessage = new HttpRequestMessage(HttpMethod.Put,
//         new Uri($"https://discord.com/api/v6/guilds/{config.GuildId}/members/{user.Id}"));
//       profileMessage.Headers.Authorization = new AuthenticationHeaderValue("Bot", config.BotAccessToken);
//       profileMessage.Content = new StringContent(JsonConvert.SerializeObject(new {access_token = accessToken}),
//         Encoding.UTF8, "application/json");
//       var response = await _httpClient.SendAsync(profileMessage, HttpCompletionOption.ResponseHeadersRead, ct);
//
//       var joined = response.IsSuccessStatusCode;
//       if (!joined)
//       {
//         _logger.LogWarning($"Can't join user {user.Username}#{user.Discriminator} to guild");
//       }
//
//       return joined;
//     }
//
//     public async ValueTask<DiscordUser?> GetProfileByIdAsync(ulong discordId, string accessToken,
//       CancellationToken ct = default)
//     {
//       var profileMessage =
//         new HttpRequestMessage(HttpMethod.Get, new Uri("https://discord.com/api/v6/users/" + discordId));
//       profileMessage.Headers.Authorization = new AuthenticationHeaderValue("Bot", accessToken);
//       var response = await _httpClient.SendAsync(profileMessage, ct);
//       if (!response.IsSuccessStatusCode)
//       {
//         return null;
//       }
//
//       var raw = await response.Content.ReadAsStringAsync(ct);
//
//       return JsonConvert.DeserializeObject<DiscordUser>(raw);
//     }
//
//     public async ValueTask<GuildMember?> GetGuildMemberAsync(DiscordConfig config, ulong discordUserId,
//       CancellationToken ct = default)
//     {
//       var requestMessage = new HttpRequestMessage(HttpMethod.Get,
//         new Uri($"https://discord.com/api/v6/guilds/{config.GuildId}/members/{discordUserId}"));
//       requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bot", config.BotAccessToken);
//       var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, ct);
//
//       if (!response.IsSuccessStatusCode)
//       {
//         return null;
//       }
//
//       return JsonConvert.DeserializeObject<GuildMember?>(await response.Content.ReadAsStringAsync(ct));
//     }
//
//     public async ValueTask<DiscordRole[]> GetGuildRolesAsync(DiscordConfig config, CancellationToken ct = default)
//     {
//       var requestMessage = new HttpRequestMessage(HttpMethod.Get,
//         new Uri($"https://discord.com/api/v6/guilds/{config.GuildId}/roles"));
//       requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bot", config.BotAccessToken);
//       var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, ct);
//
//       if (!response.IsSuccessStatusCode)
//       {
//         return Array.Empty<DiscordRole>();
//       }
//
//       return JsonConvert.DeserializeObject<DiscordRole[]>(await response.Content.ReadAsStringAsync(ct));
//     }
//
//     private async ValueTask<DiscordSecurityToken?> AuthenticateAsync(DiscordOAuthConfig config,
//       Action<Dictionary<string, string?>> requestConfigurer,
//       CancellationToken ct = default)
//     {
//       var authenticateMessage =
//         new HttpRequestMessage(HttpMethod.Post, new Uri("https://discord.com/api/v6/oauth2/token"));
//       var payload = new Dictionary<string, string?>
//       {
//         {"client_id", config.ClientId},
//         {"client_secret", config.ClientSecret},
//         {"redirect_uri", config.RedirectUrl},
//         {"scope", config.Scope}
//       };
//
//       requestConfigurer(payload);
//
//       authenticateMessage.Content = new FormUrlEncodedContent(payload!);
//       var response = await _httpClient.SendAsync(authenticateMessage, ct);
//       if (!response.IsSuccessStatusCode)
//       {
//         return null;
//       }
//
//       var rawContent = await response.Content.ReadAsStringAsync(ct);
//
//       return JsonConvert.DeserializeObject<DiscordSecurityToken>(rawContent);
//     }
//   }
// }