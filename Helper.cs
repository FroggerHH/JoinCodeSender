using System;
using System.Linq;
using DiscordWebhook;
using UnityEngine;
using static JoinCodeSender.Plugin;
using Random = System.Random;

namespace JoinCodeSender;

public static class Helper
{
    public static string GetPlayerName()
    {
        if (Player.m_localPlayer) return Player.m_localPlayer.GetPlayerName();
        return "-none-";
    }

    public static void SimplePatch(string sendKey, bool hold, Player player)
    {
        if (hold) return;
        DiscordWebhookData data = new("Bot", $"");
        Discord.SendMessage(data);
    }

    public static string RandomString(int length)
    {
        try
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            var randomString =
                new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
        catch (Exception e)
        {
            DebugError(e.Message, true);
        }

        return String.Empty;
    }
}