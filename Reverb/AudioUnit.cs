
using Hexa.NET.ImGui;

namespace Reverb;

public abstract class AudioUnit : ISampleProcessor
{
    public readonly string Name = "Audio Unit";
    public Action? OnSettingsChanged;
    public AudioUnit(string name)
    {
        Name = name;
    }

    public abstract double ProcessSample(double sample);
    public virtual void DrawUserInterface()
    {
        ImGui.Text(Name);
    }
}

public abstract class StereoAudioUnit : IStereoSampleProcessor
{
    public bool Enabled = true;
    public readonly string Name = "Stereo Audio Unit";
    public readonly string Description = "Base Class Stereo Audio Unit";

    public StereoAudioUnit(string name)
    {
        Name = name;
    }

    public abstract double[] ProcessStereoSample(double[] samples);

    public virtual void DrawUserInterface()
    {
        ImGui.Text(Name);
        ImGui.Text(Description);
        ImGui.Checkbox("Enabled (!bypass)", ref Enabled);
    }
}