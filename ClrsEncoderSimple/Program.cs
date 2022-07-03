using ClrsEncoderSimple;
using System;
using System.IO;

var parts = new[] {
    "B-trees.tex",
    "all-pairs.tex",
    "amortized-analysis.tex",
    "approximation.tex",
    "augment.tex",
    "biblio.tex",
    "binary-search-trees.tex",
    "comp-geometry.tex",
    "count.tex",
    "divide-conquer.tex",
    "dynamic-prog.tex",
    "eds.tex",
    "elementary-graph-algs.tex",
    "fft.tex",
    "fib-heaps.tex",
    "fmatter.tex",
    "greedy.tex",
    "hashing.tex",
    "heapsort.tex",
    "index-dividers.tex",
    "index.tex",
    "introduction.tex",
    "linear-programming.tex",
    "linear-time-sorting.tex",
    "matrices.tex",
    "matrix.tex",
    "max-flow.tex",
    "median.tex",
    "mst.tex",
    "multithreaded.tex",
    "npc.tex",
    "number-theory.tex",
    "orders.tex",
    //"pagei.tex",
    //"pageii.tex",
    //"pageiv.tex",
    "part-ads.tex",
    "part-advanced-design.tex",
    "part-appendix.tex",
    "part-ds.tex",
    "part-foundations.tex",
    "part-graphs.tex",
    "part-selected.tex",
    "part-sorting.tex",
    "preface.tex",
    "probability.tex",
    "quicksort.tex",
    "red-black-trees.tex",
    "role.tex",
    "sets-etc.tex",
    "shortest-paths.tex",
    "string-matching.tex",
    "sums.tex",
    "toc.tex",
    "union-find.tex",
    "veb-trees.tex",
};

var folderIn = "C:\\Yola\\i2a\\sources";
var folderOut = "C:\\Yola\\i2a\\sources.notext";

Directory.CreateDirectory(folderOut);

foreach (var p in parts)
{
    Console.Write($"Start {p}.");

    var path = Path.Combine(folderIn, p);

    var text = File.ReadAllText(path);

    var encoder = new Encoder(LatexScheme.Transitions, LatexScheme.ReplacementLevels);
    var textNoText = encoder.Apply(text);

    File.WriteAllText(Path.Combine(folderOut, p), textNoText);

    Console.WriteLine(" Done.");
}
