// using System.Threading;
// using System.Threading.Tasks;
// using ProjectIndustries.Dashboards.App.Model.Discord;
// using ProjectIndustries.Dashboards.Core.Products;
//
// namespace ProjectIndustries.Dashboards.App.Services.Discord
// {
//   public interface IDiscordClient
//   {
//     ValueTask<DiscordSecurityToken?> AuthenticateAsync(DiscordOAuthConfig config, string code,
//       CancellationToken ct = default);
//
//     ValueTask<DiscordSecurityToken?> ReauthenticateAsync(DiscordOAuthConfig config, string refreshToken,
//       CancellationToken ct = default);
//
//     ValueTask<DiscordUser?> GetProfileAsync(string accessToken, CancellationToken ct = default);
//
//     ValueTask<bool> JoinGuildAsync(DiscordConfig config, string accessToken, DiscordUser user,
//       CancellationToken ct = default);
//
//     ValueTask<DiscordUser?> GetProfileByIdAsync(ulong discordId, string accessToken, CancellationToken ct = default);
//
//     ValueTask<GuildMember?> GetGuildMemberAsync(DiscordConfig config, ulong discordUserId,
//       CancellationToken ct = default);
//
//     ValueTask<DiscordRole[]> GetGuildRolesAsync(DiscordConfig config, CancellationToken ct = default);
//   }
// }