using System;
using UnityEngine;

namespace DWG_TT
{
    public enum PlaceType
    {
        lfTrig,
        lrTrig,
        rfTrig,
        rrTrig,
    }

    class GUITrick : MonoBehaviour
    {
        static private GUIContent tricks;
        static private GUIStyle tricksStyle;
        static private Vector2 tricksSize;

        static private string trackedTricks; public string TrackedTricks { get { return trackedTricks; } }
        static private string lastTricks; public string LastTricks { get { return lastTricks; } }

        static private double trackedTime; public double TrackedTime { get { return trackedTime; } set { trackedTime = value; } }

        public int TTFontSize { get { return tricksStyle.fontSize; } set { tricksStyle.fontSize = value; } }
        public const int ttBorder = 8;

        void OnEnable()
        {
            trackedTricks = "";
            trackedTime = 0f;

            tricks = new GUIContent("");
            tricksStyle = new GUIStyle("box");
            tricksSize = Vector2.zero;
            tricksStyle.fontSize = Main.settings.tt_fontSize;
        }

        void LateUpdate()
        {
            if (SXLH.CrntState == SXLH.Bailed && ((Time.time - trackedTime) <= 0.5f)) { AddTrick(SXLH.Bailed); trackedTime -= 1f; };
        }

        void OnGUI()
        {
            if (!Main.enabled && !Main.settings.do_TrackTricks) { return; }

            if (tricks == null) { return; }

            if (((Time.time + Time.deltaTime) - trackedTime) > Main.settings.grnd_Timer) { trackedTricks = ""; }
            if (trackedTricks.Length == 0) { return; }

            tricksStyle.alignment = TextAnchor.MiddleRight;
            tricks.text = trackedTricks;
            tricksSize = tricksStyle.CalcSize(tricks);
            GUI.Label(new Rect(((Screen.width - (ttBorder + Main.settings.tt_borderX)) - tricksSize.x), ((Screen.height - (ttBorder + Main.settings.tt_borderY)) - tricksSize.y), tricksSize.x, tricksSize.y), tricks, tricksStyle);
        }

        public void AddTrick(string p_newTrick)
        {
            if (Main.settings.at_TrickLanding)
            {
                GameObject tmpObj = new GameObject();
                SceneTrick newTrick = tmpObj.AddComponent<SceneTrick>();
                newTrick.Init(p_newTrick);
            }

            trackedTime = Time.time;
            trackedTricks += (((trackedTricks.Length > 0) ? (Main.settings.grow_Vertical ? "\n " : "=> ") : " ") + p_newTrick + " ");
        }
    }

    class GUIM : MonoBehaviour
    {
        private PlayerController pCon;
        private PI plyrInfo; public PI PlyrInfo { get { return this.plyrInfo; } }
        private GUITrick guiTrck; public GUITrick GuiTrck { get { return this.guiTrck; } }
        private GM grndMan;

        private GrndTrainr[] grndTranrs = new GrndTrainr[10];

        private bool showGHelp = false; public bool GrndTrainers { get { return this.showGHelp; } }

        private GUIContent consoleCont;
        private GUIStyle consoleStyle;
        private Vector2 conSize;

        private float frmTime;

        void OnEnable()
        {
            //DebugOut.Log(this.GetType().Name + " OnEnable: ");
            this.frmTime = 0f;
            this.showGHelp = true;
        }

        public void GuiReset()
        {
            this.showGHelp = true;
        }

