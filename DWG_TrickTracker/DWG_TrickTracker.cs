using UnityEngine;
using XLShredLib;
using XLShredLib.UI;
using TMPro;

namespace DWG_TrickTracker
{
    class DWG_TrickTracker : MonoBehaviour {
        ModUIBox modMenuBox;
        ModUILabel modMenuLabelDWG_TrickTracker;

        private Rect trackerRect;
        private bool showTracker;

        private string trackerTitle = "Trick Tracker";
        private GUIContent title;
        private GUIStyle titleStyle;
        private Vector2 titleSize;

        private GUIContent tricks;
        private GUIStyle tricksStyle;
        private Vector2 tricksSize;

        static private string trackedTricks;
        static public string TrackedTricks
        {
            get { return trackedTricks; }
            set { trackedTricks = value; }
        }

        private float resetTime = 3.0f;
        static private float trackedTime;
        static public float TrackedTime
        {
            get { return trackedTime; }
            set { trackedTime = value; }
        }

        static public Transform boardPos;
        static public Transform BoardPos
        {
            get { return boardPos; }
            set { boardPos = value; }
        }

        static public Transform camTransform;
        static public Transform CamTransform
        {
            get { return camTransform; }
            set { camTransform = value; }
        }

        //GameObject TextObjectData;
        //TextMesh TextMeshData;
        //MeshRenderer TextRendererData;
        //public Material mat;
        //public Font font;

        //static public Vector3 TextPos;
        //private float xzPos, yPos;
        //private float xzPosAdd, yPosAdd;

        public void Start() {
            modMenuBox = ModMenu.Instance.RegisterModMaker("dwg", "DeadWalking");
            modMenuLabelDWG_TrickTracker = modMenuBox.AddLabel("do-trackTricks", LabelType.Text, trackerTitle + " Toggle (W)", Side.right, () => Main.enabled, false, (b) => Main.settings.do_TrackTricks = b, 1);
            Main.settings.do_TrackTricks = false;
            showTracker = false;
            trackedTricks = "";
            trackedTime = Time.time;
           // TextObjectData = new GameObject(); // GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //TextObjectData.AddComponent<TextMesh>();

            //TextMeshData = TextObjectData.GetComponent<TextMesh>();
            //TextRendererData = TextObjectData.AddComponent<MeshRenderer>();
        }

        private void TimerMess()
        {
            ModMenu.Instance.ShowMessage(trackerTitle + " Reset Time (s): " + resetTime);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.W, 0.2f, () =>
                {
                    Main.settings.do_TrackTricks = !Main.settings.do_TrackTricks;

                    modMenuLabelDWG_TrickTracker.SetToggleValue(Main.settings.do_TrackTricks);

                    showTracker = Main.settings.do_TrackTricks;
                    ModMenu.Instance.ShowMessage(Main.settings.do_TrackTricks ? trackerTitle + ": Enabled" : trackerTitle + ": Disabled");
                    //TextObjectData.SetActive(DWG_ShowTracker);
                    //TextRendererData.material = mat;
                });
                ModMenu.Instance.KeyPress(KeyCode.Alpha1, 0.2f, () =>
                {
                    resetTime = resetTime - 1.0f;
                    TimerMess();
                });
                ModMenu.Instance.KeyPress(KeyCode.Alpha2, 0.2f, () =>
                {
                    resetTime = resetTime + 1.0f;
                    TimerMess();
                });
            }
        }

        private void OnGUI()
        {
            if (Main.settings.do_TrackTricks && showTracker)
            {
                if ((Time.time - trackedTime) > resetTime)
                {
                    trackedTricks = "";
                }

                if (trackedTricks.Length >= 1)
                {
                    GUI.backgroundColor = Color.black;
                    title = new GUIContent(trackerTitle);
                    titleStyle = GUI.skin.window;
                    titleStyle.alignment = TextAnchor.UpperRight;

                    titleSize = titleStyle.CalcSize(title);

                    tricks = new GUIContent(trackedTricks);
                    tricksStyle = GUI.skin.box;
                    tricksStyle.alignment = TextAnchor.LowerCenter;

                    tricksSize = tricksStyle.CalcSize(tricks);

                    trackerRect = new Rect((Screen.width - ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 8)), (Screen.height - ((tricksSize.y * 2) + 8)), ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 4), ((tricksSize.y * 2) + 4));

                    trackerRect = GUI.Window(0, trackerRect, DWG_TrackerRender, title, titleStyle);

                    //TextObjectData.transform.position = TextPos;
                    //TextObjectData.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //TextObjectData.transform.localScale = new Vector3(1, 1, 1);

                    //TextMeshData = TextObjectData.GetComponent<TextMesh>();
                    //TextMeshData.alignment = TextAlignment.Center;
                    //TextMeshData.anchor = TextAnchor.MiddleCenter;
                    //TextMeshData.color = Color.red;
                    //TextMeshData.font = font;
                    //TextMeshData.fontSize = 64;
                    //TextMeshData.offsetZ = 1;
                    //TextMeshData.text = "THIS IS A TEST!!!!!!!!!";
                }
            }
        }

        void DWG_TrackerRender(int windowID)
        {
            trackerRect.Set((Screen.width - ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 8)), (Screen.height - ((tricksSize.y * 2) + 8)), ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 4), ((tricksSize.y * 2) + 4));
            GUI.Label(new Rect(4, ((tricksSize.y * 2) - (tricksSize.y + 2)), ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) - 4), tricksSize.y), tricks, tricksStyle);
        }

        public void OnDestroy() {
            modMenuBox.RemoveLabel("do-trackTricks");
        }

    }
}
