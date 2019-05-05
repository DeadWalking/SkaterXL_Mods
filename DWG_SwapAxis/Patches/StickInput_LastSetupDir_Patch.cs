using Harmony12;
using UnityEngine;

namespace DWG_SwapAxis.Patches {
    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("AugmentedLastSetupDir", MethodType.Getter)]
    static class StickInput_AugmentedLastSetupDir_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                if (__instance.IsRightStick)
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? __instance.augmentedInput.prevPos.y : __instance.augmentedInput.prevPos.y);
                }
                else
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.augmentedInput.prevPos.y) : (-__instance.augmentedInput.prevPos.y));
                }
                //DWG_SwapAxis.DWG_ConMessage.Add("AugmentedLastSetupDir - " + __result);
            }
            return __result;
        }
    }

    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("LastSetupDir", MethodType.Getter)]
    static class StickInput_LastSetupDir_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                if (__instance.IsRightStick)
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? __instance.rawInput.prevPos.y : __instance.rawInput.prevPos.y);
                }
                else
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.rawInput.prevPos.y) : (-__instance.rawInput.prevPos.y));
                }
                //DWG_SwapAxis.DWG_ConMessage.Add("LastSetupDir - " + __result);
            }
            return __result;
        }
    }
}
