using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.Utilities;

namespace SynthesisAPI.Simulation {
    public static class SimulationManager {
        public delegate void SimObjectEvent(SimObject entity);

        public static event SimObjectEvent OnNewSimulationObject;
        public static event SimObjectEvent OnRemoveSimulationObject;
        
        internal static Dictionary<string, SimObject> _simObjects = new Dictionary<string, SimObject>();
        public static IReadOnlyDictionary<string, SimObject> SimulationObjects
            = new ReadOnlyDictionary<string, SimObject>(_simObjects);

        public delegate void UpdateDelegate();
        public static event UpdateDelegate OnDriverUpdate;
        public static event UpdateDelegate OnBehaviourUpdate;

        public static void Update() {
            _simObjects.ForEach(x => {
                x.Value.Drivers.ForEach(y => {
                    y.Update();
                });
                if (OnDriverUpdate != null)
                    OnDriverUpdate();
                x.Value.Behaviours.ForEach(y => {
                    y.Update();
                });
                if (OnBehaviourUpdate != null)
                    OnBehaviourUpdate();
            });
        }

        public static void RegisterSimObject(SimObject so) {
            if (_simObjects.ContainsKey(so.Name)) // Probably use some GUID
                throw new Exception("Name already exists");
            _simObjects[so.Name] = so;

            if (OnNewSimulationObject != null)
                OnNewSimulationObject(so);
        }

        public static bool RemoveSimObject(SimObject so) {
            return RemoveSimObject(so.Name);
        }

        public static bool RemoveSimObject(string so) {
            bool exists = _simObjects.TryGetValue(so, out SimObject s);
            if (!exists)
                return false;
            s.RemoveDrivers();
            s.RemoveBehaviours();
            var res = _simObjects.Remove(so);
            if (res) {
                s.Destroy();
                if (OnRemoveSimulationObject != null) {
                    OnRemoveSimulationObject(s);
                }
            } else {
                Logger.Log("No sim object found by that name", LogLevel.Warning);
            }
            return res;
        }
    }
}