using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches
{
    [HarmonyPatch(typeof(PlayerState_Riding))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Riding_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CrntState = TT.TrickState.Ride;
            };
        }
    };

    [HarmonyPatch(typeof(PlayerState_Riding))]
    [HarmonyPatch("Update")]
    static class PlayerState_Riding_Update_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CrntState = TT.TrickState.Ride;
            };
        }
    };
}
