using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches
{
    //[HarmonyPatch(typeof(PlayerState_Riding))]
    //[HarmonyPatch("Enter")]
    //static class PlayerState_Riding_Enter_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static void Postfix(PlayerState_Riding __instance)
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks && __instance.IsOnGroundState())
    //        {
    //            TT.CheckRot();
    //            TT.CurrentState = TT.TrickState.Ride;
    //        };
    //    }
    //};

    //[HarmonyPatch(typeof(PlayerState_Riding))]
    //[HarmonyPatch("Update")]
    //static class PlayerState_Riding_Update_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static void Postfix(PlayerState_Riding __instance)
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks && __instance.IsOnGroundState())
    //        {
    //            TT.SktrRot = 0f;
    //            TT.BrdRot = 0f;
    //            TT.BrdFlip = 0f;
    //            TT.CurrentState = TT.TrickState.Ride;
    //        };
    //    }
    //};
}
