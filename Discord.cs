using System;
using System.Text;
using System.Threading.Tasks;
using DiscordMessenger;
using JoinCodeSender;
using static JoinCodeSender.Plugin;

namespace DiscordWebhook;

public static class Discord
{
    internal static void SendMessage(DiscordWebhookData data)
    {
        var url = urlConfig.Value;
        if (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url))
        {
            DebugError("url is EMPTY", false);
            return;
        }

        data.username = Localization.instance != null ? Localization.instance.Localize(data.username) : data.username;
        data.content = Localization.instance != null ? Localization.instance.Localize(data.content) : data.content;

        new DiscordMessage()
            .SetUsername(data.username)
            .SetContent(data.content)
            .SetAvatar(
                "https://gcdn.thunderstore.io/live/repository/icons/Frogger-JoinCodeSender-0.0.17.png.128x128_q95.png")
            //.AddEmbed()
            //.SetTimestamp(DateTime.Now).Build()
            .SendMessageAsync(url);
    }

    // internal static void SendWebhook()
    // {
    //     if (ZRoutedRpc.instance == null) return;
    //     ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, nameof(SendWebhook), url);
    //     Debug($"Sending webhook url from server {url}");
    // }
    //
    // internal static void RPC_SendWebhook(long _, string url)
    // {
    //     Plugin.url = url;
    //     Debug("Got url: https://discord.com/api/webhooks/" +
    //           Helper.RandomString(Plugin.url.Length - 33)); //Debug fake url, hehe boy
    // }
    //
    // internal static void RecuestWebhook()
    // {
    //     if (ZRoutedRpc.instance == null) return;
    //     ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, nameof(RecuestWebhook));
    //     Debug($"Recuested webhook url from server");
    // }
    //
    // internal static void RPC_RecuestWebhook(long _)
    // {
    //     if (ZNet.m_isServer) _self.Config.Reload();
    // }
}

public class DiscordWebhookData
{
    public string username;
    public string content;

    public DiscordWebhookData(string username, string content)
    {
        this.username = username;
        this.content = content;
    }
}