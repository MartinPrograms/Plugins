using AudioPluginGL.MathHelpers;

namespace Reverb.AudioEffectsTIA.Delays.Effects;

public class MultiVoiceChorus : ISampleProcessor
{
    private List<Chorus> _choruses;
    private double _mix;
    public MultiVoiceChorus(LFO lfo, int voices = 3, float msDelay = 30)
    {
        _choruses = new List<Chorus>();
        for (int i = 0; i < voices; i++)
        {
            var thelfo = lfo.Clone() as LFO;
            thelfo.Phase = (float)i / voices; // Otherwise it will have peaks and troughs at the same time, causes real hurty earys

            _choruses.Add(new Chorus(thelfo, msDelay));
        }
    }

    public float Mix = 0.5f;
    public float ChorusDepth = 0.5f;

    public double ProcessSample(double sample)
    {
        double output = 0;
        foreach (var chorus in _choruses)
        {
            chorus.Mix = Mix;
            chorus.ChorusDepth = ChorusDepth;
            output += chorus.ProcessSample(sample);
        }
        return output / _choruses.Count;
    }

    public LFO GetLFO()
    {
        return _choruses[0].Lfo.Clone() as LFO;
    }
    
    public void SetLFO(LFO lfo)
    {
        foreach (var chorus in _choruses)
        {
            chorus.Lfo = lfo;
        }
    }
}