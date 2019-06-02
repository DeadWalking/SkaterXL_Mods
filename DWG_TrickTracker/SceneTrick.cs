using UnityEngine;
using System;
using TMPro;

namespace DWG_TT
{
    class SceneTrick : MonoBehaviour {
        //private TextMesh textMeshData;
        private TextMeshPro tmData;

        private float creationTime;

        public void Init(string p_guiTrick)
        {
            Vector3 placePos = SXLH.BrdTargPos;
            placePos.y = placePos.y + 0.25f;
            this.gameObject.transform.position = placePos;
            this.gameObject.transform.rotation = Quaternion.Euler(SXLH.CamRot.eulerAngles);
            this.gameObject.transform.localScale = GetDistScale();

            this.tmData = this.gameObject.AddComponent<TextMeshPro>();
            this.tmData.alignment = TextAlignmentOptions.Center;

            this.tmData.enableAutoSizing = true;
            this.tmData.enableWordWrapping = false;
            this.tmData.outlineColor = Color.black;
            this.tmData.outlineWidth = 1f;
            //this.tmData.font = Resources.Load("Fonts/78skate_outline SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
            //this.tmData.material = Resources.Load("Fonts/78skate_outline SDF", typeof(Material)) as Material;
            //this.tmData.font = Resources.Load("Fonts & Materials/IMPACT SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
            //this.tmData.material = Resources.Load("Fonts & Materials/IMPACT SDF - Drop Shadow", typeof(Material)) as Material;
            //this.tmData.fontSharedMaterial.SetFloat("_GlowPower", 0.5f);
            //this.tmData.enableVertexGradient = true;
            //this.tmData.faceColor = new Color32(255, 128, 128, 255);
            //this.tmData.renderMode = TextRenderFlags.Render;
            //this.tmData.UpdateFontAsset();
            //this.tmData.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.5f);
            //this.tmData.UpdateMeshPadding();
            //this.tmData.material.EnableKeyword("UNDERLAY_ON");
            //this.tmData.material.DisableKeyword("UNDERLAY_INNER");
            this.tmData.fontSize = 64f;
            this.tmData.color = Color.blue;
            //this.tmData.isOverlay = true;
            //this.tmData.UpdateFontAsset();
            this.tmData.enabled = true;

            this.tmData.text = p_guiTrick;

            this.creationTime = Time.time;
        }

        public void LateUpdate() {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                this.gameObject.transform.rotation = Quaternion.Euler(SXLH.CamRot.eulerAngles);
                this.gameObject.transform.localScale = this.GetDistScale();
            }
            if ((Time.time - this.creationTime) >= 10)
            {
                Destroy(this.gameObject);
            }
        }

        private Vector3 GetDistScale()
        {
            Vector3 placeScale = new Vector3(0.02f, 0.02f, 0.02f);
            return placeScale;
        }
    }
}
