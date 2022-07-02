namespace ClrsEncoderSimple.Transitions
{
    internal class Transition
    {
        public static Transition Empty = new Transition(State.None);
        protected Transition(State from, State to) : this(new State[] { from }, to) { }
        protected Transition(State[] from, State to)
        {
            From = from;
            To = to;
            IsClosing = false;
        }
        protected Transition(State from) : this(new State[] { from }) { }
        protected Transition(State[] from)
        {
            From = from;
            To = State.None;
            IsClosing = true;
        }
        public State[] From { get; }
        public State To { get; }
        public bool IsClosing { get; }
        public virtual string? Mark => null;
        public virtual bool ShouldRewind { get; }
        public virtual Applicability IsApplicable(string next) => Applicability.No;
    }
}
