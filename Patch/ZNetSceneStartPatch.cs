using System;
using BepInEx.Bootstrap;
using DiscordWebhook;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static JoinCodeSender.Plugin;

namespace JoinCodeSender
{
    [HarmonyPatch]
    internal class ZNetSceneStartPatch
    {
        [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake)), HarmonyPostfix]
        static void ZNetSceneAwakePatch()
        {
            if (SceneManager.GetActiveScene().name != "main") return;

            // ZRoutedRpc.instance.Register(nameof(Discord.SendWebhook),
            //     new Action<long, string>(Discord.RPC_SendWebhook));
            // ZRoutedRpc.instance.Register(nameof(Discord.RecuestWebhook),
            //     new Action<long>(Discord.RPC_RecuestWebhook));
        }

        // [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer)), HarmonyPostfix]
        // static void PlayerRecuestWebhookOnStart()
        // {
        //     if (SceneManager.GetActiveScene().name != "main") return;
        //     if (!ZNet.m_isServer) Discord.RecuestWebhook();
        // }
    }
}