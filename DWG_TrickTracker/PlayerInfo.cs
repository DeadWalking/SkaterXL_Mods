using UnityEngine;

namespace DWG_TT
{
    class PI : MonoBehaviour
    {
        private GUIM guiMan;
        private GUITrick guiTrck;

        private const string grndState = SXLH.Grinding;
        private const string manState = SXLH.Manualling;
        private const string mandState = SXLH.Manualling;
        private string lastState;

        private bool leftFrnt; public bool LeftFrnt { get { return this.leftFrnt; } }
        private bool rightFrnt; public bool RightFrnt { get { return this.rightFrnt; } }
        private bool leftWasPop; public bool LeftWasPop { get { return this.leftWasPop; } }
        private bool rightWasPop; public bool RightWasPop { get { return this.rightWasPop; } }
        private bool wasSwitch; public bool WasSwitch { get { return this.wasSwitch; } }
        private bool wasBrdFwd; public bool WasBrdFwd { get { return this.wasBrdFwd; } }


        private Vector3 sktrEulLast; public Vector3 SktrEulLast { get { return this.sktrEulLast; } }
        private Vector3 sktrPosLast; public Vector3 SktrPosLast { get { return this.sktrPosLast; } }
        private float sktrRotMax; public float SktrRotMax { get { return this.sktrRotMax; } }
        private float sktrRot; public float SktrRot { get { return this.sktrRot; } }
        private float sktrFlipMax; public float SktrFlipMax { get { return this.sktrFlipMax; } }
        private float sktrFlip; public float SktrFlip { get { return this.sktrFlip; } }
        private float sktrTwkMax; public float SktrTwkMax { get { return this.sktrTwkMax; } }
        private float sktrTwk; public float SktrTwk { get { return this.sktrTwk; } }


        private Vector3 brdEulLast; public Vector3 BrdEulLast { get { return this.brdEulLast; } }
        private Vector3 brdPosLast; public Vector3 BrdPosLast { get { return this.brdPosLast; } }
        private Vector3 brdStrtPos; public Vector3 BrdStrtPos { get { return this.brdStrtPos; } }
        private float brdRotMax; public float BrdRotMax { get { return this.brdRotMax; } }
        private float brdRot; public float BrdRot { get { return this.brdRot; } }
        private float brdFlipMax; public float BrdFlipMax { get { return this.brdFlipMax; } }
        private float brdFlip; public float BrdFlip { get { return this.brdFlip; } }
        private float brdTwkMax; public float BrdTwkMax { get { return this.brdTwkMax; } }
        private float brdTwk; public float BrdTwk { get { return this.brdTwk; } }

        private bool lateFlip;
        private bool didApex;
        private float brdHeightMax;

        private float ftDwnTime;
        private float trickTimer;
        private float manTime;

        private string crntMan = "";
        private string lastMan = "";
        private float manWait = 0.35f;

        PlayerState_InAir pStateAir;
        void OnEnable()
        {
            //DebugOut.Log(this.GetType().Name + " OnEnable");
            this.lastState = "";
        }

        public void PlyrReset(string p_sender)
        {
            //DebugOut.Log(this.GetType().Name + " PlayerInfo Reset:" + p_sender);
        }

