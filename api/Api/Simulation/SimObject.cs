using SynthesisAPI.Utilities;
using System.Collections.Generic;

namespace SynthesisAPI.Simulation {
    public class SimObject {
        protected string _name;
        public string Name {
            get => _name;
        }

        protected ControllableState _state;
        public ControllableState State {
            get => _state;
        }

        protected bool _behavioursEnabled = true;
        public bool BehavioursEnabled {
            get => _behavioursEnabled;
            set {
                _behavioursEnabled = value;
            }
        }

        private List<Driver> _drivers = new List<Driver>();
        public IReadOnlyCollection<Driver> Drivers => _drivers.AsReadOnly();
        private List<SimBehaviour> _behaviours = new List<SimBehaviour>();
        public IReadOnlyCollection<SimBehaviour> Behaviours => _behaviours.AsReadOnly();

        // This was Init. No idea why but it might need to be
        public SimObject(string name, ControllableState state)
        {
            _name = name;
            _state = state;
        }

        public virtual void Destroy() { }

        public List<string> GetAllReservedInputs() {
            var res = new List<string>();
            _behaviours.ForEach(x => x.ReservedInput.ForEach(y => res.Add(y)));
            return res;
        }

        public void AddDriver(Driver d) {
            _drivers.Add(d);
        }

        public void RemoveDriver(Driver d) {
            _drivers.RemoveAll(x => x.Equals(d));
        }

        public void RemoveDrivers() {
            _drivers.Clear();
        }

        public void AddBehvaiour(SimBehaviour sb) {
            _behaviours.Add(sb);
        }

        public void RemoveBehaviour(SimBehaviour sb) {
            _behaviours.RemoveAll(x => x.Equals(sb));
        }

        public void RemoveBehaviours() {
            _behaviours.Clear();
        }

        public override int GetHashCode()
            => _name.GetHashCode() * 482901849;
    }
}