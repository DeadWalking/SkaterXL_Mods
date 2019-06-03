using Harmony12;
using XInputDotNetPure;
using UnityEngine;
//using System.Collections.Generic;
//using System.Linq;

namespace DWG_SwapAxis.Patches
{
    [HarmonyPatch(typeof(InputController))]
    [HarmonyPatch("FixedUpdateTriggers")]
    static class InputController_FixedUpdateTriggers_Patch
    {
        [HarmonyPriority(999)]
        static bool Prefix(InputController __instance, ref StickInput ____leftStick, ref StickInput ____rightStick, ref bool ____leftHeld, ref bool ____rightHeld, ref float ____leftTrigger, ref float ____rightTrigger, ref float ____triggerMultiplier)
        {
            if (Main.enabled && Main.settings.do_SwapAxis)
            {
                //if (global::PlayerController.Instance.playerSM.IsOnGroundStateSM())
                if (PlayerController.Instance.playerSM.IsOnGroundStateSM())
                {
                    if (!____leftStick.IsPopStick)
                    {
                        float outPos = (Mathf.Abs(____leftStick.rawInput.pos.x) * ____triggerMultiplier);
                        if (PlayerController.Instance.IsSwitch) { outPos = (outPos * -1); };

                        if (____leftStick.rawInput.pos.x < -0.3f)
                        {
                            __instance.BroadcastMessage("LeftTriggerHeld", outPos);
                        }
                        else if (____leftStick.rawInput.pos.x > 0.3f)
                        {
                            __instance.BroadcastMessage("RightTriggerHeld", outPos);
                        }
                    }
                    if (!____rightStick.IsPopStick)
                    {
                        float outPos = (Mathf.Abs(____rightStick.rawInput.pos.x) * ____triggerMultiplier);
                        if (PlayerController.Instance.IsSwitch) { outPos = (outPos * -1); };

                        if (____rightStick.rawInput.pos.x < -0.3f)
                        {
                            __instance.BroadcastMessage("LeftTriggerHeld", outPos);
                        }
                        else if (____rightStick.rawInput.pos.x > 0.3f)
                        {
                            __instance.BroadcastMessage("RightTriggerHeld", outPos);
                        }
                    }
                }
                if (____leftHeld)
                {
                    __instance.BroadcastMessage("LeftTriggerHeld", (____leftTrigger * ____triggerMultiplier));
                }
                if (____rightHeld)
                {
                    __instance.BroadcastMessage("RightTriggerHeld", (____rightTrigger * ____triggerMultiplier));
                }
                return false;
            }
            return true;
        }
    }
}
