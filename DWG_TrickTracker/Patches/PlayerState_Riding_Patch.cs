using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {
    //[HarmonyPatch(typeof(PlayerState_Riding))]
    //[HarmonyPatch("Enter")]
    //static class PlayerState_Riding_Enter_Patch
    //{
    //    static void Postfix()
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks)
    //        {
    //            DWG_TrickTracker.DWG_TrackedTricks = "";
    //        }
    //    }
    //}

    [HarmonyPatch(typeof(PlayerState_Riding))]
    [HarmonyPatch("OnBrakeHeld")]
    static class PlayerState_Riding_OnBrakeHeld_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.DWG_TrackedTricks = "";
            }
        }
    }

    [HarmonyPatch(typeof(PlayerState_Riding))]
    [HarmonyPatch("OnBrakePressed")]
    static class PlayerState_Riding_OnBrakePressed_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.DWG_TrackedTricks = "";
            }
        }
    }

    [HarmonyPatch(typeof(PlayerState_Riding))]
    [HarmonyPatch("OnGrindDetected")]
    static class PlayerState_Riding_OnGrindDetected_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.DWG_TrackedTricks = "";
            }
        }
    }

    [HarmonyPatch(typeof(PlayerState_Riding))]
    [HarmonyPatch("OnManualEnter")]
    static class PlayerState_Riding_OnManualEnter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.DWG_TrackedTricks = DWG_TrickTracker.DWG_TrackedTricks + ((DWG_TrickTracker.DWG_TrackedTricks.Length > 0) ? " + Manual" : "Manual");
            }
        }
    }

    [HarmonyPatch(typeof(PlayerState_Riding))]
    [HarmonyPatch("OnNoseManualEnter")]
    static class PlayerState_Riding_OnNoseManualEnter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.DWG_TrackedTricks = DWG_TrickTracker.DWG_TrackedTricks + ((DWG_TrickTracker.DWG_TrackedTricks.Length > 0) ? " + NoseManual" : "NoseManual");
            }
        }
    }

    [HarmonyPatch(typeof(PlayerState_Riding))]
    [HarmonyPatch("OnPushButtonHeld")]
    static class PlayerState_Riding_OnPushButtonHeld_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.DWG_TrackedTricks = "";
            }
        }
    }

    [HarmonyPatch(typeof(PlayerState_Riding))]
    [HarmonyPatch("OnPushButtonPressed")]
    static class PlayerState_Riding_OnPushButtonPressed_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.DWG_TrackedTricks = "";
            }
        }
    }
}
