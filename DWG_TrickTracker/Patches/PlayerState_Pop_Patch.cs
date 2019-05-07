using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {

    [HarmonyPatch(typeof(PlayerState_Pop))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Pop_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(PlayerState_Pop __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                string outPopType = "Nollie";
                if (PlayerController.Instance.animationController.skaterAnim.GetFloat(outPopType) == 0f)
                {
                    outPopType = "Ollie";
                };

                DWG_TrickTracker.TrackedTime = Time.time;
                DWG_TrickTracker.AddTrick(outPopType, false, false);
            };
        }
    };

    //[HarmonyPatch(typeof(PlayerState_Pop))]
    //[HarmonyPatch("KickAdd")]
    //static class PlayerState_Pop_KickAdd_Patch
    //{
    //    static float testRot = 0f;
    //    static float testKick = 0f;
    //    static void Postfix(PlayerState_Pop __instance, ref float ____popVel, ref float ____kickAddSoFar)
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks)
    //        {
    //            testRot = testRot + Mathf.Clamp(Mathf.Abs(____popVel) / 5f, -0.7f, 0.7f);
    //            testKick = testKick + ____kickAddSoFar;
    //            DWG_TrickTracker.AddTrick((testRot + " Board Rotation " + testKick), false, false);

    //            if (testRot >= 720) { testRot = 0f; };
    //            if (testKick >= 720) { testKick = 0f; };
    //        }
    //    }
    //}
}
