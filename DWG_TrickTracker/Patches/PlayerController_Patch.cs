using Harmony12;
using System;
using UnityEngine;

namespace DWG_TT.Patches
{
    [HarmonyPatch(typeof(PlayerController))]
    [HarmonyPatch("Update")]
    static class PlayerController_Update_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerController __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.IsSwitch = __instance.IsSwitch;
            };
        }
    };

    [HarmonyPatch(typeof(PlayerController))]
    [HarmonyPatch("OnPop")]
    [HarmonyPatch(new Type[] { typeof(float), typeof(float) })]
    static class PlayerController_OnPopA_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerController __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.WasSwitch = __instance.IsSwitch;
                TT.StickRight = __instance.inputController.RightStick.IsPopStick;
            };
        }
    };

    [HarmonyPatch(typeof(PlayerController))]
    [HarmonyPatch("OnPop")]
    [HarmonyPatch(new Type[] { typeof(float), typeof(float), typeof(Vector3) })]
    static class PlayerController_OnPopB_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerController __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.WasSwitch = __instance.IsSwitch;
                TT.StickRight = __instance.inputController.RightStick.IsPopStick;
            };
        }
    };
}
