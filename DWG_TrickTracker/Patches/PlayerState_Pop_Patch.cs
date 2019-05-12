using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches {

    [HarmonyPatch(typeof(PlayerState_Pop))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Pop_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerState_Pop __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CaughtLeft = !__instance.LeftFootOff();
                TT.CaughtRight = !__instance.RightFootOff();
            };
        }
    };

    [HarmonyPatch(typeof(PlayerState_Pop))]
    [HarmonyPatch("Update")]
    static class PlayerState_Pop_Update_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerState_Pop __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CaughtLeft = !__instance.LeftFootOff();
                TT.CaughtRight = !__instance.RightFootOff();
            };
        }
    };

    [HarmonyPatch(typeof(PlayerState_Pop))]
    [HarmonyPatch("Exit")]
    static class PlayerState_Pop_Exit_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerState_Pop __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CaughtLeft = !__instance.LeftFootOff();
                TT.CaughtRight = !__instance.RightFootOff();
            };
        }
    };
}
