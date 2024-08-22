using AudioPluginGL.MathHelpers;

namespace Reverb.AudioEffectsTIA.Delays.Effects;

public class Chorus : ISampleProcessor
{
    // Practically the same as a flanger, but with a higher delay time.
    public LFO Lfo;
    private readonly FractionalDelayLine _delayLine;
    public double Mix = 0.5;
    public Chorus(LFO lfo, float msDelay = 30)
    {
        Lfo = lfo;
        _delayLine = new FractionalDelayLine(AudioSample.SecondsToSamples(0.0f, Plugin.Instance.SampleRate), 1, 0, false, false, AudioSample.RoundUp(AudioSample.SecondsToSamples(msDelay / 1000.0f, Plugin.Instance.SampleRate), 100)); // Round it up to the nearest 100 samples
        _delayLine.Flange = true;
        _delayLine.FlangeDepth = 0.5;
    }

    public double ProcessSample(double sample)
    {
        Lfo.Update(Plugin.Instance.BPM, Plugin.Instance.SampleRate);
        _delayLine.CurrentDelay = ((float)Lfo.SweepWidth / Plugin.Instance.SampleRate) * (0.5 + Lfo.Value * 0.5);
        _delayLine.SweepWidth = Lfo.SweepWidth;
        
        var output = _delayLine.ProcessSample(sample);
        
        return AudioSample.Mix(sample, output, Mix);
    }
    
    public double ChorusDepth
    {
        get => _delayLine.FlangeDepth;
        set => _delayLine.FlangeDepth = value;
    }

    public int GetDelay()
    {
        return _delayLine.GetBufferLength();
    }

}