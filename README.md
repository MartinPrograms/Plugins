# martins exquisite plugin collection 9000  

An expansion of the awesome [AudioPlugSharp](https://github.com/mikeoliphant/AudioPlugSharp/) library to support OpenGL!

This repo contains the following items  
- AudioPluginGL, a rendering engine for vst instances.
- A modified copy of the Hexa.Net [ImGui](https://github.com/HexaEngine/Hexa.NET.ImGui) and [ImPlot](https://github.com/HexaEngine/Hexa.NET.ImGui/blob/master/Hexa.NET.ImPlot/ImPlot.cs) projects, to support loading files in the runtimes directory when the host is not in that same directory.
- And a project called Reverb which is a collection of implementations from the Audio Effects: Theory, Implementation, and Application book, using the AudioPluginGL library.

How do you use AudioPluginGL?  
Create a new console application (or class library, but this does not copy necessary dlls to the output folder)  
Import AudioPluginGL as a reference  
Create a new class and inherit from AudioPluginOpenGL  
You're done, you now have a UI using ImGui and you can use the Reverb project to learn how to process audio!  

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
