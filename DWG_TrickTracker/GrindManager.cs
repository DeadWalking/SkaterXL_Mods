using System;
using System.Collections.Generic;
using UnityEngine;
using XLShredLib;

namespace DWG_TT
{
    class GM : MonoBehaviour
    {
        private GUIM guiMan;
        private GUITrick guiTrck;

        public float GrindDistA { get; private set; }
        public float GrindDistB { get; private set; }
        public float GrndLeft { get; private set; }
        public float GrindLngt { get; private set; }
        public double EdgeFwdAngl { get; private set; }
        public double EdgeUpAngl { get; private set; }
        public double HghtDiff { get; private set; }
        public double DistX { get; private set; }
        public double DistZ { get; private set; }
        public float SpdAdjst { get; private set; }
        public float SideStrt { get; private set; }
        public bool NoSwap { get; private set; }
        public bool SameSide { get; private set; }
        public bool MetalGrnd { get; private set; }
        public bool ToeFwd { get; private set; }
        public string CrntGrind { get; private set; }
        public string GrndSide { get; private set; }

        private string lastState;
        private string lastGrind;
        private List<string> allGrnds;

        private float frmTimer;
        private float grndTime;
        private float bonkTime;

        private const float bonkWait = 0.20f;

        private Vector3 brdStartPos;
        private Vector3 brdCheckPos;

        private int nID = GrndTrigs.NTrig;
        private int ftID = GrndTrigs.FTTrig;
        private int brdID = GrndTrigs.BrdTrig;
        private int btID = GrndTrigs.BTTrig;
        private int tID = GrndTrigs.TTrig;

        public bool NoseTrigg { get; private set; }
        public bool FrntTrckTrigg { get; private set; }
        public bool BrdTrigg { get; private set; }
        public bool BckTrckTrigg { get; private set; }
        public bool TailTrigg { get; private set; }

        // 0 = Nose
        // 1 = Tail
        private int nOrTWheel = 0;

        public bool FLWheel { get; private set; }
        public bool FRWheel { get; private set; }
        public bool RLWheel { get; private set; }
        public bool RRWheel { get; private set; }

        public bool WasSwitch { get; private set; }

        private int noseFivCnt;
        private int noseTailCnt;
        private int brdCnt;

        private int speedMulti = 3;

        public void OnEnable()
        {
            //DebugOut.Log(this.GetType().Name + " Enable");
            this.lastState = "";
            this.CrntGrind = "";
            this.lastGrind = "";
            this.allGrnds = new List<string>();

            this.EdgeFwdAngl = 0f;
            this.EdgeUpAngl = 0f;

            this.frmTimer = 0f;
            this.grndTime = 0f;
            this.bonkTime = 0f;
        }

        public void OnDestroy()
        {
            //DebugOut.Log(this.GetType().Name + " OnDestroy");
        }

