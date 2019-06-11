using System;
using System.Collections.Generic;
using Harmony12;
using UnityEngine;

namespace DWG_TT
{
    [HarmonyPatch(typeof(BoardController), "FixedUpdate")]
    static public class BoardCon
    {
        static private bool[] _wheelsDown = new bool[4] { false, false, false, false };
        static public bool GetWheelDown(int p_wId) { return _wheelsDown[p_wId]; }
        [HarmonyPriority(999)]
        static void Postfix(BoardController __instance, ref bool[] ____wheelsDown)
        {
            if (!Main.settings.do_TrackTricks) { return; };
            _wheelsDown = ____wheelsDown;
        }
    }

    //[HarmonyPatch(typeof(PlayerState_InAir), "Update")]
    //static public class PStateAir
    //{
    //    public static bool LeftOff { get ; private set; }
    //    public static bool RightOff { get; private set; }

    //    [HarmonyPriority(999)]
    //    static void Postfix(PlayerState_InAir __instance, ref bool ____caughtLeft, ref bool ____caughtRight)
    //    {
    //        if (!Main.settings.do_TrackTricks) { return; };
    //        LeftOff = ____caughtLeft;
    //        RightOff = ____caughtRight;
    //    }
    //}

    [HarmonyPatch(typeof(GrindTrigger), "OnTriggerEnter")]
    static public class GrndTrigEnter
    {
        [HarmonyPriority(999)]
        static void Postfix(GrindTrigger __instance)
        {
            if (!Main.settings.do_TrackTricks) { return; };
            if (__instance.Colliding)
            {
                //GrndTrigs.Hit[GrndTrigs.TrigByName(__instance.name)] = (__instance.name == "Board Trigger" && (GrndTrigs.Hit[GrndTrigs.NTrig] || GrndTrigs.Hit[GrndTrigs.TTrig]) ? false : __instance.Colliding);
                GrndTrigs.Hit[GrndTrigs.TrigByName(__instance.name)] = __instance.Colliding;
            }
        }
    }

    [HarmonyPatch(typeof(GrindTrigger), "OnTriggerStay")]
    static public class GrndTrigStay
    {
        [HarmonyPriority(999)]
        static void Postfix(GrindTrigger __instance)
        {
            if (!Main.settings.do_TrackTricks) { return; };
            if (__instance.Colliding)
            {
                //GrndTrigs.Hit[GrndTrigs.TrigByName(__instance.name)] = (__instance.name == "Board Trigger" && (GrndTrigs.Hit[GrndTrigs.NTrig] || GrndTrigs.Hit[GrndTrigs.TTrig]) ? false : __instance.Colliding);
                GrndTrigs.Hit[GrndTrigs.TrigByName(__instance.name)] = __instance.Colliding;
            }
        }
    }

    [HarmonyPatch(typeof(GrindTrigger), "OnTriggerExit")]
    static public class GrndTrigExit
    {
        [HarmonyPriority(999)]
        static void Postfix(GrindTrigger __instance)
        {
            if (!Main.settings.do_TrackTricks) { return; };
            GrndTrigs.Hit[GrndTrigs.TrigByName(__instance.name)] = false;
        }
    }

    [HarmonyPatch(typeof(TriggerManager), "GrindTriggerCheck")]
    static public class GrndTrigs
    {
        static public bool[] Hit = new bool[5] { false, false, false, false, false };
        static private Vector3[] grndTrigPos = new Vector3[5] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        static public Vector3 GetTrigPos(int p_trigIndx) { return grndTrigPos[p_trigIndx]; }

        public const int NTrig = 1;
        public const int FTTrig = 4;
        public const int BrdTrig = 0;
        public const int BTTrig = 3;
        public const int TTrig = 2;

        static public int TrigByName(string p_sType)
        {
            switch (p_sType)
            {
                case "Nose Trigger":
                    return NTrig;// SXLH.FlipTrigs ? NTrig : TTrig;
                case "Front Truck Trigger":
                    return FTTrig;// SXLH.FlipTrigs ? FTTrig : BTTrig;
                case "Board Trigger":
                    return BrdTrig;
                case "Back Truck Trigger":
                    return BTTrig;// SXLH.FlipTrigs ? BTTrig : FTTrig;
                case "Tail Trigger":
                    return TTrig;//SXLH.FlipTrigs ? TTrig : NTrig;
                default:
                    return BrdTrig;
            }
        }

        [HarmonyPriority(999)]
        static void Postfix(TriggerManager __instance, ref GrindTrigger[] ____grindTriggers)
        {
            if (!Main.enabled) { return; };
            if (__instance == null || ____grindTriggers == null) {
                DebugOut.Log("GrndTrigs" + " Waiting on  GrindTriggers");
                return;
            };

            for (int i = 0; i < ____grindTriggers.Length; i++)
            {
                if (____grindTriggers[i] != null)
                {
                    grndTrigPos[TrigByName(____grindTriggers[i].name)] = ____grindTriggers[i].transform.position;
                };
            }
            return;
        }
    }
}