using Harmony12;
using UnityEngine;

// Grind timer using update and exit for reset and check for rotation during grind.

namespace DWG_TT.Patches {

    [HarmonyPatch(typeof(PlayerState_Grinding))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Grinding_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.PrevState = TT.CrntState;
                TT.CrntState = TT.TrickState.Grnd;

                TT.TrackedTime = Time.time;
                TT.CheckRot();
            };
        }
    };

    [HarmonyPatch(typeof(PlayerState_Grinding))]
    [HarmonyPatch("Update")]
    static class PlayerState_Grinding_Update_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.TrackedTime = Time.time;
            };
        }
    };

    [HarmonyPatch(typeof(PlayerState_Grinding))]
    [HarmonyPatch("Exit")]
    static class PlayerState_Grinding_Exit_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.PrevState = TT.CrntState;
                TT.CrntState = TT.TrickState.Air;
                TT.TrackedTime = Time.time;

                //TT.UpdateBools();
                TT.UpdateRots(); // Change for TT.CheckRots(); when there is logic for Grind tricks and timer
            };
        }
    };
}
