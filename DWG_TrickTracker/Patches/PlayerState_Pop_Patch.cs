using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {

    [HarmonyPatch(typeof(PlayerState_Pop))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Pop_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                string tmpPopType = "Nollie";
                float popId = PlayerController.Instance.animationController.skaterAnim.GetFloat(tmpPopType);
                if (popId == 0f)
                {
                    tmpPopType = "Ollie";
                }
                DWG_TrickTracker.DWG_TrackedTricks = DWG_TrickTracker.DWG_TrackedTricks +
                                                    ((DWG_TrickTracker.DWG_TrackedTricks.Length > 0) ? " + " : "") +
                                                    (PlayerController.Instance.IsSwitch ? "Switch" : "") +
                                                    tmpPopType;
            }
        }
    }

    //[HarmonyPatch(typeof(PlayerState_Pop))]
    //[HarmonyPatch("KickAdd")]
    //static class PlayerState_Pop_KickAdd_Patch
    //{
    //    static void Postfix(PlayerState_Pop __instance, ref float ____kickAddSoFar)
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks)
    //        {
    //            DWG_TrickTracker.DWG_TrackedTricks = DWG_TrickTracker.DWG_TrackedTricks +
    //                                                 ((DWG_TrickTracker.DWG_TrackedTricks.Length > 0) ? " + " : "") +
    //                                                 "Board Rotation " + ____kickAddSoFar;
    //        }
    //    }
    //}
}
