using Harmony12;
using System;
using UnityEngine;

namespace DWG_TrickTracker.Patches {

    [HarmonyPatch(typeof(SkaterController))]
    [HarmonyPatch("InAirRotation")]
    [HarmonyPatch(new Type[] { typeof(float) })]
    static class SkaterContorller_InAirRotation_Patch
    {
        static private Quaternion lastRot;

        [HarmonyPriority(999)]
        static void Postfix(SkaterController __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                Quaternion crntRot = __instance.skaterRigidbody.rotation;
                DWG_TrickTracker.TrackSkaterRot = DWG_TrickTracker.TrackSkaterRot + Quaternion.Angle(lastRot, crntRot);
                lastRot = crntRot;
            };
        }
    };

    [HarmonyPatch(typeof(SkaterController))]
    [HarmonyPatch("UpdateRidingPositionsCOMTempWallie")]
    static class SkaterContorller_UpdateRidingPositionsCOMTempWallie_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.TrackedTime = Time.time;
                DWG_TrickTracker.AddTrick("Wallie" ,false, false);
            };
        }
    };
}
