using Modding;
using System.Collections.Generic;
using UnityEngine;
using SFCore.Utils;

namespace VoidManipulation {
    public class VoidManipulation : Mod {
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
        }
    }
}