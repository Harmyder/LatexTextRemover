using System.IO;

namespace ClrsEncoderSimple.Transitions
{
    internal sealed class TransitionStrings : Transition
    {
        private readonly string[] _trigger;

        public TransitionStrings(State from, State to, params string[] trigger) : base(from, to)
        {
            _trigger = trigger;
        }
        public TransitionStrings(State[] from, State to, params string[] trigger) : base(from, to)
        {
            _trigger = trigger;
        }
        public TransitionStrings(State from, params string[] trigger) : base(from)
        {
            _trigger = trigger;
        }
        public TransitionStrings(State[] from, params string[] trigger) : base(from)
        {
            _trigger = trigger;
        }
        public override Applicability IsApplicable(string next)
        {
            var ret = Applicability.None;
            var isPrefix = false;
            foreach (var trigger in _trigger)
            {
                if (trigger.StartsWith(next))
                {
                    isPrefix = true;
                    if (trigger == next)
                    {
                        if (ret != Applicability.None)
                        {
                            throw new InvalidDataException("More than one trigger works");
                        }
                        ret = Applicability.Yes;
                    }
                }
            }
            if (ret == Applicability.None)
            {
                ret = isPrefix ? Applicability.Inconclusive : Applicability.None;
            }
            return ret;
        }

        public override bool ShouldRewind => false;
    }
}
