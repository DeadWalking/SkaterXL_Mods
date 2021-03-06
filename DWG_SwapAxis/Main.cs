﻿using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;
using XLShredLib;

namespace DWG_SwapAxis{
    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        public bool do_SwapAxis;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }
    }

    static class Main
    {
        public static string modId;
        public static bool enabled;
        public static Settings settings;
        public static HarmonyInstance harmonyInstance;

        static bool Load(UnityModManager.ModEntry modEntry) {
            settings = Settings.Load<Settings>(modEntry);

            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;

            modId = modEntry.Info.Id;

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            if (enabled == value) return true;
            enabled = value;
            if (enabled) {
                harmonyInstance = HarmonyInstance.Create(modId);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                ModMenu.Instance.gameObject.AddComponent<DWG_SwapAxis>();
            } else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<DWG_SwapAxis>());
            }
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
