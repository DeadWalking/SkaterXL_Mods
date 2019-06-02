using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XLShredLib;
using XLShredLib.UI;

namespace DWG_TT
{
    static class DebugOut
    {
        static private readonly bool debugMe = true;
        static private string debugTitle = "DebugOut";
        static public void SetTitle(string p_newTitle) { debugTitle = p_newTitle; }
        static public void Log(string p_dbgStr) { if (!debugMe) { return; }; Debug.Log("<----> " + debugTitle + " - " + p_dbgStr + " <---->"); }
    }

    class TT : MonoBehaviour
    {
        private GUIM guiMan;
        private PI plyrInfo;
        private GM grndMan;

        private readonly string trackerTitle = "Trick Tracker";

        private ModUIBox modMenuBox;
        private ModUILabel labelTglTrack;
        private ModUILabel labelAtLanding;
        private ModUILabel labelshowGHelp;
        private ModUILabel labelTimerU;
        private ModUILabel labelTimerD;
        private ModUILabel labelVertHorz;
        //private ModUILabel labelCustomTrigs;

        private readonly Dictionary<KeyCode, string> keyNames = new Dictionary<KeyCode, string>()
        {
            { KeyCode.Alpha1 , "1" },
            { KeyCode.Alpha2 , "2" },
            { KeyCode.Alpha3 , "3" },
            { KeyCode.Alpha4 , "4" },
            { KeyCode.Alpha5 , "5" },
            { KeyCode.Alpha6 , "6" },
            { KeyCode.Alpha7 , "7" },
        };

        private readonly KeyCode ttTimerD = KeyCode.Alpha1;
        private readonly KeyCode ttTimerU = KeyCode.Alpha2;
        private readonly KeyCode ttEnable = KeyCode.Alpha3;
        private readonly KeyCode ttAxis = KeyCode.Alpha6;
        private readonly KeyCode ttLanding = KeyCode.Alpha4;
        private readonly KeyCode ttGHelp = KeyCode.Alpha5;
        private readonly KeyCode ttCtrigs = KeyCode.Alpha7;

        private string titleWithKey;
        private string textTimerD;
        private string textTimerU;
        private string trackerGrow;
        private string tricksAtLanding;
        private string showGhelp;
        private string customTrigs;
        private string lableLine;
        private bool lameBool;

        //private float keyDownTime;

        //private List<SceneTrick> GuiTricks = new List<SceneTrick>();

        //static private Assembly FindReplayEditor;
        //Type kiwiReplay;
        //MethodInfo kiwiReplayCamCon;
        //Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
        //foreach (Assembly asm in asms)
        //{
        //    if (asm.FullName.Contains("XLShredReplayEditor")) { FindReplayEditor = asm; };
        //};
        ////FindReplayEditor.EntryPoint.Module.
        //kiwiReplay = Type.GetType("XLShredReplayEditor.ReplayCameraController");
        //kiwiReplayCamCon = kiwiReplay.GetMethod("Camera");
        //object instance = Activator.CreateInstance(kiwiReplay);

        void Awake()
        {
            DebugOut.SetTitle(this.trackerTitle);
            //DebugOut.Log(this.GetType().Name + " Awake");
        }

        void OnEnable()
        {
            //DebugOut.Log(this.GetType().Name + " OnEnable");
            SceneManager.sceneLoaded += this.OnSceneLoaded;
            SceneManager.sceneUnloaded += this.OnSceneUnLoaded;
            this.plyrInfo = new PI();
            this.guiMan = new GUIM();
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            DebugOut.Log(this.GetType().Name + " OnSceneLoaded: Scene: " + scene.name + " Mode: " + mode);
            this.guiMan.GuiReset();
        }

        void OnSceneUnLoaded(Scene scene)
        {
            this.TTReset("OnSceneUnLoaded: Scene: " + scene.name);
        }

        void OnDestroy()
        {
            this.TTReset("OnDestroy");
            SceneManager.sceneLoaded -= this.OnSceneLoaded;
            SceneManager.sceneUnloaded -= this.OnSceneUnLoaded;
            this.modMenuBox.RemoveLabel("strt-lineL");
            this.modMenuBox.RemoveLabel("strt-lineR");
            this.modMenuBox.RemoveLabel("do-trackTricks");
            this.modMenuBox.RemoveLabel("at-trickLanding");
            this.modMenuBox.RemoveLabel("show_gHelp");
            //this.modMenuBox.RemoveLabel("cust-trigs");
            //this.modMenuBox.RemoveLabel("fillerR");
            this.modMenuBox.RemoveLabel("info-TimerD");
            this.modMenuBox.RemoveLabel("info-TimerU");
            this.modMenuBox.RemoveLabel("grow-vertical");
            this.modMenuBox.RemoveLabel("end-lineL");
            this.modMenuBox.RemoveLabel("end-lineR");
        }

