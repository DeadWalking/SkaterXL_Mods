using Harmony12;
using UnityEngine;

namespace DWG_SwapAxis.Patches {
    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("AugmentedManualAxis", MethodType.Getter)]
    static class StickInput_AugmentedManualAxis_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                if (__instance.IsRightStick)
                {
                    if (!PlayerController.Instance.IsSwitch)
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? __instance.augmentedInput.pos.y : 0f);
                    }
                    else
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? 0f : __instance.augmentedInput.pos.y);
                    }
                }
                else
                {
                    if (!PlayerController.Instance.IsSwitch)
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? 0f : (-__instance.augmentedInput.pos.y));
                    }
                    else
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.augmentedInput.pos.y) : 0f);
                    }
                }
                //DWG_SwapAxis.DWG_ConMessage.Add("AugmentedManualAxis - " + __result);
            }
            return Mathf.Clamp(__result, 0f, 1f);
        }
    }

    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("ManualAxis", MethodType.Getter)]
    static class StickInput_ManualAxis_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                if (__instance.IsRightStick)
                {
                    if (!PlayerController.Instance.IsSwitch)
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? __instance.rawInput.pos.y : 0f);
                    }
                    else
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? 0f : __instance.rawInput.pos.y);
                    }
                }
                else
                {
                    if (!PlayerController.Instance.IsSwitch)
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? 0f : (-__instance.rawInput.pos.y));
                    }
                    else
                    {
                        __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.rawInput.pos.y) : 0f);
                    }
                }
                //DWG_SwapAxis.DWG_ConMessage.Add("ManualAxis - " + __result);
            }
            return Mathf.Clamp(__result, 0f, 1f);
        }
    }
}