        void OnGUI()
        {
            if (this.pCon == null)
            {
                DebugOut.Log(this.GetType().Name + " Waiting on PlayerController");
                this.pCon = FindObjectOfType(typeof(PlayerController)) as PlayerController;
                return;
            }

            if (this.guiTrck == null)
            {
                //DebugOut.Log(this.GetType().Name + " Waiting on  GUITrick");
                GameObject tmpObj = new GameObject();
                tmpObj.transform.parent = this.gameObject.transform;
                this.guiTrck = tmpObj.AddComponent(typeof(GUITrick)) as GUITrick;
                return;
            }

            if (this.plyrInfo == null)
            {
                //DebugOut.Log(this.GetType().Name + " Waiting on  PlayerInfo");
                this.plyrInfo = this.GetComponentInParent(typeof(PI)) as PI;
                return;
            }

            if (this.grndMan == null)
            {
                //DebugOut.Log(this.GetType().Name + " Waiting on  GrindManager");
                this.grndMan = this.GetComponentInParent(typeof(GM)) as GM;
                return;
            }

            if (Main.settings.show_ghelpers && this.showGHelp)
            {
                this.showGHelp = false;
                for (int i = 0; i < 5; i++)
                {
                    GameObject tmpObj = new GameObject();
                    tmpObj.transform.parent = this.gameObject.transform;
                    GrndTrainr tmpGT = tmpObj.gameObject.AddComponent<GrndTrainr>();
                    tmpGT.Init(i, 1);
                    this.grndTranrs[i] = tmpGT;

                    tmpObj = new GameObject();
                    tmpObj.transform.parent = this.gameObject.transform;
                    tmpGT = tmpObj.gameObject.AddComponent<GrndTrainr>();
                    tmpGT.Init(i, -1);
                    this.grndTranrs[i] = tmpGT;
                }
            }
            if (!this.showGHelp && !Main.settings.show_ghelpers) { this.showGHelp = true; }

            GUI.backgroundColor = Color.black;
            if (Main.settings.con_Enable)
            {
                if (this.consoleCont == null)
                {
                    this.consoleCont = new GUIContent("");
                    this.consoleStyle = new GUIStyle("box");
                    this.conSize = Vector2.zero;
                }

                this.consoleStyle.alignment = TextAnchor.UpperLeft;
                this.consoleCont.text = (
                    //"SktrEulLast X:" + this.plyrInfo.SktrEulLast.x + " Y:" + this.plyrInfo.SktrEulLast.y + " Z:" + this.plyrInfo.SktrEulLast.z + "\n" +
                    //" BrdEulLast X:" + this.plyrInfo.BrdEulLast.x + " Y:" + this.plyrInfo.BrdEulLast.y + " Z:" + this.plyrInfo.BrdEulLast.z + "\n" +
                    " SktrRot : " + this.plyrInfo.SktrRot + "\n" +
                    " SktrRotMax : " + this.plyrInfo.SktrRotMax + "\n" + "\n" +
                    " SktrFlip : " + this.plyrInfo.SktrFlip + "\n" +
                    " SktrFlipMax : " + this.plyrInfo.SktrFlipMax + "\n" + "\n" +
                    " SktrTwk : " + this.plyrInfo.SktrTwk + "\n" +
                    " SktrTwkMax : " + this.plyrInfo.SktrTwkMax + "\n" + "\n" +
                    " BrdRot : " + this.plyrInfo.BrdRot + "\n" +
                    " BrdRotMax : " + this.plyrInfo.BrdRotMax + "\n" + "\n" +
                    " BrdFlip : " + this.plyrInfo.BrdFlip + "\n" +
                    " BrdFlipMax : " + this.plyrInfo.BrdFlipMax + "\n" + "\n" +
                    " BrdTwk : " + this.plyrInfo.BrdTwk + "\n" +
                    " BrdTwkMax : " + this.plyrInfo.BrdTwkMax + "\n" + "\n" +
                    " BrdSpeed : " + SXLH.BrdSpeed + "\n" +
                    " GrndWait : " + this.grndMan.SpdAdjst + "\n" +
                    " GrindSplLngt : " + (this.grndMan.GrindLngt / 100) + "\n" +
                    " GrndLeft : " + this.grndMan.GrndLeft + "\n" + "\n" +
                    " FwdAngle : " + this.grndMan.EdgeFwdAngl + "\n" +
                    " UpAngle : " + this.grndMan.EdgeUpAngl + "\n" +
                    " EdgeHght : " + this.grndMan.HghtDiff + "\n" +
                    " DistX : " + this.grndMan.DistX + "\n" +
                    " DistZ : " + this.grndMan.DistZ + "\n" + "\n" +
                    " AllDwn : " + SXLH.AllDown + "\n" +
                    " TwoDwn : " + SXLH.TwoDown + "\n" +
                    " OneDwn : " + this.grndMan.OneDown() + "\n" +
                    " NoneDwn : " + this.grndMan.NoneDown() + "\n" + "\n" +
                    " FWheels : " + this.grndMan.FLWheel + " " + this.grndMan.FRWheel + "\n" +
                    " NoseTrig : " + this.grndMan.NoseTrigg + "\n" +
                    " FrntTrig : " + this.grndMan.FrntTrckTrigg + "\n" +
                    " BrdTrig : " + this.grndMan.BrdTrigg + "\n" +
                    " BckTrig : " + this.grndMan.BckTrckTrigg + "\n" +
                    " TailTrig : " + this.grndMan.TailTrigg + "\n" +
                    " RWheels : " + this.grndMan.RLWheel + " " + this.grndMan.RRWheel + "\n" + "\n" +
                    " CrntState : " + SXLH.CrntState + "\n" + "\n" +
                    " Stance : " + (SXLH.IsReg ? "Regular" : "Goofy") + "\n" +
                    " CrntGrind : " + this.grndMan.CrntGrind + "\n" +
                    " SideEntr : " + this.grndMan.GrndSide + "\n" +
                    " GrndMetal : " + this.grndMan.MetalGrnd + "\n" +
                    " FrntSide : " + SXLH.FrntSide + "\n" +
                    " BackSide : " + SXLH.BackSide + "\n" +
                    " SideStrt : " + this.grndMan.SideStrt + "\n" +
                    " SameSide : " + this.grndMan.SameSide + "\n" +
                    " ToeFwd : " + this.grndMan.ToeFwd + "\n" +
                    " IsSwitch : " + SXLH.IsSwitch + "\n" +
                    " IsBrdFwd : " + SXLH.IsBrdFwd + "\n" + "\n" +
                    " LeftFrnt : " + this.plyrInfo.LeftFrnt + "\n" +
                    " LeftPop : " + SXLH.LeftPop + "\n" + "\n" +
                    " RightFrnt : " + this.plyrInfo.RightFrnt + "\n" +
                    " RightPop : " + SXLH.RightPop + "\n" + "\n" +
                    " GetPrefix : " + this.plyrInfo.GetPrefix(SXLH.CrntState) + "\n" + "\n" +
                    " MoveMaster : " + PlayerController.Instance.movementMaster.ToString() + "\n" +
                    ""
                );

                this.conSize = this.consoleStyle.CalcSize(this.consoleCont);
                GUI.Label(new Rect(4, 4, (Screen.width / 5), this.conSize.y), this.consoleCont, this.consoleStyle);
            }
        }
    }
}
