using UnityEngine;
using XLShredLib;
using XLShredLib.UI;
using XInputDotNetPure;

namespace DWG_SwapAxis {
    class DWG_SwapAxis : MonoBehaviour {
        static private ModUIBox modMenuBox;
        static private ModUILabel modMenuLabelDWG_SwapAxis;

        //private Rect consoleRect = new Rect(20, 10, 532, 128);
        //static public List<string> DWG_ConMessage = new List<string> { };
        //static public string DWG_ConSingleMessage = "";

        static private bool showCon = false;
        static private GUIContent consoleCont;
        static private GUIStyle consoleStyle;

        static public bool SwapToesAxis = false;

        public void Start() {
            modMenuBox = ModMenu.Instance.RegisterModMaker("dwg", "DeadWalking");
            modMenuLabelDWG_SwapAxis = modMenuBox.AddLabel("do-swapaxis", LabelType.Toggle, "SwapAxis Toggle (Ctrl+Q)", Side.left, () => Main.enabled, Main.settings.do_SwapAxis && Main.enabled, (b) => Main.settings.do_SwapAxis = b, 1);
        }

        static private bool initGui = false;
        private void Update() {
            if (Main.enabled && Input.GetKey(KeyCode.LeftControl))
            {
                ModMenu.Instance.KeyPress(KeyCode.W, 0.2f, () => { showCon = !showCon; });
                ModMenu.Instance.KeyPress(KeyCode.Q, 0.2f, () =>
                {
                    Main.settings.do_SwapAxis = !Main.settings.do_SwapAxis;

                    modMenuLabelDWG_SwapAxis.SetToggleValue(Main.settings.do_SwapAxis);
                    ModMenu.Instance.ShowMessage("DWG SwapAxis: " + (Main.settings.do_SwapAxis ? "Enabled" : "Disabled"));
                });
            }
            //SwapToesAxis = (((SettingsManager.Instance.stance == SettingsManager.Stance.Regular) && PlayerController.Instance.IsSwitch) || (!(SettingsManager.Instance.stance == SettingsManager.Stance.Regular) && !PlayerController.Instance.IsSwitch));
        }

        private void OnGUI()
        {
            //if (Main.enabled && showCon)
            //{
            //    if (!initGui)
            //    {
            //        GUI.backgroundColor = Color.black;
            //        initGui = true;
            //        consoleCont = new GUIContent("");
            //        consoleStyle = GUI.skin.box;
            //    }

            //    if (showCon)
            //    {
            //        consoleStyle.alignment = TextAnchor.UpperLeft;
            //        consoleCont.text = (
            //                            "LFlipDir:   " + PlayerController.Instance.inputController.LeftStick.FlipDir + "\n" +
            //                            "RFlipDir:   " + PlayerController.Instance.inputController.RightStick.FlipDir + "\n" +
            //                            "BrdFwd:   " + PlayerController.Instance.boardController.IsBoardBackwards + "\n" +
            //                            "IsSwitch: " + PlayerController.Instance.IsSwitch + "\n" +
            //                            "RightPop: " + PlayerController.Instance.inputController.RightStick.IsPopStick
            //                            );

            //        Vector2 conSize = consoleStyle.CalcSize(consoleCont);
            //        GUI.Label(new Rect(4, 4, (Screen.width / 5), conSize.y), consoleCont, consoleStyle);
            //    };
            //};
        }

        private void OnDestroy() {
            modMenuBox.RemoveLabel("do-swapaxis");
        }

    }
}
