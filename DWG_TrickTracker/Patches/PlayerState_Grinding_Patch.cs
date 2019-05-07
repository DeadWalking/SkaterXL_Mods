using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {

    [HarmonyPatch(typeof(PlayerState_Grinding))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Grinding_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.CheckRot();
                DWG_TrickTracker.TrackedTime = Time.time;
                DWG_TrickTracker.AddTrick(PlayerController.Instance.boardController.triggerManager.grindDetection.grindType.ToString(), false, true);
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
                DWG_TrickTracker.TrackedTime = Time.time;
            };
        }
    };
}
