using Harmony12;
using System;
using UnityEngine;

namespace DWG_SwapAxis.Patches
{
    //[HarmonyPatch(typeof(StickInput), "AugmentedToeAxis", MethodType.Getter)]
    //static class AugToePosX
    //{
    //    [HarmonyPriority(999)]
    //    static float Postfix(float __result, StickInput __instance)
    //    {
    //        return (DWG_SwapAxis.SwapToesAxis ? (__instance.augmentedInput.pos.x * -1) : __instance.augmentedInput.pos.x);
    //    }
    //}

    //[HarmonyPatch(typeof(StickInput), "ToeAxis", MethodType.Getter)]
    //static class ToePosX
    //{
    //    [HarmonyPriority(999)]
    //    static float Postfix(float __result, StickInput __instance)
    //    {
    //        return (DWG_SwapAxis.SwapToesAxis ? (__instance.rawInput.pos.x * -1) : __instance.rawInput.pos.x); ;
    //    }
    //}

    //[HarmonyPatch(typeof(StickInput), "AugmentedLastToeAxis", MethodType.Getter)]
    //static class AugLastToePosX
    //{
    //    [HarmonyPriority(999)]
    //    static float Postfix(float __result, StickInput __instance)
    //    {
    //        return (DWG_SwapAxis.SwapToesAxis ? (__instance.augmentedInput.prevPos.x * -1) : __instance.augmentedInput.prevPos.x);
    //    }
    //}

    //[HarmonyPatch(typeof(StickInput), "LastToeAxis", MethodType.Getter)]
    //static class LastToePosX
    //{
    //    [HarmonyPriority(999)]
    //    static float Postfix(float __result, StickInput __instance)
    //    {
    //        return (DWG_SwapAxis.SwapToesAxis ? (__instance.rawInput.prevPos.x * -1) : __instance.rawInput.prevPos.x);
    //    }
    //}

    //[HarmonyPatch(typeof(StickInput), "AugmentedToeAxisVel", MethodType.Getter)]
    //static class AugToeMaxVelX
    //{
    //    [HarmonyPriority(999)]
    //    static float Postfix(float __result, StickInput __instance)
    //    {
    //        return (DWG_SwapAxis.SwapToesAxis ? (__instance.augmentedInput.maxVelLastUpdate.x * -1) : __instance.augmentedInput.maxVelLastUpdate.x);
    //    }
    //}

    //[HarmonyPatch(typeof(StickInput), "ToeAxisVel", MethodType.Getter)]
    //static class ToeMaxVelX
    //{
    //    [HarmonyPriority(999)]
    //    static float Postfix(float __result, StickInput __instance)
    //    {
    //        return (DWG_SwapAxis.SwapToesAxis ? (__instance.rawInput.maxVelLastUpdate.x * -1) : __instance.rawInput.maxVelLastUpdate.x);
    //    }
    //}

    //[HarmonyPatch(typeof(PlayerController), "OnFlipStickUpdate")]
    //static class PlayerController_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static bool Prefix(PlayerController __instance, ref bool p_p_flipDetected, ref bool p_potentialFlip, ref Vector2 p_initialFlipDir, ref int p_p_flipFrameCount, ref int p_p_flipFrameMax, ref float p_toeAxis, ref float p_p_flipVel, ref float p_popVel, ref float p_popDir, ref float p_flip, ref StickInput p_flipStick, ref bool p_releaseBoard, ref bool p_isSettingUp, ref float p_invertVel, ref float p_augmentedAngle, ref bool popRotationDone, ref bool p_forwardLoad, ref float p_flipWindowTimer)
    //    {
    //        if (!p_p_flipDetected)
    //        {
    //            float num = __instance.flipMult * p_flipStick.PopToeSpeed;
    //            if (num > __instance.flipThreshold)
    //            {
    //                float num2 = Vector3.Angle(p_flipStick.PopToeVector, Vector2.up);
    //                if (num2 < 150f && num2 > 15f && p_flipStick.PopToeVector.magnitude > __instance.flipStickDeadZone && Vector2.Angle(p_flipStick.PopToeVel, p_flipStick.PopToeVector - Vector2.zero) < 90f)
    //                {
    //                    //if (PlayerController.Instance.inputController.LeftStick.FlipDir
    //                    //{

