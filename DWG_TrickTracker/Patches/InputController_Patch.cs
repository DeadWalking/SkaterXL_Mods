using Harmony12;
using System;
using UnityEngine;

namespace DWG_TrickTracker.Patches {

    [HarmonyPatch(typeof(InputController))]
    [HarmonyPatch("UpdateTriggers")]
    static class InputController_UpdateTriggers_Patch
    {
        static private Quaternion lastRot;

        [HarmonyPriority(999)]
        static void Postfix(ref bool ____leftHeld, ref bool ____rightHeld)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                if (DWG_TrickTracker.LeftIsFront() && ____leftHeld && !____rightHeld)
                {
                    DWG_TrickTracker.TrackTrig = "Fs";
                }
                else if(DWG_TrickTracker.LeftIsFront() && ____rightHeld && !____leftHeld)
                {
                    DWG_TrickTracker.TrackTrig = "Bs";
                };
            };
        }
    };
}
