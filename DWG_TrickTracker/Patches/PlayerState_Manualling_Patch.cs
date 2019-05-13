using Harmony12;
using UnityEngine;

// Manual timer using update and exit for reset and check for rotation.

namespace DWG_TT.Patches
{
    [HarmonyPatch(typeof(PlayerState_Manualling))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Manualling_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(int ____manualSign)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CrntState = TT.TrickState.Man;

                string outMan = "";
                if (____manualSign < 0)
                {
                    outMan = (TT.IsSwitch ? "FakieManny" : "Manual"  );
                }
                else
                {

                    outMan = (TT.IsSwitch ? "SwitchManny" : "NoseManny");
                }
                TT.PrevTrick = outMan;
                TT.CheckRot();
            };
        }
    };

    [HarmonyPatch(typeof(PlayerState_Manualling))]
    [HarmonyPatch("Update")]
    static class PlayerState_Manualling_Update_Patch
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

    [HarmonyPatch(typeof(PlayerState_Manualling))]
    [HarmonyPatch("Exit")]
    static class PlayerState_Manualling_Exit_Patch
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
