using Harmony12;
using UnityEngine;

namespace DWG_SwapAxis.Patches {
    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("AugmentedLastToeAxis", MethodType.Getter)]
    static class StickInput_AugmentedLastToeAxis_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                __result = __instance.augmentedInput.prevPos.x;
                //DWG_SwapAxis.DWG_ConMessage.Add("AugmentedLastToeAxis - " + __result);
            }
            return __result;
        }
    }

    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("LastToeAxis", MethodType.Getter)]
    static class StickInput_LastToeAxis_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                __result = __instance.rawInput.prevPos.x;
                //DWG_SwapAxis.DWG_ConMessage.Add("LastToeAxis - " + __result);
            }
            return __result;
        }
    }
}
