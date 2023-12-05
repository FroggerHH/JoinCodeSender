using DiscordWebhook;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static JoinCodeSender.Plugin;

namespace JoinCodeSender;

[HarmonyPatch]
internal class Patch
{
    [HarmonyPatch(typeof(ZPlayFabMatchmaking), nameof(ZPlayFabMatchmaking.GenerateJoinCode))] [HarmonyPostfix]
    [HarmonyWrapSafe]
    private static void Send()
    {
        var code = ZPlayFabMatchmaking.JoinCode;
        string message;
        var random = Random.Range(0, _self.messagesList.Count);
        if (random <= _self.messagesList.Count) message = _self.messagesList[random];
        else message = _self.messagesList[random - 1];
        message = message.Replace("#JoinCode", code);

        Discord.SendMessage(message);
    }
}