using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

namespace DWG_TT
{
    class SplnToTrigMark : MonoBehaviour
    {
        private TextMeshPro tmData;

        private int thisTrig;

        private float creationTime;
        private float frmTimer;

        private GUIM guiM;

        public void Init(int p_thisIndex)
        {
            this.thisTrig = p_thisIndex;

            this.gameObject.transform.localScale = new Vector3(0.0075f, 0.0075f, 0.0075f);

            this.tmData = this.gameObject.AddComponent<TextMeshPro>();
            this.tmData.alignment = TextAlignmentOptions.Center;
            this.tmData.fontSize = 64f;
            this.tmData.text = "+";
            this.tmData.color = Color.blue;
            this.tmData.enabled = true;

            this.creationTime = Time.time;
            this.frmTimer = this.creationTime;
        }

        public void FixedUpdate()
        {
            if (this.guiM == null) {
                this.guiM = this.GetComponentInParent(typeof(GUIM)) as GUIM;
                return;
            };

            Vector3 thisTrigPos = GrndTrigs.GetTrigPos(this.thisTrig);

            this.gameObject.transform.position = SXLH.TrigManIsColl
                ? FNCS.GetPlanePos(thisTrigPos, SXLH.GrindSplnPos, SXLH.GrindDir)
                : thisTrigPos + -(SXLH.GrindUp * -1000f);

            this.gameObject.transform.rotation = Quaternion.Euler(SXLH.CamEul);

            if (!this.guiM.SplnMarks || !Main.settings.use_custTrigs) { Destroy(this.gameObject); }
        }
    }
}