        void LateUpdate()
        {
            if (!Main.enabled || !Main.settings.do_TrackTricks) { return; };
            if (this.guiMan == null)
            {
                //DebugOut.Log(this.GetType().Name + " Waiting on GuiManager");
                this.guiMan = this.gameObject.GetComponent(typeof(GUIM)) as GUIM;
                return;
            }

            if (this.guiTrck == null)
            {
                //DebugOut.Log(this.GetType().Name + " Waiting on PlayerInfo");
                this.guiTrck = this.guiMan.GuiTrck;
                return;
            }

            float chkTime = (Time.time + Time.deltaTime);

            if (this.lastState != SXLH.CrntState)
            {
                //DebugOut.Log("\n" +
                //    "State Changed to : " + SXLH.CrntState + "\n" +
                //    "IsBrdFwd = " + SXLH.IsBrdFwd + "\n" +
                //    "IsSwitch = " + SXLH.IsSwitch + "\n"
                //    );

                switch (SXLH.CrntState)
                {
                    case SXLH.Impact:
                    case SXLH.Manualling:
                    case SXLH.Grinding:
                        this.CheckRot();
                        switch (SXLH.CrntState)
                        {
                            // Need Manual Trick Logic (Rotations while Manualling and another Manual trick I don't recall LOL)
                            case SXLH.Manualling:
                                this.manTime = chkTime;
                                break;
                        };
                        break;

                    case SXLH.BeginPop:
                    case SXLH.Pop:
                    case SXLH.Released:
                    case SXLH.InAir:
                        if (SXLH.CrntState == SXLH.BeginPop) {
                            //this.ResetRots();
                            //this.guiTrck.TrackedTime = chkTime;
                            // Needs some work I've run across some logic in dnSpy that should aid in NoComply detection. Might require a Harmony_Patch
                            //if ((chkTime - ftDwnTime) <= 0.1f) { this.guiMan.AddTrick("NoComply"); };
                        }

                        if (SXLH.CrntState == SXLH.Pop || SXLH.CrntState == SXLH.Released || SXLH.CrntState == SXLH.InAir)
                        {
                            if (SXLH.CrntState == SXLH.Pop) { this.ResetRots();/* this.guiTrck.TrackedTime = chkTime;*/ }

                            //Needs trajectory or at least velocity
                            //if (BrdPos.y < (brdPosLast.y - ((brdPosLast.y - BrdPos.y)/0.75f)))
                            //{
                            //     this.didApex = true;
                            //     brdHeightMax = brdPosLast.y;
                            //};
                            //if (SXLH.CrntState == SXLH.Released && didApex) {  this.lateFlip = true; }
                        }

                        switch (this.lastState)
                        {
                            //case SXLH.BeginPop:
                            //case SXLH.Pop:
                            case SXLH.Riding:
                            case SXLH.Impact:
                            case SXLH.Pushing:
                            case SXLH.Setup:
                            case SXLH.Manualling:
                            case SXLH.Grinding:
                                if (SXLH.CrntState == SXLH.InAir) { this.ResetRots(); }
                                //this.ResetRots();
                                break;
                        };
                        break;

                    case SXLH.Riding:
                    case SXLH.Setup:
                    case SXLH.Bailed:
                    case SXLH.Pushing:
                        // Need to build checks for Mongo, should be able to grab a variable from PlayerController
                    case SXLH.Braking:
                        break;
                };
                this.lastState = SXLH.CrntState;
            };

            switch (SXLH.CrntState)
            {
                case SXLH.Riding:
                    // Check rotation of board with a time check to see if it has maintained a wallride.
                    // Probably difficult due to transitions in ramps causing the same affect.
                    // Mostly when landing sideways or riding across the face of a surface.
                    break;
                //case SXLH.Setup:
                //    break;
                case SXLH.Pushing:
                    this.ftDwnTime = chkTime;
                    break;
                case SXLH.Manualling:
                    this.guiTrck.TrackedTime = chkTime;
                    string chkman = this.GetPrefix(SXLH.Manualling) + "Manual";
                    if ((this.manWait <= (chkTime - this.manTime)) && (this.lastMan == chkman))
                    {
                        if (this.lastMan == chkman)
                        {
                            if (this.crntMan != chkman)
                            {
                                this.crntMan = chkman;
                                this.guiTrck.AddTrick(this.crntMan);
                                this.manTime = chkTime;
                            };
                        }
                        else
                        {
                            this.manTime = chkTime;
                        };
                    }
                    else
                    {
                        this.lastMan = chkman;
                    };
                    break;
            };

            this.UpdateRots();
        }

        public string GetPrefix(string p_crntState)
        {
            return ((SXLH.IsReg && (this.leftWasPop && this.wasSwitch)) || (SXLH.IsGoofy && (this.rightWasPop && this.wasSwitch))) ? "Switch " :
                  (((SXLH.IsReg && (this.leftWasPop && !this.wasSwitch)) || (SXLH.IsGoofy && (this.rightWasPop && !this.wasSwitch)) ? (p_crntState == SXLH.Manualling ? "Nose " : "Nollie ") :
                  (((SXLH.IsReg && (this.rightWasPop && this.wasSwitch)) || (SXLH.IsGoofy && (this.leftWasPop && this.wasSwitch)) ? "Fakie " :
                     ""))));
        }

