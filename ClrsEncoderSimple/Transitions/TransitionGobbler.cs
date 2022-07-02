using System.Linq;

namespace ClrsEncoderSimple.Transitions
{
    internal class TransitionGobbler : Transition
    {
        private readonly string[] _trigger;
        public TransitionGobbler(State from, params string[] trigger) : this(new[] { from }, trigger) { }
        public TransitionGobbler(State[] from, params string[] trigger) : base(from, State.None)
        {
            _trigger = trigger;
        }

        public override Applicability IsApplicable(string next)
        {
            string? fallback = null;
            foreach (var trigger in _trigger)
            {
                if (trigger == next)
                {
                    fallback = trigger;
                }
            }
            var inconclusive = _trigger.Where(t => t.Length > next.Length && t.StartsWith(next));
            if (inconclusive.Any())
            {
                return Applicability.Inconclusive;
            }
            else
            {
                if (fallback != null)
                {
                    fallback = null;
                    return Applicability.Yes;
                }
                return Applicability.No;
            }
        }
    }
}
