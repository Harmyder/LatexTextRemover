using ClrsEncoderSimple;
using ClrsEncoderSimple.Transitions;
using System.IO;

var path = "C:\\Yola\\i2a\\sources\\approximation.tex";

var text = File.ReadAllText(path);
// !"#$%&'()*+,-./0123456789:;<=>?@[\]^_`{|}~



var encoder = new Encoder(LatexScheme.Transitions);
var textNoText = encoder.Apply(text);

var pathNoText = Path.ChangeExtension(path, "notext.tex");
File.WriteAllText(pathNoText, textNoText);
