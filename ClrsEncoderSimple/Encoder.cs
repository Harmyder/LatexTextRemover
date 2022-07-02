using ClrsEncoderSimple.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClrsEncoderSimple
{
    internal sealed class Encoder
    {
        private readonly Transition[] _transitions;
        private readonly Level[] _replacementLevels;

        public Encoder(Transition[] transitions, Level[] replacementLevels)
        {
            _transitions = transitions;
            _replacementLevels = replacementLevels;
        }

        public string Apply(string text)
        {
            var stack = new Stack<Level>();
            stack.Push(new Level(State.Plain, null));

            var textNoText = new StringBuilder();
            var toExclude = new Transition[0];

            for (var i = 0; i < text.Length;)
            {
                var next = string.Empty;
                var step = 0;
                var localTransitions = _transitions.Where(t => t.From.Contains(stack.Peek().State)).Concat(new[] { Transition.Empty }).Except(toExclude);
                Transition? fallback = null;
                while (localTransitions.Any())
                {
                    next += text[i + step++];
                    var localApplicable = new List<Transition>();
                    var localInconclusive = new List<Transition>();
                    foreach (var transition in localTransitions)
                    {
                        var transitionApplicability = transition.IsApplicable(next);
                        if (transitionApplicability == Applicability.Yes)
                        {
                            localApplicable.Add(transition);
                        }
                        else if (transitionApplicability == Applicability.Inconclusive)
                        {
                            localInconclusive.Add(transition);
                        }
                    }
                    if (localApplicable.Count() > 1)
                    {
                        throw new InvalidOperationException($"At position {i} more than one transition is applicable.");
                    }
                    if (localInconclusive.Any())
                    {
                        localTransitions = localInconclusive;
                        if (localApplicable.Any())
                        {
                            fallback = localApplicable.Single();
                        }
                    }
                    else
                    {
                        localTransitions = new Transition[0];
                        var shouldRewind = false;
                        var currLevel = stack.Peek();
                        Transition? applied = null;
                        if (localApplicable.Any())
                        {
                            applied = localApplicable.Single();
                            currLevel = stack.Add(applied);
                            shouldRewind = applied.ShouldRewind;
                        }
                        else
                        {
                            if (fallback != null)
                            {
                                applied = fallback;
                                currLevel = stack.Add(applied);
                                shouldRewind = fallback.ShouldRewind;
                            }
                            step = 1;
                        }
                        if (shouldRewind)
                        {
                            toExclude = new Transition[] { applied };
                            step = 0;
                        }
                        else
                        {
                            toExclude = new Transition[0];
                            textNoText.Append(applied == null && ShouldReplace(currLevel) ? Encode(text.Substring(i, step)) : text.Substring(i, step));
                        }
                    }
                }
                //Console.WriteLine();
                //Console.WriteLine(string.Format("{0,3}: ", i) + text.Substring(0, i));
                //Console.WriteLine(string.Join(">", stack.Reverse().Select(x => x.ToString())));
                i += step;
            }

            TryForEofTransitions(stack);

            return textNoText.ToString();
        }

        private void TryForEofTransitions(Stack<Level> stack)
        {
            while (stack.Count > 1)
            {
                var eofTransitions = _transitions.Where(t => t.From.Contains(stack.Peek().State)).Concat(new[] { Transition.Empty });
                var transitions = eofTransitions.Where(t => t.IsApplicable(string.Empty) == Applicability.Yes).ToArray();
                if (transitions.Length == 0)
                {
                    throw new ArgumentException("Invalid text supplied. Stack still contains\n" + string.Join("\n", stack.Reverse().Select(x => x.ToString())));
                }
                if (transitions.Length > 1)
                {
                    throw new ArgumentException("Ambiguous transitions");
                }
                stack.Add(transitions.Single());
            }
        }

        private string Encode(string input)
        {
            var output = string.Empty;
            foreach (var ch in input)
            {
                output += char.IsLetterOrDigit(ch) ? '?' : ch;
            }
            return output;
        }

        private bool ShouldReplace(Level level)
        {
            foreach (var l in _replacementLevels)
            {
                if (l.State == level.State && l.Mark == level.Mark)
                {
                    return true;
                }
            }
            return false;
        }
    }

    internal static class Extensions
    {
        public static Level Add(this Stack<Level> stack, Transition transition)
        {
            if (transition.IsClosing)
            {
                var fromMark = stack.Peek().Mark;
                if (fromMark != transition.Mark)
                {
                    throw new InvalidOperationException($"Marks are not the same: '{fromMark}' -> '{transition.Mark}'");
                }
                return stack.Pop();
            }
            else
            {
                stack.Push(new Level(transition.To, transition.Mark));
                return stack.Peek();
            }
        }
    }
}
