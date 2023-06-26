﻿using BepInEx.Bootstrap;
using DiscordWebhook;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static JoinCodeSender.Plugin;

namespace JoinCodeSender;

[HarmonyPatch]
internal class PickableObjectPatch
{
    [HarmonyPatch(typeof(Pickable), nameof(Pickable.Interact)), HarmonyPostfix]
    public static void ItemDropPatchInteract(Pickable __instance, bool repeat, Humanoid character)
    {
        if (!sendPickableMessagesConfig.Value) return;


        Helper.PickablePatch(repeat, __instance, character as Player);
    }
}