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
        static public void Log(string p_dbgStr) { if (!debugMe) { return; }; Debug.Log("<-DWG-> " + debugTitle + " - " + p_dbgStr + " <-DWG->"); }
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
        private ModUILabel labelPosL;
        private ModUILabel labelPosR;
        private ModUILabel labelPosU;
        private ModUILabel labelPosD;
        private ModUILabel labelFntSzeD;
        private ModUILabel labelFntSzeU;
        //private ModUILabel labelSkpErroneous;
        private ModUILabel labelConEnable;

        private readonly Dictionary<KeyCode, string> keyNames = new Dictionary<KeyCode, string>()
        {
            { KeyCode.Alpha1 , "1" },
            { KeyCode.Alpha2 , "2" },
            { KeyCode.Alpha3 , "3" },
            { KeyCode.Alpha4 , "4" },
            { KeyCode.Alpha5 , "5" },
            { KeyCode.Alpha6 , "6" },
        };

        private readonly KeyCode ttTimerD = KeyCode.Alpha1;
        private readonly KeyCode ttTimerU = KeyCode.Alpha2;
        private readonly KeyCode ttEnable = KeyCode.Alpha3;
        private readonly KeyCode ttLanding = KeyCode.Alpha4;
        private readonly KeyCode ttGHelp = KeyCode.Alpha5;
        private readonly KeyCode ttAxis = KeyCode.Alpha6;

        private string titleWithKey;
        private string textTimerD;
        private string textTimerU;
        private string trackerGrow;
        private string tricksAtLanding;
        private string showGhelp;
        private string trackerPosL;
        private string trackerPosR;
        private string trackerPosU;
        private string trackerPosD;
        private string trackerFntSzeD;
        private string trackerFntSzeU;
        //private string trackerSkpErroneous;
        private string trackerConEnable;
        private string lableFiller;
        private string lableLine;
        private bool lameBool;

        private bool isDestroyed;
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
            this.isDestroyed = false;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            DebugOut.Log(this.GetType().Name + " OnSceneLoaded: Scene: " + scene.name + " Mode: " + mode);
            this.guiMan.GuiReset();
        }

        void OnSceneUnLoaded(Scene scene)
        {
            DebugOut.Log(this.GetType().Name + "OnSceneUnLoaded: Scene: " + scene.name);
        }

        void OnDisable()
        {
            this.TTReset("OnDisable");
        }

        void OnDestroy()
        {
            this.TTReset("OnDestroy");
        }

        void OnApplicationQuit()
        {
            this.TTReset("OnApplicationQuit");
        }

        void TTReset(string p_sender)
        {
            DebugOut.Log(this.GetType().Name + " TTReset: " + p_sender + ": AlreadyDestroyed: " + this.isDestroyed);
            if (this.isDestroyed) { return; }
            this.isDestroyed = true;

            SceneManager.sceneLoaded -= this.OnSceneLoaded;
            SceneManager.sceneUnloaded -= this.OnSceneUnLoaded;
            Main.settings.con_Enable = false;
            this.labelConEnable.SetToggleValue(Main.settings.con_Enable);

            this.modMenuBox.RemoveLabel("strt-lineL");
            this.modMenuBox.RemoveLabel("strt-lineR");
            this.modMenuBox.RemoveLabel("do-trackTricks");
            this.modMenuBox.RemoveLabel("at-trickLanding");
            this.modMenuBox.RemoveLabel("show_gHelp");
            //this.modMenuBox.RemoveLabel("skp_Erron");
            //this.modMenuBox.RemoveLabel("fillerRA");
            this.modMenuBox.RemoveLabel("info-TimerD");
            this.modMenuBox.RemoveLabel("info-TimerU");
            this.modMenuBox.RemoveLabel("grow-vertical");
            this.modMenuBox.RemoveLabel("mid-lineL");
            this.modMenuBox.RemoveLabel("mid-lineR");
            this.modMenuBox.RemoveLabel("pos-Left");
            this.modMenuBox.RemoveLabel("pos-Rght");
            this.modMenuBox.RemoveLabel("pos-Up");
            this.modMenuBox.RemoveLabel("pos-Dwn");
            this.modMenuBox.RemoveLabel("fnt-SizeD");
            this.modMenuBox.RemoveLabel("fnt-SizeU");
            this.modMenuBox.RemoveLabel("con-Enable");
            this.modMenuBox.RemoveLabel("fillerRC");
            this.modMenuBox.RemoveLabel("end-lineL");
            this.modMenuBox.RemoveLabel("end-lineR");
        }

        void Start()
        {
            //DebugOut.Log(this.GetType().Name + " Start: ");

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

            this.SetupButtonStrings();

            this.modMenuBox = ModMenu.Instance.RegisterModMaker("dwg", "DeadWalking");

            this.modMenuBox.AddLabel("strt-lineL", LabelType.Text, this.lableLine, Side.left, () => Main.enabled, true, (b) => this.lameBool = b, 99);
            this.modMenuBox.AddLabel("strt-lineR", LabelType.Text, this.lableLine, Side.right, () => Main.enabled, true, (b) => this.lameBool = b, 99);

            this.labelTglTrack = this.modMenuBox.AddLabel("do-trackTricks", LabelType.Toggle, this.titleWithKey, Side.left, () => Main.enabled, Main.settings.do_TrackTricks, (b) => Main.settings.do_TrackTricks = b, 98);
            this.labelAtLanding = this.modMenuBox.AddLabel("at-trickLanding", LabelType.Toggle, this.tricksAtLanding, Side.left, () => Main.enabled, Main.settings.at_TrickLanding, (b) => Main.settings.at_TrickLanding = b, 97);
            this.labelshowGHelp = this.modMenuBox.AddLabel("show_gHelp", LabelType.Toggle, this.showGhelp, Side.left, () => Main.enabled, Main.settings.show_ghelpers, (b) => Main.settings.show_ghelpers = b, 96);
            //this.labelSkpErroneous = this.modMenuBox.AddLabel("skp-Erron", LabelType.Toggle, this.trackerSkpErroneous, Side.left, () => Main.enabled, Main.settings.skp_Erron, (b) => Main.settings.skp_Erron = !b, 95);

            this.labelTimerD = this.modMenuBox.AddLabel("info-TimerD", LabelType.Toggle, this.textTimerD, Side.right, () => Main.enabled, true, (b) => this.SetTimerDwn(b), 98);
            this.labelTimerU = this.modMenuBox.AddLabel("info-TimerU", LabelType.Toggle, this.textTimerU, Side.right, () => Main.enabled, true, (b) => this.SetTimerUp(b), 97);
            this.labelVertHorz = this.modMenuBox.AddLabel("grow-vertical", LabelType.Toggle, this.trackerGrow, Side.right, () => Main.enabled, Main.settings.grow_Vertical, (b) => Main.settings.grow_Vertical = b, 96);
            //this.modMenuBox.AddLabel("fillerRA", LabelType.Text, this.lableFiller, Side.right, () => Main.enabled, true, (b) => this.lameBool = b, 95);

            this.modMenuBox.AddLabel("mid-lineL", LabelType.Text, this.lableLine, Side.left, () => Main.enabled, true, (b) => this.lameBool = b, 79);
            this.modMenuBox.AddLabel("mid-lineR", LabelType.Text, this.lableLine, Side.right, () => Main.enabled, true, (b) => this.lameBool = b, 79);

            this.labelPosL = this.modMenuBox.AddLabel("pos-Left", LabelType.Toggle, this.trackerPosL, Side.left, () => Main.enabled, true, (b) => this.SetPosL(b), 78);
            this.labelPosU= this.modMenuBox.AddLabel("pos-Up", LabelType.Toggle, this.trackerPosU, Side.left, () => Main.enabled, true, (b) => this.SetPosU(b), 77);
            this.labelFntSzeD = this.modMenuBox.AddLabel("fnt-SizeD", LabelType.Toggle, this.trackerFntSzeD, Side.left, () => Main.enabled, true, (b) => this.SetFntSzeD(b), 76);
            this.labelConEnable = this.modMenuBox.AddLabel("con-Enable", LabelType.Toggle, this.trackerConEnable, Side.left, () => Main.enabled, Main.settings.con_Enable, (b) => Main.settings.con_Enable = b, 75);

            this.labelPosR = this.modMenuBox.AddLabel("pos-Rght", LabelType.Toggle, this.trackerPosR, Side.right, () => Main.enabled, true, (b) => this.SetPosR(b), 78);
            this.labelPosD = this.modMenuBox.AddLabel("pos-Dwn", LabelType.Toggle, this.trackerPosD, Side.right, () => Main.enabled, true, (b) => this.SetPosD(b), 77);
            this.labelFntSzeU = this.modMenuBox.AddLabel("fnt-SizeU", LabelType.Toggle, this.trackerFntSzeU, Side.right, () => Main.enabled, true, (b) => this.SetFntSzeU(b), 76);
            this.modMenuBox.AddLabel("fillerRC", LabelType.Text, this.lableFiller, Side.right, () => Main.enabled, true, (b) => this.lameBool = b, 75);

            this.modMenuBox.AddLabel("end-lineL", LabelType.Text, this.lableLine, Side.left, () => Main.enabled, true, (b) => this.lameBool = b, 50);
            this.modMenuBox.AddLabel("end-lineR", LabelType.Text, this.lableLine, Side.right, () => Main.enabled, true, (b) => this.lameBool = b, 50);
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
            this.trackerPosL = "Move Left";
            this.trackerPosR = "Move Right";
            this.trackerPosU = "Move Up";
            this.trackerPosD = "Move Down";
            this.trackerFntSzeU = "Font Size Smaller";
            this.trackerFntSzeD = "Font Size Larger";
            //this.trackerSkpErroneous = "Display Excessive Tricks";
            this.trackerConEnable = "Display Tracker Data";
            this.lableFiller = "                                                      ";
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

        private void SendTrackerUpdate(string p_update)
        {
            if (this.guiMan.GuiTrck.TrackedTricks.Length == 0)
            {
                this.guiMan.GuiTrck.AddTrick(p_update);
            }
        }

        private void SetPosL(bool p_bool)
        {
            Main.settings.tt_borderX += GUITrick.ttBorder;
            this.SendTrackerUpdate("Moved");
            this.labelPosL.SetToggleValue(true);
        }
        private void SetPosR(bool p_bool)
        {
            Main.settings.tt_borderX -= ((Main.settings.tt_borderX - GUITrick.ttBorder) < 0 ? 0 : GUITrick.ttBorder);
            this.SendTrackerUpdate("Moved");
            this.labelPosR.SetToggleValue(true);
        }
        private void SetPosU(bool p_bool)
        {
            Main.settings.tt_borderY += GUITrick.ttBorder;
            this.SendTrackerUpdate("Moved");
            this.labelPosU.SetToggleValue(true);
        }
        private void SetPosD(bool p_bool)
        {
            Main.settings.tt_borderY -= ((Main.settings.tt_borderY - GUITrick.ttBorder) < 0 ? 0 : GUITrick.ttBorder);
            this.SendTrackerUpdate("Moved");
            this.labelPosD.SetToggleValue(true);
        }
        private void SetFntSzeD(bool p_bool)
        {
            Main.settings.tt_fontSize += 1;
            this.guiMan.GuiTrck.TTFontSize = Main.settings.tt_fontSize;
            this.guiMan.GuiTrck.AddTrick("Font Size: " + this.guiMan.GuiTrck.TTFontSize);
            this.labelFntSzeD.SetToggleValue(true);
        }
        private void SetFntSzeU(bool p_bool)
        {
            Main.settings.tt_fontSize -= ((Main.settings.tt_fontSize - 1) < 14 ? 0 : 1);
            this.guiMan.GuiTrck.TTFontSize = Main.settings.tt_fontSize;
            this.guiMan.GuiTrck.AddTrick("Font Size: " + this.guiMan.GuiTrck.TTFontSize);
            this.labelFntSzeU.SetToggleValue(true);
        }

        private void CheckButtons()
        {
            if (Main.enabled && Input.GetKey(KeyCode.LeftControl))
            {
                ModMenu.Instance.KeyPress(this.ttEnable, 0.2f, () =>
                {
                    Main.settings.do_TrackTricks = !Main.settings.do_TrackTricks;
                    this.labelTglTrack.SetToggleValue(Main.settings.do_TrackTricks);
                    ModMenu.Instance.ShowMessage(this.trackerTitle + (Main.settings.do_TrackTricks ? ": Enabled" : ": Disabled"));
                });

                ModMenu.Instance.KeyPress(this.ttLanding, 0.2f, () =>
                {
                    Main.settings.at_TrickLanding = !Main.settings.at_TrickLanding;
                    this.labelAtLanding.SetToggleValue(Main.settings.at_TrickLanding);
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
                    ModMenu.Instance.ShowMessage(this.trackerTitle + " Extend: " + (Main.settings.grow_Vertical ? "Vertical" : "Horizontal"));

                });
            };
        }
    }
}
