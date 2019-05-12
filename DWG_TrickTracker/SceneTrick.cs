using UnityEngine;
using System;

namespace DWG_TT
{
    class SceneTrick : MonoBehaviour {
        private TextMesh textMeshData;
        public TextMesh TextMeshData
        {
            get { return textMeshData; }
            set { textMeshData = value; }
        }
        //private MeshRenderer meshRendererData;
        //public MeshRenderer MeshRendererData
        //{
        //    get { return meshRendererData; }
        //    set { meshRendererData = value; }
        //}
        private float creationTime;
        public float CreationTime
        {
            get { return creationTime; }
            set { creationTime = value; }
        }
        private bool isMain;
        public bool IsMain
        {
            get { return isMain; }
            set { isMain = value; }
        }

        public void Init(bool p_isMain, string p_guiTrick)
        {
            IsMain = p_isMain;

            Tuple<Vector3, Quaternion, Vector3> guiTransform;
            if (IsMain)
            {
                guiTransform = TT.GetWorldTransform();
            }
            else
            {
                guiTransform = TT.GetWorldTransform();
            }

            gameObject.transform.position = guiTransform.Item1;
            gameObject.transform.rotation = Quaternion.Euler(guiTransform.Item2.eulerAngles);   // Quaternion.Euler(0, 0, 0);
            gameObject.transform.localScale = guiTransform.Item3;                               // new Vector3(1, 1, 1);

            TextMeshData = gameObject.AddComponent<TextMesh>();
            TextMeshData.alignment = TextAlignment.Right;
            TextMeshData.anchor = TextAnchor.MiddleCenter;
            TextMeshData.color = Color.red;
            TextMeshData.richText = true;
            TextMeshData.characterSize = 0.01f;
            TextMeshData.fontSize = 64;
            TextMeshData.offsetZ = (IsMain ? 0 : 1);
            TextMeshData.text = p_guiTrick;

            CreationTime = Time.time;
        }

        public void FixedUpdate() {
            if (Main.enabled && Main.settings.do_TrackTricks) {

                Tuple<Vector3, Quaternion, Vector3> guiTransform;
                if (!IsMain && Main.settings.at_TrickLanding)
                {
                    guiTransform = TT.GetWorldTransform();
                }
                else
                {
                    guiTransform = TT.GetWorldTransform();
                    gameObject.transform.position = guiTransform.Item1;
                }

                gameObject.transform.rotation = Quaternion.Euler(guiTransform.Item2.eulerAngles);
                gameObject.transform.localScale = guiTransform.Item3;
            }
            if (!IsMain && (Time.time - creationTime) >= 10)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
