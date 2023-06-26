﻿using BepInEx.Bootstrap;
using DiscordWebhook;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static JoinCodeSender.Plugin;

namespace JoinCodeSender;

[HarmonyPatch]
internal class CraftingStationPatch
{
    [HarmonyPatch(typeof(CraftingStation), nameof(CraftingStation.Interact)), HarmonyPostfix]
    public static void CraftingStationPatchInteract(CraftingStation __instance, bool repeat, Humanoid user)
    {
        if (!sendCraftMessagesConfig.Value) return;

        Helper.PiecePatch("$CraftingStation", repeat, __instance.GetComponent<Piece>(), user as Player);
    }
}