using AudioPluginGL.MathHelpers;
using AudioPluginGL.UI;
using Hexa.NET.ImGui;
using Reverb.AudioEffectsTIA.Delays.Effects;

namespace Reverb.AudioEffectsTIA.AudioUnits;

public class StereoTremolo : StereoAudioUnit
{
    public Tremolo LeftTremolo;
    public Tremolo RightTremolo;
    private LFO _lfo = new LFO(8.5f, 0.012f, 0, LFOShape.Sine, 0, 130, 44100);

    public StereoTremolo() : base("Stereo Tremolo")
    {
        LeftTremolo = new Tremolo(_lfo);
        RightTremolo = new Tremolo(_lfo);
    }

    public override double[] ProcessStereoSample(double[] samples)
    {
        if (!Enabled)
        {
            return samples;
        }

        try
        {
            LeftTremolo.Lfo.SetBpm(Plugin.Instance.BPM);
            RightTremolo.Lfo.SetBpm(Plugin.Instance.BPM);
            var output = new double[]
                { LeftTremolo.ProcessSample(samples[0]), RightTremolo.ProcessSample(samples[1]) };
            return output;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return samples;
        }
    }
    private bool _advancedMode;
    private float _depth; // This controls amplitude of the LFO
    private float _rate; // This controls frequency of the LFO (sine)
    public override void DrawUserInterface()
    {
        base.DrawUserInterface();
        ImGui.Checkbox("Advanced Mode##" + GetHashCode(), ref _advancedMode);
        if (_advancedMode) // Allows full control over the LFO:
        {
            ExtImGui.LfoEditor("LFO##" + GetHashCode(), ref _lfo, Plugin.Instance.BPM, Plugin.Instance.SampleRate);
            if (ImGui.Button("Apply##" + GetHashCode()))
            {
                LeftTremolo.Lfo = _lfo.Clone() as LFO;
                RightTremolo.Lfo = _lfo.Clone() as LFO;
            }
        }
        else
        {
            bool changedD = ImGui.SliderFloat("Depth##" + GetHashCode(), ref _depth, 0.0f, 1.0f); // amplitude: from 0 to 1
            bool changedR = ImGui.SliderFloat("Rate##" + GetHashCode(), ref _rate, 0.2f, 20.0f);
            bool changed = changedD || changedR;
            if (changed)
            {
                _lfo = new LFO(AudioSample.HzToSeconds(_rate), _depth, 1.0f - _depth, LFOShape.Sine, 0, Plugin.Instance.BPM, Plugin.Instance.SampleRate);
                
                LeftTremolo.Lfo = _lfo.Clone() as LFO;
                RightTremolo.Lfo = _lfo.Clone() as LFO;
            }
        }
    }
}