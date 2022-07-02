using System.Linq;

namespace ClrsEncoderSimple.Transitions
{
    internal class TransitionEnvironmentAny : Transition
    {
        private const string BeginPrefix = "\\begin";
        private const string EndPrefix = "\\end";
        private readonly string Prefix;

        private string? _lastFoundEnvironment;

        public TransitionEnvironmentAny(State[] from) : base(from, State.Environment)
        {
            Prefix = BeginPrefix;
        }

        public TransitionEnvironmentAny(State from) : base(from, State.Environment)
        {
            Prefix = BeginPrefix;
        }

        public TransitionEnvironmentAny() : base(State.Environment)
        {
            Prefix = EndPrefix;
        }

        public override string? Mark => _lastFoundEnvironment;
        public override bool ShouldRewind => true;
        public override Applicability IsApplicable(string next)
        {
            _lastFoundEnvironment = null;
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
                    _lastFoundEnvironment = envName;
                    return Applicability.Yes;
                }
                else
                {
                    return Applicability.Inconclusive;
                }
            }
        }
    }
}
