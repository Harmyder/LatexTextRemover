Very simple text remover for latex files.

See LatexScheme.cs for some settings.

It is likely to be impossible to create comprehensive text remover. This one, for example, doesn't really read commands.

One shortcoming is the following:

`\vbox to \textheight` will be converted to `\vbox ?? \textheight`.

To build one needs to install Visual Studio. There is a free edition for Windows.
