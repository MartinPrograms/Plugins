using AudioPluginGL.MathHelpers;
using AudioPluginGL.UI;
using Hexa.NET.ImGui;

namespace Reverb.Reverberator;
public class Delay : AudioUnit
{
    // Circular buffer
    private DelayLine _delayLine;
    private AllPassFilter _allPassFilter;
    
    public DelaySettings Settings;
    public Delay(DelaySettings settings) : base("Delay")
    {
        SetSettings(settings);
    }
    
    public override double ProcessSample(double sample)
    {
        double delayedSample = _delayLine.ProcessSample(sample);
        
        _allPassFilter.SetFeedback(Settings.Feedback); // Update feedback
        double earlyOutput = _allPassFilter.ProcessSample(delayedSample);
        return AudioSample.Mix(sample, earlyOutput, Settings.DryLevel, Settings.WetLevel);
    }

    public override void DrawUserInterface()
    {
        base.DrawUserInterface();
        if (ExtImGui.DragIntTimeSnapToSeconds($"Delay##{GetHashCode()}", ref Settings.Delay, Plugin.Instance.BPM))
        {
            SetSettings(Settings);
        }
        ImGui.SliderFloat($"Feedback##{GetHashCode()}", ref Settings.Feedback, 0.0f, 1.0f);
        ImGui.SliderFloat($"Wet Level##{GetHashCode()}", ref Settings.WetLevel, 0.0f, 1.0f);
        ImGui.SliderFloat($"Dry Level##{GetHashCode()}", ref Settings.DryLevel, 0.0f, 1.0f);
        if (ImGui.Button("Update Delay##" + GetHashCode()))
        {
            SetSettings(Settings);
        }

        ImGui.SameLine();
        if (ImGui.Button("Reset##" + GetHashCode()))
        {
            Settings = new DelaySettings();
            SetSettings(Settings);
        }
    }
    
    public void SetSettings(DelaySettings settings)
    {
        Settings = settings;
        _delayLine = new DelayLine(AudioSample.SecondsToSamples(settings.Delay, Plugin.Instance.SampleRate));
        _allPassFilter = new AllPassFilter(AudioSample.SecondsToSamples(settings.Delay, Plugin.Instance.SampleRate), settings.Feedback);
        
        OnSettingsChanged?.Invoke();
    }
}

public class DelaySettings
{
    // Delay (seconds)
    public float Delay;
    public float Feedback;
    public float WetLevel;
    public float DryLevel;
    
    public DelaySettings(float delay, float feedback, float wetLevel, float dryLevel)
    {
        Delay = delay;
        Feedback = feedback;
        WetLevel = wetLevel;
        DryLevel = dryLevel;
    }
    
    public DelaySettings()
    {
        Delay = 0.5f;
        Feedback = 0.5f;
        WetLevel = 0.5f;
        DryLevel = 0.5f;
    }

    public DelaySettings Clone()
    {
        return new DelaySettings(Delay, Feedback, WetLevel, DryLevel);
    }
}