        void TTReset(string p_sender)
        {
            DebugOut.Log(this.GetType().Name + " TTReset: " + p_sender);
        }

        void Start()
        {
            //DebugOut.Log(this.GetType().Name + " Start: ");

            this.SetupButtonStrings();

            this.modMenuBox = ModMenu.Instance.RegisterModMaker("dwg", "DeadWalking");

            this.modMenuBox.AddLabel("strt-lineL", LabelType.Text, this.lableLine, Side.left, () => Main.enabled, true, (b) => this.lameBool = b, 99);
            this.modMenuBox.AddLabel("strt-lineR", LabelType.Text, this.lableLine, Side.right, () => Main.enabled, true, (b) => this.lameBool = b, 99);

            this.labelTglTrack = this.modMenuBox.AddLabel("do-trackTricks", LabelType.Toggle, this.titleWithKey, Side.left, () => Main.enabled, Main.settings.do_TrackTricks, (b) => Main.settings.do_TrackTricks = b, 98);
            this.labelAtLanding = this.modMenuBox.AddLabel("at-trickLanding", LabelType.Toggle, this.tricksAtLanding, Side.left, () => Main.enabled, Main.settings.at_TrickLanding, (b) => Main.settings.at_TrickLanding = b, 97);
            this.labelshowGHelp = this.modMenuBox.AddLabel("show_gHelp", LabelType.Toggle, this.showGhelp, Side.left, () => Main.enabled, Main.settings.show_ghelpers, (b) => Main.settings.show_ghelpers = b, 96);
            //this.labelCustomTrigs = this.modMenuBox.AddLabel("cust-trigs", LabelType.Toggle, this.customTrigs, Side.left, () => Main.enabled, Main.settings.use_custTrigs, (b) => Main.settings.use_custTrigs = b, 95);

            this.labelTimerD = this.modMenuBox.AddLabel("info-TimerD", LabelType.Toggle, this.textTimerD, Side.right, () => Main.enabled, true, (b) => this.SetTimerDwn(b), 98);
            this.labelTimerU = this.modMenuBox.AddLabel("info-TimerU", LabelType.Toggle, this.textTimerU, Side.right, () => Main.enabled, true, (b) => this.SetTimerUp(b), 97);
            this.labelVertHorz = this.modMenuBox.AddLabel("grow-vertical", LabelType.Toggle, this.trackerGrow, Side.right, () => Main.enabled, Main.settings.grow_Vertical, (b) => Main.settings.grow_Vertical = b, 96);
            //this.modMenuBox.AddLabel("fillerR", LabelType.Text, "", Side.right, () => Main.enabled, true, (b) => this.lameBool = b, 95);

            this.modMenuBox.AddLabel("end-lineL", LabelType.Text, this.lableLine, Side.left, () => Main.enabled, true, (b) => this.lameBool = b, 50);
            this.modMenuBox.AddLabel("end-lineR", LabelType.Text, this.lableLine, Side.right, () => Main.enabled, true, (b) => this.lameBool = b, 50);

            if (this.plyrInfo == null)
            {
                //DebugOut.Log(this.GetType().Name + " Starting PlayerInfo");
                this.plyrInfo = this.gameObject.AddComponent(typeof(PI)) as PI;
                this.plyrInfo.enabled = true;
            };
            if (this.grndMan == null)
            {
                //DebugOut.Log(this.GetType().Name + " Starting GrindManager");
                this.grndMan = this.gameObject.AddComponent(typeof(GM)) as GM;
                this.grndMan.enabled = true;
            };
            if (this.guiMan == null)
            {
                //DebugOut.Log(this.GetType().Name + " Starting GuiManager");
                this.guiMan = this.gameObject.AddComponent(typeof(GUIM)) as GUIM;
                this.guiMan.enabled = true;
            };
        }

        void LateUpdate()
        {
            this.CheckButtons();
        }

