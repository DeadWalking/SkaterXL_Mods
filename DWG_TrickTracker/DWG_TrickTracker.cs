using UnityEngine;
using XLShredLib;
using XLShredLib.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DWG_TT
{
    class TT : MonoBehaviour {
        ModUIBox modMenuBox;
        ModUILabel modLabelTrack;
        ModUILabel modLabelAxis;
        ModUILabel modLabelOnLand;

        static private Dictionary<KeyCode, string> keyNames = new Dictionary<KeyCode, string>()
        {
            { KeyCode.Alpha1 , "1" },
            { KeyCode.Alpha2 , "2" },
            { KeyCode.Alpha3 , "3" },
            { KeyCode.Alpha4 , "4" },
            { KeyCode.Alpha5 , "5" },
        };

        static private KeyCode ttTimerD = KeyCode.Alpha1;
        static private KeyCode ttTimerU = KeyCode.Alpha2;
        static private KeyCode ttEnable = KeyCode.Alpha3;
        static private KeyCode ttAxis = KeyCode.Alpha4;
        static private KeyCode ttLanding = KeyCode.Alpha5;

        static private readonly string trackerTitle = "Trick Tracker";
        static private string titleWithKey = "";
        static private string labelInfo = "";
        static private string labelInfoR = "";
        static private string trackerGrow = "";
        static private string tricksAtLanding = "";
        static private string lableEnd = "";
        private bool growBool = true;
        private bool lameBool;

        private readonly float ttEdge = 4f;
        private readonly float ttBorder = 8f;
        private bool initTT = false;

        Rect trackerRect;
        GUIContent tricks;
        GUIStyle tricksStyle;
        Vector2 tricksPos;
        Vector2 tricksSize;

        public enum TrickState
        {
            Ride,
            Man,
            Grnd,
            Air,
            Bail,
            Spawn
        }

        static private TrickState crntState;
        static public TrickState CrntState
        {
            get { return crntState; }
            set { crntState = value; }
        }
        static private TrickState prevState;
        static public TrickState PrevState
        {
            get { return prevState; }
            set { prevState = value; }
        }

        static private string prevTrick;
        static public string PrevTrick
        {
            get { return prevTrick; }
            set { prevTrick = value; }
        }

        static private string trackedTricks;
        static public string TrackedTricks
        {
            get { return trackedTricks; }
            set { trackedTricks = value; }
        }

        static private float trackedTime;
        static public float TrackedTime
        {
            get { return trackedTime; }
            set { trackedTime = value; }
        }

        static private bool caughtLeft;
        static public bool CaughtLeft
        {
            get { return caughtLeft; }
            set { caughtLeft = value; }
        }

        static private bool caughtRight;
        static public bool CaughtRight
        {
            get { return caughtRight; }
            set { caughtRight = value; }
        }

        static private bool stickRight;
        static public bool StickRight
        {
            get { return stickRight; }
            set { stickRight = value; }
        }

        static private bool isSwitch;
        static public bool IsSwitch
        {
            get { return isSwitch; }
            set { isSwitch = value; }
        }

        static private bool wasSwitch;
        static public bool WasSwitch
        {
            get { return wasSwitch; }
            set { wasSwitch = value; }
        }

        static private float lastSktrRot;
        static private float maxSktrRot;
        static public float MaxSktrRot
        {
            get { return maxSktrRot; }
            set { maxSktrRot = value; }
        }
        static private float sktrRot;
        static public float SktrRot
        {
            get { return sktrRot; }
            set { sktrRot = value; }
        }

        static private float lastBrdRot;
        static private float maxBrdRot;
        static public float MaxBrdRot
        {
            get { return maxBrdRot; }
            set { maxBrdRot = value; }
        }
        static private float brdRot;
        static public float BrdRot
        {
            get { return brdRot; }
            set { brdRot = value; }
        }

        static private float lastBrdFlip;
        static private float maxBrdFlip;
        static public float MaxBrdFlip
        {
            get { return maxBrdFlip; }
            set { maxBrdFlip = value; }
        }
       static private float brdFlip;
        static public float BrdFlip
        {
            get { return brdFlip; }
            set { brdFlip = value; }
        }

        static private float lastBrdTwk;
        static private float maxBrdTwk;
        static public float MaxBrdTwk
        {
            get { return maxBrdTwk; }
            set { maxBrdTwk = value; }
        }
        static private float brdTwk;
        static public float BrdTwk
        {
            get { return brdTwk; }
            set { brdTwk = value; }
        }

        static private bool lateFlip;
        static public bool LateFlip
        {
            get { return lateFlip; }
            set { lateFlip = value; }
        }

        static private bool didApex;
        static public bool DidApex
        {
            get { return didApex; }
            set { didApex = value; }
        }

        static private float grndTime;

        static private float manTime;

        private SceneTrick mainTracker;
        static private List<SceneTrick> GuiTricks = new List<SceneTrick>();

        static private void SetupKeyNames()
        {
            labelInfo = "    (Ctrl+" + keyNames[ttTimerD] +  ") Lower Timer 0.25(s)";
            labelInfoR = "    (Ctrl+" + keyNames[ttTimerU] + ") Raise Timer 0.25(s)";
            titleWithKey = "(Ctrl + " + keyNames[ttEnable] + ") " + trackerTitle + " Toggle";
            trackerGrow = "(Ctrl+" + keyNames[ttAxis] + ") Extend " + (Main.settings.grow_Vertical ? "Vertical" : "Horizontal");
            tricksAtLanding = "(Ctrl+" + keyNames[ttLanding] + ") Track Tricks at Landing Point";
            lableEnd = "------------------------------------------------------";
        }

        static public void UpdateRots()
        {
           lastSktrRot = sktrRot;
            maxSktrRot = 0f;
            sktrRot = 0f;

            lastBrdRot = brdRot;
            maxBrdRot = 0f;
            brdRot = 0f;

            lastBrdFlip = brdFlip;
            maxBrdFlip = 0f;
            brdFlip = 0f;

            lastBrdTwk = brdTwk;
            maxBrdTwk = 0f;
            brdTwk = 0f;
        }

        static public void UpdateBools()
        {
            //caughtLeft = false;
            //caughtRight = false;

            //stickRight = false;
            //wasSwitch = isSwitch;
            //isSwitch = false;

            //lateFlip = false;
            //didApex = false;
        }

        private void ResetVars()
        {
            crntState = TrickState.Ride;
            prevState = TrickState.Ride;
            prevTrick = "";

            trackedTricks = "";
            trackedTime = Time.time;

            caughtLeft = false;
            caughtRight = false;
            stickRight = false;

            wasSwitch = isSwitch;
            isSwitch = false;

            lateFlip = false;
            didApex = false;

            grndTime = Time.time;
            manTime = Time.time;

            UpdateRots();
        }

        private void SetupMainTracker()
        {
            //if (Main.settings.do_TrackTricks && !mainTracker)
            //{
            //    GameObject tmpObj = new GameObject();
            //    mainTracker = tmpObj.AddComponent<SceneTrick>();
            //    mainTracker.Init(true, "");
            //}
        }

        private void Start() {
            SetupKeyNames();
            modMenuBox = ModMenu.Instance.RegisterModMaker("dwg", "DeadWalking");
                            modMenuBox.AddLabel("top-lineL",        LabelType.Text,     lableEnd,       Side.left,  () => Main.enabled, true,                           (b) => lameBool = b,                     99);
                            modMenuBox.AddLabel("top-lineR",        LabelType.Text,     lableEnd,       Side.right, () => Main.enabled, true,                           (b) => lameBool = b,                     99);
                            modMenuBox.AddLabel("info-L",           LabelType.Text,     labelInfo,      Side.left,  () => Main.enabled, true,                           (b) => lameBool = b,                     98);
                            modMenuBox.AddLabel("info-R",           LabelType.Text,     labelInfoR,     Side.right, () => Main.enabled, true,                           (b) => lameBool = b,                     98);
            modLabelTrack = modMenuBox.AddLabel("do-trackTricks",   LabelType.Toggle,   titleWithKey,   Side.left,  () => Main.enabled, Main.settings.do_TrackTricks,   (b) => Main.settings.do_TrackTricks = b, 97);
            modLabelAxis =  modMenuBox.AddLabel("grow-vertical",    LabelType.Toggle,   trackerGrow,    Side.right, () => Main.enabled, Main.settings.grow_Vertical,    (b) => lameBool = b,                                97);
                            modMenuBox.AddLabel("mid-lineL",        LabelType.Text,     lableEnd,       Side.left,  () => Main.enabled, true,                           (b) => lameBool = b,                     96);
                            modMenuBox.AddLabel("mid-lineR",        LabelType.Text,     lableEnd,       Side.right, () => Main.enabled, true,                           (b) => lameBool = b,                     96);
            modLabelOnLand= modMenuBox.AddLabel("at-trickLanding",  LabelType.Toggle,   tricksAtLanding,Side.left,  () => Main.enabled, Main.settings.at_TrickLanding,  (b) => Main.settings.at_TrickLanding = b,95);
                            modMenuBox.AddLabel("filler-A",         LabelType.Text,     "Experimental", Side.right, () => Main.enabled, true,                           (b) => lameBool = b,                     95);
                            modMenuBox.AddLabel("end-lineL",        LabelType.Text,     lableEnd,       Side.left,  () => Main.enabled, true,                           (b) => lameBool = b,                     94);
                            modMenuBox.AddLabel("end-lineR",        LabelType.Text,     lableEnd,       Side.right, () => Main.enabled, true,                           (b) => lameBool = b,                     94);

            ResetVars();
            //SetupMainTracker();
        }

        // Playing aruond with manipulating the XLshredMenu Toggle
        //private Action<bool> SetGrow(object b)
        //{
        //    return new Action<bool>(TrueGrow);
        //}
        //private void TrueGrow(bool inBool) { Main.settings.grow_Vertical = !Main.settings.grow_Vertical; modLabelTrack.SetToggleValue(true); }

        private void Update() {
            if (Main.enabled && Input.GetKey(KeyCode.LeftControl)) {
                ModMenu.Instance.KeyPress(ttEnable, 0.2f, () =>
                {
                    Main.settings.do_TrackTricks = !Main.settings.do_TrackTricks;
                    modLabelTrack.SetToggleValue(Main.settings.do_TrackTricks);
                    ModMenu.Instance.ShowMessage(trackerTitle + (Main.settings.do_TrackTricks ? ": Enabled" : ": Disabled"));
                });

                ModMenu.Instance.KeyPress(ttAxis, 0.2f, () =>
                {
                    Main.settings.grow_Vertical = !Main.settings.grow_Vertical;
                    modLabelAxis.text = "(Ctrl+" + keyNames[ttAxis] + ") Extend " + (Main.settings.grow_Vertical ? "Vertical" : "Horizontal");
                    modLabelTrack.SetToggleValue(true);
                    ModMenu.Instance.ShowMessage(trackerTitle + " Extend: " + (Main.settings.grow_Vertical ? "Vertical" : "Horizontal"));
                });

                ModMenu.Instance.KeyPress(ttLanding, 0.2f, () =>
                {
                    Main.settings.at_TrickLanding = !Main.settings.at_TrickLanding;
                    modLabelOnLand.SetToggleValue(Main.settings.at_TrickLanding);
                    ModMenu.Instance.ShowMessage(trackerTitle + " Place Tricks at Contact" + (Main.settings.at_TrickLanding ? ": Enabled" : ": Disabled"));
                });

                float increment = 0.0f;
                ModMenu.Instance.KeyPress(ttTimerD, 0.2f, () =>
                {
                    increment = -0.25f;
                });
                ModMenu.Instance.KeyPress(ttTimerU, 0.2f, () =>
                {
                    increment = 0.25f;
                });
                if (increment != 0f)
                {
                    Main.settings.get_Timer = Main.settings.get_Timer + increment;
                    increment = 0.0f;
                    ModMenu.Instance.ShowMessage(trackerTitle + " Reset Time (s): " + Main.settings.get_Timer);
                };
            }
        }

        private void OnGUI()
        {
            // Playing aruond with manipulating the XLshredMenu Toggle
            //if (!Main.settings.grow_Vertical) { modLabelTrack.SetToggleValue(true); };
            if ((Time.time - TrackedTime) > Main.settings.get_Timer) { TrackedTricks = ""; };

            if (Main.settings.do_TrackTricks)
            {
                //if (mainTracker) { mainTracker.TextMeshData.text = TrackedTricks; };

                if (TrackedTricks.Length < 1) { return; };

                GUI.backgroundColor = Color.black;
                tricks = new GUIContent(TrackedTricks);
                tricksStyle = GUI.skin.box;
                tricksStyle.alignment = TextAnchor.MiddleRight;
                tricksSize = tricksStyle.CalcSize(tricks);

                GUI.Label(new Rect(((Screen.width - ttBorder) - tricksSize.x), ((Screen.height - ttBorder) - tricksSize.y), tricksSize.x, tricksSize.y), tricks, tricksStyle);
            };
        }
        private void TrackerRender(int p_windowID) { }

        private void OnDestroy() {
            modMenuBox.RemoveLabel("top-lineL");
            modMenuBox.RemoveLabel("top-lineR");
            modMenuBox.RemoveLabel("info-L");
            modMenuBox.RemoveLabel("info-R");
            modMenuBox.RemoveLabel("do-trackTricks");
            modMenuBox.RemoveLabel("grow-vertical");
            modMenuBox.RemoveLabel("mid-lineL");
            modMenuBox.RemoveLabel("mid-lineR");
            modMenuBox.RemoveLabel("at-trickLanding");
            modMenuBox.RemoveLabel("filler-A");
            modMenuBox.RemoveLabel("end-lineL");
            modMenuBox.RemoveLabel("end-lineR");
        }

        static public Tuple<Vector3, Quaternion, Vector3> GetWorldTransform()
        {
            // 2M to the right and 2m forward from the camera pos
            //Vector3 placePos = (PlayerController.Instance.cameraController.transform.position + PlayerController.Instance.cameraController.transform.right * 2);
            //placePos = (placePos + PlayerController.Instance.cameraController.transform.forward * 2);

            // Board estimated contact pos
            Vector3 placePos = (PlayerController.Instance.boardController.boardTargetPosition.position);
            Quaternion placeRot = PlayerController.Instance.cameraController.transform.rotation;
            UnityEngine.
            //dist = Vector3.Distance(cameraPos, textPos);
            Vector3 placeScale = new Vector3(1f, 1f, 1f);

            return Tuple.Create(placePos, placeRot, placeScale);
        }

        static private float ClampRot(float p_inRot)
        {
            float outRot = Mathf.Abs(p_inRot);
            switch (outRot)
            {
                case var _ when outRot >= 1390f:
                    outRot = 1440f;
                    break;
                case var _ when outRot >= 1210f:
                    outRot = 1260f;
                    break;
                case var _ when outRot >= 1030f:
                    outRot = 1080f;
                    break;
                case var _ when outRot >= 850f:
                    outRot = 900f;
                    break;
                case var _ when outRot >= 670f:
                    outRot = 720f;
                    break;
                case var _ when outRot >= 490f:
                    outRot = 540f;
                    break;
                case var _ when outRot >= 310f:
                    outRot = 360f;
                    break;
                case var _ when outRot >= 130f:
                    outRot = 180f;
                    break;
                default:
                    outRot = 0f;
                    break;
            };
            return outRot;
        }

        static private string DTQCheck(float p_inFlip)
        {
            string dtq = "";
            switch (p_inFlip)
            {
                case (1440):
                    dtq = "Quad";
                    break;
                case (1080f):
                    dtq = "Triple";
                    break;
                case (720f):
                    dtq = "Double";
                    break;
                default:
                    break;
            }
            return dtq;
        }

        static public void CheckRot()
        {
            string trickPrefix = ((!StickRight && WasSwitch) ? "Switch" : (!StickRight && !WasSwitch ? "Nollie" : (StickRight && WasSwitch ? "Fakie" : "")));

            float clampSRot = ClampRot(sktrRot);
            float clampBRot = ClampRot(brdRot);
            float clampBFlip = ClampRot(brdFlip);
            float clampBrdTwk = ClampRot(brdTwk);

            string sktrDir = ((clampSRot != 0f) ? ((sktrRot < 0f) ? "Bs" : "Fs") : "");
            string brdDir = ((clampBRot != 0f) ? ((brdRot < 0f) ? "Bs" : "Fs") : "");

            bool sameDir = ((brdDir.Length > 0) && (brdDir == sktrDir));
            string sktrBrdDir = ((sameDir || (clampSRot == 0f)) ? "" : " BVar " + clampSRot);

            bool kick = ((brdFlip > 0f) && (clampBFlip >= 360f));
            bool heel = ((brdFlip < 0f) && (clampBFlip >= 360f));

            bool shuvit = (!sameDir && (clampBRot >= 180f));

            string dtqFlip = DTQCheck(clampBFlip);
            string flipType = "";
            if (kick || heel)
            {
                if (clampBRot == clampBFlip)
                {
                    flipType = (kick ? "Flip" : (heel ? "HeelFlip" : ""));
                }
                else
                {
                    flipType = (kick ? "KickFlip" : (heel ? "HeelFlip" : ""));
                    flipType = (((flipType.Length > 0) && (dtqFlip.Length > 0)) ? dtqFlip + " " + flipType : flipType);
                }
            }

            //AddTrick("FlipType: " + flipType + "  clampBFlip: " + clampBFlip + "  brdFlip: " + brdFlip);

            bool hardFlip = (Mathf.Abs(MaxBrdTwk) >= 30f);

            string trickSpin = ((clampBRot >= 180f) ? brdDir + clampBRot.ToString() + (!sameDir ? sktrBrdDir : "") : "");
            string trickFlip = "";

            // Kick/Heel Flip while BoardRotation > FlipRotation by 180
            if ((clampBRot >= 360f) && ((clampBFlip - clampBRot) >= 180f))
            {
                if (kick)
                {
                    trickFlip = ((brdDir == "Fs") ? ((dtqFlip.Length > 0) ? dtqFlip + " " : "") + "HardFlip" : ((clampBFlip >= 720f) ? "NightmareFlip" : "Varial KickFlip"));
                }
                else if (heel)
                {
                    trickFlip = (((dtqFlip.Length > 0) ? dtqFlip + " " : "") + ((brdDir == "Fs") ? "Varial " : "Inward ") + "HeelFlip");
                };
            };

            // Kick/Heel Flip while BoardRotation == FlipRotation
            if ((clampBRot >= 360f) && (clampBRot == clampBFlip))
            {
                if (kick)
                {
                    trickFlip = (((dtqFlip.Length > 0) ? dtqFlip + " " : "") + ((brdDir == "Fs") ? (hardFlip ? "Hard" : "Tre") + "Flip" : (hardFlip ? "Varial Kick" : "") + "Flip"));
                }
                else if (heel)
                {
                    trickFlip = (((dtqFlip.Length > 0) ? dtqFlip + " " : "") + ((brdDir == "Fs") ? "Laser" : "Inward Heel") + "Flip");
                };
            };

            // Change HardFlips to Gingersnaps if the stance is right
            if (trickFlip.Contains("Hard") && ((trickPrefix == "Switch") || (trickPrefix == "Nollie")))
            {
                trickFlip.Replace("HardFlip", "Gingersnap");
                if ((trickPrefix == "Switch") || (trickPrefix == "Nollie")) { trickPrefix = ""; };
            };

            if (trickFlip.Length == 0 && (kick || heel))
            {
                trickFlip = flipType;
            };

            // Run through a check of known specific Rotation Combonations.
            string trickName = "";
            if (sameDir)
            {
                if (clampBRot == clampSRot)
                {
                    if (trickPrefix == "Fakie")
                    {
                        trickName = ((clampBRot < 360f) ? " HalfCab" : " Caballerial");
                    }
                    else if (trickPrefix == "Nollie" && brdDir == "Bs")
                    {
                        trickName = "HeliPop";
                    };
                }
                else if(clampBRot != clampSRot)
                {
                    if ((clampBRot - clampSRot) == 180f && (clampBRot == clampBFlip))
                    {
                        trickName = ((((clampBRot <= 900f) && (clampBRot == 720f) || (clampBRot == 900f)) ? "Double" : "") + (((clampBRot == 360f) || (clampBRot == 720f)) ? "BigSpin" : "Gazelle"));
                    };
                };
            };

            // Verify Current Checks and build new Trick if none exists.
            if (trickName.Length == 0)
            {
                trickName = (((trickSpin.Length > 0) ? trickSpin + " " : "") + trickFlip);
            }
            else
            {
                trickName = (((trickSpin.Length > 0) ? trickSpin + " " : "") + trickName + (((trickFlip.Length > 0) ? " " + trickFlip : "")));
            };
            //trickPrefix;


            //if (clampSRot > 0f || clampBRot > 0f || clampBFlip > 0f && (knownTitle.Length >= 2)) { AddTrick(knownTitle); };
            if (trickName.Length > 0)
            {
                AddTrick(((trickPrefix.Length > 0) ? trickPrefix + " " : "") + trickName);
            };

            // Plans for Calculating rotations/tweaks while grinding.
            if (CrntState == TrickState.Grnd && ((Time.time - grndTime) > 0.5f)) {
                grndTime = Time.time;
                AddTrick(/*trickPrefix + ((trickPrefix.Length > 0) ? " " : "") + */PlayerController.Instance.boardController.triggerManager.grindDetection.grindType.ToString());
            };

            // Plans for Calculating a couple Manual specific tricks.
            if (CrntState == TrickState.Man && ((Time.time - manTime) > 0.5f)) {
                manTime = Time.time;
                AddTrick(PrevTrick);
            };

            UpdateRots();
        }

        static public void AddTrick(string p_newTrick)
        {
            if (Main.settings.at_TrickLanding) {
                GameObject tmpObj = new GameObject();
                SceneTrick newTrick = tmpObj.AddComponent<SceneTrick>();
                newTrick.Init(false, p_newTrick);

                GuiTricks.Add(newTrick);
            };

            TT.TrackedTime = Time.time;
            TrackedTricks += (((TrackedTricks.Length > 0) ? (Main.settings.grow_Vertical ? " \n " : " + ") : "") + p_newTrick);
        }
    }
}
