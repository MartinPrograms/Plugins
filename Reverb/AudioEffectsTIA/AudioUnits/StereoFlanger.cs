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
    public float Phase = 0.0f; // the difference in phase between the left and right flanger, higher values introduce more stereo width

    public StereoFlanger() : base("Stereo Flanger",
        "A flanger! A flanger modulates the delay time by a low-frequency oscillator (LFO). This LFO then creates a sweeping effect.",
        "Mix is the percentage of the wet signal.\n" +
        "Depth is the intensity of the flanger effect.\n" +
        "Rate is the speed of the LFO.\n" +
        "Amplitude is the intensity of the oscillator, higher values introduce more pitch variations.\n" + 
        "Phase is the difference in phase between the left and right flanger, higher values introduce more stereo width.\n" +
        "Delay is the delay time in milliseconds. (0.1 to 15ms)")
    {
        LeftFlanger = new Flanger(_lfo.Clone() as LFO, 15);
        RightFlanger = new Flanger(_lfo.Clone() as LFO, 15);
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

    private bool _advancedMode = false;
    public const float MaxOscAmplitude = 0.005f;
    private float _amp = 0.5f; // half of the above value
    
    
    public override void DrawUserInterface()
    {
        base.DrawUserInterface();
        ImGui.Checkbox("Advanced Mode##" + GetHashCode(), ref _advancedMode);
        
        float mix = (float)LeftFlanger.Mix;
        bool changedMix = ImGui.SliderFloat("Mix##" + GetHashCode(), ref mix, 0.0f, 1.0f);
        if (changedMix)
        {
            LeftFlanger.Mix = mix;
            RightFlanger.Mix = mix;
        }

        if (_advancedMode)
        {
            ExtImGui.LfoEditor("LFO##" + GetHashCode(), ref _lfo, Plugin.Instance.BPM, Plugin.Instance.SampleRate);
            if (ImGui.Button("Apply##" + GetHashCode()))
            {
                LeftFlanger.Lfo = _lfo.Clone() as LFO;
                RightFlanger.Lfo = _lfo.Clone() as LFO;
                RightFlanger.Lfo.Phase = Phase;
            }
        }
        else
        {
            float flangerRate = (float)AudioSample.SecondsToHz(_lfo.Frequency);
            bool changedRate = ExtImGui.SliderHz("Rate##" + GetHashCode(), ref flangerRate, 0.05f, 10.0f);
            
            bool changedLFODepth = ImGui.SliderFloat("Amplitude##" + GetHashCode(), ref _amp, 0.0f, 1.0f); // this is a percentage of the max amplitude
            
            if (changedRate || changedLFODepth)
            {
                _lfo = new LFO(AudioSample.HzToSeconds(flangerRate), (AudioSample.ValueFromPercentage(MaxOscAmplitude, _amp)), _lfo.Offset, _lfo.Shape, _lfo.Phase, Plugin.Instance.BPM, Plugin.Instance.SampleRate);
                LeftFlanger.Lfo = _lfo.Clone() as LFO;
                RightFlanger.Lfo = _lfo.Clone() as LFO;
                RightFlanger.Lfo.Phase = Phase;
            }
        }
        
        // Phase is between 0 and 1, where 1 is a full cycle
        bool changedPhase = ImGui.SliderFloat("Phase##" + GetHashCode(), ref Phase, 0.0f, 1.0f);
        if (changedPhase)
        {
            RightFlanger.Lfo.Phase = Phase; // So at 1 it'd be the same as the left flanger
        }
        
        float flangeDepth = (float)LeftFlanger.FlangeDepth;
        bool changedDepth = ImGui.SliderFloat("Depth##" + GetHashCode(), ref flangeDepth, 0.0f, 1.0f);
        if (changedDepth)
        {
            LeftFlanger.FlangeDepth = flangeDepth;
            RightFlanger.FlangeDepth = flangeDepth;
        }
        
    }
}