        private void ResetRots()
        {
            this.leftFrnt = ((!SXLH.IsSwitch && SXLH.IsReg) || (SXLH.IsSwitch && SXLH.IsGoofy));
            this.leftWasPop = SXLH.LeftPop;
            this.rightFrnt = ((SXLH.IsSwitch && SXLH.IsReg) || (!SXLH.IsSwitch && SXLH.IsGoofy));
            this.rightWasPop = SXLH.RightPop;

            this.sktrEulLast = SXLH.SktrEul;
            this.sktrPosLast = SXLH.SktrPos;

            this.sktrRot = 0f;
            this.sktrFlip = 0f;
            this.sktrTwk = 0f;
            this.sktrRotMax = 0f;
            this.sktrFlipMax = 0f;
            this.sktrTwkMax = 0f;

            this.brdEulLast = SXLH.BrdEul;
            this.brdPosLast = SXLH.BrdPos;
            this.brdStrtPos = this.brdPosLast;

            this.brdRot = 0f;
            this.brdFlip = 0f;
            this.brdTwk = 0f;
            this.brdRotMax = 0f;
            this.brdFlipMax = 0f;
            this.brdTwkMax = 0f;

            this.brdHeightMax = 0f;

            this.wasSwitch = SXLH.IsSwitch;
            this.wasBrdFwd = SXLH.IsBrdFwd;

            this.lateFlip = false;
            this.didApex = false;

            this.ftDwnTime = Time.time;
            this.manTime = this.ftDwnTime;

            this.crntMan = "";
            this.lastMan = "";
        }

        private void UpdateRots()
        {
            switch (SXLH.CrntState)
            {
                case SXLH.Riding:
                case SXLH.Impact:
                case SXLH.Bailed:
                case SXLH.Pushing:
                case SXLH.Braking:
                    return;
            };

            this.sktrRot += Mathd.AngleBetween(SXLH.SktrEul.y, this.sktrEulLast.y);
            this.sktrFlip += Mathd.AngleBetween(SXLH.SktrEul.z, this.sktrEulLast.z);
            this.sktrTwk += Mathd.AngleBetween(SXLH.SktrEul.x, this.sktrEulLast.x);

            this.sktrRotMax = ((Mathf.Abs(this.sktrRotMax) < Mathf.Abs(this.sktrRot)) ? this.sktrRot : this.sktrRotMax);
            this.sktrFlipMax = ((Mathf.Abs(this.sktrFlipMax) < Mathf.Abs(this.sktrFlip)) ? this.sktrFlip : this.sktrFlipMax);
            this.sktrTwkMax = ((Mathf.Abs(this.sktrTwkMax) < Mathf.Abs(this.sktrTwk)) ? this.sktrTwk : this.sktrTwkMax);

            this.brdRot += Mathd.AngleBetween(SXLH.BrdEul.y, this.brdEulLast.y);
            this.brdFlip += Mathd.AngleBetween(SXLH.BrdEul.z, this.brdEulLast.z);
            this.brdTwk += Mathd.AngleBetween(SXLH.BrdEul.x, this.brdEulLast.x);

            this.brdRotMax = ((Mathf.Abs(this.brdRotMax) < Mathf.Abs(this.brdRot)) ? this.brdRot : this.brdRotMax);
            this.brdFlipMax = ((Mathf.Abs(this.brdFlipMax) < Mathf.Abs(this.brdFlip)) ? this.brdFlip : this.brdFlipMax);
            this.brdTwkMax = ((Mathf.Abs(this.brdTwkMax) < Mathf.Abs(this.brdTwk)) ? this.brdTwk : this.brdTwkMax);

            //DebugOut.Log("\n" +
            //    "IsBrdFwd = " + SXLH.IsBrdFwd + " : IsSwitch = " + SXLH.IsSwitch + "\n" +
            //    "sktrRotMax = " + this.sktrRotMax + " : this.sktrRot = " + this.sktrRot + "\n" +
            //    "brdRotMax = " + this.brdRotMax + " : this.brdRot = " + this.brdRot + "\n" +
            //    "brdFlipMax = " + this.brdFlipMax + " : this.brdFlip = " + this.brdFlip + "\n\n" +
            //    "IsRightStick = " + PlayerController.Instance.playerSM.GetPopStickSM().IsRightStick + "\n" +
            //    "IsFrontFoot = " + PlayerController.Instance.playerSM.GetPopStickSM().IsFrontFoot + "\n"
            //    );


            this.sktrPosLast = SXLH.SktrPos;
            this.sktrEulLast = SXLH.SktrEul;

            this.brdPosLast = SXLH.BrdPos;
            this.brdEulLast = SXLH.BrdEul;
        }

