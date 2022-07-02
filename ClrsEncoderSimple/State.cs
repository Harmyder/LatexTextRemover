namespace ClrsEncoderSimple
{
    internal enum State
    {
        None,
        Plain,
        MathInline,
        MathDisplayDollars,
        MathDisplayLatex,
        CommandFirstSymbol,
        Command,
        CommandOneArg,
        Environment,
        Brackets,
        CurlyBraces,
    }
}
