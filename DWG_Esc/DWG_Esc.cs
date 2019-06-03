using UnityEngine;
using XLShredLib;
using XLShredLib.UI;
using XInputDotNetPure;

namespace DWG_SwapAxis {
    class DWG_Esc : MonoBehaviour {
        static private ModUIBox modMenuBox;
        static private ModUILabel modMenuLabelDWG_SwapAxis;


        public void Start() {
            modMenuBox = ModMenu.Instance.RegisterModMaker("dwg", "DeadWalking");
            modMenuLabelDWG_SwapAxis = modMenuBox.AddLabel("do-swapaxis", LabelType.Toggle, "SwapAxis Toggle (Ctrl+Q)", Side.left, () => Main.enabled, Main.settings.do_SwapAxis && Main.enabled, (b) => Main.settings.do_SwapAxis = b, 1);
        }

        static private bool initGui = false;
        private void Update() {
            ModMenu.Instance.
            if (Main.enabled && Input.GetKey(KeyCode.LeftControl))
            {
                //ModMenu.Instance.KeyPress(KeyCode.W, 0.2f, () => { showCon = !showCon; });
                //ModMenu.Instance.KeyPress(KeyCode.Q, 0.2f, () =>
                //{
                //    Main.settings.do_SwapAxis = !Main.settings.do_SwapAxis;

                //    modMenuLabelDWG_SwapAxis.SetToggleValue(Main.settings.do_SwapAxis);
                //    ModMenu.Instance.ShowMessage("DWG SwapAxis: " + (Main.settings.do_SwapAxis ? "Enabled" : "Disabled"));
                //});
            }
        }

        private void OnGUI()
        {
            
        }

        private void OnDestroy() {
            modMenuBox.RemoveLabel("do-swapaxis");
        }

    }
}
