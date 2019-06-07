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
            tricksStyle = GUI.skin.box;
            tricksSize = Vector2.zero;
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
        private SplnToTrigMark[] splnTrckrs = new SplnToTrigMark[5];
        private RayCastTrigLines[] lfTrckrs = new RayCastTrigLines[5];
        private RayCastTrigLines[] lrTrckrs = new RayCastTrigLines[5];
        private RayCastTrigLines[] rfTrckrs = new RayCastTrigLines[5];
        private RayCastTrigLines[] rrTrckrs = new RayCastTrigLines[5];

        private bool showGHelp = false; public bool GrndTrainers { get { return this.showGHelp; } }
        private bool makeSplnMarks = false;
        private bool showSplnMarks = false; public bool SplnMarks { get { return this.showSplnMarks; } }
        private bool makeRayLines = false;
        private bool showRayLines = false; public bool RayLines { get { return this.showRayLines; } }

        private GUIContent consoleCont;
        private GUIStyle consoleStyle;
        private Vector2 conSize;

        private float frmTime;

        void OnEnable()
        {
            //DebugOut.Log(this.GetType().Name + " OnEnable: ");
            this.frmTime = 0f;
            this.showGHelp = true;
            this.makeSplnMarks = false;
            this.makeRayLines = false;
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

            if (Main.settings.use_custTrigs && this.showSplnMarks && this.makeSplnMarks)
            {
                this.makeSplnMarks = false;
                for (int i = 0; i < 5; i++)
                {
                    GameObject splnCollObj = new GameObject();
                    splnCollObj.transform.parent = this.gameObject.transform;
                    SplnToTrigMark splnColl = splnCollObj.AddComponent<SplnToTrigMark>();
                    splnColl.Init(i);
                    this.splnTrckrs[i] = splnColl;
                }
            }

            if (Main.settings.use_custTrigs && this.showRayLines && this.makeRayLines)
            {
                this.makeRayLines = false;
                for (int i = 0; i < 5; i++)
                {
                    GameObject rrCollObj = new GameObject();
                    rrCollObj.transform.parent = this.gameObject.transform;
                    RayCastTrigLines rrColl = rrCollObj.AddComponent<RayCastTrigLines>();
                    this.rrTrckrs[i] = rrColl;

                    GameObject lrCollObj = new GameObject();
                    lrCollObj.transform.parent = this.gameObject.transform;
                    RayCastTrigLines lrColl = lrCollObj.AddComponent<RayCastTrigLines>();
                    this.lrTrckrs[i] = lrColl;

                    GameObject rfCollObj = new GameObject();
                    rfCollObj.transform.parent = this.gameObject.transform;
                    RayCastTrigLines rfColl = rfCollObj.AddComponent<RayCastTrigLines>();
                    this.rfTrckrs[i] = rfColl;

                    GameObject lfCollObj = new GameObject();
                    lfCollObj.transform.parent = this.gameObject.transform;
                    RayCastTrigLines lfColl = lfCollObj.AddComponent<RayCastTrigLines>();
                    this.lfTrckrs[i] = lfColl;

                    rrColl.Init(i, PlaceType.rrTrig);
                    rfColl.Init(i, PlaceType.rfTrig);
                    lrColl.Init(i, PlaceType.lrTrig);
                    lfColl.Init(i, PlaceType.lfTrig);
                }
            }

            if (Input.GetKey(KeyCode.LeftControl) && (Time.time - this.frmTime) > 0.1f)
            {
                this.frmTime = Time.time + Time.deltaTime;
                if (Input.GetKey(KeyCode.Period))
                {
                    this.showSplnMarks = !this.showSplnMarks;
                    this.makeSplnMarks = this.showSplnMarks;
                }
                if (Input.GetKey(KeyCode.Comma))
                {
                    this.showRayLines = !this.showRayLines;
                    this.makeRayLines = this.showRayLines;
                }
            }

            GUI.backgroundColor = Color.black;
            if (Main.settings.con_Enable)
            {
                if (this.consoleCont == null)
                {
                    this.consoleCont = new GUIContent("");
                    this.consoleStyle = GUI.skin.box;
                    this.conSize = Vector2.zero;
                }

                this.consoleStyle.alignment = TextAnchor.UpperLeft;
                this.consoleCont.text = (
                    "SplineMakrs:      " + this.showSplnMarks + "\n" +
                    "RayLines---:      " + this.showRayLines + "\n" + "\n" +
                    //"SktrEulLast:    X:" + this.plyrInfo.SktrEulLast.x + " Y:" + this.plyrInfo.SktrEulLast.y + " Z:" + this.plyrInfo.SktrEulLast.z + "\n" +
                    //"BrdEulLast:     X:" + this.plyrInfo.BrdEulLast.x + " Y:" + this.plyrInfo.BrdEulLast.y + " Z:" + this.plyrInfo.BrdEulLast.z + "\n" +
                    "SktrRot:          " + this.plyrInfo.SktrRot + "\n" +
                    "SktrRotMax:       " + this.plyrInfo.SktrRotMax + "\n" + "\n" +
                    "SktrFlip:         " + this.plyrInfo.SktrFlip + "\n" +
                    "SktrFlipMax:      " + this.plyrInfo.SktrFlipMax + "\n" + "\n" +
                    "SktrTwk:          " + this.plyrInfo.SktrTwk + "\n" +
                    "SktrTwkMax:       " + this.plyrInfo.SktrTwkMax + "\n" + "\n" +
                    "BrdRot:           " + this.plyrInfo.BrdRot + "\n" +
                    "BrdRotMax:        " + this.plyrInfo.BrdRotMax + "\n" + "\n" +
                    "BrdFlip:          " + this.plyrInfo.BrdFlip + "\n" +
                    "BrdFlipMax:       " + this.plyrInfo.BrdFlipMax + "\n" + "\n" +
                    "BrdTwk:           " + this.plyrInfo.BrdTwk + "\n" +
                    "BrdTwkMax:        " + this.plyrInfo.BrdTwkMax + "\n" + "\n" +
                    //"TrigManColl:      " + SXLH.TrigManIsColl + "\n" +
                    "BrdSpeed---:      " + SXLH.BrdSpeed + "\n" +
                    "FwdAngle---:      " + this.grndMan.EdgeFwdAngl + "\n" +
                    "UpAngle----:      " + this.grndMan.EdgeUpAngl + "\n" + "\n" +
                    "EdgeHght---:      " + this.grndMan.HghtDiff + "\n" + "\n" +
                    "TrickHght--:      " + this.grndMan.TrickHght + "\n" + "\n" +
                    "noseTrig:         " + GrndTrigs.Hit[SXLH.FlipTrigs ? GrndTrigs.NTrig : GrndTrigs.TTrig] + "\n" +
                    "frntTrig:         " + GrndTrigs.Hit[SXLH.FlipTrigs ? GrndTrigs.FTTrig : GrndTrigs.BTTrig] + "\n" +
                    "brdTrig:          " + GrndTrigs.Hit[GrndTrigs.BrdTrig] + "\n" +
                    "bckTrig:          " + GrndTrigs.Hit[SXLH.FlipTrigs ? GrndTrigs.BTTrig : GrndTrigs.FTTrig] + "\n" +
                    "tailTrig:         " + GrndTrigs.Hit[SXLH.FlipTrigs ? GrndTrigs.TTrig : GrndTrigs.NTrig] + "\n" + "\n" +
                    "CrntState:        " + SXLH.CrntState + "\n" + "\n" +
                    "Stance-----:      " + (SXLH.IsReg ? "Regular" : "Goofy") + "\n" +
                    "CrntGrind--:      " + this.grndMan.CrntGrind + "\n" + "\n" +
                    "FrntSide---:      " + SXLH.FrntSide + "\n" +
                    "SameSide---:      " + this.grndMan.SameSide + "\n" +
                    "ToeFwd-----:      " + this.grndMan.ToeFwd + "\n" +
                    //"IsAbove:          " + (this.grndMan.HghtDiff > 0) + "\n" +
                    //"AllDown:          " + SXLH.AllDown + "\n" +
                    "TwoDwn-----:      " + SXLH.TwoDown + "\n" +
                    "IsSwitch---:      " + SXLH.IsSwitch + "\n" +
                    "IsBrdFwd---:      " + SXLH.IsBrdFwd + "\n" + "\n" +
                    "LeftFrnt:         " + this.plyrInfo.LeftFrnt + "\n" +
                    "LeftPop:          " + SXLH.LeftPop + "\n" + "\n" +
                    "RightFrnt:        " + this.plyrInfo.RightFrnt + "\n" +
                    "RightPop:         " + SXLH.RightPop + "\n" + "\n" +
                    "GetPrefix:        " + this.plyrInfo.GetPrefix(SXLH.CrntState) + "\n" + "\n" +
                    //"GO Name:          " + PlayerController.Instance.gameObject.name + "\n" +
                    "MovementMaster:   " + PlayerController.Instance.movementMaster.ToString() + "\n" +
                    //"Skater RBGOName:      " + PlayerController.Instance.skaterController.skaterRigidbody.gameObject.name + "\n" +
                    //"Skater BPTag:     " + PlayerController.Instance.skaterController.skaterRigidbody.ToString() + "\n" +
                    ""
                );

                this.conSize = this.consoleStyle.CalcSize(this.consoleCont);
                GUI.Label(new Rect(4, 4, (Screen.width / 5), this.conSize.y), this.consoleCont, this.consoleStyle);
            }
        }
    }
}
