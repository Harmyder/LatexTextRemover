using System;
using System.Linq;

namespace ClrsEncoderSimple.Transitions
{
    internal sealed class TransitionChars : Transition
    {
        private readonly char[] _trigger;
        private readonly TriggerMeaning _triggerMeaning;

        public TransitionChars(State from, State to, string trigger, TriggerMeaning triggerMeaning)
            : this(new[] { from }, to, trigger, triggerMeaning)
        {
        }
        public TransitionChars(State[] from, State to, string trigger, TriggerMeaning triggerMeaning) : base(from, to)
        {
            _trigger = trigger.ToCharArray();
            _triggerMeaning = triggerMeaning;
        }
        public TransitionChars(State from, string trigger, TriggerMeaning triggerMeaning)
            : this(new[] { from }, trigger, triggerMeaning)
        {
        }
        public TransitionChars(State[] from, string trigger, TriggerMeaning triggerMeaning) : base(from)
        {
            _trigger = trigger.ToCharArray();
            _triggerMeaning = triggerMeaning;
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
        public override bool ShouldRewind => _triggerMeaning == TriggerMeaning.Negative;
    }
}
