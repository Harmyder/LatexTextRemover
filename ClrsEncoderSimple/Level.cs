namespace ClrsEncoderSimple
{
    internal class Level
    {
        public Level(State state, string? mark)
        {
            State = state;
            Mark = mark;
        }
        public State State { get; }
        public string? Mark { get; }
        public override string? ToString()
        {
            return $"{State}({Mark})";
        }
    }
}
