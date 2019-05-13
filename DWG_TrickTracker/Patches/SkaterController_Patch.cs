using Harmony12;
using System;
using UnityEngine;

namespace DWG_TT.Patches
{
    [HarmonyPatch(typeof(SkaterController))]
    [HarmonyPatch("Update")]
    static class SkaterContorller_Update_Patch
    {
        static private Vector3 lastSktrEul;
        static float lastSktrZPos;

        [HarmonyPriority(999)]
        static void Postfix(SkaterController __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                float thisHeight = __instance.skaterTransform.position.y;
                TT.DidApex = (lastSktrZPos < thisHeight);
                lastSktrZPos = thisHeight;

                Vector3 crntSktrEul = __instance.skaterTransform.eulerAngles;
                TT.SktrRot += Mathd.AngleBetween(lastSktrEul.y, crntSktrEul.y);
                TT.MaxSktrRot = ((Mathf.Abs(TT.MaxSktrRot) < Mathf.Abs(TT.SktrRot)) ? TT.SktrRot : TT.MaxSktrRot);

                lastSktrEul = crntSktrEul;
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
                TT.AddTrick("Wallie");
            };
        }
    };
}
