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

        public Encoder(Transition[] transitions)
        {
            _transitions = transitions;
        }

        public string Apply(string text)
        {
            var stack = new Stack<State>();
            stack.Push(State.Plain);

            var textNoText = new StringBuilder();

            for (var i = 0; i < text.Length;)
            {
                var next = string.Empty;
                var step = 0;
                var localTransitions = _transitions.Where(t => t.From == stack.Peek()).Concat(new[] { Transition.Empty });
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
                        var currState = stack.Peek();
                        if (localApplicable.Any())
                        {
                            var applicableTransition = localApplicable.Single();
                            currState = stack.Add(applicableTransition);
                            shouldRewind = applicableTransition.ShouldRewind;
                        }
                        else
                        {
                            if (fallback != null)
                            {
                                currState = stack.Add(fallback);
                                shouldRewind = fallback.ShouldRewind;
                            }
                            step = 1;
                        }
                        if (shouldRewind)
                        {
                            step = 0;
                        }
                        else
                        {
                            textNoText.Append(currState == State.Plain ? Encode(text.Substring(i, step)) : text.Substring(i, step));
                        }
                    }
                }
                //Console.WriteLine();
                //Console.WriteLine(text.Substring(0, i));
                //Console.WriteLine(string.Join(">", stack.Reverse().Select(x => x.ToString())));
                i += step;
            }
            return textNoText.ToString();
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
    }

    internal static class Extensions
    {
        public static State Add(this Stack<State> stack, Transition transition)
        {
            if (transition.IsClosing)
            {
                return stack.Pop();
            }
            else
            {
                stack.Push(transition.To);
                return transition.To;
            }
        }
    }
}
