using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;
using System.Collections.Generic;

namespace DWG_SwapAxis {
    class DWG_SwapAxis : MonoBehaviour {
        ModUIBox modMenuBox;
        ModUILabel modMenuLabelDWG_SwapAxis;

        //private Rect consoleRect = new Rect(20, 10, 532, 128);
        //static public List<string> DWG_ConMessage = new List<string> { };
        //static public string DWG_ConSingleMessage = "";

        //private bool showCon = false;
        //private bool createdCon = false;

        //private Vector2 messageScrollPos = Vector2.zero;

        public void Start() {
            modMenuBox = ModMenu.Instance.RegisterModMaker("dwg", "DeadWalking");
            modMenuLabelDWG_SwapAxis = modMenuBox.AddLabel("do-swapaxis", LabelType.Toggle, "SwapAxis Toggle (Q)", Side.left, () => Main.enabled, Main.settings.do_SwapAxis && Main.enabled, (b) => Main.settings.do_SwapAxis = b, 1);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.Q, 0.2f, () =>
                {
                    Main.settings.do_SwapAxis = !Main.settings.do_SwapAxis;

                    modMenuLabelDWG_SwapAxis.SetToggleValue(Main.settings.do_SwapAxis);
                    if (Main.settings.do_SwapAxis)
                    {
                        ModMenu.Instance.ShowMessage("DWG SwapAxis: Enabled");
                    }
                    else
                    {
                        ModMenu.Instance.ShowMessage("DWG SwapAxis: Disabled");
                    }
                });
                //ModMenu.Instance.KeyPress(KeyCode.W, 0.2f, () => {
                //    showCon = !showCon;
                //});
            }
        }

        //private void OnGUI()
        //{
        //    //if (!createdCon)
        //    //{
        //    //    GUI.backgroundColor = Color.black;
        //    //    consoleRect = GUI.Window(0, consoleRect, DWG_Console_Draw, "DWG Console");
        //    //    //GUI.enabled = false;

        //    //    createdCon = true;
        //    //}

        //    if (showCon)
        //    {
        //        GUI.backgroundColor = Color.black;
        //        consoleRect = GUI.Window(0, consoleRect, DWG_Console_Draw, "DWG Console");

        //        //GUI.enabled = true;
        //        //ModMenu.Instance.ShowCursor(Main.modId);
        //    }
        //    else
        //    {
        //        //GUI.enabled = false;
        //        //ModMenu.Instance.HideCursor(Main.modId);
        //    }
        //}

        //void DWG_Console_Draw(int windowID)
        //{
        //    //if (!createdCon)
        //    //{
        //    //    //    consoleStyle = new GUIStyle(GUI.skin.box)
        //    //    //    {
        //    //    //        padding = new RectOffset(14, 14, 24, 9),
        //    //    //        contentOffset = new Vector2(0, 0),
        //    //    //        fontSize = 18
        //    //    //    };
        //    //    //    consoleStyle.normal.textColor = Color.red;

        //    //    //    //scrollStyle = new GUIStyle(GUI.skin.verticalScrollbar);

        //    //    createdCon = true;

        //    //    GUI.DragWindow(new Rect(0, 0, 0, 0));

        //    //    GUI.BeginScrollView(new Rect(10, 20, 512, 127), new Vector2(0, 127), new Rect(10, 20, 512, 124), false, false, GUIStyle.none, GUIStyle.none);
        //    //    GUI.Label(new Rect(0, 20, 512, 18), "DOUBLE CHECK");
        //    //};
        //    //GUI.Label(new Rect(0, (20 + (20 * DWG_ConMessage.Count)), 512, 18), "DOUBLE CHECK");
        //    //for (int i = 0; i < DWG_ConMessage.Count; i++)
        //    //{
        //    //    GUI.Label(new Rect(0, (20 + (20 * DWG_ConMessage.Count)), 512, 18), DWG_ConMessage[DWG_ConMessage.Count]);
        //    //    if (DWG_ConMessage.Count >= 100)
        //    //    {
        //    //        DWG_ConMessage.RemoveAt(0);
        //    //        //messageScrollPos.y = messageScrollPos.y - 18;
        //    //    }
        //    //    //else
        //    //    //{
        //    //    //    messageScrollPos.y = messageScrollPos.y + 18;
        //    //    //}
        //    //}
        //    //GUI.ScrollTo(new Rect(10, (20 + (20 * DWG_ConMessage.Count)), 512, 127));
        //    //GUI.EndScrollView();

        //    GUI.DragWindow(new Rect(0, 0, 0, 0));
        //    GUI.Label(new Rect(0, 20, 512, 18), DWG_ConSingleMessage);
        //}


        public void OnDestroy() {
            modMenuBox.RemoveLabel("do-swapaxis");
        }

    }
}
