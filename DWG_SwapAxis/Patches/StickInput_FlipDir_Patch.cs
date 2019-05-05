using Harmony12;
using UnityEngine;

namespace DWG_SwapAxis.Patches {
    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("AugmentedFlipDir", MethodType.Getter)]
    static class StickInput_AugmentedFlipDir_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.augmentedInput.pos.x) : __instance.augmentedInput.pos.x);
                //DWG_SwapAxis.DWG_ConMessage.Add("AugmentedFlipDir - " + __result);
            }
            return __result;
        }
    }

    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("FlipDir", MethodType.Getter)]
    static class StickInput_FlipDir_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                __result = ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) ? (-__instance.rawInput.pos.x) : __instance.rawInput.pos.x);
                //DWG_SwapAxis.DWG_ConMessage.Add("FlipDir - " + __result);
            }
            return __result;
        }
    }
}
