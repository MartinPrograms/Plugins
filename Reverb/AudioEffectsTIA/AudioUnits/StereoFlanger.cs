using AudioPluginGL.MathHelpers;
using AudioPluginGL.UI;
using Hexa.NET.ImGui;
using Reverb.AudioEffectsTIA.Delays.Effects;

namespace Reverb.AudioEffectsTIA.AudioUnits;

public class StereoFlanger : StereoAudioUnit
{
    public Flanger LeftFlanger;
    public Flanger RightFlanger;
    private LFO _lfo = new LFO(AudioSample.HzToSeconds(1f), 0.004f, 0, LFOShape.Sine, 0, 130, 44100);

    public StereoFlanger() : base("Stereo Flanger")
    {
        LeftFlanger = new Flanger(_lfo);
        RightFlanger = new Flanger(_lfo);
    }

    public override double[] ProcessStereoSample(double[] samples)
    {
        if (!Enabled)
        {
            return samples;
        }

        try
        {
            LeftFlanger.Lfo.SetBpm(Plugin.Instance.BPM);
            RightFlanger.Lfo.SetBpm(Plugin.Instance.BPM);
            var output = new double[]
                { LeftFlanger.ProcessSample(samples[0]), RightFlanger.ProcessSample(samples[1]) };
            return output;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return samples;
        }
    }

    public override void DrawUserInterface()
    {
        base.DrawUserInterface();

        float mix = (float)LeftFlanger.Mix;
        bool changedMix = ImGui.SliderFloat("Mix##" + GetHashCode(), ref mix, 0.0f, 1.0f);
        if (changedMix)
        {
            LeftFlanger.Mix = mix;
            RightFlanger.Mix = mix;
        }
        
        ExtImGui.LfoEditor("LFO##" + GetHashCode(), ref _lfo, Plugin.Instance.BPM, Plugin.Instance.SampleRate);
        if (ImGui.Button("Apply##" + GetHashCode()))
        {
            LeftFlanger.Lfo = _lfo.Clone() as LFO;
            RightFlanger.Lfo = _lfo.Clone() as LFO;
        }
        
        float flangeDepth = (float)LeftFlanger.FlangeDepth;
        bool changedDepth = ImGui.SliderFloat("Flange Depth##" + GetHashCode(), ref flangeDepth, 0.0f, 1.0f);
        if (changedDepth)
        {
            LeftFlanger.FlangeDepth = flangeDepth;
            RightFlanger.FlangeDepth = flangeDepth;
        }
    }
}