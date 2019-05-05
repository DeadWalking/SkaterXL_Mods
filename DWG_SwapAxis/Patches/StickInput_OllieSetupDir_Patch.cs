using Harmony12;
using UnityEngine;

namespace DWG_SwapAxis.Patches {
    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("AugmentedOllieSetupDir", MethodType.Getter)]
    static class StickInput_AugmentedOllieSetupDir_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                if (__instance.IsRightStick)
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? __instance.augmentedInput.pos.y : 0f);
                }
                else
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? 0f : (-__instance.augmentedInput.pos.y));
                }
                //DWG_SwapAxis.DWG_ConMessage.Add("AugmentedOllieSetupDir - " + __result);
            }
            return __result;
        }
    }

    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("OllieSetupDir", MethodType.Getter)]
    static class StickInput_OllieSetupDir_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                if (__instance.IsRightStick)
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? __instance.rawInput.pos.y : 0f);
                }
                else
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? 0f : (-__instance.rawInput.pos.y));
                }
                //DWG_SwapAxis.DWG_ConMessage.Add("OllieSetupDir - " + __result);
            }
            return __result;
        }
    }
}