        private void SetupButtonStrings()
        {
            this.textTimerD = "(Ctrl+" + this.keyNames[this.ttTimerD] + ") Lower Timer 0.25(s)";
            this.textTimerU = "(Ctrl+" + this.keyNames[this.ttTimerU] + ") Raise Timer 0.25(s)";
            this.titleWithKey = "(Ctrl+" + this.keyNames[this.ttEnable] + ") " + this.trackerTitle + " Toggle";
            this.trackerGrow = "(Ctrl+" + this.keyNames[this.ttAxis] + ") Extend " + /*(Main.settings.grow_Vertical ? */"Vertical"/* : "Horizontal")*/;
            this.tricksAtLanding = "(Ctrl+" + this.keyNames[this.ttLanding] + ") Display at Landing(Experimental)";
            this.showGhelp = "(Ctrl+" + this.keyNames[this.ttGHelp] + ") Display Grind Trainers";
            this.customTrigs = "(Ctrl+" + this.keyNames[this.ttCtrigs] + ") Experimental Grind Triggers";
            this.lableLine = "------------------------------------------------------";
        }

        private void SetTimerDwn(bool p_bool)
        {
            float increment = -0.25f;
            Main.settings.grnd_Timer = (Main.settings.grnd_Timer + increment >= 4f ? 4f : (Main.settings.grnd_Timer + increment < 0f ? 0f : Main.settings.grnd_Timer + increment));
            ModMenu.Instance.ShowMessage(this.trackerTitle + " Reset Time (s): " + Main.settings.grnd_Timer);
            this.labelTimerD.SetToggleValue(true);
        }

        private void SetTimerUp(bool p_bool)
        {
            float increment = 0.25f;
            Main.settings.grnd_Timer = (Main.settings.grnd_Timer + increment >= 4f ? 4f : (Main.settings.grnd_Timer + increment < 0f ? 0f : Main.settings.grnd_Timer + increment));
            ModMenu.Instance.ShowMessage(this.trackerTitle + " Reset Time (s): " + Main.settings.grnd_Timer);
            this.labelTimerU.SetToggleValue(true);
        }

        //private void SetGrowHV(bool p_bool)
        //{
        //    Main.settings.grow_Vertical = p_bool;
        //    this.labelVertHorz.SetToggleValue(true);
        //    //this.labelVertHorz.text = /*"<color=#ffff00ff>" +*/"(Ctrl+" + this.keyNames[this.ttAxis] + ") Extend " + (Main.settings.grow_Vertical ? "Horizontal" : "Vertical")/* + "</color> "*/;
        //    ModMenu.Instance.ShowMessage(this.trackerTitle + " Extend: " + (Main.settings.grow_Vertical ? "Vertical" : "Horizontal"));
        //}

