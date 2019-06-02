using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

namespace DWG_TT
{
    class DataPoints : MonoBehaviour
    {
        private int thisTrig;
        private PlaceType pType;

        private float creationTime;
        private float frmTimer;

        private bool colliding; public bool Colliding { get { return this.colliding; } }

        private RaycastHit dwnHit, fwdHit, rghtHit, forFivHit;

        private GUIM guiM;

        public void Init(int p_thisIndex, PlaceType p_pType)
        {
            this.thisTrig = p_thisIndex;
            this.pType = p_pType;

            this.dwnHit = new RaycastHit();
            this.fwdHit = new RaycastHit();
            this.rghtHit = new RaycastHit();
            this.forFivHit = new RaycastHit();

            this.creationTime = Time.time;
            this.frmTimer = this.creationTime;
            this.colliding = false;
        }

        public void FixedUpdate()
        {
            if (!Main.settings.use_custTrigs)
            {
                this.enabled = false;
            }
            else
            {
                if (this.guiM == null)
                {
                    this.guiM = this.GetComponentInParent(typeof(GUIM)) as GUIM;
                    return;
                };

                Vector3 thisTrigPos = GrndTrigs.GetTrigPos(this.thisTrig);

                Tuple<Vector3, Vector3, Vector3, Vector3, Vector3> posInfo = DataPoints.GetPosInfo(this.thisTrig, this.pType);

                Vector3 brdFwd = posInfo.Item2 - posInfo.Item4;
                Vector3 brdRght = posInfo.Item2 - posInfo.Item5;
                Vector3 brdForFiv = posInfo.Item1 - posInfo.Item2; 

                Vector3 trigDir = posInfo.Item1 - posInfo.Item3;

                float rayDwnDist = FNCS.Get3DDist(posInfo.Item3, posInfo.Item1);
                float rayFwdDist = FNCS.Get3DDist(posInfo.Item2, posInfo.Item4);
                float rayRghtDist = FNCS.Get3DDist(posInfo.Item2, posInfo.Item5);
                float rayTrigDist = FNCS.Get3DDist(posInfo.Item2, posInfo.Item1);

                this.colliding = SXLH.TrigManIsColl
                    ? Physics.Raycast(posInfo.Item1, trigDir, out this.dwnHit, rayDwnDist, SXLH.BrdLayer) || // 1 << LayerMask.NameToLayer("Grindable")) ||
                        Physics.Raycast(posInfo.Item2, brdFwd, out this.fwdHit, rayFwdDist, SXLH.BrdLayer) || // 1 << LayerMask.NameToLayer("Grindable")) ||
                        Physics.Raycast(posInfo.Item2, brdRght, out this.rghtHit, rayRghtDist, SXLH.BrdLayer) || // 1 << LayerMask.NameToLayer("Grindable")) ||
                        Physics.Raycast(posInfo.Item1, brdForFiv, out this.forFivHit, rayTrigDist, SXLH.BrdLayer)
                    : false;
            }
        }

        static public Tuple<Vector3, Vector3, Vector3, Vector3, Vector3> GetPosInfo(int p_trig, PlaceType p_pType)
        {
            Vector3 thisTrigPos = GrndTrigs.GetTrigPos(p_trig);

            Tuple<Vector3, Vector3, Vector3> posOffset = GetOffsets(p_trig, p_pType);

            Vector3 thisOffsetPos = (thisTrigPos + posOffset.Item1 + posOffset.Item2 + posOffset.Item3);

            Vector3 offset = GetAllowedOffset(p_trig);

            Vector3 lineDwnStrt = thisOffsetPos + (SXLH.BrdUp * offset.y);
            Vector3 lineEndA = (thisTrigPos + posOffset.Item1 + posOffset.Item3);
            Vector3 lineEndB = (thisTrigPos + posOffset.Item2 + posOffset.Item3);

            return Tuple.Create(thisTrigPos, thisOffsetPos, lineDwnStrt, lineEndA, lineEndB);
        }

        static public Tuple<Vector3, Vector3, Vector3> GetOffsets(int p_trig, PlaceType p_pType)
        {
            Vector3 lineFwd = SXLH.BrdFwd;
            Vector3 lineRght = SXLH.BrdRght;
            Vector3 lineUp = SXLH.BrdUp;

            Vector3 offset = GetAllowedOffset(p_trig);

            switch (p_pType)
            {
                case PlaceType.lfTrig:
                    lineFwd *= offset.z;
                    lineRght *= -offset.x;
                    lineUp *= offset.y;
                    break;
                case PlaceType.lrTrig:
                    lineFwd *= -offset.z;
                    lineRght *= -offset.x;
                    lineUp *= offset.y;
                    break;
                case PlaceType.rfTrig:
                    lineFwd *= offset.z;
                    lineRght *= offset.x;
                    lineUp *= offset.y;
                    break;
                case PlaceType.rrTrig:
                    lineFwd *= -offset.z;
                    lineRght *= offset.x;
                    lineUp *= offset.y;
                    break;
            };

            return Tuple.Create(lineFwd, lineRght, lineUp);
        }

        static public Vector3 GetAllowedOffset(int p_trig)
        {
            Vector3 offset = Vector3.zero;

            switch (p_trig)
            {
                case 0:
                    offset = Main.settings.brdOffsets;
                    break;
                case 1:
                case 2:
                    offset = Main.settings.ntOffsets;
                    break;
                case 3:
                case 4:
                    offset = Main.settings.trckOffsets;
                    break;
            };
            return offset;
        }
    }
}
