using Harmony12;
using UnityEngine;

namespace DWG_SwapAxis.Patches {
    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("AugmentedToeAxisVel", MethodType.Getter)]
    static class StickInput_AugmentedToeAxisVel_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                __result = __instance.augmentedInput.maxVelLastUpdate.x;
                //DWG_SwapAxis.DWG_ConMessage.Add("AugmentedToeAxisVel - " + __result);
            }
            return __result;
        }
    }

    [HarmonyPatch(typeof(StickInput))]
    [HarmonyPatch("ToeAxisVel", MethodType.Getter)]
    static class StickInput_ToeAxisVel_Patch
    {
        [HarmonyPriority(999)]
        static float Postfix(float __result, StickInput __instance)
        {
            if (Main.enabled && Main.settings.do_SwapAxis && (SettingsManager.Instance.controlType == SettingsManager.ControlType.Same))
            {
                __result = __instance.rawInput.maxVelLastUpdate.x;
                //DWG_SwapAxis.DWG_ConMessage.Add("ToeAxisVel - " + __result);
            }
            return __result;
        }
    }
}
