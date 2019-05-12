﻿using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches
{
    [HarmonyPatch(typeof(PlayerState_Bailed))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Bailed_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.PrevState = TT.CrntState;
                TT.CrntState = TT.TrickState.Bail;
                TT.TrackedTricks = ""; // "WahWaahhWaaahhh!";

                //TT.UpdateBools();
                TT.UpdateRots();
            };
        }
    };
}