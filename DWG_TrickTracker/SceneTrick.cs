using UnityEngine;
using System;

namespace DWG_TT
{
    class SceneTrick : MonoBehaviour {
        private TextMesh textMeshBG;
        private TextMesh textMeshData;

        //private MeshRenderer meshRendererData;

        private float creationTime;
        private bool isMain;

        public void Init(bool p_isMain, string p_guiTrick)
        {
            isMain = p_isMain;

            Tuple<Vector3, Quaternion, Vector3> guiTransform;
            if (isMain)
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

            textMeshData = gameObject.AddComponent<TextMesh>();

            textMeshData.alignment = TextAlignment.Right;
            textMeshData.anchor = TextAnchor.MiddleCenter;
            textMeshData.color = Color.blue;
            textMeshData.richText = true;
            textMeshData.characterSize = 0.01f;
            textMeshData.fontSize = 64;
            textMeshData.offsetZ = (isMain ? 0 : 1);
            textMeshData.text = p_guiTrick;

            creationTime = Time.time;

            //textMeshBG = gameObject.AddComponent<TextMesh>();
            //textMeshBG.alignment = TextAlignment.Right;
            //textMeshBG.anchor = TextAnchor.MiddleCenter;
            //textMeshBG.color = Color.black;
            //textMeshBG.richText = true;
            //textMeshBG.characterSize = 0.01f;
            //textMeshBG.fontSize = 68;
            //textMeshBG.offsetZ = (isMain ? 0 : 1);
            //textMeshBG.text = p_guiTrick;
        }

        //FixedUpdate
        public void FixedUpdate() {
            if (Main.enabled && Main.settings.do_TrackTricks) {

                Tuple<Vector3, Quaternion, Vector3> guiTransform = TT.GetWorldTransform();
                if (isMain) { gameObject.transform.position = guiTransform.Item1; };

                gameObject.transform.rotation = Quaternion.Euler(guiTransform.Item2.eulerAngles);
                gameObject.transform.localScale = guiTransform.Item3;

                //textMeshBG.transform.position = gameObject.transform.forward *1;
                //textMeshBG.transform.rotation = gameObject.transform.rotation;
                //textMeshBG.transform.localScale = gameObject.transform.localScale * 1;
            }
            if (!isMain && (Time.time - creationTime) >= 10)
            {
                //Destroy(textMeshBG.gameObject);
                //Destroy(textMeshData.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
