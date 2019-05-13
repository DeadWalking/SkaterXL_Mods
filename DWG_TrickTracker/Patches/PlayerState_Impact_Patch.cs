using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches
{
    [HarmonyPatch(typeof(PlayerState_Impact))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Impact_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CheckRot();
                TT.CrntState = TT.TrickState.Ride;
            };
        }
    };
}