        private int ClampRot(bool p_isFlipRot, float p_inRot, float p_maxOffset)
        {
            float p_chkRot = Mathf.Abs(p_inRot);
            int outRot;
            switch (p_chkRot)                                     // Comments below were a consideration for allowing a larger leeway window as the rotations increased.
            {
                case var _ when p_chkRot >= (1440f - p_maxOffset):// (p_maxOffset * 2.000f)):
                    outRot = 1440;
                    break;
                case var _ when p_chkRot >= (1260f - p_maxOffset):// (p_maxOffset * 1.750f)):
                    outRot = p_isFlipRot ? 1440 : 1260;
                    break;
                case var _ when p_chkRot >= (1080f - p_maxOffset):// (p_maxOffset * 1.625f)):
                    outRot = 1080;
                    break;
                case var _ when p_chkRot >= (900f - p_maxOffset):// (p_maxOffset * 1.500f)):
                    outRot = p_isFlipRot ? 1080 : 900;
                    break;
                case var _ when p_chkRot >= (720f - p_maxOffset):// (p_maxOffset * 1.375f)):
                    outRot = 720;
                    break;
                case var _ when p_chkRot >= (540f - p_maxOffset):// (p_maxOffset * 1.250f)):
                    outRot = p_isFlipRot ? 720 : 540;
                    break;
                case var _ when p_chkRot >= (360f - p_maxOffset):// (p_maxOffset * 1.125f)):
                    outRot = 360;
                    break;
                case var _ when p_chkRot >= (180f - p_maxOffset):
                    outRot = p_isFlipRot ? 360 : 180;
                    break;
                default:
                    outRot = 0;
                    break;
            };
            return outRot;
        }

        private string DTQCheck(float p_inFlip)
        {
            switch (p_inFlip)
            {
                case (1440f):
                    return "Quad ";
                case (1260f):
                case (1080f):
                    return "Triple ";
                case (900f):
                case (720f):
                    return "Double ";
                case (540f):
                default:
                    return "";
            }
        }

