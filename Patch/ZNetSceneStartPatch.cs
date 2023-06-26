using System;
using BepInEx.Bootstrap;
using DiscordWebhook;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static JoinCodeSender.Plugin;
using Random = UnityEngine.Random;

namespace JoinCodeSender
{
    [HarmonyPatch]
    internal class ZNetSceneStartPatch
    {
        [HarmonyPatch(typeof(JoinCode), nameof(JoinCode.Init)), HarmonyPostfix]
        static void JoinCodeAwakePatch()
        {
            if (SceneManager.GetActiveScene().name != "main") return;

            // ZRoutedRpc.instance.Register(nameof(Discord.SendWebhook),
            //     new Action<long, string>(Discord.RPC_SendWebhook));
            // ZRoutedRpc.instance.Register(nameof(Discord.RecuestWebhook),
            //     new Action<long>(Discord.RPC_RecuestWebhook));

            var code = JoinCode.m_instance.m_joinCode;
            string message;
            int random = Random.Range(0, _self.messagesList.Count);
            if (random <= _self.messagesList.Count) message = _self.messagesList[random];
            else message = _self.messagesList[random - 1];
            message = message.Replace("#JoinCode", code);

            Discord.SendMessage("Bot", message);
        }

        // [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer)), HarmonyPostfix]
        // static void PlayerRecuestWebhookOnStart()
        // {
        //     if (SceneManager.GetActiveScene().name != "main") return;
        //     if (!ZNet.m_isServer) Discord.RecuestWebhook();
        // }
    }
}