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
        Environment,
        Brackets,
        CurlyBraces,
    }
}
