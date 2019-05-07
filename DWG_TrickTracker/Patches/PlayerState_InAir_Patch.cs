using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {
    //[HarmonyPatch(typeof(PlayerState_InAir))]
    //[HarmonyPatch("FixedUpdate")]
    //static class PlayerState_InAir_Update_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static void Postfix(PlayerState_InAir __instance)
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks)
    //        {
    //            bool trackNorth = (
    //                               //Ollie
    //                               ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) && !PlayerController.Instance.IsSwitch && __instance.LeftFootOff() && !__instance.RightFootOff()) ||
    //                               ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) && PlayerController.Instance.IsSwitch && __instance.RightFootOff() && !__instance.LeftFootOff()) ||
    //                               ((SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) && !PlayerController.Instance.IsSwitch && __instance.RightFootOff() && !__instance.LeftFootOff()) ||
    //                               ((SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) && PlayerController.Instance.IsSwitch && __instance.LeftFootOff() && !__instance.RightFootOff()) ||
    //                               //Nollie
    //                               ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) && !PlayerController.Instance.IsSwitch && __instance.RightFootOff() && !__instance.LeftFootOff()) ||
    //                               ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) && PlayerController.Instance.IsSwitch && __instance.LeftFootOff() && !__instance.RightFootOff()) ||
    //                               ((SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) && !PlayerController.Instance.IsSwitch && __instance.LeftFootOff() && !__instance.RightFootOff()) ||
    //                               ((SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) && PlayerController.Instance.IsSwitch && __instance.RightFootOff() && !__instance.LeftFootOff()) ?
    //                               true : false);

    //            if (trackNorth)
    //            {
    //                DWG_TrickTracker.TrackedTime = Time.time;
    //                DWG_TrickTracker.AddTrick("North", false, false);
    //            };

    //            if (__instance.RightFootOff() && __instance.LeftFootOff())
    //            {
    //                DWG_TrickTracker.TrackedTime = Time.time;
    //                DWG_TrickTracker.AddTrick("Varial", false, false);
    //            };
    //        }
    //    }
    //}
}
