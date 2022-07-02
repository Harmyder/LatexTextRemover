using System.Linq;

namespace ClrsEncoderSimple.Transitions
{
    internal class TransitionCommandOneArg : Transition
    {
        private readonly string[]? _trigger;

        public TransitionCommandOneArg(State from, params string[] trigger) : this(new[] { from }, trigger) { }

        public TransitionCommandOneArg(State[] from, params string[] trigger) : base(from, State.CommandOneArg)
        {
            _trigger = trigger;
        }

        public TransitionCommandOneArg() : base(State.CommandOneArg)
        {
            _trigger = null;
        }

        public override bool ShouldRewind { get; }
        public override Applicability IsApplicable(string next)
        {
            if (From.Length == 1 && From.Single() == State.CommandOneArg)
            {
                return next == "}" ? Applicability.Yes : Applicability.No;
            }
            else
            {
                if (next.Length == 1)
                {
                    return next == "\\" ? Applicability.Inconclusive : Applicability.No;
                }
                if (!CharClasses.Alphas.Contains(next.Last()))
                {
                    if (next.Last() != '{')
                    {
                        return Applicability.No;
                    }
                    var command = next.Substring(1, next.Length - 2);
                    foreach (var trigger in _trigger)
                    {
                        if (trigger == command)
                        {
                            return Applicability.Yes;
                        }
                    }
                    return Applicability.No;
                }
                else
                {
                    var commandPrefix = next.Substring(1);
                    foreach (var trigger in _trigger)
                    {
                        if (trigger.StartsWith(commandPrefix))
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
