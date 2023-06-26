﻿using BepInEx.Bootstrap;
using DiscordWebhook;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static JoinCodeSender.Plugin;

namespace JoinCodeSender;

[HarmonyPatch]
internal class ContainerPatch
{
    [HarmonyPatch(typeof(Container), nameof(Container.Interact)), HarmonyPostfix]
    public static void ContainerPatchInteract(Beehive __instance, bool hold, Humanoid character)
    {
        if (!sendChestMessagesConfig.Value) return;

        Helper.SimplePatch("$Chest", hold, character as Player);
    }
}