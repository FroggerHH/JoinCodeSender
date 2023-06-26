﻿using BepInEx.Bootstrap;
using DiscordWebhook;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static JoinCodeSender.Plugin;

namespace JoinCodeSender;

[HarmonyPatch]
internal class ItemDropPatch
{
    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Interact)), HarmonyPostfix]
    public static void ItemDropPatchInteract(ItemDrop __instance, bool repeat, Humanoid character)
    {
        if (!sendPickupMessagesConfig.Value) return;

        Helper.ItemDropPatch(repeat, __instance, character as Player);
    }
}