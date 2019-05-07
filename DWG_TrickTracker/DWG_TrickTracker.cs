using UnityEngine;
using XLShredLib;
using XLShredLib.UI;
using TMPro;
using System.Collections.Generic;

namespace DWG_TrickTracker
{
    class DWG_TrickTracker : MonoBehaviour {
        ModUIBox modMenuBox;
        ModUILabel modMenuLabel;

        private Rect trackerRect;
        private bool showTracker;
        private readonly string trackerTitle = "Trick Tracker";

        private GUIContent title;
        private GUIStyle titleStyle;
        private Vector2 titleSize;

        private GUIContent tricks;
        private GUIStyle tricksStyle;
        private Vector2 tricksSize;

        static public bool trackBail;
        static public bool TrackBail
        {
            get { return trackBail; }
            set { trackBail = value; }
        }

        static public string trackedTricks;
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
            modMenuLabel = modMenuBox.AddLabel("do-trackTricks", LabelType.Text, trackerTitle + " Toggle (W)", Side.right, () => Main.enabled, false, (b) => Main.settings.do_TrackTricks = b, 1);
            Main.settings.do_TrackTricks = false;
            showTracker = false;
            TrackedTricks = "";
            TrackedTime = Time.time;
            //TextObjectData = new GameObject(); // GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //TextObjectData.AddComponent<TextMesh>();

            //TextMeshData = TextObjectData.GetComponent<TextMesh>();
            //TextRendererData = TextObjectData.AddComponent<MeshRenderer>();
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.W, 0.2f, () =>
                {
                    Main.settings.do_TrackTricks = !Main.settings.do_TrackTricks;

                    modMenuLabel.SetToggleValue(Main.settings.do_TrackTricks);

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
                if ((Time.time - TrackedTime) > resetTime)
                {
                    TrackedTricks = "";
                }

                if (TrackedTricks.Length >= 1)
                {
                    GUI.backgroundColor = Color.black;
                    title = new GUIContent(trackerTitle);
                    titleStyle = GUI.skin.window;
                    titleStyle.alignment = TextAnchor.UpperRight;

                    titleSize = titleStyle.CalcSize(title);

                    tricks = new GUIContent(TrackedTricks);
                    tricksStyle = GUI.skin.box;
                    tricksStyle.alignment = TextAnchor.LowerCenter;

                    tricksSize = tricksStyle.CalcSize(tricks);

                    trackerRect = new Rect((Screen.width - ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 8)), (Screen.height - ((tricksSize.y * 2) + 8)), ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 4), ((tricksSize.y * 2) + 4));

                    trackerRect = GUI.Window(0, trackerRect, TrackerRender, title, titleStyle);

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

        public void OnDestroy() {
            modMenuBox.RemoveLabel("do-trackTricks");
        }

        private void TimerMess()
        {
            ModMenu.Instance.ShowMessage(trackerTitle + " Reset Time (s): " + resetTime);
        }

        private void TrackerRender(int windowID)
        {
            trackerRect.Set((Screen.width - ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 16)), (Screen.height - ((tricksSize.y * 2) + 8)), ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 8), ((tricksSize.y * 2) + 4));
            GUI.Label(new Rect(4, ((tricksSize.y * 2) - (tricksSize.y + 2)), (titleSize.x >= tricksSize.x ? (titleSize.x - 4) : tricksSize.x), tricksSize.y), tricks, tricksStyle);
        }

        static public void AddTrick(string newTrick, bool isRot, bool isGrind)
        {
            TrackedTricks = TrackedTricks +
                            (((TrackedTricks.Length > 0 && !isRot) ? " + " : "") +
                            (isRot ? " " : "") +
                            (PlayerController.Instance.IsSwitch && !isRot && !isGrind ? "Switch" : "") +
                            newTrick);
        }

        static public bool LeftIsFront()
        {
            return (
                   ((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) && PlayerController.Instance.IsSwitch) ||
                   ((SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) && !PlayerController.Instance.IsSwitch)
                   ) ? true : false;
        }

        //static public Transform boardPos;
        //static public Transform BoardPos
        //{
        //    get { return boardPos; }
        //    set { boardPos = value; }
        //}

        //static public Transform camTransform;
        //static public Transform CamTransform
        //{
        //    get { return camTransform; }
        //    set { camTransform = value; }
        //}

        //static private int trackManual;
        //static public int TrackManual
        //{
        //    get { return trackManual; }
        //    set { trackManual = value; }
        //}

        static private float trackSkaterRot;
        static public float TrackSkaterRot
        {
            get { return trackSkaterRot; }
            set { trackSkaterRot = value; }
        }

        static private string trackBoardRot;
        static public string TrackBoardRot
        {
            get { return trackBoardRot; }
            set { trackBoardRot = value; }
        }

        static private string trackTrig;
        static public string TrackTrig
        {
            get { return trackTrig; }
            set { trackTrig = value; }
        }

        static public void CheckRot()
        {
            string outRot;

            switch (TrackSkaterRot)
            {
                case var _ when TrackSkaterRot >= 670:
                    outRot = "720";
                    break;
                case var _ when TrackSkaterRot >= 490:
                    outRot = "540";
                    break;
                case var _ when TrackSkaterRot >= 310:
                    outRot = "360";
                    break;
                case var _ when TrackSkaterRot >= 130:
                    outRot = "180";
                    break;
                default:
                    outRot = "";
                    break;
            };

            TrackSkaterRot = 0f;

            if ((outRot != "" || TrackBoardRot != "") && !TrackBail)
            {
                AddTrick(TrackTrig + ((outRot != "") ? outRot : "") + ((TrackBoardRot != "") ? " " + TrackBoardRot : ""), true, false);
                TrackTrig = "";
                TrackBoardRot = "";
            };
        }
    }
}
