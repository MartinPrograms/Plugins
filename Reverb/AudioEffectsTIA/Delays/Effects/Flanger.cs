using AudioPluginGL.MathHelpers;

namespace Reverb.AudioEffectsTIA.Delays.Effects;

public class Flanger : ISampleProcessor
{ 
    // A flanger is very similar to a tremolo, but it modulates the delay time instead of the amplitude.
    // The delay time is modulated by a low-frequency oscillator (LFO).
    // But unfortunately i kinda wrote the fractionaldelayline class purely for vibrato, so i'll have to rewrite it for flanger.
    
    public LFO Lfo;
    private readonly FractionalDelayLine _delayLine;
    public double Mix = 0.5;
    public Flanger(LFO lfo)
    {
        Lfo = lfo;
        _delayLine = new FractionalDelayLine(AudioSample.SecondsToSamples(0.0f, Plugin.Instance.SampleRate), 1, 0, false, false, AudioSample.SecondsToSamples(2.0f, Plugin.Instance.SampleRate));
        _delayLine.Flange = true;
        _delayLine.FlangeDepth = 0.5;
        _delayLine.UseSweepWidthAsBufferLength = true;
    }

    public double ProcessSample(double sample)
    {
        Lfo.Update(Plugin.Instance.BPM, Plugin.Instance.SampleRate);
        _delayLine.CurrentDelay = ((float)Lfo.SweepWidth / Plugin.Instance.SampleRate) * (0.5 + Lfo.Value * 0.5);
        _delayLine.SweepWidth = Lfo.SweepWidth;
        
        var output = _delayLine.ProcessSample(sample);
        
        return AudioSample.Mix(sample, output, Mix);
    }
    
    public double FlangeDepth
    {
        get => _delayLine.FlangeDepth;
        set => _delayLine.FlangeDepth = value;
    }
}