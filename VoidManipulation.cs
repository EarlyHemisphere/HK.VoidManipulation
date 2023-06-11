using Modding;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using SFCore.Utils;

namespace VoidManipulation {
    public class VoidManipulation : Mod, ITogglableMod {
        internal static VoidManipulation instance;

        public VoidManipulation() : base("Void Manipulation") {
            instance = this;
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            Log("Initializing");

            instance = this;
            On.PlayMakerFSM.OnEnable += OnFsmEnable;

            Log("Initialized");
        }

        private void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);

            if (self.FsmName == "raise_abyss_pit") {
                EnableDescent(self);
            }
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public bool ToggleButtonInsideMenu => true;

        public void EnableDescent(PlayMakerFSM fsm) {
            fsm.AddFsmTransition("Set", "FINISHED", "Idle");
            HazardRespawnTrigger trigger = fsm.gameObject.GetComponent<HazardRespawnTrigger>();
            trigger.fireOnce = false;
            trigger.GetType().GetField("inactive", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(trigger, false);
        }

        public void Unload() {
            GameObject ascendRespawns = GameObject.Find("Ascend Respawns");
            if (ascendRespawns) {
                foreach (Transform child in ascendRespawns.transform) {
                    if (child.gameObject.name.Contains("Hazard Respawn Trigger")) {
                        child.gameObject.LocateMyFSM("raise_abyss_pit").RemoveFsmTransition("Set", "FINISHED");
                        HazardRespawnTrigger trigger = child.gameObject.GetComponent<HazardRespawnTrigger>();
                        trigger.fireOnce = true;
                    }
                }
            }
        }
    }
}