namespace ClrsEncoderSimple
{
    internal enum State
    {
        None,
        Plain,
        MathInline,
        MathDisplayDollars,
        MathDisplayLatex,
        Command,
        EnvironmentNonreplaceable,
        EnvironmentReplaceable,
        Brackets,
        CurlyBraces,
    }
}
