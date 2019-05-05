using Harmony12;
using UnityEngine;

namespace DWG_SwapAxis.Patches {
    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("AugmentedToeAxis", MethodType.Getter)]
    static class StickInput_AugmentedToeAxis_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                __result = __instance.augmentedInput.pos.x;
                //DWG_SwapAxis.DWG_ConMessage.Add("AugmentedToeAxis - " + __result);
            }
            return __result;
        }
    }

    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("ToeAxis", MethodType.Getter)]
    static class StickInput_ToeAxis_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                __result = __instance.rawInput.pos.x;
                //DWG_SwapAxis.DWG_ConMessage.Add("ToeAxis - " + __result);
            }
            return __result;
        }
    }
}
