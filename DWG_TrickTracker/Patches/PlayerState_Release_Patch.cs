using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches {
    [HarmonyPatch(typeof(PlayerState_Released))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Released_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerState_Released __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CaughtLeft = !__instance.LeftFootOff();
                TT.CaughtRight = !__instance.RightFootOff();
            };
        }
    };

    [HarmonyPatch(typeof(PlayerState_Released))]
    [HarmonyPatch("Update")]
    static class PlayerState_Released_Update_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerState_Released __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CaughtLeft = !__instance.LeftFootOff();
                TT.CaughtRight = !__instance.RightFootOff();
            };
        }
    };

    [HarmonyPatch(typeof(PlayerState_Released))]
    [HarmonyPatch("Exit")]
    static class PlayerState_Released_Exit_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerState_Released __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CaughtLeft = !__instance.LeftFootOff();
                TT.CaughtRight = !__instance.RightFootOff();
            };
        }
    };
}
