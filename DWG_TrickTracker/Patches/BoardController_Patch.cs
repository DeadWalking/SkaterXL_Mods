using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {

    [HarmonyPatch(typeof(BoardController))]
    [HarmonyPatch("Rotate")]
    static class BoardController_Rotate_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(BoardController __instance, ref Transform ___boardTransform)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.BoardPos = ___boardTransform;
            }
        }
    }

    [HarmonyPatch(typeof(CameraController))]
    [HarmonyPatch("MoveCameraToPlayer")]
    static class CameraController_MoveCameraToPlayer_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(CameraController __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.CamTransform = __instance._actualCam;
            }
        }
    }
}
