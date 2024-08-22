using AudioPluginGL.MathHelpers;
using AudioPluginGL.UI;
using Hexa.NET.ImGui;
using Reverb.AudioEffectsTIA.Delays.Effects;

namespace Reverb.AudioEffectsTIA.AudioUnits;

public class StereoChorus : StereoAudioUnit
{
    private MultiVoiceChorus _multiVoiceChorus;
    private LFO _lfo = new LFO(0.8f, 0.012f / 100f, 0, LFOShape.Sine, 0, 130, 44100);
    private bool _advancedMode;
    private float _depth = 0.012f / 100; // This controls amplitude of the LFO
    private float _rate = AudioSample.SecondsToHz(0.8f);
    
    public StereoChorus() : base("Stereo Chorus", "")
    {
        _multiVoiceChorus = new MultiVoiceChorus(_lfo);
    }
    
    public override double[] ProcessStereoSample(double[] samples)
    {
        if (!Enabled)
        {
            return samples;
        }

        try
        {
            var output = new double[]
                { _multiVoiceChorus.ProcessSample(samples[0]), _multiVoiceChorus.ProcessSample(samples[1]) };
            return output;
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return samples;
        }
    }

    public override void DrawUserInterface()
    {
        base.DrawUserInterface();
        
        ExtImGui.LfoEditor("LFO##" + GetHashCode(), ref _lfo, Plugin.Instance.BPM, Plugin.Instance.SampleRate);
        if (ImGui.Button("Apply##" + GetHashCode()))
        {
            _multiVoiceChorus.SetLFO(_lfo.Clone() as LFO);
        }
        
        ImGui.SliderFloat("Mix##" + GetHashCode(), ref _multiVoiceChorus.Mix, 0.0f, 1.0f);
        ImGui.SliderFloat("Depth##" + GetHashCode(), ref _multiVoiceChorus.ChorusDepth, 0.0f, 1.0f);
    }
}