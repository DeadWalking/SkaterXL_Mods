using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches
{
    [HarmonyPatch(typeof(BoardController))]
    [HarmonyPatch("Update")]
    static class BoardController_Update_Patch
    {
        static private Vector3 lastBrdEul;
        static TT.TrickState lastState;

        [HarmonyPriority(999)]
        static void Postfix(BoardController __instance)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                if (lastState != TT.CrntState) {
                    lastState = TT.CrntState;
                    TT.UpdateRots();
                };

                Vector3 boardEul = __instance.boardTransform.eulerAngles;

                TT.BrdRot += Mathd.AngleBetween(lastBrdEul.y, boardEul.y);
                TT.MaxBrdRot = ((Mathf.Abs(TT.MaxBrdRot) < Mathf.Abs(TT.BrdRot)) ? TT.BrdRot : TT.MaxBrdRot);
                TT.BrdFlip += Mathd.AngleBetween(lastBrdEul.z, boardEul.z);
                TT.MaxBrdFlip = ((Mathf.Abs(TT.MaxBrdFlip) < Mathf.Abs(TT.BrdFlip)) ? TT.BrdFlip : TT.MaxBrdFlip);
                TT.BrdTwk += Mathd.AngleBetween(lastBrdEul.x, boardEul.x);
                TT.MaxBrdTwk = ((Mathf.Abs(TT.MaxBrdTwk) < Mathf.Abs(TT.BrdFlip)) ? TT.BrdFlip : TT.MaxBrdTwk);
                lastBrdEul = boardEul;
            };
        }
    };
}
