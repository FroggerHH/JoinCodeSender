﻿using DiscordMessenger;
using static JoinCodeSender.Plugin;

namespace DiscordWebhook;

public static class Discord
{
    internal static void SendMessage(string message)
    {
        var url = urlConfig.Value;
        if (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url))
        {
            DebugError("url is EMPTY", false);
            return;
        }

        message = Localization.instance != null ? Localization.instance.Localize(message) : message;

        new DiscordMessage()
            .SetUsername("JoinCodeSender")
            .SetContent(message)
            .SetAvatar(
                "https://gcdn.thunderstore.io/live/repository/icons/Frogger-JoinCodeSender-1.1.0.png.128x128_q95.png")
            .SendMessageAsync(url);
    }
}