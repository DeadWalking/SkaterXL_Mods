using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches
{
    [HarmonyPatch(typeof(PlayerState_Impact))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Impact_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CheckRot();
                TT.PrevState = TT.CrntState;
                TT.CrntState = TT.TrickState.Ride;
            };
        }
    };

    //[HarmonyPatch(typeof(PlayerState_Impact))]
    //[HarmonyPatch("Update")]
    //static class PlayerState_Impact_Update_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static void Postfix()
    //    {
    //        if (Main.enabled && Main.settings.do_TrackTricks)
    //        {
    //            TT.PopWasSwitch = (PlayerController.Instance.IsSwitch);
    //            TT.CurrentState = TT.TrickState.Ride;
    //        };
    //    }
    //};
}