        private void CheckButtons()
        {
            if (Main.enabled && Input.GetKey(KeyCode.LeftControl))
            {
                ModMenu.Instance.KeyPress(this.ttEnable, 0.2f, () =>
                {
                    Main.settings.do_TrackTricks = !Main.settings.do_TrackTricks;
                    this.labelshowGHelp.SetToggleValue(Main.settings.do_TrackTricks);
                    ModMenu.Instance.ShowMessage(this.trackerTitle + (Main.settings.do_TrackTricks ? ": Enabled" : ": Disabled"));
                });

                ModMenu.Instance.KeyPress(this.ttLanding, 0.2f, () =>
                {
                    Main.settings.at_TrickLanding = !Main.settings.at_TrickLanding;
                    this.labelshowGHelp.SetToggleValue(Main.settings.at_TrickLanding);
                    ModMenu.Instance.ShowMessage(this.trackerTitle + " Place Tricks at Contact" + (Main.settings.at_TrickLanding ? ": Enabled" : ": Disabled"));
                });

                ModMenu.Instance.KeyPress(this.ttGHelp, 0.2f, () =>
                {
                    Main.settings.show_ghelpers = !Main.settings.show_ghelpers;
                    this.labelshowGHelp.SetToggleValue(Main.settings.show_ghelpers);
                    ModMenu.Instance.ShowMessage(this.trackerTitle + " Grind Trainers" + (Main.settings.show_ghelpers ? ": Enabled" : ": Disabled"));
                });

                ModMenu.Instance.KeyPress(this.ttTimerD, 0.2f, () => { this.SetTimerDwn(true); });
                ModMenu.Instance.KeyPress(this.ttTimerU, 0.2f, () => { this.SetTimerUp(true); });

                ModMenu.Instance.KeyPress(this.ttAxis, 0.2f, () => {
                    //this.SetGrowHV(!Main.settings.grow_Vertical);
                    Main.settings.grow_Vertical = !Main.settings.grow_Vertical;
                    this.labelVertHorz.SetToggleValue(Main.settings.grow_Vertical);
                    //this.labelVertHorz.text = /*"<color=#ffff00ff>" +*/"(Ctrl+" + this.keyNames[this.ttAxis] + ") Extend " + (Main.settings.grow_Vertical ? "Horizontal" : "Vertical")/* + "</color> "*/;
                    ModMenu.Instance.ShowMessage(this.trackerTitle + " Extend: " + (Main.settings.grow_Vertical ? "Vertical" : "Horizontal"));

                });

                //ModMenu.Instance.KeyPress(this.ttCtrigs, 0.2f, () =>
                //{
                //    Main.settings.use_custTrigs = !Main.settings.use_custTrigs;
                //    this.labelCustomTrigs.SetToggleValue(Main.settings.use_custTrigs);
                //    ModMenu.Instance.ShowMessage(this.trackerTitle + " Cutoms Grind Triggers" + (Main.settings.use_custTrigs ? ": Enabled" : ": Disabled"));
                //});
            };

            //if ((Time.time - this.keyDownTime) < 0.1f) { return; }
            //this.keyDownTime = Time.time;

            //int chkKey = (Input.GetKey(KeyCode.Keypad1) ? 1 : Input.GetKey(KeyCode.Keypad2) ? 2 : Input.GetKey(KeyCode.Keypad3) ? 3 : Input.GetKey(KeyCode.Keypad4) ? 4 :
            //                Input.GetKey(KeyCode.Keypad5) ? 5 : Input.GetKey(KeyCode.Keypad6) ? 6 : Input.GetKey(KeyCode.Keypad7) ? 7 : Input.GetKey(KeyCode.Keypad8) ? 8 : -1);
            //string messText = "";
            //if (Main.enabled && Input.GetKey(KeyCode.RightAlt))
            //{
            //    switch (chkKey)
            //    {
            //        case 1:
            //            Main.settings.brdOffsets.x = (Main.settings.brdOffsets.x < -1.0f ? -1.0f : Main.settings.brdOffsets.x -= 0.001000000f);
            //            messText = "BrdX: " + Main.settings.brdOffsets.x;
            //            break;
            //        case 2:
            //            Main.settings.brdOffsets.x = (Main.settings.brdOffsets.x > 1.0f ? 1.0f : Main.settings.brdOffsets.x += 0.001000000f);
            //            messText = "BrdX: " + Main.settings.brdOffsets.x;
            //            break;
            //        case 3:
            //            Main.settings.brdOffsets.y = (Main.settings.brdOffsets.y < -1.0f ? -1.0f : Main.settings.brdOffsets.y -= 0.001000000f);
            //            messText = "BrdY: " + Main.settings.brdOffsets.y;
            //            break;
            //        case 4:
            //            Main.settings.brdOffsets.y = (Main.settings.brdOffsets.y > 1.0f ? 1.0f : Main.settings.brdOffsets.y += 0.001000000f);
            //            messText = "BrdY: " + Main.settings.brdOffsets.y;
            //            break;
            //        case 5:
            //            Main.settings.brdOffsets.z = (Main.settings.brdOffsets.z < -1.0f ? -1.0f : Main.settings.brdOffsets.z -= 0.001000000f);
            //            messText = "BrdZ: " + Main.settings.brdOffsets.z;
            //            break;
            //        case 6:
            //            Main.settings.brdOffsets.z = (Main.settings.brdOffsets.z > 1.0f ? 1.0f : Main.settings.brdOffsets.z += 0.001000000f);
            //            messText = "BrdZ: " + Main.settings.brdOffsets.z;
            //            break;
            //        case 7:
            //            Main.settings.rayDist.x = (Main.settings.rayDist.x < -1.0f ? -1.0f : Main.settings.rayDist.x -= 0.001000000f);
            //            messText = "BrdRayDist: " + Main.settings.rayDist.x;
            //            break;
            //        case 8:
            //            Main.settings.rayDist.x = (Main.settings.rayDist.x > 1.0f ? 1.0f : Main.settings.rayDist.x += 0.001000000f);
            //            messText = "BrdRayDist: " + Main.settings.rayDist.x;
            //            break;
            //    };
            //};
            //if (Main.enabled && Input.GetKey(KeyCode.RightControl))
            //{
            //    switch (chkKey)
            //    {
            //        case 1:
            //            Main.settings.trckOffsets.x = (Main.settings.trckOffsets.x < -1.0f ? -1.0f : Main.settings.trckOffsets.x -= 0.001000000f);
            //            messText = "TrckX: " + Main.settings.trckOffsets.x;
            //            break;
            //        case 2:
            //            Main.settings.trckOffsets.x = (Main.settings.trckOffsets.x > 1.0f ? 1.0f : Main.settings.trckOffsets.x += 0.001000000f);
            //            messText = "TrckX: " + Main.settings.trckOffsets.x;
            //            break;
            //        case 3:
            //            Main.settings.trckOffsets.y = (Main.settings.trckOffsets.y < -1.0f ? -1.0f : Main.settings.trckOffsets.y -= 0.001000000f);
            //            messText = "TrckY: " + Main.settings.trckOffsets.y;
            //            break;
            //        case 4:
            //            Main.settings.trckOffsets.y = (Main.settings.trckOffsets.y > 1.0f ? 1.0f : Main.settings.trckOffsets.y += 0.001000000f);
            //            messText = "TrckY: " + Main.settings.trckOffsets.y;
            //            break;
            //        case 5:
            //            Main.settings.trckOffsets.z = (Main.settings.trckOffsets.z < -1.0f ? -1.0f : Main.settings.trckOffsets.z -= 0.001000000f);
            //            messText = "TrckZ: " + Main.settings.trckOffsets.z;
            //            break;
            //        case 6:
            //            Main.settings.trckOffsets.z = (Main.settings.trckOffsets.z > 1.0f ? 1.0f : Main.settings.trckOffsets.z += 0.001000000f);
            //            messText = "TrckZ: " + Main.settings.trckOffsets.z;
            //            break;
            //        case 7:
            //            Main.settings.rayDist.y = (Main.settings.rayDist.y < -1.0f ? -1.0f : Main.settings.rayDist.y -= 0.001000000f);
            //            messText = "TrckRayDist: " + Main.settings.rayDist.y;
            //            break;
            //        case 8:
            //            Main.settings.rayDist.y = (Main.settings.rayDist.y > 1.0f ? 1.0f : Main.settings.rayDist.y += 0.001000000f);
            //            messText = "TrckRayDist: " + Main.settings.rayDist.y;
            //            break;
            //    };
            //};
            //if (Main.enabled && Input.GetKey(KeyCode.KeypadDivide))
            //{
            //    switch (chkKey)
            //    {
            //        case 1:
            //            Main.settings.ntOffsets.x = (Main.settings.ntOffsets.x < -1.0f ? -1.0f : Main.settings.ntOffsets.x -= 0.001000000f);
            //            messText = "NTX: " + Main.settings.ntOffsets.x;
            //            break;
            //        case 2:
            //            Main.settings.ntOffsets.x = (Main.settings.ntOffsets.x > 1.0f ? 1.0f : Main.settings.ntOffsets.x += 0.001000000f);
            //            messText = "NTX: " + Main.settings.ntOffsets.x;
            //            break;
            //        case 3:
            //            Main.settings.ntOffsets.y = (Main.settings.ntOffsets.y < -1.0f ? -1.0f : Main.settings.ntOffsets.y -= 0.001000000f);
            //            messText = "NTY: " + Main.settings.ntOffsets.y;
            //            break;
            //        case 4:
            //            Main.settings.ntOffsets.y = (Main.settings.ntOffsets.y > 1.0f ? 1.0f : Main.settings.ntOffsets.y += 0.001000000f);
            //            messText = "NTY: " + Main.settings.ntOffsets.y;
            //            break;
            //        case 5:
            //            Main.settings.ntOffsets.z = (Main.settings.ntOffsets.z < -1.0f ? -1.0f : Main.settings.ntOffsets.z -= 0.001000000f);
            //            messText = "NTZ: " + Main.settings.ntOffsets.z;
            //            break;
            //        case 6:
            //            Main.settings.ntOffsets.z = (Main.settings.ntOffsets.z > 1.0f ? 1.0f : Main.settings.ntOffsets.z += 0.001000000f);
            //            messText = "NTZ: " + Main.settings.ntOffsets.z;
            //            break;
            //        case 7:
            //            Main.settings.rayDist.z = (Main.settings.rayDist.z < -1.0f ? -1.0f : Main.settings.rayDist.z -= 0.001000000f);
            //            messText = "NtRayDist: " + Main.settings.rayDist.z;
            //            break;
            //        case 8:
            //            Main.settings.rayDist.z = (Main.settings.rayDist.z > 1.0f ? 1.0f : Main.settings.rayDist.z += 0.001000000f);
            //            messText = "NtRayDist: " + Main.settings.rayDist.z;
            //            break;
            //    };
            //};
            //if (messText.Length > 0) { ModMenu.Instance.ShowMessage(this.trackerTitle + " " + messText); };
        }
    }
}
