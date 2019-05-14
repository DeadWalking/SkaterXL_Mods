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
        private bool lameBool;

        private readonly float ttBorder = 8f;
        private bool guiInit = false;

        GUIContent tricks;
        GUIStyle tricksStyle;
        Vector2 tricksSize;

        GUIContent consoleCont;
        GUIStyle consoleStyle;
        Vector2 conSize;
        private bool conEnable = false;

        static private string trackedTricks = "";
        static private float trackedTime = 0f;
        static private float lastTrickTime = 0f;

        static private string lastState = "";
        static private string crntState = "";

        static public bool leftCaught = false;
        static public bool leftPop = false;
        static public bool leftFrnt = false;

        static public bool rightCaught = false;
        static public bool rightPop = false;
        static public bool rightFrnt = false;

        static public bool isSwitch = false;
        static private bool wasSwitch = false;

        static public Vector3 sktrEul;
        static public Vector3 sktrPos;
        static private float sktrRotLast = 0f;
        static private float sktrRotMax = 0f;
        static private float sktrRot = 0f;

        static public Vector3 brdEul;
        static public Vector3 brdPos;
        static private float brdRotLast = 0f;
        static private float brdRotMax = 0f;
        static private float brdRot = 0f;
        static private float brdHeightMax = 0f;
        static private bool brdFwd = false;

        static private float brdFlipLast = 0f;
        static private float brdFlipMax = 0f;
        static private float brdFlip = 0f;

        static private float brdTwkLast = 0f;
        static private float brdTwkMax = 0f;
        static private float brdTwk = 0f;

        static private bool lateFlip = false;
        static private bool didApex = false;

        static private float grndTime = 0f;

        static private float manTime = 0f;

        static private List<SceneTrick> GuiTricks = new List<SceneTrick>();

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
        }

        private void Update() {
            crntState = PlayerController.Instance.playerSM.ActiveStateTreeString();

            Vector3 chkSktrPos = PlayerController.Instance.skaterController.skaterTransform.position;
            Vector3 chkSktrEul = PlayerController.Instance.skaterController.skaterTransform.localEulerAngles;

            Vector3 chkBrdPos = PlayerController.Instance.boardController.boardTransform.position;
            Vector3 chkBrdEul = PlayerController.Instance.boardController.boardTransform.localEulerAngles;

            if (lastState != crntState)
            {
                switch (crntState)
                {
                    case "Riding":              // AddTrick("Riding");
                        break;

                    case "Impact":              // AddTrick("Impact");
                        CheckRot();
                        break;

                    case "Manualling":          // AddTrick("Manualling");
                        CheckRot();
                        ResetRots();
                        manTime = Time.time;
                        break;

                    case "Setup":               // AddTrick("Setup");
                        //ResetRots();
                        break;

                    case "BeginPop":            // AddTrick("BeginPop");
                        ResetRots();
                        if (lastState == "Pushing") { AddTrick("NoComply"); };
                        break;

                    case "Pop":                 // AddTrick("Pop");
                        break;

                    case "Released":            // AddTrick("Released");
                        if (didApex) { lateFlip = true; }
                        break;

                    case "InAir":               // AddTrick("InAir");
                        switch (lastState)
                        {
                            case "Riding":
                            case "Pushing":
                            case "Setup":
                            case "Manualling":
                            case "Grinding":
                                ResetRots();
                                break;
                        };
                        break;

                    case "Grinding":            // AddTrick("Grinding");
                        CheckRot();
                        ResetRots();
                        grndTime = Time.time;
                        break;

                    case "Bailed":              // AddTrick("Bailed");
                        if ((Time.time - lastTrickTime) <= 0.5f) { AddTrick("Bailed"); };
                        //trackedTricks = "";
                        break;

                    case "Pushing":             // AddTrick("Pushing");
                        //trackedTricks = "";
                        break;

                    case "Braking":             // AddTrick("Braking");
                        trackedTricks = "";
                        break;
                };

                lastState = crntState;
            };

            switch (crntState)
            {
                //case "Impact":
                //    CheckRot();
                //    break;
                case "Manualling":
                case "Grinding":
                    if (crntState == "Manualling" || crntState == "Grinding") { trackedTime = Time.time; };
                    break;
            };
            UpdateRots(chkSktrPos, chkSktrEul, chkBrdPos, chkBrdEul);
            if ((Time.time - trackedTime) > Main.settings.get_Timer) { trackedTricks = ""; };
            CheckButtons();
        }

        private void OnGUI()
        {
            if (Main.settings.do_TrackTricks)
            {
                if (!guiInit)
                {
                    GUI.backgroundColor = Color.black;
                    guiInit = true;
                    consoleCont = new GUIContent("");
                    consoleStyle = GUI.skin.box;
                    tricks = new GUIContent("");
                    tricksStyle = GUI.skin.box;
                }

                if (conEnable)
                {

                    consoleStyle.alignment = TextAnchor.UpperLeft;
                    consoleCont.text = (
                                        "SktrEul:          " + sktrEul + "\n" +
                                        "BrdEul:           " + brdEul + "\n" +
                                        "SktrRot:          " + sktrRot + "\n" +
                                        "BrdRot:           " + brdRot + "\n" +
                                        "BrdFlip:          " + brdFlip + "\n" +
                                        "Accel:            " + PlayerController.Instance.boardController.acceleration + "\n" +
                                        "FirstVel:         " + PlayerController.Instance.boardController.firstVel + "\n" +
                                        "SecVel:           " + PlayerController.Instance.boardController.secondVel + "\n" +
                                        "ThirdVel:         " + PlayerController.Instance.boardController.thirdVel + "\n" +
                                        "\n" +
                                        PlayerController.Instance.playerSM.ActiveStateTreeString() + "\n" +
                                        "IsBrdBackwards:   " + PlayerController.Instance.boardController.IsBoardBackwards + "\n" +
                                        "IsSwitch:         " + PlayerController.Instance.IsSwitch + "\n" +
                                        "PopStick:         " + PlayerController.Instance.inputController.RightStick.IsPopStick
                                        );

                    conSize = consoleStyle.CalcSize(consoleCont);
                    GUI.Label(new Rect(4, 4, (Screen.width / 5), conSize.y), consoleCont, consoleStyle);
                };

                if (trackedTricks.Length == 0) { return; };

                tricksStyle.alignment = TextAnchor.MiddleRight;
                tricks.text = trackedTricks;
                tricksSize = tricksStyle.CalcSize(tricks);
                GUI.Label(new Rect(((Screen.width - ttBorder) - tricksSize.x), ((Screen.height - ttBorder) - tricksSize.y), tricksSize.x, tricksSize.y), tricks, tricksStyle);
            };
        }
        //private void TrackerRender(int p_windowID) { }

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

        static private void SetupKeyNames()
        {
            labelInfo = "    (Ctrl+" + keyNames[ttTimerD] + ") Lower Timer 0.25(s)";
            labelInfoR = "    (Ctrl+" + keyNames[ttTimerU] + ") Raise Timer 0.25(s)";
            titleWithKey = "(Ctrl + " + keyNames[ttEnable] + ") " + trackerTitle + " Toggle";
            trackerGrow = "(Ctrl+" + keyNames[ttAxis] + ") Extend " + (Main.settings.grow_Vertical ? "Vertical" : "Horizontal");
            tricksAtLanding = "(Ctrl+" + keyNames[ttLanding] + ") Track Tricks at Landing Point";
            lableEnd = "------------------------------------------------------";
        }

        static public void ResetRots()
        {
            brdFwd = !PlayerController.Instance.boardController.IsBoardBackwards;

            wasSwitch = PlayerController.Instance.IsSwitch;

            leftFrnt = PlayerController.Instance.inputController.LeftStick.IsFrontFoot;
            leftPop = PlayerController.Instance.inputController.LeftStick.IsPopStick;

            rightFrnt = PlayerController.Instance.inputController.RightStick.IsFrontFoot;
            rightPop = PlayerController.Instance.inputController.RightStick.IsPopStick;

            sktrRotLast = 0f;
            sktrRotMax = 0f;
            sktrRot = 0f;

            brdRotLast = 0f;
            brdRotMax = 0f;
            brdRot = 0f;
            brdHeightMax = 0f;

            brdFlipLast = 0f;
            brdFlipMax = 0f;
            brdFlip = 0f;

            brdTwkLast = 0f;
            brdTwkMax = 0f;
            brdTwk = 0f;

            didApex = false;
            lateFlip = false;
        }

        static public void UpdateRots(Vector3 p_chkSktrPos, Vector3 p_chkSktrEul, Vector3 p_chkBrdPos, Vector3 p_chkBrdEul)
        {
            float absZRot = Mathf.Abs(Mathd.AngleBetween(brdEul.x, p_chkBrdEul.x));
            Vector3 tmpSktrEul = p_chkSktrEul;
            Vector3 tmpBrdEul = p_chkBrdEul;
            if (absZRot >= 45)
            {
                tmpSktrEul.y = p_chkSktrEul.z;
                tmpSktrEul.z = p_chkSktrEul.y;
                tmpBrdEul.y = p_chkBrdEul.z;
                tmpBrdEul.z = p_chkBrdEul.y;
            }

            sktrRot += Mathd.AngleBetween(tmpSktrEul.y, sktrEul.y);
            sktrRotMax = ((Mathf.Abs(sktrRotMax) < Mathf.Abs(sktrRot)) ? sktrRot : sktrRotMax);

            sktrPos = p_chkSktrPos;
            sktrEul = tmpSktrEul;


            if (p_chkBrdPos.y < brdPos.y)
            {
                didApex = true;
                brdHeightMax = brdPos.y;
            }

            brdTwk += Mathd.AngleBetween(tmpBrdEul.x, brdEul.x);
            brdTwkMax = ((Mathf.Abs(brdTwkMax) < Mathf.Abs(brdFlip)) ? brdFlip : brdTwkMax);
            brdRot += Mathd.AngleBetween(tmpBrdEul.y, brdEul.y);
            brdRotMax = ((Mathf.Abs(brdRotMax) < Mathf.Abs(brdRot)) ? brdRot : brdRotMax);
            brdFlip += Mathd.AngleBetween(tmpBrdEul.z, brdEul.z);
            brdFlipMax = ((Mathf.Abs(brdFlipMax) < Mathf.Abs(brdFlip)) ? brdFlip : brdFlipMax);

            brdPos = p_chkBrdPos;
            brdEul = tmpBrdEul;
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

        private void CheckButtons()
        {
            if (Main.enabled && Input.GetKey(KeyCode.LeftControl))
            {
                ModMenu.Instance.KeyPress(KeyCode.BackQuote, 0.2f, () => { conEnable = !conEnable; });
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
            if ((crntState != "Grinding") && (crntState != "Manualling") && (Time.time - lastTrickTime) < 0.25f) { return; };

            // When the board and skater look like they are rotating the same direction on the Unity world Y axis.
            // Their local euler y axis rotations are opposite of one another.
            if (wasSwitch) { brdRot *= -1; } else { sktrRot *= -1; };
            // When the board has been rotated 180 the kick side becomes the heel side.
            if (!brdFwd) { brdFlip *= -1; };
            // Still seems to be some odd behavior when using a Vert. Sometimes, only sometimes, the rotation seems less than expected, and/or it misses a kick/heel flip.
            // I have a feeling it has to do with the board angle rotating over 45 degrees on the local z axis. At that point the rotation addition stops being on the x axis
            // and I need to check for that and apply math accordingly.


            string trickPrefix = ((!rightPop && wasSwitch) ? "Switch" : (!rightPop && !wasSwitch ? "Nollie" : (rightPop && wasSwitch ? "Fakie" : "")));

            float clampSRot = ClampRot(sktrRot);
            float clampBRot = ClampRot(brdRot);
            float clampBFlip = ClampRot(brdFlip);
            float clampBrdTwk = ClampRot(brdTwk);

            string sktrDir = ((clampSRot != 0f) ? ((sktrRot < 0f) ? "Bs" : "Fs") : "");
            string brdDir = ((clampBRot != 0f) ? ((brdRot < 0f) ? "Bs" : "Fs") : "");

            bool sameDir = ((brdDir.Length > 0) && (brdDir == sktrDir));
            string sktrBrdDir = ((sameDir || (clampSRot == 0f)) ? "" : " BVar " + clampSRot);

            bool kick = ((brdFlip < 0f) && (clampBFlip >= 360f));
            bool heel = ((brdFlip > 0f) && (clampBFlip >= 360f));

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

            bool hardFlip = (Mathf.Abs(brdTwkMax) >= 30f);

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


            if (trickName.Length > 0)
            {
                AddTrick(((trickPrefix.Length > 0) ? trickPrefix + " " : "") + trickName);
            };

             bool crntSwitch = PlayerController.Instance.IsSwitch;
            // Plans for Calculating rotations/tweaks while grinding.
            if (crntState == "Grinding" && lastState != "Grinding"/*((Time.time - grndTime) > 0.25f)*/)
            {
                AddTrick((crntSwitch ? "Switch " : "") + PlayerController.Instance.boardController.triggerManager.grindDetection.grindType.ToString());
            };

            // Plans for Calculating a couple Manual specific tricks.
            if (crntState == "Manualling" && ((Time.time - manTime) > 0.1f))
            {
                bool crntRightPop = PlayerController.Instance.inputController.RightStick.IsPopStick;
                AddTrick(((!crntRightPop && crntSwitch) ? "Switch " : (!crntRightPop && !crntSwitch ? "Nose " : (crntRightPop && crntSwitch ? "Fakie " : ""))) + "Manual");
            };

            lastTrickTime = Time.time;
            ResetRots();
        }

        static public void AddTrick(string p_newTrick)
        {
            if (Main.settings.at_TrickLanding) {
                GameObject tmpObj = new GameObject();
                SceneTrick newTrick = tmpObj.AddComponent<SceneTrick>();
                newTrick.Init(false, p_newTrick);

                GuiTricks.Add(newTrick);
            };

            trackedTime = Time.time;
            trackedTricks += (((trackedTricks.Length > 0) ? (Main.settings.grow_Vertical ? " \n " : " + ") : "") + p_newTrick);
        }
    }
}
