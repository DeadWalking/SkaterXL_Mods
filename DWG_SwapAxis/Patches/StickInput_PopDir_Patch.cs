using Harmony12;
using UnityEngine;

namespace DWG_SwapAxis.Patches {
    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("AugmentedPopDir", MethodType.Getter)]
    static class StickInput_AugmentedPopDir_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                if (__instance.IsPopStick)
                {
                    if (__instance.IsRightStick)
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.augmentedInput.pos.y) : (-__instance.augmentedInput.pos.y));
                    }
                    else
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? __instance.augmentedInput.pos.y : __instance.augmentedInput.pos.y);
                    }
                }
                else if (__instance.IsRightStick)
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.augmentedInput.pos.y) : __instance.augmentedInput.pos.y);
                }
                else
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.augmentedInput.pos.y) : (-__instance.augmentedInput.pos.y));
                }
                //DWG_SwapAxis.DWG_ConMessage.Add("AugmentedPopDir - " + __result);
            }
            return __result;
        }
    }

    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("PopDir", MethodType.Getter)]
    static class StickInput_PopDir_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                if (__instance.IsPopStick)
                {
                    if (__instance.IsRightStick)
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.rawInput.pos.y) : (-__instance.rawInput.pos.y));
                    }
                    else
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? __instance.rawInput.pos.y : __instance.rawInput.pos.y);
                    }
                }
                else if (__instance.IsRightStick)
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.rawInput.pos.y) : __instance.rawInput.pos.y);
                }
                else
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.rawInput.pos.y) : (-__instance.rawInput.pos.y));
                }
                //DWG_SwapAxis.DWG_ConMessage.Add("PopDir - " + __result);
            }
            return __result;
        }
    }
}
