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
        CommandOneArg,
        Environment,
        Brackets,
        CurlyBraces,
    }
}
