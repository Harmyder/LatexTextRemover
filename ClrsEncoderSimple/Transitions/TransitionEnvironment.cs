using System.Linq;

namespace ClrsEncoderSimple.Transitions
{
    internal class TransitionEnvironment : Transition
    {
        private readonly string[] _trigger;
        private const string BeginPrefix = "\\begin";
        private const string EndPrefix = "\\end";
        private readonly string Prefix;

        public TransitionEnvironment(State from, params string[] trigger) : base(from, State.Environment)
        {
            _trigger = trigger;
            Prefix = BeginPrefix;
        }

        public TransitionEnvironment(params string[] trigger) : base(State.Environment)
        {
            _trigger = trigger;
            Prefix = EndPrefix;
        }

        public override bool ShouldRewind => false;
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
                if (next.Last() == '}')
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
