using DiscordWebhook;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static JoinCodeSender.Plugin;

namespace JoinCodeSender;

[HarmonyPatch]
internal class Patch
{
    private static bool done = false;

    [HarmonyPatch(typeof(JoinCode), nameof(JoinCode.Init))] [HarmonyPostfix] [HarmonyWrapSafe]
    private static void Send()
    {
        if (done) return;
        done = true;
        if (SceneManager.GetActiveScene().name != "main") return;

        var code = JoinCode.m_instance.m_joinCode;
        string message;
        var random = Random.Range(0, _self.messagesList.Count);
        if (random <= _self.messagesList.Count) message = _self.messagesList[random];
        else message = _self.messagesList[random - 1];
        message = message.Replace("#JoinCode", code);

        Discord.SendMessage(message);
    }
}