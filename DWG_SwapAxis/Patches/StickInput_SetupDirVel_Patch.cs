using Harmony12;
using UnityEngine;

namespace DWG_SwapAxis.Patches {
    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("AugmentedSetupDirVel", MethodType.Getter)]
    static class StickInput_AugmentedSetupDirVel_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                if (__instance.IsRightStick)
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? __instance.augmentedInput.maxVelLastUpdate.y : __instance.augmentedInput.maxVelLastUpdate.y);
                }
                else
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.augmentedInput.maxVelLastUpdate.y) : (-__instance.augmentedInput.maxVelLastUpdate.y));
                }
                //DWG_SwapAxis.DWG_ConMessage.Add("AugmentedSetupDirVel - " + __result);
            }
            return __result;
        }
    }

    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("SetupDirVel", MethodType.Getter)]
    static class StickInput_SetupDirVel_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                if (__instance.IsRightStick)
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? __instance.rawInput.maxVelLastUpdate.y : __instance.rawInput.maxVelLastUpdate.y);
                }
                else
                {
                    __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.rawInput.maxVelLastUpdate.y) : (-__instance.rawInput.maxVelLastUpdate.y));
                }
                //DWG_SwapAxis.DWG_ConMessage.Add("SetupDirVel - " + __result);
            }
            return __result;
        }
    }
}