        public void FixedUpdate()
        {
            if (!Main.enabled || !Main.settings.do_TrackTricks) { return; }

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

            if (Input.GetKey(KeyCode.LeftControl) && (Time.time - this.frmTimer) > 0.1f)
            {
                this.frmTimer = Time.time + Time.deltaTime;
                if (Input.GetKey(KeyCode.Period))
                {
                    this.speedMulti = ((this.speedMulti - 1) < 1 ? 1 : --this.speedMulti);
                    this.guiTrck.AddTrick("SpeedMulti: " + this.speedMulti);
                }
                if (Input.GetKey(KeyCode.Comma))
                {
                    this.guiTrck.AddTrick("SpeedMulti: " + (++this.speedMulti));
                }
            }

            float chkTime = Time.time;
            float brdSpeed = Mathf.Clamp(SXLH.BrdSpeed, 1f, 4f);

            // Grind Enter
            if (SXLH.CrntState == SXLH.Grinding && this.lastState != SXLH.Grinding)
            {
                this.allGrnds.Clear();
                this.grndTime = chkTime;
                this.frmTimer = chkTime;
                this.bonkTime = chkTime;
                this.GrindLngt = 0f;
                this.noseFivCnt = 0;
                this.noseTailCnt = 0;
                this.brdCnt = 0;

                this.brdCheckPos = GrndTrigs.GetTrigPos(GrndTrigs.BrdTrig);

                this.GetGrindLeft(this.brdCheckPos);

                this.NoSwap = SXLH.NoSwap;
                this.nID = this.NoSwap ? GrndTrigs.NTrig : GrndTrigs.TTrig;
                this.ftID = this.NoSwap ? GrndTrigs.FTTrig : GrndTrigs.BTTrig;
                this.btID = this.NoSwap ? GrndTrigs.BTTrig : GrndTrigs.FTTrig;
                this.tID = this.NoSwap ? GrndTrigs.TTrig : GrndTrigs.NTrig;

                this.nOrTWheel = (this.nID == GrndTrigs.NTrig ? 0 : 1);

                //Multiply by 1000 to get a whole number representation in what should be millimeters.
                //Allows for easier checks > < = to 0. Extremely small floats or doubles between -1.0 and 1.0 are hard to test against 0
                this.SideStrt = (Vector3.Dot(SXLH.GrindSplnRght, (SXLH.GrindSplnPos - this.brdStartPos).normalized) * 1000);

                this.MetalGrnd = PlayerController.Instance.IsCurrentGrindMetal();

                this.GrndSide = SXLH.GrndSide;
            }

            // Grind Stay
            else if (SXLH.CrntState == SXLH.Grinding && this.lastState == SXLH.Grinding)
            {

                this.guiTrck.TrackedTime = chkTime;
                this.brdCheckPos = GrndTrigs.GetTrigPos(GrndTrigs.BrdTrig);

                this.GrndLeft = this.GetGrindLeft(this.brdCheckPos);

                if (this.GrindLngt == 0f && this.GrndLeft != 0f) { this.GrindLngt = this.GrndLeft; }

                this.SpdAdjst = Mathf.Clamp((((this.GrindLngt - 6f) / 100) / (brdSpeed * this.speedMulti)), 0.25f, 1.0f);

                if (SXLH.BrdSpeed < 0.8f && SXLH.BrdSpeed > 0.2f) {
                    bool didStall = false;
                    for (int i = 0; i < this.allGrnds.Count; i++)
                    {
                        if (this.allGrnds[i] == "Grind Stall") { didStall = true; break; }
                    }
                    if (!didStall) { this.allGrnds.Add("Grind Stall"); this.guiTrck.AddTrick("Grind Stall"); }
                }
                else if (this.GrndLeft > 3f)
                {
                    if ((chkTime - this.grndTime) >= this.SpdAdjst)
                    {
                        this.CrntGrind = this.GrndCheck();

                        if (this.CrntGrind.Length > 0)
                        {
                            if (this.lastGrind == this.CrntGrind && !this.allGrnds.Contains(this.CrntGrind))
                            {
                                this.allGrnds.Add(this.CrntGrind);
                                this.guiTrck.AddTrick(((SXLH.IsSwitch ? "Switch " : "") + (SXLH.FrntSide ? "Fs " : "Bs ")) + this.CrntGrind);
                                this.grndTime = chkTime + Time.deltaTime;
                                this.GrindLngt = this.GrndLeft;
                            }
                        }
                        this.lastGrind = this.CrntGrind;
                    }
                }
            }

            // Grind Exit
            else if (SXLH.CrntState != SXLH.Grinding && this.lastState == SXLH.Grinding)
            {
                if ((chkTime - this.bonkTime) < bonkWait) { this.bonkTime += 100; this.guiTrck.AddTrick("Bonk"); }
            }

            // Last Pos
            else if (SXLH.CrntState != SXLH.Grinding && this.lastState != SXLH.CrntState)
            {
                if (SXLH.CrntState == SXLH.BeginPop || (SXLH.CrntState == SXLH.InAir && this.lastState == SXLH.Grinding)) {
                    this.brdStartPos = SXLH.BrdPos;
                    this.WasSwitch = SXLH.IsSwitch;
                }
            }
            this.lastState = SXLH.CrntState;
        }

