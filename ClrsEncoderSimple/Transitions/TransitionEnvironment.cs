using System.Linq;

namespace ClrsEncoderSimple.Transitions
{
    internal class TransitionEnvironment : Transition
    {
        private readonly string[] _trigger;
        private const string Prefix = "\\begin";

        public TransitionEnvironment(State from, State to, params string[] trigger) : base(from, to)
        {
            _trigger = trigger;
        }

        public override bool ShouldRewind { get; }
        public override Applicability IsApplicable(string next)
        {
            if (next.Length <= Prefix.Length)
            {
                return Prefix.StartsWith(next) ? Applicability.Inconclusive : Applicability.No;
            }
            else
            {
                if (next[Prefix.Length] != '{')
                {
                    return Applicability.No;
                }
                var suffix = next.Substring(Prefix.Length + 1);
                if (suffix.Last() == '}')
                {
                    var envName = suffix.Substring(0, suffix.Length - 1);
                    foreach (var trigger in _trigger)
                    {
                        if (trigger == envName)
                        {
                            return Applicability.Yes;
                        }
                    }
                    return Applicability.No;
                }
                else
                {
                    foreach (var trigger in _trigger)
                    {
                        if (trigger.StartsWith(suffix))
                        {
                            return Applicability.Inconclusive;
                        }
                    }
                    return Applicability.No;
                }
            }
        }
    }
}
