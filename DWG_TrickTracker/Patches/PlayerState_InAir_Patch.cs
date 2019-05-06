using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {
    //[HarmonyPatch(typeof(PlayerState_InAir))]
    //[HarmonyPatch("OnManualUpdate")]
    //static class PlayerState_InAir_OnManualUpdate_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static void Postfix()
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks)
    //        {
    //            string tmpPopType = "Nollie";
    //            float popId = PlayerController.Instance.animationController.skaterAnim.GetFloat(tmpPopType);
    //            if (popId == 0f)
    //            {
    //                tmpPopType = "Ollie";
    //            }
    //            DWG_TrickTracker.trackedTime = Time.time;
    //            DWG_TrickTracker.DWG_TrackedTricks = DWG_TrickTracker.DWG_TrackedTricks +
    //                                                ((DWG_TrickTracker.DWG_TrackedTricks.Length > 0) ? " + " : "") +
    //                                                (PlayerController.Instance.IsSwitch ? "Switch" : "") +
    //                                                tmpPopType;
    //        }
    //    }
    //}
}