        private void CheckRot()
        {
            if ((Time.time - this.trickTimer) < 0.2f) { /*this.guiTrck.AddTrick("Bonk"); */return; }
            this.trickTimer = Time.time;
            //Shuffle	A sidewards re-entry into a transition, which is then turned fakie
            //Shifty / 9090	Shifting your board 90 degrees, with feet still in contact with the board, then bringing it back to starting position
            //Revert	A trick that is added on to the end of any other trick, and it means to spin one's self and the board 180 after completing the initial trick
            //Alley - oop   Alley - oop A spinning trick on transition that entails spinning to the right while airing to the left, or vice versa
            //Aciddrop	Skating off the end of an object without touching the board with your hands.
            //Burly	A big trick involving lots of potential for pain if it is not pulled off; a skater can be called burly if they are partial to these tricks (Jamie Thomas and his tricks are burly)
            //Gap	A distance between two riding surfaces which skaters ollie over, and often do other more sophisticated tricks over

            float sktrRotMaxOffset = 60f;
            float brdRotMaxOffset = 60f;
            float brdFlipMaxOffset = 45f;

            int clampSRot = this.ClampRot(false,this.sktrRot, sktrRotMaxOffset);
            int clampSRotMax = this.ClampRot(false, this.sktrRotMax, sktrRotMaxOffset);
            int clampBRot = this.ClampRot(false, this.brdRot, brdRotMaxOffset);
            int clampBRotMax = this.ClampRot(false, this.brdRotMax, brdRotMaxOffset);
            int clampBFlip = this.ClampRot(true, this.brdFlip, brdFlipMaxOffset);
            int clampBFlipMax = this.ClampRot(true, this.brdFlipMax, brdFlipMaxOffset);

            DebugOut.Log
            (
                "\n\n\n" +
                "IsBrdFwd = " + SXLH.IsBrdFwd + " : IsSwitch = " + SXLH.IsSwitch + "\n" +
                "sktrRotMax = " + this.sktrRotMax + " : this.sktrRot = " + this.sktrRot + "\n" +
                "brdRotMax = " + this.brdRotMax + " : this.brdRot = " + this.brdRot + "\n" +
                "brdFlipMax = " + this.brdFlipMax + " : this.brdFlip = " + this.brdFlip + "\n\n" +
                "clampSRotMax = " + clampSRotMax + " : clampSRot = " + clampSRot + "\n" +
                "clampBRotMax = " + clampBRotMax + " : clampBRot = " + clampBRot + "\n" +
                "clampBFlipMax = " + clampBFlipMax + " : clampBFlip = " + clampBFlip + "\n\n\n" +
                "forced this should be 180? (clampBFlipMax % 360) = " + (clampBFlipMax % 360) + "\n" +
                "impcaspFlip = ((clampBFlipMax == 0) && (Mathf.Abs(this.brdFlipMax) > 90f)) = " + ((clampBFlipMax == 0) && (Mathf.Abs(this.brdFlipMax) > 90f)) + "\n" +
                "\n\n\n"
            );

            string trickPrefix = this.GetPrefix(SXLH.InAir);

            bool didSktrRot = (clampSRotMax >= 180);
            bool didShuv = (clampBRotMax >= 180);

            // Bs is - w leftFrnt
            // Bs is + w rightFrnt
            // Fs is + w leftFrnt
            // Fs is - w rightFrnt
            // Bs is - w leftFrnt
            // Bs is + w rightFrnt
            // Fs is + w leftFrnt
            // Fs is - w rightFrnt

            string sktrDir;
            string brdDir;
            switch (this.leftFrnt)
            {
                case true:
                    sktrDir = (didSktrRot ? ((this.sktrRotMax < 0f) ? "Bs" : "Fs") : "");
                    brdDir = (didShuv ? ((this.brdRotMax < 0f) ? "Bs" : "Fs") : "");
                    break;
                default:
                    sktrDir = (didSktrRot ? ((this.sktrRotMax > 0f) ? "Bs" : "Fs") : "");
                    brdDir = (didShuv ? ((this.brdRotMax > 0f) ? "Bs" : "Fs") : "");
                    break;
            };

            bool sameDir = (sktrDir == brdDir);
            //bool diffDir = (((this.brdRotMax > 0f) && (this.sktrRotMax < 0f)) || ((this.brdRotMax < 0f) && (this.sktrRotMax > 0f)) || ((this.brdRotMax == 0f) && (this.sktrRotMax != 0f)) || ((this.brdRotMax != 0f) && (this.sktrRotMax == 0f)));


            bool didFlip = (clampBFlipMax >= 360);
            //int flipRemain = (clampBFlipMax % 360);
            bool forcedFlip = ((Mathf.Abs(this.brdFlipMax) - Mathf.Abs(this.brdFlip)) <= 180); // Should be within 180 less than clampBFlipMax
            //bool impcaspFlip = ((clampBFlipMax == 0) && (Mathf.Abs(this.brdFlipMax) > 90f)); // impossible Bs casper Fs w/ half kick // May need to be 180 - brdFlipMaxOffset for true imp/casp

            bool illusionAngle = (Mathf.Abs(this.brdTwkMax) >= 45f && clampSRotMax == 180);
            bool dolphinAngle = (Mathf.Abs(this.brdTwkMax) >= 45f && clampSRotMax == 0);

            // kick is - w leftFrnt w brdfwd
            // kick is + w leftFrnt w !brdfwd
            // kick is - w rightFrnt w brdfwd
            // kick is + w rightFrnt w !brdfwd
            // heel is - w leftFrnt w !brdfwd
            // heel is + w leftFrnt w brdfwd
            // heel is - w rightFrnt w !brdfwd
            // heel is + w rightFrnt w brdfwd

            bool kick;
            bool heel;
            switch (this.wasBrdFwd)
            {
                case true:
                    kick = (didFlip && (SXLH.IsReg && this.brdFlipMax < 0f) || (SXLH.IsGoofy && this.brdFlipMax > 0f));
                    heel = (didFlip && (SXLH.IsReg && this.brdFlipMax > 0f) || (SXLH.IsGoofy && this.brdFlipMax < 0f));
                    break;
                default:
                    kick = (didFlip && (SXLH.IsReg && this.brdFlipMax > 0f) || (SXLH.IsGoofy && this.brdFlipMax < 0f));
                    heel = (didFlip && (SXLH.IsReg && this.brdFlipMax < 0f) || (SXLH.IsGoofy && this.brdFlipMax > 0f));
                    break;

            }

            string dtqShuv = this.DTQCheck(clampBFlipMax);
            string dtqFlip = this.DTQCheck(clampBFlipMax);

            string kickType = ((clampBFlipMax == clampBRotMax) || ((clampBFlipMax - clampBRotMax) == (clampBFlipMax / 2)) ? "" : "Kick") + "Flip";
            string flipType = kick ? kickType : heel ? "HeelFlip" : "";//(didFlip ? (halfFlip ? "Forced " : "") + (this.lateFlip ? "Late " : "") + dtqFlip + (kick ? kickType : (heel ? "HeelFlip" : "")) : "");

            bool skpBrdDir = false;
            bool skpBrdRot = false;
            bool skpBrdFlip = false;

            string bodyOut = "";
            if (didSktrRot)
            {
                if (sameDir)
                {
                    if (clampSRotMax == clampBRotMax && /*sktrDir == "Bs" && */(trickPrefix.Contains("Fakie")|| trickPrefix.Contains("Nollie")))
                    {
                        bodyOut = (trickPrefix.Contains("Fakie") ? (clampBRotMax < 360 ? "HalfCab" : "Caballerial") : "HeliPop");
                        skpBrdDir = true;
                        if ((clampBRotMax < 540 && trickPrefix.Contains("Fakie")) || (clampBRotMax < 360 && trickPrefix.Contains("Nollie"))) { skpBrdRot = true; }
                        trickPrefix = "";
                    }
                    else if (clampSRotMax == 180 && (clampBRotMax == 360 || clampBRotMax == 540))
                    {
                        bodyOut = "Big" + (clampBRotMax == 540 ? "ger" : "") + (/*(clampBRotMax == clampBFlipMax) ? "" : */" Spin");
                        skpBrdRot = true;
                    }
                    else if (clampSRotMax == 360 && (clampBRotMax == 540 || clampBRotMax == 720))
                    {
                        bodyOut = (clampBRotMax == 720 ? "Big " : "") + "Gazelle";
                        skpBrdRot = true;
                    }
                    else if (clampSRotMax == 720 && clampBRotMax == 720 && dtqFlip.Length > 0)
                    {
                        bodyOut = "Bomb Flip";
                        skpBrdRot = true;
                        skpBrdFlip = true;
                    }
                    else if (clampSRotMax >= 180 && clampSRotMax != clampBRotMax)
                    {
                        bodyOut = clampSRotMax + " BVar";
                        //if (Main.settings.skp_Erron)
                        //{
                        //    bodyOut = "";
                        //    skpBrdRot = true;
                        //}
                    }
                }
                else
                {
                    if (didFlip && clampSRotMax == 180)
                    {
                        if (clampBRotMax == 0)
                        {
                            bodyOut = dtqFlip + (sktrDir == "Fs" ? "Royal " : "Disco ") + "Flip";
                            skpBrdDir = true;
                            skpBrdRot = true;
                            skpBrdFlip = true;
                        }
                        else if (clampBRotMax == clampBFlipMax)
                        {
                            bodyOut = dtqFlip + "Follow " + "Flip";
                            skpBrdRot = true;
                            skpBrdFlip = true;
                        }
                        else if ((clampBFlipMax - clampBRotMax) == (clampBFlipMax / 2))
                        {
                            bodyOut = dtqFlip + /*(lateSpin ? "Ghetto Bird" : "") +*/ "Techno " + "Flip";
                            skpBrdRot = true;
                            skpBrdFlip = true;
                        }

                    }
                    else if (didFlip && clampSRotMax == 360)
                    {
                        if (clampBRotMax == 0)
                        {
                            bodyOut = dtqFlip + (sktrDir == "Fs" ? "Royal " : "Disco ") + "Flip";
                            skpBrdDir = true;
                            skpBrdRot = true;
                            skpBrdFlip = true;
                        }
                        else if (clampBRotMax == clampBFlipMax)
                        {
                            bodyOut = dtqFlip + (kick ? "Mother " : "Father ") + "Flip";
                            skpBrdRot = true;
                            skpBrdFlip = true;
                        }
                        else if ((clampBFlipMax - clampBRotMax) == (clampBFlipMax / 2))
                        {
                            bodyOut = dtqFlip + (kick ? "Heaven " : "Hell ") + "Flip";
                            skpBrdRot = true;
                            skpBrdFlip = true;
                        }
                    }
                    else if (trickPrefix.Contains("Fakie") && clampSRotMax == 360 && clampBRotMax == 360 && !didFlip)
                    {
                        bodyOut = "Izzy Spin";
                        trickPrefix = "";
                        skpBrdRot = true;
                    }
                    else if (clampSRotMax != 0 && clampSRotMax != clampBRotMax)
                    {
                        bodyOut = (kick || heel ? "" : "Twisted ") + clampSRotMax + (kick || heel ? " Sex Change" : "");
                        //if (Main.settings.skp_Erron)
                        //{
                        //    bodyOut = "";
                        //    skpBrdRot = true;
                        //}
                    }
                }
            }

            string flipOut = "";
            if (!didShuv && didFlip)
            {
                flipOut = dtqFlip + flipType;
            }
            else if (didShuv && !didFlip)
            {
                flipOut = (!skpBrdRot ? (clampBRotMax >= 360 || (clampBRotMax >= 180 && sameDir && clampSRotMax == clampBRotMax) ? clampBRotMax.ToString() + " " : "") + ((!sameDir || clampSRotMax != clampBRotMax) ? "Shove It" : ""): "");
            }
            else if (didShuv && didFlip)
            {
                if ((clampBFlipMax - clampBRotMax) == (clampBFlipMax / 2))
                {
                    if (brdDir == "Fs")
                    {
                        flipOut = (kick ? (dtqFlip.Length > 0 ? "Nightmare " : "") + ((trickPrefix.Contains("Fakie") || trickPrefix.Contains("Nollie")) ? "Gingersnap" : "HardFlip") : (dolphinAngle ? "Dolphin " + (dtqFlip.Length > 0 ? "Nightmare " : "") : (dtqFlip.Length > 0 ? "Nightmare " : "Varial")) + "HeelFlip");
                        if (trickPrefix.Contains("Fakie") || trickPrefix.Contains("Nollie")) { trickPrefix = ""; };
                    }
                    else if (brdDir == "Bs")
                    {
                        flipOut = (kick ? (dolphinAngle ? "Dolphin " + (dtqFlip.Length > 0 ? "Nightmare " : "") + "Flip" : (dtqFlip.Length > 0 ? "Nightmare Flip" : "Varial KickFlip")) : (dtqFlip.Length > 0 ? "Nightmare HeelFlip" : "Inward HeelFlip"));
                    }
                    skpBrdDir = true;
                    skpBrdRot = true;
                }
                else if (clampBFlipMax == clampBRotMax)
                {
                    if (brdDir == "Fs")
                    {
                        flipOut = dtqFlip + (kick ? (trickPrefix.Contains("Fakie") || trickPrefix.Contains("Nollie")) ? "Gingersnap" : "HardFlip" : "Laser Flip");
                        if (trickPrefix.Contains("Fakie") || trickPrefix.Contains("Nollie")) { trickPrefix = ""; };
                    }
                    else if (brdDir == "Bs")
                    {
                        flipOut = dtqFlip + (kick ? "Tre Flip" : "Tre HeelFlip");
                    }
                    skpBrdDir = true;
                    skpBrdRot = true;
                }
                else
                {
                    flipOut = (!skpBrdRot ? (clampBRotMax >= 360 || (clampBRotMax >= 180 && sameDir && clampSRotMax == clampBRotMax) ? clampBRotMax.ToString() + " " : "") + ((!sameDir || clampSRotMax != clampBRotMax) ? "Shove It" : "") + (flipType.Length > 0 ? " " : ""): "") + dtqFlip + flipType;
                    //if (Main.settings.skp_Erron)
                    //{
                    //    skpBrdRot = true;
                    //    skpBrdFlip = true;
                    //}
                }
            }

            string trckOut = (skpBrdDir ? "" : brdDir) + (!skpBrdDir &&  brdDir.Length > 0  ? " " : "") + bodyOut + (!skpBrdFlip && (bodyOut.Length > 0 && flipOut.Length > 0) ? " " : "") + (skpBrdFlip ? "" : flipOut);

            if (trckOut.Length > 0)
            {
                this.guiTrck.AddTrick(trickPrefix + trckOut);
            }

            this.ResetRots();
        }
    }
}
