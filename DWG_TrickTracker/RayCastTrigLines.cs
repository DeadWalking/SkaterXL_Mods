using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

namespace DWG_TT
{
    class RayCastTrigLines : MonoBehaviour
    {
        private int thisTrig;
        private PlaceType pType;

        private float creationTime;
        private float frmTimer;

        private LineRenderer[] lines = new LineRenderer[2];

        private GUIM guiM;

        public void Init(int p_thisIndex, PlaceType p_pType)
        {
            this.thisTrig = p_thisIndex;
            this.pType = p_pType;

            this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            for (int i = 0; i < 2; i++)
            {
                GameObject tmpObj = new GameObject();
                tmpObj.transform.parent = this.gameObject.transform;

                LineRenderer tmpLine = tmpObj.AddComponent<LineRenderer>();
                Material lineMat = new Material(Shader.Find("Unlit/Texture")); //Particles/Alpha  "Unlit/Texture" Line/Glow

                tmpLine.material = lineMat;
                tmpLine.material.color = Color.red;
                tmpLine.positionCount = 3;
                tmpLine.startColor = Color.red;
                tmpLine.endColor = Color.yellow;
                tmpLine.startWidth = 0.005f;
                tmpLine.useWorldSpace = true;
                tmpLine.alignment = LineAlignment.TransformZ;
                tmpLine.endWidth = 0.005f;

                tmpLine.enabled = true;
                tmpLine.gameObject.layer = 11;

                this.lines[i] = tmpLine;
            }

            this.creationTime = Time.time;
            this.frmTimer = this.creationTime;
        }

        public void FixedUpdate()
        {
            if (this.guiM == null)
            {
                this.guiM = this.GetComponentInParent(typeof(GUIM)) as GUIM;
                return;
            };

            Tuple<Vector3, Vector3, Vector3, Vector3, Vector3> posInfo = DataPoints.GetPosInfo(this.thisTrig, this.pType);

            this.gameObject.transform.position = posInfo.Item2;

            this.lines[0].SetPosition(0, posInfo.Item3);
            this.lines[0].SetPosition(1, posInfo.Item1);
            this.lines[0].SetPosition(2, posInfo.Item2);

            this.lines[1].SetPosition(0, posInfo.Item4);
            this.lines[1].SetPosition(1, posInfo.Item2);
            this.lines[1].SetPosition(2, posInfo.Item5);

            this.gameObject.transform.rotation = Quaternion.Euler(SXLH.CamEul);

            this.lines[0].gameObject.transform.rotation = Quaternion.Euler(SXLH.CamEul);
            this.lines[1].gameObject.transform.rotation = Quaternion.Euler(SXLH.CamEul);

            if (!this.guiM.RayLines || !Main.settings.use_custTrigs) {
                for (int i = 0; i < 2; i++) { Destroy(this.lines[i].material); };
                Destroy(this.gameObject);
            }
        }
    }
}
