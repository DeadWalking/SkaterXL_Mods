using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

namespace DWG_TT
{
    class GrndTrainr : MonoBehaviour
    {
        private TextMeshPro tmData;

        private int thisTrig = 0;
        private int lrOffset = 0;

        private const float offsetX = 0.150f;
        private const float offsetY = 0.125f;
        private float creationTime;
        private float frmTimer;

        private bool isEnabled = false;
        private bool isColliding = false;

        private Color thisColor;

        public void Init(int p_thisIndex, int p_lrOffset)
        {
            if (this.isEnabled) { return; }

            this.lrOffset = p_lrOffset;
            this.thisTrig = p_thisIndex;

            this.gameObject.transform.localScale = new Vector3(0.0075f, 0.0075f, 0.0075f);

            this.tmData = this.gameObject.AddComponent<TextMeshPro>();
            this.tmData.alignment = TextAlignmentOptions.Center;
            this.tmData.fontSize = 64f;
            this.tmData.text = this.lrOffset < 0 || this.lrOffset > 0 ? "+" : "+";

            this.thisColor = this.thisTrig == 0 ? Color.white : (this.thisTrig == 1 || this.thisTrig == 2) ? Color.magenta : Color.cyan;
            this.tmData.color = this.thisColor;

            this.creationTime = Time.time;
            this.frmTimer = this.creationTime;

            this.isEnabled = true;
        }

        public void LateUpdate()
        {
            if (!this.isEnabled) { return; };

            this.gameObject.transform.position = GrndTrigs.GetTrigPos(this.thisTrig) + (SXLH.BrdFwd * (this.thisTrig == 1 || this.thisTrig == 2 ? this.thisTrig == 1 ? -0.01f  : 0.01f : (this.thisTrig == 3 || this.thisTrig == 4 ? this.thisTrig == 3 ? -0.04f : 0.04f : 0f))) + (SXLH.BrdRght * (offsetX * this.lrOffset)) + (SXLH.BrdUp * (offsetY));
            this.gameObject.transform.rotation = Quaternion.Euler(SXLH.CamEul);// * Quaternion.Euler(-90, 0, 0);

            this.isColliding = SXLH.TrigManIsColl && GrndTrigs.Hit[this.thisTrig];

            if (this.isColliding && this.tmData.color != Color.green)
            {
                this.tmData.color = Color.green;
            }

            if (!this.isColliding && this.tmData.color == Color.green)
            {
                this.tmData.color = this.thisColor;
            };
            if (!Main.settings.show_ghelpers) { Destroy(this.gameObject); };
        }
    }
}
