using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

namespace DWG_TrickTracker
{
    class DWG_TrickTracker : MonoBehaviour {
        ModUIBox modMenuBox;
        ModUILabel modMenuLabelDWG_TrickTracker;

        GUIContent title;
        GUIStyle titleStyle;
        Vector2 titleSize;

        GUIContent tricks;
        GUIStyle tricksStyle;
        Vector2 tricksSize;

        //private Rect trackerRect = new Rect((Screen.width - 516), (Screen.height - 132), 512, 128);
        private Rect TrackerRect;
        static public string DWG_TrackedTricks;
        private bool DWG_ShowTracker;

        public void Start() {
            modMenuBox = ModMenu.Instance.RegisterModMaker("dwg", "DeadWalking");
            modMenuLabelDWG_TrickTracker = modMenuBox.AddLabel("do-trackTricks", LabelType.Text, "TickTracker Toggle (W)", Side.right, () => Main.enabled, false, (b) => Main.settings.do_TrackTricks = b, 1);
            Main.settings.do_TrackTricks = false;
            DWG_ShowTracker = false;
            DWG_TrackedTricks = "";
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.W, 0.2f, () =>
                {
                    Main.settings.do_TrackTricks = !Main.settings.do_TrackTricks;

                    modMenuLabelDWG_TrickTracker.SetToggleValue(Main.settings.do_TrackTricks);

                    DWG_ShowTracker = Main.settings.do_TrackTricks;
                    ModMenu.Instance.ShowMessage(Main.settings.do_TrackTricks ? "DWG TrickTracker: Enabled" : "DWG TrickTracker: Disabled");
                });
            }
        }

        private void OnGUI()
        {
            if (Main.settings.do_TrackTricks && DWG_ShowTracker && DWG_TrackedTricks.Length >= 1)
            {
                GUI.backgroundColor = Color.black;
                title = new GUIContent("DWG Trick Tracker");
                titleStyle = GUI.skin.window;
                titleStyle.alignment = TextAnchor.UpperRight;

                titleSize = titleStyle.CalcSize(title);

                tricks = new GUIContent(DWG_TrackedTricks);
                tricksStyle = GUI.skin.box;
                tricksStyle.alignment = TextAnchor.LowerCenter;

                tricksSize = tricksStyle.CalcSize(tricks);

                TrackerRect = new Rect((Screen.width - ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 8)), (Screen.height - ((tricksSize.y * 2) + 8)), ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 4), ((tricksSize.y * 2) + 4));

                TrackerRect = GUI.Window(0, TrackerRect, DWG_TrackerRender, title, titleStyle);
            }
        }

        void DWG_TrackerRender(int windowID)
        {
            TrackerRect.Set((Screen.width - ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 8)), (Screen.height - ((tricksSize.y * 2) + 8)), ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) + 4), ((tricksSize.y * 2) + 4));
            GUI.Label(new Rect(4, ((tricksSize.y * 2) - (tricksSize.y + 2)), ((titleSize.x >= tricksSize.x ? titleSize.x : tricksSize.x) - 4), tricksSize.y), tricks, tricksStyle);
        }


        public void OnDestroy() {
            modMenuBox.RemoveLabel("do-trackTricks");
        }

    }
}
