using System;
using System.Linq;

namespace ClrsEncoderSimple.Transitions
{
    internal sealed class TransitionChars : Transition
    {
        private readonly char[] _trigger;
        private readonly TriggerMeaning _triggerMeaning;
        private readonly bool _shouldRewind;

        public TransitionChars(State from, State to, string trigger, TriggerMeaning triggerMeaning, bool shouldGobble = false) : base(from, to)
        {
            _trigger = trigger.ToCharArray();
            _triggerMeaning = triggerMeaning;
            _shouldRewind = shouldGobble;
        }
        public TransitionChars(State from, string trigger, TriggerMeaning triggerMeaning, bool shouldRewind = false) : base(from)
        {
            _trigger = trigger.ToCharArray();
            _triggerMeaning = triggerMeaning;
            _shouldRewind = shouldRewind;
        }
        public override Applicability IsApplicable(string next)
        {
            if (string.IsNullOrEmpty(next))
            {
                return _trigger.Length > 0 ? Applicability.No : Applicability.Yes;
            }
            if (next.Length > 1)
            {
                throw new ArgumentException("Expected single character");
            }
            if (_triggerMeaning == TriggerMeaning.Positive)
            {
                return _trigger.Contains(next[0]) ? Applicability.Yes : Applicability.No;
            }
            else if (_triggerMeaning == TriggerMeaning.Negative)
            {
                return _trigger.Contains(next[0]) ? Applicability.No : Applicability.Yes;
            }
            else
            {
                throw new InvalidOperationException("Wrong trigger meaning");
            }
        }
        public override bool ShouldRewind => _shouldRewind;
    }
}
