using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {
    [HarmonyPatch(typeof(BoardController))]
    [HarmonyPatch("FixedUpdate")]
    static class BoardController_FixedUpdate_Patch
    {
        static float groundTime = Time.time;

        [HarmonyPriority(999)]
        static void Postfix(BoardController __instance, ref bool ____grounded)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                if (____grounded && (Time.time - groundTime) > 1)
                {
                    groundTime = Time.time;
                    DWG_TrickTracker.CheckRot();
                }
            }
        }
    }

    [HarmonyPatch(typeof(BoardController))]
    [HarmonyPatch("Rotate")]
    static class BoardController_Rotate_Patch
    {
        static private Quaternion lastRot;
        static private Quaternion lastFlip;

        [HarmonyPriority(999)]
        static void Postfix(BoardController __instance, ref bool doPop, ref bool doFlip, ref Quaternion ____bufferedRotation, ref Quaternion ____bufferedFlip)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.TrackBoardRot = ((doPop || doFlip) ? "BoardFlip" : "");
                //DWG_TrickTracker.AddTrick(((doFlip ? "Flip" : "") + (doPop ? "Shuv" : "")), true, false);
                //float num = Mathd.AngleBetween(0f, __instance.boardTransform.localEulerAngles.x);
                //float num2 = Mathd.AngleBetween(0f, __instance.boardTransform.localEulerAngles.z);
                //return Mathf.Sqrt(num * num + num2 * num2);
                //Quaternion crntRot = ____bufferedRotation;// __instance.currentRotationTarget;
                //Quaternion crntFlip = ____bufferedFlip;// __instance.currentRotationTarget;


                // DWG_TrickTracker.AddTrick((" BoardX " + num + " BoardZ " + num2), true, false);
                //lastRot = lastRot + crntRot;
                //lastFlip = lastFlip + crntFlip;

                //if (lastRot >= 720) { testRot = 0f; };
                //if (lastFlip >= 720) { testKick = 0f; };
            };
        }
    }

    //[HarmonyPatch(typeof(BoardController))]
    //[HarmonyPatch("RotateWithPop")]
    //static class BoardController_RotateWithPop_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static void Postfix(BoardController __instance, ref float _popDir)
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks)
    //        {
    //            DWG_TrickTracker.AddTrick((((_popDir > 0) ? "Kick" : "Heel") + "Flip"), true, false);
    //        };
    //    }
    //}

    //[HarmonyPatch(typeof(CameraController))]
    //[HarmonyPatch("MoveCameraToPlayer")]
    //static class CameraController_MoveCameraToPlayer_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static void Postfix(CameraController __instance)
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks)
    //        {
    //            DWG_TrickTracker.CamTransform = __instance._actualCam;
    //        }
    //    }
    //}
}
