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

        public double EdgeFwdAngl { get; private set; }
        public double EdgeUpAngl { get; private set; }
        public double HghtDiff { get; private set; }
        public bool SameSide { get; private set; }
        public bool ToeFwd { get; private set; }
        public string TrickArea { get; private set; }
        public string TrickHght { get; private set; }
        public string CrntGrind { get; private set; }

        private string lastState;
        private string lastGrind;
        private List<string> allGrnds;

        private float frmTimer;
        private float grndTime;
        private float bonkTime;

        private const float bonkWait = 0.20f;
        private float grndWaitStart;
        private float grndWait;

        private const float grndAnglRot = 0.01f;
        private const float grndAnglFlip = 0.05f;
        private float grndAnglTwk = 0.08f;

        private Vector3 brdStartPos;
        private Vector3 brdRot;

        private bool enablCTrigs;

        private DataPoints[] lfTrckrs;
        private DataPoints[] lrTrckrs;
        private DataPoints[] rfTrckrs;
        private DataPoints[] rrTrckrs;

        public void OnEnable()
        {
            //DebugOut.Log(this.GetType().Name + " Enable");
            this.lastState = "";
            this.CrntGrind = "";
            this.lastGrind = "";
            this.allGrnds = new List<string>();

            this.enablCTrigs = true;
            this.EdgeFwdAngl = 0f;
            this.EdgeUpAngl = 0f;

            this.frmTimer = 0f;
            this.grndTime = 0f;
            this.bonkTime = 0f;
            this.grndWaitStart = 0.20f;

            this.lfTrckrs = new DataPoints[5];
            this.lrTrckrs = new DataPoints[5];
            this.rfTrckrs = new DataPoints[5];
            this.rrTrckrs = new DataPoints[5];

            for (int i = 0; i < 5; i++)
            {
                GameObject rrCollObj = new GameObject();
                rrCollObj.transform.parent = this.gameObject.transform;
                DataPoints rrColl = rrCollObj.AddComponent<DataPoints>();
                this.rrTrckrs[i] = rrColl;

                GameObject lrCollObj = new GameObject();
                lrCollObj.transform.parent = this.gameObject.transform;
                DataPoints lrColl = lrCollObj.AddComponent<DataPoints>();
                this.lrTrckrs[i] = lrColl;

                GameObject rfCollObj = new GameObject();
                rfCollObj.transform.parent = this.gameObject.transform;
                DataPoints rfColl = rfCollObj.AddComponent<DataPoints>();
                this.rfTrckrs[i] = rfColl;

                GameObject lfCollObj = new GameObject();
                lfCollObj.transform.parent = this.gameObject.transform;
                DataPoints lfColl = lfCollObj.AddComponent<DataPoints>();
                this.lfTrckrs[i] = lfColl;

                rrColl.Init(i, PlaceType.rrTrig);
                rfColl.Init(i, PlaceType.rfTrig);
                lrColl.Init(i, PlaceType.lrTrig);
                lfColl.Init(i, PlaceType.lfTrig);
            }
        }

        public void OnDestroy()
        {
            //DebugOut.Log(this.GetType().Name + " OnDestroy");
        }

        public void LateUpdate()
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
                if (Input.GetKey(KeyCode.LeftBracket))
                {
                    this.grndWaitStart += 0.01f;
                    ModMenu.Instance.ShowMessage(" GrindWait: " + this.grndWaitStart);
                }
                if (Input.GetKey(KeyCode.RightBracket))
                {
                    this.grndWaitStart -= 0.01f;
                    ModMenu.Instance.ShowMessage(" GrindWait: " + this.grndWaitStart);
                }
                if (Input.GetKey(KeyCode.Semicolon))
                {
                    this.grndAnglTwk -= 0.01f;
                    ModMenu.Instance.ShowMessage(" GrndAnglTwk: " + this.grndAnglTwk);
                }
                if (Input.GetKey(KeyCode.Quote))
                {
                    this.grndAnglTwk -= 0.01f;
                    ModMenu.Instance.ShowMessage(" GrndAnglTwk: " + this.grndAnglTwk);
                }
            }

            if (Main.settings.use_custTrigs)
            {
                if (this.enablCTrigs)
                {
                    this.enablCTrigs = false;
                    for (int i = 0; i < 5; i++)
                    {
                        this.rrTrckrs[i].enabled = true;
                        this.lrTrckrs[i].enabled = true;
                        this.rfTrckrs[i].enabled = true;
                        this.lfTrckrs[i].enabled = true;
                    };
                    return;
                }

                for (int i = 0; i < 5; i++)
                {
                    bool hit = (this.rrTrckrs[i].Colliding || this.lrTrckrs[i].Colliding || this.rfTrckrs[i].Colliding || this.lfTrckrs[i].Colliding);
                    if (hit || !hit && GrndTrigs.Hit[i]) { GrndTrigs.Hit[i] = hit; }
                }
            }
            if (!this.enablCTrigs && !Main.settings.use_custTrigs) { this.enablCTrigs = true; }

            float chkTime = Time.time;
            float spdAdjust = Mathf.Clamp(SXLH.BrdSpeed, 1f, 8f);

            switch (SXLH.CrntState)
            {
                case SXLH.Grinding:
                    if (this.lastState != SXLH.Grinding)
                    {
                        // Grind Enter
                        this.allGrnds.Clear();
                        this.grndTime = chkTime;
                        this.frmTimer = chkTime;
                        this.brdRot = Vector3.zero;
                        this.bonkTime = chkTime;
                        this.grndWait = this.grndWaitStart;
                    }

                    // Grind Stay
                    this.guiTrck.TrackedTime = chkTime;

                    if ((chkTime - this.grndTime) >= (this.grndWait/* / spdAdjust*/))
                    {
                        this.CrntGrind = this.GrndCheck();

                        if (this.CrntGrind.Length > 0)
                        {
                            //double chkBrdRot = Math.Abs(Mathd.AngleBetween(this.brdRot.y, SXLH.BrdEulLoc.y));
                            //double chkBrdFlip = Math.Abs(Mathd.AngleBetween(this.brdRot.z, SXLH.BrdEulLoc.z));
                            //double chkBrdTwk = Math.Abs(Mathd.AngleBetween(this.brdRot.x, SXLH.BrdEulLoc.x));
                            //DebugOut.Log("chkBrdRot " + chkBrdRot + /*" chkBrdFlip " + chkBrdFlip +*/ " chkBrdTwk " + chkBrdTwk);

                            if (!this.allGrnds.Contains(this.CrntGrind)/* && ((chkBrdRot - grndAnglRot) <= 0f) && *//*((chkBrdFlip - grndAnglFlip) <= 0f) && *//*((chkBrdTwk - this.grndAnglTwk) <= 0f)*/)
                            {
                                this.lastGrind = this.CrntGrind;
                                this.allGrnds.Add(this.CrntGrind);
                                this.guiTrck.AddTrick((SXLH.IsSwitch ? "Switch " : "") + (SXLH.FrntSide ? "Fs " : "Bs ") + this.CrntGrind);
                                this.grndTime = chkTime;
                                this.grndWait = 1.0f;
                            }
                        }
                    }
                    this.brdRot.x = SXLH.BrdEulLoc.x;
                    this.brdRot.y = SXLH.BrdEulLoc.y;
                    this.brdRot.z = SXLH.BrdEulLoc.z;

                    break;
                default:
                    // Grind Exit
                    if (this.lastState == SXLH.Grinding || this.lastState == SXLH.Impact)
                    {
                        for (int i = 0; i < 5; i++) { GrndTrigs.Hit[i] = false; }
                        if (SXLH.CrntState == SXLH.Impact || SXLH.CrntState == SXLH.InAir || SXLH.CrntState == SXLH.Released || SXLH.CrntState == SXLH.BeginPop || SXLH.CrntState == SXLH.Pop || SXLH.CrntState == SXLH.Manualling)
                        {
                            if ((chkTime - this.bonkTime) < bonkWait) { this.guiTrck.AddTrick("Bonk"); }
                        }
                    }
                    this.brdStartPos = SXLH.BrdPos;
                    break;
            };
            this.lastState = SXLH.CrntState;
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
            // Whatchamajig/Losi grind
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

            // Random code I was going to use as an example for calculating if the board is beyond the plane of the grind edge
            //    public Transform cam;
            //public Vector3 cameraRelative;

            //void Start()
            //{
            //    cam = Camera.main.transform;
            //    Vector3 cameraRelative = cam.InverseTransformPoint(transform.position);

            //    if (cameraRelative.z > 0)
            //        print("The object is in front of the camera");
            //    else
            //        print("The object is behind the camera");
            //}


            bool noseTrigg = GrndTrigs.Hit[GrndTrigs.NTrig];
            bool frntTrckTrigg = GrndTrigs.Hit[GrndTrigs.FTTrig];
            bool brdTrigg = GrndTrigs.Hit[GrndTrigs.BrdTrig];
            bool bckTrckTrigg = GrndTrigs.Hit[GrndTrigs.BTTrig];
            bool tailTrigg = GrndTrigs.Hit[GrndTrigs.TTrig];

            double crntFwdAngle = Vector3.SignedAngle(SXLH.BrdDirTwk, SXLH.GrindSplnFwd, -SXLH.GrindSplnRght);
            double crntFwdAngleAbs = Math.Abs(crntFwdAngle);
            double crntUpAngl = Vector3.Angle(SXLH.BrdDirTwk, SXLH.GrindSplnUp) - 90; //Vector3.Angle(SXLH.BrdDirTwk, SXLH.GrindSplnUp); //Vector3.Angle(SXLH.BrdUp, SXLH.GrindUp);
            double heightDiff = FNCS.GetHghtDiff(SXLH.BrdPos, SXLH.GrindSplnPos);

            bool toeIsFwd = Vector3.SignedAngle(Vector3.ProjectOnPlane(SXLH.BrdEdgeFwd, SXLH.GrindUp), SXLH.GrindDir, SXLH.GrindUp) > 0;
            bool isSameSide = (SXLH.FrntSide && !toeIsFwd) || (!SXLH.FrntSide && toeIsFwd);
            bool didSwitch = (crntFwdAngleAbs >= 90);

            bool aboveEdge = ((heightDiff - 0.080) >= 0);
            bool flatZone = (!aboveEdge && (heightDiff - 0.04) >= 0);
            bool belowEdge = (!aboveEdge && !flatZone);
            if (tailTrigg || bckTrckTrigg) { crntUpAngl *= -1; }

            bool fiftyfiftyRotRange = (crntFwdAngleAbs <= 17f || crntFwdAngleAbs >= 163f);

            bool lowEntrRotRange = (crntFwdAngleAbs >= 11f && crntFwdAngleAbs <= 45f);
            bool lowExitRotRange = (crntFwdAngleAbs >= 135f && crntFwdAngleAbs <= 169f);

            bool entrGrndRotRange = (crntFwdAngleAbs >= 34f && crntFwdAngleAbs <= 55f);
            bool perpGrndRotRange = (crntFwdAngleAbs >= 55f && crntFwdAngleAbs <= 125f);
            bool exitGrndRotRange = (crntFwdAngleAbs >= 124f && crntFwdAngleAbs <= 145f);

            bool brdLipRotRange = (crntFwdAngleAbs >= 11f && crntFwdAngleAbs <= 169f);

            bool highTrick = (aboveEdge && crntUpAngl >= 10f);
            bool flatTrick = (flatZone && crntUpAngl >= -1f && crntUpAngl <= 10f);
            bool ntslideTrick = (flatZone && crntUpAngl >= -6f);
            bool lowTrick = (crntUpAngl <= -1f);

            if (this.guiMan.ConEnable)
            {
                this.EdgeFwdAngl = crntFwdAngle;
                this.EdgeUpAngl = crntUpAngl;
                this.HghtDiff = heightDiff;
                this.ToeFwd = toeIsFwd;
                this.SameSide = isSameSide;
                this.TrickHght = (belowEdge ? "BelowEdge" : flatZone ? "FlatEdge" : aboveEdge ? "AboveEdge" : "");
                this.TrickArea = ("\n Low:    " + lowTrick + "\n NTSlide:    " + ntslideTrick + "\n FlatTrick:    " + flatTrick + "\n HighTrick:    " + highTrick + "\n");
            }

            if (flatZone && fiftyfiftyRotRange && frntTrckTrigg && bckTrckTrigg)
            {
                return "FiftyFifty";
            }
            else
            {
                if ((noseTrigg || frntTrckTrigg && !bckTrckTrigg && !tailTrigg) || (!noseTrigg && !frntTrckTrigg && bckTrckTrigg || tailTrigg))
                {
                    if (crntUpAngl <= 0f)
                    {
                        if (lowEntrRotRange)
                        {
                            return (frntTrckTrigg ? isSameSide ? "Lazy" : "Whatchamajig" : isSameSide ? "Feeble" : "Smith");
                        }
                        else if (perpGrndRotRange && crntUpAngl <= -22f && frntTrckTrigg || bckTrckTrigg)
                        {
                            return "Anchor";
                        }
                        else if (lowExitRotRange)
                        {
                            return (!SXLH.IsSwitch ? "Switch " : "") + (frntTrckTrigg ? isSameSide ? "Smith" : "Feeble" : isSameSide ? "Whatchamajig" : "Lazy");
                        }
                        else if ((noseTrigg && !bckTrckTrigg && !tailTrigg) || (!noseTrigg && !frntTrckTrigg && tailTrigg))
                        {
                            return (noseTrigg || frntTrckTrigg ? "Noseslide" : "Tailslide");
                        }
                    }
                    else
                    {
                        if (crntUpAngl >= 5f)
                        {
                            if (entrGrndRotRange)
                            {
                                return (noseTrigg || frntTrckTrigg ? isSameSide ? "Crook" : "Overcrook" : isSameSide ? "Salad" : "Suski");
                            }
                            else if (perpGrndRotRange)
                            {
                                return (noseTrigg || frntTrckTrigg ? isSameSide ? "Noseslide" : (SXLH.TwoDown ? "NoseBluntslide" : "Noseslide") : isSameSide ? "Tailslide" : (SXLH.TwoDown ? "Bluntslide" : "Tailslide"));
                            }
                            else if (exitGrndRotRange)
                            {
                                return (!SXLH.IsSwitch ? "Switch " : "") + (noseTrigg || frntTrckTrigg ? isSameSide ? "Suski" : "Salad" : isSameSide ? "Overcrook" : "Crook");
                            }
                            else if ((noseTrigg || frntTrckTrigg && !bckTrckTrigg && !tailTrigg) || (!noseTrigg && !frntTrckTrigg && bckTrckTrigg || tailTrigg))
                            {
                                return (noseTrigg || frntTrckTrigg ? (!SXLH.TwoDown && frntTrckTrigg ? "Nosegrind" : "Noseslide") : (!SXLH.TwoDown && bckTrckTrigg ? "FiveO" : "Tailslide"));
                            }
                        }
                    }
                }
                else
                {
                    if (brdLipRotRange && brdTrigg && !aboveEdge)
                    {
                        return !SXLH.TwoDown && !frntTrckTrigg && !bckTrckTrigg
                            ? "Boardslide"
                            : "Lipslide";
                    }
                }
            }
            return "";
        }
    }
}
