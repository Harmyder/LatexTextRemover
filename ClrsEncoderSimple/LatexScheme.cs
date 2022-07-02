using ClrsEncoderSimple.Transitions;
using System.Linq;

namespace ClrsEncoderSimple
{
    internal static class LatexScheme
    {
        private static readonly State[] PlainEnv = new[] { State.Plain, State.Environment };
        private static readonly State[] MathAll = new[] { State.MathInline, State.MathDisplayLatex, State.MathDisplayDollars };
        private static readonly State[] PlainEnvBra = new[] { State.Plain, State.Environment, State.Brackets, State.CurlyBraces, State.CommandOneArg };

        public static readonly Transition[] Transitions = new Transition[]
        {
            new TransitionChars(PlainEnvBra, State.Command, "\\", TriggerMeaning.Positive),
            new TransitionGobbler(PlainEnvBra.Concat(MathAll).ToArray(), "\\$", "\\{", "\\\\"),
            new TransitionChars(PlainEnvBra, State.MathInline, "$", TriggerMeaning.Positive),
            new TransitionChars(State.MathInline, "$", TriggerMeaning.Positive),
            new TransitionStrings(PlainEnv, State.MathDisplayLatex, "\\["),
            new TransitionStrings(State.MathDisplayLatex, "\\]"),
            new TransitionStrings(PlainEnv, State.MathDisplayDollars, "$$"),
            new TransitionStrings(State.MathDisplayDollars, "$$"),
            new TransitionChars(State.Command, CharClasses.Alphas, TriggerMeaning.Negative),
            new TransitionChars(PlainEnvBra, State.Brackets, "[", TriggerMeaning.Positive),
            new TransitionChars(PlainEnvBra, State.CurlyBraces, "{", TriggerMeaning.Positive),
            new TransitionChars(State.Brackets, "]", TriggerMeaning.Positive),
            new TransitionChars(State.CurlyBraces, "}", TriggerMeaning.Positive),
            new TransitionEnvironmentAny(PlainEnv),
            new TransitionEnvironmentAny(),
            new TransitionCommandOneArg(PlainEnv, "subheading", "section", "figcaption"),
            new TransitionCommandOneArg(),
        };

        public static readonly Level[] ReplacementLevels = new Level[]
        {
            new Level(State.Plain, null),
            new Level(State.Environment, "corollary"),
            new Level(State.Environment, "corollary"),
            new Level(State.Environment, "description"),
            new Level(State.Environment, "descriptiontext"),
            new Level(State.Environment, "enumerate"),
            new Level(State.Environment, "exercises"),
            new Level(State.Environment, "hanglist"),
            new Level(State.Environment, "itemize"),
            new Level(State.Environment, "lemma"),
            new Level(State.Environment, "letters"),
            new Level(State.Environment, "letters"),
            new Level(State.Environment, "letters"),
            new Level(State.Environment, "loopinv"),
            new Level(State.Environment, "loopinvproof"),
            new Level(State.Environment, "minipage"),
            new Level(State.Environment, "problemparts"),
            new Level(State.Environment, "problems"),
            new Level(State.Environment, "proof"),
            new Level(State.Environment, "quotation"),
            new Level(State.Environment, "quote"),
            new Level(State.Environment, "theorem"),
            new Level(State.CommandOneArg, null),
        };
    }
}
