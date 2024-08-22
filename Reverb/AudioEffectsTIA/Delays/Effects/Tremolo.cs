using AudioPluginGL.MathHelpers;

namespace Reverb.AudioEffectsTIA.Delays.Effects;

public class Tremolo : ISampleProcessor
{
    public LFO Lfo;
    
    public Tremolo(LFO lfo)
    {
        Lfo = lfo;
    }

    public double ProcessSample(double sample)
    {
        Lfo.Update(Plugin.Instance.BPM, Plugin.Instance.SampleRate);
        return sample * (0.5 + Lfo.Value * 0.5);
    }
}