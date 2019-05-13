using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches
{
    [HarmonyPatch(typeof(PlayerState_BeginPop))]
    [HarmonyPatch("Enter")]
    static class PlayerState_BeginPop_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CrntState = TT.TrickState.Air;
            };
        }
    };
}
