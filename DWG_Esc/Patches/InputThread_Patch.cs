using Harmony12;
using XInputDotNetPure;
using UnityEngine;
//using System.Collections.Generic;
//using System.Linq;

namespace DWG_SwapAxis.Patches
{
    [HarmonyPatch(typeof(InputThread))]
    [HarmonyPatch("InputUpdate")]
    static class InputThread_Patch
    {
        private static float GetVel(float thisVal, float lastVal, long thisTime, long lastTime)
        {
            return (thisVal - lastVal) / ((float)(thisTime - lastTime) * 1E-07f);

        }

        [HarmonyPriority(999)]
        static bool Prefix(InputThread __instance, ref GamePadState ___prevState, ref GamePadState ___state, PlayerIndex ___playerIndex, ref int ____pos, int ____maxLength, ref InputThread.InputStruct ____lastFrameData)
        {
            if (Main.enabled && Main.settings.do_SwapAxis)
            {
                ___prevState = ___state;
                ___state = GamePad.GetState(___playerIndex);
                if (____pos < ____maxLength)
                {
                    float newLX = __instance.leftXFilter.Filter((double)((Mathf.Abs(__instance.inputController.player.GetAxis("LeftStickY")) < 0.1f) ? 0f : __instance.inputController.player.GetAxis("LeftStickY")));
                    float newLY = (__instance.leftYFilter.Filter((double)__instance.inputController.player.GetAxis("LeftStickX")) * -1);
                    float newRX = __instance.rightXFilter.Filter((double)((Mathf.Abs(__instance.inputController.player.GetAxis("RightStickY")) < 0.1f) ? 0f : __instance.inputController.player.GetAxis("RightStickY")));
                    float newRY = (__instance.rightYFilter.Filter((double)__instance.inputController.player.GetAxis("RightStickX")) * -1);

                    if (SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) { newLX *= -1; newLY *= -1; newRX *= -1; newRY *= -1; };
                    //if (PlayerController.Instance.IsSwitch) { newLX *= -1; newRX *= -1; };

                    __instance.inputsIn[____pos].leftX = newLX;
                    __instance.inputsIn[____pos].leftY = newLY;
                    __instance.inputsIn[____pos].rightX = newRX;
                    __instance.inputsIn[____pos].rightY = newRY;

                    __instance.inputsIn[____pos].time = System.DateTime.UtcNow.Ticks;
                    __instance.inputsIn[____pos].leftXVel = GetVel(__instance.inputsIn[____pos].leftX, ____lastFrameData.leftX, __instance.inputsIn[____pos].time, ____lastFrameData.time);
                    __instance.inputsIn[____pos].leftYVel = GetVel(__instance.inputsIn[____pos].leftY, ____lastFrameData.leftY, __instance.inputsIn[____pos].time, ____lastFrameData.time);
                    __instance.inputsIn[____pos].rightXVel = GetVel(__instance.inputsIn[____pos].rightX, ____lastFrameData.rightX, __instance.inputsIn[____pos].time, ____lastFrameData.time);
                    __instance.inputsIn[____pos].rightYVel = GetVel(__instance.inputsIn[____pos].rightY, ____lastFrameData.rightY, __instance.inputsIn[____pos].time, ____lastFrameData.time);
                    ____lastFrameData.leftX = __instance.inputsIn[____pos].leftX;
                    ____lastFrameData.leftY = __instance.inputsIn[____pos].leftY;
                    ____lastFrameData.rightX = __instance.inputsIn[____pos].rightX;
                    ____lastFrameData.rightY = __instance.inputsIn[____pos].rightY;
                    ____lastFrameData.time = __instance.inputsIn[____pos].time;
                    ____pos++;
                }
                return false;
            }
            return true;
        }
    }
}