        private float GetGrindLeft(Vector3 p_brdPos)
        {
            Dreamteck.Splines.SplinePoint[] splnPnts = PlayerController.Instance.boardController.triggerManager.spline.GetPoints();
            float distA = (Vector3.Distance(splnPnts[0].position, p_brdPos) * 100);
            float distB = (Vector3.Distance(splnPnts[(splnPnts.Length - 1)].position, p_brdPos) * 100);

            float closestDist = ((this.GrindDistA > distA) ? distA : (this.GrindDistB > distB) ? distB : SXLH.GrindLngt);

            this.GrindDistA = distA;
            this.GrindDistB = distB;

            return closestDist;
        }

        public bool NoneDown()
        {
            return (!this.FLWheel && !this.FRWheel && !this.RLWheel && !this.RRWheel);
        }

        public bool OneDown()
        {
            return
            (
                (this.FLWheel && !this.FRWheel && !this.RLWheel && !this.RRWheel) ||
                (!this.FLWheel && this.FRWheel && !this.RLWheel && !this.RRWheel) ||
                (!this.FLWheel && !this.FRWheel && this.RLWheel && !this.RRWheel) ||
                (!this.FLWheel && !this.FRWheel && !this.RLWheel && this.RRWheel)
            );
        }

        private string GrndCheck()
        {
            // Salad Grind	A grind on either truck, with the opposite end of the board pointing forward, up, and towards the obstacle
            // Pressure Flip	Popular a few years ago, and still today with a lot of grommets, it entails putting pressure on the correct spot of the tail to make it flip around without hardly leaving the ground
            // Pivot Grind A trick or part of a trick where the back truck is grinding atop an obstacle for just a moment before the trick's completion
            // Nose Pick	A stall in the nose grind position, usually involving an indy grab for control
            // Lock in	The act of getting your board into a slide or grind position in a way that is most stable, allowing for a lengthy maneuver
            // Anchor // grind	A grind on the front truck, with the tail pointing back, down, and away from the obstacle
            // Hurricane // Fs 180 into FsFeeble A grind starting with an ollie 180, then the back truck (which is now in front) is placed on the obstacle with the nose pointing back, down and towards the obstacle
            // Barley  // grind	An ollie 180 to switch smith grind *Named after Donny Barley
            // Bennet  // backside 180 to switch back smith
            // BsDarkslide; // Bs entrance upside down board grind
            // FsDarkslide; // Fs entrance upside down board grind
            // Kinked Rail	A rail that is kinked or bent, increasing its difficulty
            // Slappie	A grind on a ledge without ollieing
            // Feeble Grind	A grind on the back truck, with the nose pointing forward, down, and toward the obstacle
            // Smith Grind A grind on the back truck, with the nose pointing forward, down, and away from the obstacle
            // 5-0 overturn
            // Nosegrind overturn
            // Primo slides/grinds
            //           When in rail stance and slide on the ledge slash rail.Primo grind in when you grind on both wheels/ bearings in rail stance.
            // 5 - 0 grind
            //          Pronounced "Five-Oh".In this maneuver, the back truck grinds the rail/ edge, while the front truck is suspended directly above the rail / edge.This move is similar to the manual, although the tail may be scraped against the obstacle as well as the back truck, which is not considered proper on a manual.
            // Nosegrind
            //          In a Nosegrind, the skateboard's front truck grinds a rail or edge, while the back truck is suspended over the rail/edge. It is similar to the nose manual, except performed on a rail, coping, or ledge. This move originated on vert, initially in the form of Neil Blender's New Deal(nose pivot to disaster), then by his more advanced progression of said move, the "Newer Deal", which left out the disaster part and just pivoted all the way back in.
            //          A skateboarder performing a nosegrind
            // Crooked grind
            //          Also known as Crooks, Crux, Pointer Grind, or the K-grind after the man to whom the trick is most commonly accredited, Eric Koston. It is like a nosegrind, but the tail of the board is angled away from the rail/ ledge on which the trick is performed, causing the edge of the deck's nose to also rub. Due to the lack of historical evidence there is no way to prove when this trick was first landed, nevertheless, both Eric Koston and Dan Peterka were the first two skaters documented purposely performing this trick in their respective H-Street's Next Generation video parts.As a result, both skateboarders are regarded as co - creators of this trick.[3]
            // Overcrook grind
            //          Also known as Overcrooks, or Overcrux, is similar to the Crooked grind, but with the tail of the board being angled towards the far side of the rail/ box.
            // Feeble grind
            //          In this move, the back truck grinds a rail while the front truck hangs over the rail's far side. Professional skateboarer Josh Nelson is the inventor of this grind back in 1986 at the Del Mar skate ranch in Del Mar, California. The name feeble grind came from Josh Nelson's friend and fellow skateboarder Sean Donnelley.Sean used to call Josh "the feeb" which was short for feeble, because Nelson was so skinny and often had broken limbs and injuries from skateboarding.Many people watched Josh create this unique type of grind on the parking blocks that were mounted in the reservoir at the Del Mar Skate Ranch.
            //          A skateboarder performing a Smith grind
            // Smith grind
            //          This maneuver entails the back truck grinding an edge or rail, while the front truck hangs over the near side of the object, leaving the edge of the deck to rub the lip / edge.This trick was named after its inventor Mike Smith(skateboarder). It is considered by many to be the most difficult basic grind trick.The backside version known as a Monty Grind was originated by Florida powerhouse Monty Nolder.
            // Lazy/Willy grind
            //          Popularized by Willy Santos. Very rarely executed, the Willy is done with the front truck sliding on the grinding surface(as in a nose grind) while the back truck hangs down below the surface on the side to which the skateboarder approached.Also called "Lazy Grind", "Nosesmith", or "Scum Grind."
            // Whatchamajig/Losi grind Losi called it a Whatchamajig
            //          Popularized by Allen Losi. Can be best described as an Overcrook with the tail hanging below a rail or with wheels sliding along on a ledge or as a Willy with the tail on the other side of the obstacle.Also called "Nosefeeble," "Over Willy Grind" or "Over-Scum."
            // Suski grind
            //          Popularized by Aaron Suski. This is also very similar to the 5 - 0 but unlike the salad grind your front trucks are pointed towards you like a smith grind but above the ledge unlike the smith grind. Simply a raised smith grind.
            // Salad grind
            //          A cross between a bluntslide and a 5 - 0, the front truck is suspended over the far side of the rail/ edge the grind is performed on and pointed upwards to where the tail may even touch and slide on the side of the grinding surface.Like the "overcrook" grind is like a crooked nosegrind, the Salad grind is like a crooked 5 - 0, or a combo 5 - 0 / bluntslide.This trick has been invented by Eric Dressen, hence the name(dressen, dressing, salad dressing).Simply a raised feeble.
            // Hurricane grind
            //          A 180 degree turn into a switch Over - Willy, exiting via a little less than 180 degree return spin.This trick was invented on vert by Neil Blender in 1985; an early proto - version can by witnessed in Powell Peralta's second video, Future Primitive, during Blender's brief cameo appearance on Lance Mountain's backyard ramp. Many of today's pros also do it on street obstacles such as handrails and ledges. This trick is easier to perform backside, but Tony Hawk did introduce the rarer frontside version in 1989.
            // Sugarcane grind
            //              This trick can be described as a 180 into switch Willy[4]
            // Bennett grind
            //              The same as the Hurricane and Sugarcane grinds, except instead of landing into a switch Over - Willy or Willy, land into a switch smith grind.Named after Matt Bennett.[5]
            // Grapefruit grind
            //          It is a frontside 180 to switch backside feeble.
            // Carousel
            //          This is a specific Truck - To - Truck Transfer.Think of it as a half Impossible from a 50 / 50 to a switch 50 / 50 – still standing on the back foot. The rider starts from a 50 / 50, "throws" the board over the foot that stands on the truck and jumps up.When the board has done the "half wrap", the rider lands on the truck and catches the nose of the board with the same hand he used to flip it. Marco Sassi became the first person in the world to do a 360 Carousel in 2014, successfully completing a full impossible around the foot to land back in the original 50 - 50 position.To date(July 2015), only two other freestylers have managed to do the same.


            double crntFwdAngle = (double)Vector3.SignedAngle(Vector3.ProjectOnPlane((this.NoSwap ? SXLH.BrdFwd : -SXLH.BrdFwd), SXLH.GrindSplnUp), SXLH.GrindSplnFwd, SXLH.GrindSplnUp);
            double crntFwdAngleAbs = Math.Abs(crntFwdAngle);
            double crntUpAngl = Math.Abs((double)Vector3.SignedAngle(SXLH.BrdUp, Vector3.up, Vector3.up));

            //Multiply by 1000 to get a whole number representation in what should be millimeters.
            //Allows for easier checks > < = to 0.***. Extremely small floats or doubles between -0.09 and 0.09 are hard to test against 0
            double heightDiff = (FNCS.GetHghtDiff(this.brdCheckPos, SXLH.GrindSplnPos) * 1000);
            double distX = (Math.Abs((double)(SXLH.GrindSplnPos.x - this.brdCheckPos.x)) * 1000);
            double distZ = (Math.Abs((double)(SXLH.GrindSplnPos.z - this.brdCheckPos.z)) * 1000);
            double dotProd = ((double)Vector3.Dot(SXLH.GrindSplnRght, (SXLH.GrindSplnPos - this.brdCheckPos).normalized) * 1000);

            // 33 is the height that the board trigger is at when all 4 wheels are flat on a surface.
            // 13-14 is the height that the board trigger is at with middle of trucks contact in fiftyfifty grind, happens on a ledge with 2 wheels down.
            // 4-5 is the height that the board trigger is at with middle of trucks contact in fiftyfifty grind, happens on a rail with no wheels down.
            // -45-50 is the height that the board trigger is at with middle of board contact when in boardslide, happens on a rail with no wheels down.
            if (heightDiff <= 33) { crntUpAngl *= -1; }


            bool angleGrndMinDist = (Math.Abs(distX + distZ) >= 75);
            bool brdSlideMaxDist = (Math.Abs(distX - distZ) <= 155);
            bool isSameSide = (dotProd >= 0 && this.SideStrt >= 0) || (dotProd < 0 && this.SideStrt < 0);
            bool toeIsFwd = Vector3.SignedAngle(Vector3.ProjectOnPlane(SXLH.BrdEdgeFwd, SXLH.GrindUp), SXLH.GrindDir, SXLH.GrindUp) >= 0;
            if (SXLH.IsGoofy) { toeIsFwd = !toeIsFwd; }

            this.NoseTrigg = GrndTrigs.Hit[this.nID];
            this.FrntTrckTrigg = GrndTrigs.Hit[this.ftID];
            this.BrdTrigg = GrndTrigs.Hit[this.brdID];
            this.BckTrckTrigg = GrndTrigs.Hit[this.btID];
            this.TailTrigg = GrndTrigs.Hit[this.tID];

            this.FLWheel = BoardCon.GetWheelDown(((this.nOrTWheel == 0) ? 2 : 1));
            this.FRWheel = BoardCon.GetWheelDown(((this.nOrTWheel == 0) ? 3 : 0));
            this.RLWheel = BoardCon.GetWheelDown(((this.nOrTWheel == 0) ? 0 : 3));
            this.RRWheel = BoardCon.GetWheelDown(((this.nOrTWheel == 0) ? 1 : 2));

            if (Main.settings.con_Enable)
            {
                this.EdgeFwdAngl = crntFwdAngle;
                this.EdgeUpAngl = crntUpAngl;
                this.HghtDiff = heightDiff;
                this.DistX = distX;
                this.DistZ = distZ;
                this.ToeFwd = toeIsFwd;
                this.SameSide = isSameSide;
            }

            bool highGrnd = (crntUpAngl >= 5);
            bool lowGrnd = (crntUpAngl < 5);
            bool exLowGrnd = (crntUpAngl < -15);

            bool canBrdLipSlide = (this.BrdTrigg && !this.NoseTrigg && !this.TailTrigg && brdSlideMaxDist);

            bool canBluntSlide = ((this.NoseTrigg && this.FrntTrckTrigg && !this.BckTrckTrigg && !this.TailTrigg) ||
                                  (!this.NoseTrigg && !this.FrntTrckTrigg && this.BckTrckTrigg && this.TailTrigg) ||
                                  (SXLH.TwoDown &&
                                        ((this.NoseTrigg || this.FrntTrckTrigg) && !this.BckTrckTrigg && !this.TailTrigg) ||
                                        (!this.NoseTrigg && !this.FrntTrckTrigg && ((this.BckTrckTrigg || this.TailTrigg)))));
            //              FrntTrck   BckTrck
            // Fs Small Neg Crook/Lazy Salad/Feeble HeelFwd
            // Fs Small Pos Over/Losi  Suski/Smith  ToeFwd

            //              FrntTrck   BckTrck
            // Bs Small Neg Over/Losi  Suski/Smith  HeelFwd
            // Bs Small Pos Crook/Lazy Salad/Feeble ToeFwd

            //              FrntTrck   BckTrck
            // Fs Small Neg Crook/Lazy Salad/Feeble HeelFwd
            // Fs Small Pos Over/Losi  Suski/Smith  ToeFwd
            //              FrntTrck   BckTrck
            // Bs Small Neg Over/Losi  Suski/Smith  HeelFwd
            // Bs Small Pos Crook/Lazy Salad/Feeble ToeFwd

            // Fs ToeFwd
            // OverCrook Suski Smith Losi
            // Fs HeelFwd
            // Crooked Salad Lazy Feeble

            // Fs FR Crook/OverCrook
            // Fs FL Suski/Salad
            // Fs RR Suski/Salad
            // Fs RL Crook/OverCrook

            // Bs ToeFwd
            // Crook Salad Lazy Feeble
            // Bs HeelFwd
            // OverCrook Suski Smith Losi

            // Bs FL Crook/OverCrook
            // Bs FR Suski/Salad
            // Bs RL Suski/Salad
            // Bs RR Crook/OverCrook

            bool fiftyfiftyRange = ((crntFwdAngleAbs <= 10) && (heightDiff >= 0) && (heightDiff <= 33));
            bool enterRange = (crntFwdAngleAbs < 20);
            bool smallRange = ((crntFwdAngleAbs >= 20) && (crntFwdAngleAbs <= 60));
            bool perpRange = ((crntFwdAngleAbs > 60) && (crntFwdAngleAbs < 120));
            bool largeRange = ((crntFwdAngleAbs >= 120) && (crntFwdAngleAbs <= 160));
            bool exitRange = (crntFwdAngleAbs > 160);
            // crntFwdAngle NoseTrigg Samll Pos is Right hand side of grind
            // crntFwdAngle NoseTrigg Small Neg is Left hand side of grind
            // crntFwdAngle TailTrigg Samll Neg is Right hand side of grind
            // crntFwdAngle TailTrigg Small Pos is Left hand side of grind
            // crntFwdAngleAbs == largeRange switch (nose trick with tail trick) or (tail trick with nose trick)
            // crntFwdAngleAbs == exitRange might be unneeded

            //if (SXLH.BrdSpeed < 0.8f && SXLH.BrdSpeed > 0.2f) { return "Grind Stall"; }

            if (perpRange)
            {
                if (highGrnd && canBluntSlide)
                {
                    return ((this.NoseTrigg || this.FrntTrckTrigg) ? (isSameSide ? "Noseslide" : "NoseBluntslide") : ((this.BckTrckTrigg || this.TailTrigg) ? (isSameSide ? "Tailslide" : "Bluntslide") : ""));
                }
                if (!highGrnd)
                {
                    if (this.NoneDown())
                    {
                        if (canBrdLipSlide && (heightDiff <= -40))
                        {
                            return ("Boardslide");
                        }
                        else if (canBrdLipSlide && (heightDiff <= -40) && (this.FrntTrckTrigg || this.BckTrckTrigg))
                        {
                            return (exLowGrnd ? "Anchor" : "Boardslide");
                        }
                        else if ((this.NoseTrigg && !this.TailTrigg) || (!this.NoseTrigg && this.TailTrigg))
                        {
                            return (this.NoseTrigg ? "Noseslide" : (this.TailTrigg ? "Tailslide" : ""));
                        }
                    }
                    else if(canBrdLipSlide && SXLH.TwoDown && (this.FrntTrckTrigg || this.BckTrckTrigg || this.BrdTrigg))
                    {
                        return (exLowGrnd ? "Anchor" : "Lipslide");
                    }
                }
            }
            else if (angleGrndMinDist && (smallRange || largeRange) && (
                    ((this.FrntTrckTrigg && this.BrdTrigg) || (this.BckTrckTrigg && this.BrdTrigg)) ||
                    ((this.FrntTrckTrigg || this.BckTrckTrigg) && (this.OneDown() || (this.NoneDown() && this.MetalGrnd))) ||
                    ((this.FrntTrckTrigg || this.BckTrckTrigg) && SXLH.TwoDown)))
            {
                if (smallRange)
                {
                    if (highGrnd)
                    {
                        return (this.FrntTrckTrigg ? (isSameSide ? "Crooked" : "OverCrook") : (this.BckTrckTrigg ? (isSameSide ? "Suski" : "Salad") : ""));
                        //return (this.FrntTrckTrigg ? ((crntFwdAngle >= 0 && isSameSide) ? "Crooked" : ((crntFwdAngle < 0 && !isSameSide) ? "OverCrook" : "")) : (this.BckTrckTrigg ? ((crntFwdAngle <= 0 && isSameSide) ? "Suski" : ((crntFwdAngle > 0 && !isSameSide) ? "Salad" : "")) : ""));
                    }
                    else if (lowGrnd)
                    {
                        return (this.FrntTrckTrigg ? (isSameSide ? "Lazy/Willy" : "Losi/Whatchamajig") : (this.BckTrckTrigg ? (isSameSide ? "Smith" : "Feeble") : ""));
                    }
                }
                else if (largeRange)
                {
                    if (highGrnd)
                    {
                        return (this.FrntTrckTrigg ? (isSameSide ? "Suski" : "Salad") : (this.BckTrckTrigg ? (isSameSide ? "Suski" : "Salad") : ""));
                        //return (this.FrntTrckTrigg ? ((crntFwdAngle >= 0 && isSameSide) ? "Crooked" : ((crntFwdAngle < 0 && !isSameSide) ? "OverCrook" : "")) : (this.BckTrckTrigg ? ((crntFwdAngle <= 0 && isSameSide) ? "Suski" : ((crntFwdAngle > 0 && !isSameSide) ? "Salad" : "")) : ""));
                    }
                    else if (lowGrnd)
                    {
                        return (this.FrntTrckTrigg ? (isSameSide ? "Smith" : "Feeble") : (this.BckTrckTrigg ? (isSameSide ? "Lazy/Willy" : "Losi/Whatchamajig") : ""));
                    }
                }
            }
            else if (enterRange || exitRange)
            {
                if (fiftyfiftyRange && (this.FrntTrckTrigg && this.BckTrckTrigg) && (this.NoneDown() || ((this.FLWheel && !this.FRWheel && this.RLWheel && !this.RRWheel) || (!this.FLWheel && this.FRWheel && !this.RLWheel && this.RRWheel))))
                {
                    return "FiftyFifty";
                }

                if (/*(this.noseFivCnt == 0) && */highGrnd && ((this.FrntTrckTrigg || this.BckTrckTrigg)))
                {
                    this.noseFivCnt++;
                    return (this.FrntTrckTrigg ? "Nosegrind" : this.BckTrckTrigg ? "FiveO" : "");
                }
            }
            else if ((this.allGrnds.Count == 0) && this.NoneDown())
            {
                if (canBrdLipSlide)
                {
                    return ("Boardslide");
                }
                else if (canBrdLipSlide && (this.FrntTrckTrigg || this.BckTrckTrigg))
                {
                    return (exLowGrnd ? "Anchor" : "Boardslide");
                }
                else if ((this.NoseTrigg && !this.TailTrigg) || (!this.NoseTrigg && this.TailTrigg))
                {
                    return (this.NoseTrigg ? "Noseslide" : (this.TailTrigg ? "Tailslide" : ""));
                }
            }

            return "";
        }
    }
}
