using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {

    [HarmonyPatch(typeof(PlayerState_Manualling))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Manualling_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(ref int ____manualSign)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.CheckRot();
                DWG_TrickTracker.TrackedTime = Time.time;
                //DWG_TrickTracker.TrackManual = ((____manualSign < 0) ? 1 : 2);
                string outManual = ((____manualSign < 0) ? "Manual" : "NoseManual");
                DWG_TrickTracker.AddTrick(outManual, false, false);
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
                DWG_TrickTracker.TrackedTime = Time.time;
            };
        }
    };

    //[HarmonyPatch(typeof(PlayerState_Manualling))]
    //[HarmonyPatch("Exit")]
    //static class PlayerState_Manualling_Exit_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static void Postfix(ref int ____manualSign)
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks)
    //        {
    //            DWG_TrickTracker.TrackManual = 0;
    //        }
    //    }
    //}
}
