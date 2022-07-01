using ClrsEncoderSimple.Transitions;

namespace ClrsEncoderSimple
{
    internal static class LatexScheme
    {
        public static readonly Transition[] Transitions = new Transition[]
        {
            new TransitionChars(State.Plain, State.Command, "\\", TriggerMeaning.Positive),
            new TransitionChars(State.Plain, State.MathInline, "$", TriggerMeaning.Positive),
            new TransitionChars(State.MathInline, "$", TriggerMeaning.Positive),
            new TransitionStrings(State.Plain, State.MathDisplayLatex, "\\["),
            new TransitionStrings(State.MathDisplayLatex, "\\]"),
            new TransitionStrings(State.Plain, State.MathDisplayDollars, "$$"),
            new TransitionStrings(State.MathDisplayDollars, "$$"),
            new TransitionChars(State.Command, CharClasses.Alphas, TriggerMeaning.Negative, true),
            new TransitionChars(State.Plain, State.Brackets, "[", TriggerMeaning.Positive),
            new TransitionChars(State.Brackets, State.Brackets, "[", TriggerMeaning.Positive),
            new TransitionChars(State.Plain, State.CurlyBraces, "{", TriggerMeaning.Positive),
            new TransitionChars(State.CurlyBraces, State.CurlyBraces, "{", TriggerMeaning.Positive),
            new TransitionChars(State.Brackets, "]", TriggerMeaning.Positive),
            new TransitionChars(State.CurlyBraces, "}", TriggerMeaning.Positive),
            new TransitionChars(State.Brackets, State.MathInline, "$", TriggerMeaning.Positive),
            new TransitionChars(State.CurlyBraces, State.MathInline, "$", TriggerMeaning.Positive),
        };
    }
}
