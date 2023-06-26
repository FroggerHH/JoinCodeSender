﻿using BepInEx.Bootstrap;
using DiscordWebhook;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static JoinCodeSender.Plugin;

namespace JoinCodeSender;

[HarmonyPatch]
internal class BeehivePatch
{
    [HarmonyPatch(typeof(Beehive), nameof(Beehive.Interact)), HarmonyPostfix]
    public static void BeehivePatchInteract(Beehive __instance, bool repeat, Humanoid character)
    {
        if (!sendBeehiveMessagesConfig.Value) return;

        Helper.SimplePatch("$Honey", repeat, character as Player);
    }
}