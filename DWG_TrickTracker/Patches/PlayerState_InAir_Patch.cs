using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches {
    [HarmonyPatch(typeof(PlayerState_InAir))]
    [HarmonyPatch("Enter")]
    static class PlayerState_InAir_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerState_InAir __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CaughtLeft = !__instance.LeftFootOff();
                TT.CaughtRight = !__instance.RightFootOff();
            };
        }
    };

    [HarmonyPatch(typeof(PlayerState_InAir))]
    [HarmonyPatch("Update")]
    static class PlayerState_InAir_Update_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerState_InAir __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CaughtLeft = !__instance.LeftFootOff();
                TT.CaughtRight = !__instance.RightFootOff();
            };
        }
    };

    [HarmonyPatch(typeof(PlayerState_InAir))]
    [HarmonyPatch("Exit")]
    static class PlayerState_InAir_Exit_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerState_InAir __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CaughtLeft = !__instance.LeftFootOff();
                TT.CaughtRight = !__instance.RightFootOff();
            };
        }
    };
}
