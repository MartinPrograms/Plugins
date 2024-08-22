using AudioPluginGL.MathHelpers;

namespace Reverb.AudioEffectsTIA.Delays.Effects;

public class Vibrato : ISampleProcessor
{ 
    public LFO Lfo;
    private readonly FractionalDelayLine _delayLine;

    public Vibrato(LFO lfo)
    {
        Lfo = lfo;
        _delayLine = new FractionalDelayLine(AudioSample.SecondsToSamples(0.00f, Plugin.Instance.SampleRate), 1, 0, false, false, AudioSample.SecondsToSamples(2.0f, Plugin.Instance.SampleRate));
        _delayLine.UseSweepWidthAsBufferLength = true;
    }

    public double ProcessSample(double sample)
    {
        Lfo.Update(Plugin.Instance.BPM, Plugin.Instance.SampleRate);
        _delayLine.CurrentDelay = ((float)Lfo.SweepWidth / Plugin.Instance.SampleRate) * (0.5 + Lfo.Value * 0.5);
        _delayLine.SweepWidth = Lfo.SweepWidth;
        return _delayLine.ProcessSample(sample);
    }
}