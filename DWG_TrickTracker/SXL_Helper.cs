using UnityEngine;
using System;

namespace DWG_TT
{
    class SXLH
    {
        static public Vector3 CamEul { get { return PlayerController.Instance.cameraController.transform.rotation.eulerAngles; } }
        static public Quaternion CamRot { get { return PlayerController.Instance.cameraController.transform.rotation; } }

        static public string CrntState { get { return PlayerController.Instance.playerSM.ActiveStateTreeString(); } }

        public const string Bailed = "Bailed";
        public const string BeginPop = "BeginPop";
        public const string Braking = "Braking";
        public const string Grinding = "Grinding";
        public const string Impact = "Impact";
        public const string InAir = "InAir";
        public const string Manualling = "Manualling";
        public const string Pop = "Pop";
        public const string Pushing = "Pushing";
        public const string Released = "Released";
        public const string Riding = "Riding";
        public const string Setup = "Setup";

        static public bool IsReg { get { return SettingsManager.Instance.stance == SettingsManager.Stance.Regular; } }
        static public bool IsGoofy { get { return SettingsManager.Instance.stance == SettingsManager.Stance.Goofy; } }
        static public bool IsSwitch { get { return PlayerController.Instance.IsSwitch; } }
        static public bool IsBrdFwd { get { return !PlayerController.Instance.GetBoardBackwards(); } }

        static public bool FrntSide { get { return (PlayerController.Instance.boardController.triggerManager.grindDetection.grindSide == GrindDetection.GrindSide.Frontside); } }
        static public bool BackSide { get { return (PlayerController.Instance.boardController.triggerManager.grindDetection.grindSide == GrindDetection.GrindSide.Backside); } }
        static public bool LeftPop { get { return PlayerController.Instance.inputController.LeftStick.IsPopStick; ; } }
        static public bool RightPop { get { return PlayerController.Instance.inputController.RightStick.IsPopStick; ; } }

        static public bool AllDown { get { return PlayerController.Instance.boardController.AllDown; } }
        static public bool TwoDown { get { return PlayerController.Instance.TwoWheelsDown(); } }

        static public Vector3 SktrEul { get { return PlayerController.Instance.skaterController.skaterTransform.eulerAngles; } }
        static public Vector3 SktrEulLoc { get { return PlayerController.Instance.skaterController.skaterTransform.localEulerAngles; } }
        static public Vector3 SktrPos { get { return PlayerController.Instance.skaterController.skaterTransform.position; } }

        static public int BrdLayer { get { return PlayerController.Instance.boardController.gameObject.layer; } }
        static public float BrdSpeed { get { return PlayerController.Instance.boardController.boardRigidbody.velocity.magnitude; } }
        static public Vector3 BrdEul { get { return PlayerController.Instance.boardController.boardTransform.eulerAngles; } }
        static public Vector3 BrdEulLoc { get { return PlayerController.Instance.boardController.boardTransform.localEulerAngles; } }
        static public Vector3 BrdPos { get { return PlayerController.Instance.boardController.boardTransform.position; } }
        static public Vector3 BrdFwd { get { return PlayerController.Instance.boardController.boardTransform.forward; } }
        static public Vector3 BrdRght { get { return PlayerController.Instance.boardController.boardTransform.right; } }
        static public Vector3 BrdUp { get { return PlayerController.Instance.boardController.boardTransform.up; } }
        static public Vector3 BrdTargPos { get { return PlayerController.Instance.boardController.boardTargetPosition.position; } }

        static public bool TrigManIsColl { get { return PlayerController.Instance.boardController.triggerManager.IsColliding; } }
        static public Vector3 GrindSplnPos { get { return PlayerController.Instance.boardController.triggerManager.grindContactSplinePosition.position; } }
        static public Vector3 GrindSplnFwd { get { return PlayerController.Instance.boardController.triggerManager.grindContactSplinePosition.forward; } }
        static public Vector3 GrindSplnRght { get { return PlayerController.Instance.boardController.triggerManager.grindContactSplinePosition.right; } }
        static public Vector3 GrindSplnUp { get { return PlayerController.Instance.boardController.triggerManager.grindContactSplinePosition.up; } }
        static public Vector3 GrindDir { get { return PlayerController.Instance.boardController.triggerManager.grindDirection; } }
        static public Vector3 GrindUp { get { return PlayerController.Instance.boardController.triggerManager.grindUp; } }

        static public bool FlipTrigs { get { return ((!SXLH.IsSwitch && SXLH.IsBrdFwd) || (SXLH.IsSwitch && !SXLH.IsBrdFwd)); } }
        static public Vector3 BrdDirTwk { get { return FlipTrigs ? BrdFwd : -BrdFwd; } }
        static public Vector3 BrdEdgeFwd { get { return (IsBrdFwd && IsReg || !IsBrdFwd && IsGoofy ? BrdFwd : -BrdFwd); } }
    }
}