    //                    //} PlayerController.Instance.inputController.
    //                    p_initialFlipDir = p_flipStick.PopToeVector;
    //                    p_toeAxis = p_flipStick.FlipDir;
    //                    p_flip = p_flipStick.ToeAxis;// ( ? p_flipStick.ToeAxis : -p_flipStick.ToeAxis); //PlayerController.Instance.IsSwitch PlayerController.Instance.inputController.RightStick.IsPopStick
    //                    float num3 = p_flipStick.ForwardDir;
    //                    if (num3 <= 0.2f)
    //                    {
    //                        num3 += 0.2f;
    //                    }
    //                    p_popDir = Mathf.Clamp(num3, 0f, 1f);
    //                    p_p_flipVel = num;
    //                    p_flipWindowTimer = 0f;
    //                    p_p_flipDetected = true;
    //                    __instance.playerSM.PoppedSM();
    //                    return false;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            //float p_value = (p_flip == 0f) ? 0f : (PlayerController.Instance.IsSwitch ? 1f : -1f);//((p_flip > 0f) ? 1f : -1f);
    //            float p_value = (p_flip == 0f) ? 0f : ((p_flip > 0f) ? 1f : -1f);
    //            __instance.AnimSetFlip(p_value);
    //            __instance.AnimRelease(true);
    //            if (__instance.playerSM.PoppedSM())
    //            {
    //                __instance.SetLeftIKLerpTarget(1f);
    //                __instance.SetRightIKLerpTarget(1f);
    //            }
    //            float num4 = (p_toeAxis == 0f) ? 0f : ((p_toeAxis > 0f) ? 1f : -1f);
    //            float num5 = __instance.boneMult * p_flipStick.PopToeVel.y;
    //            float num6 = (float)(p_forwardLoad ? -1 : 1);
    //            bool flag = false;
    //            float num7 = __instance.flipMult * p_flipStick.PopToeSpeed;
    //            if ((Mathf.Sign(p_flipStick.ToeAxisVel) == Mathf.Sign(p_flip) || !__instance.playerSM.PoppedSM()) && Mathf.Abs(num7) > Mathf.Abs(p_p_flipVel))
    //            {
    //                p_p_flipVel = num7;
    //                p_flipWindowTimer = 0f;
    //            }
    //            if (Mathf.Abs(p_invertVel) == 0f || (Mathf.Sign(num5) == Mathf.Sign(1f) && Mathf.Abs(num5) > Mathf.Abs(p_invertVel)))
    //            {
    //                p_invertVel = num6 * num5;
    //            }
    //            if (!flag && !__instance.playerSM.PoppedSM())
    //            {
    //                p_flipWindowTimer += Time.deltaTime;
    //                if (p_flipWindowTimer >= 0.3f)
    //                {
    //                    p_p_flipVel = 0f;
    //                    p_invertVel = 0f;
    //                    p_p_flipDetected = false;
    //                    __instance.AnimRelease(false);
    //                    __instance.AnimSetFlip(0f);
    //                    __instance.AnimForceFlipValue(0f);
    //                    p_flipWindowTimer = 0f;
    //                }
    //            }
    //            __instance.SetFlipSpeed(Mathf.Clamp(p_p_flipVel, -4000f, 4000f) * num4);
    //        }
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(PlayerController), "SetBackFootAxis", new Type[] { typeof(float), typeof(float) })]
    //static class SetBackFootAxis_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static bool Prefix(ref float ____backFootForwardAxis, ref float ____backFootToeAxis, ref float p_backForwardAxis, ref float p_backToeAxis)
    //    {
    //        ____backFootForwardAxis = p_backForwardAxis;
    //        ____backFootToeAxis = (DWG_SwapAxis.SwapToesAxis ? p_backToeAxis * -1 : p_backToeAxis);

    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(PlayerController), "SetFrontFootAxis", new Type[] { typeof(float), typeof(float) })]
    //static class SetFrontFootAxis_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static bool Prefix(ref float ____frontFootForwardAxis, ref float ____frontFootToeAxis, ref float p_frontForwardAxis, ref float p_frontToeAxis)
    //    {
    //        ____frontFootForwardAxis = p_frontForwardAxis;
    //        ____frontFootToeAxis = (DWG_SwapAxis.SwapToesAxis ? p_frontToeAxis * -1 : p_frontToeAxis);

    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(PlayerState_Released), "OnStickUpdate", new Type[] { typeof(StickInput), typeof(StickInput) })]
    //static class OnStickUpdate_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static bool Prefix(ref StickInput p_leftStick, ref StickInput p_rightStick, ref float ____lMagnitude, ref float ____rMagnitude)
    //    {
    //        ____lMagnitude = (DWG_SwapAxis.SwapToesAxis ? p_leftStick.rawInput.pos.magnitude * 1 : p_leftStick.rawInput.pos.magnitude);
    //        ____rMagnitude = (DWG_SwapAxis.SwapToesAxis ? p_rightStick.rawInput.pos.magnitude * 1 : p_rightStick.rawInput.pos.magnitude);
    //        PlayerController.Instance.SetLeftIKOffset(p_leftStick.ToeAxis, p_leftStick.ForwardDir, p_leftStick.PopDir, p_leftStick.IsPopStick, true, PlayerController.Instance.GetAnimReleased());
    //        PlayerController.Instance.SetRightIKOffset(p_rightStick.ToeAxis, p_rightStick.ForwardDir, p_rightStick.PopDir, p_rightStick.IsPopStick, true, PlayerController.Instance.GetAnimReleased());

    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(PlayerState_Released), "OnStickFixedUpdate", new Type[] { typeof(StickInput), typeof(StickInput) })]
    //static class OnStickFixedUpdate_Patch
    //{
    //    [HarmonyPriority(999)]
    //    static bool Prefix(ref StickInput p_leftStick, ref StickInput p_rightStick, ref float ____leftToeAxis, ref float ____rightToeAxis, ref float ____leftForwardDir, ref float ____rightForwardDir, ref bool ____caughtLeft, ref bool ____caughtRight)
    //    {
    //        ____leftToeAxis = (____caughtLeft ? p_leftStick.ToeAxis : 0f);
    //        ____rightToeAxis = (____caughtRight ? p_rightStick.ToeAxis : 0f);
    //        ____leftForwardDir = (____caughtLeft ? p_leftStick.ForwardDir : 0f);
    //        ____rightForwardDir = (____caughtRight ? p_rightStick.ForwardDir : 0f);

    //        PlayerController.Instance.SetFrontPivotRotation(____rightToeAxis);
    //        PlayerController.Instance.SetBackPivotRotation(____leftToeAxis);
    //        PlayerController.Instance.SetPivotForwardRotation((____leftForwardDir + ____rightForwardDir) * 0.7f, 15f);
    //        PlayerController.Instance.SetPivotSideRotation(____leftToeAxis - ____rightToeAxis);

    //        //PlayerController.Instance.SetFrontPivotRotation(____leftToeAxis);
    //        //PlayerController.Instance.SetBackPivotRotation(-____rightToeAxis);
    //        //PlayerController.Instance.SetPivotForwardRotation((p_leftStick.ForwardDir + p_rightStick.ForwardDir) * 0.7f, 15f);
    //        //PlayerController.Instance.SetPivotSideRotation(____leftToeAxis - ____rightToeAxis);

    //        return false;
    //    }
    //}
}
