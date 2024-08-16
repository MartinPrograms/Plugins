using AudioPluginGL.MathHelpers;

namespace Reverb.Reverberator;
public class Delay : AudioUnit
{
    // Circular buffer
    private DelayLine _delayLine;
    private AllPassFilter _allPassFilter;
    
    private DelaySettings _settings;
    public Delay(DelaySettings settings) : base()
    {
        _settings = settings;
        _delayLine = new DelayLine(AudioSample.SecondsToSamples(settings.Delay, Plugin.Instance.SampleRate));
        _allPassFilter = new AllPassFilter(AudioSample.SecondsToSamples(settings.Delay, Plugin.Instance.SampleRate), settings.Feedback);
    }
    
    public override double ProcessSample(double sample)
    {
        double delayedSample = _delayLine.ProcessSample(sample);
        double earlyOutput = _allPassFilter.ProcessSample(delayedSample);
        return AudioSample.Mix(sample, earlyOutput, _settings.DryLevel, _settings.WetLevel);
    }
}

public class DelaySettings
{
    // Delay (seconds)
    public float Delay;
    public float Feedback;
    public float WetLevel;
    public float DryLevel;
    
    public float HighPassFrequency; // in hz
    public float LowPassFrequency; // in hz
    public float HighDamping;
    public float LowDamping;
    
    public DelaySettings(float delay, float feedback, float wetLevel, float dryLevel, float highPassFrequency, float lowPassFrequency, float highDamping, float lowDamping)
    {
        Delay = delay;
        Feedback = feedback;
        WetLevel = wetLevel;
        DryLevel = dryLevel;
        HighPassFrequency = highPassFrequency;
        LowPassFrequency = lowPassFrequency;
        HighDamping = highDamping;
        LowDamping = lowDamping;
    }
    
    public DelaySettings()
    {
        Delay = 0.5f;
        Feedback = 0.5f;
        WetLevel = 0.5f;
        DryLevel = 0.5f;
        HighPassFrequency = 650;
        LowPassFrequency = 8000;
        HighDamping = 0.25f; 
        LowDamping = 0.25f;
    }

    public DelaySettings Clone()
    {
        return new DelaySettings(Delay, Feedback, WetLevel, DryLevel, HighPassFrequency, LowPassFrequency, HighDamping, LowDamping);
    }
}
