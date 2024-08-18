using AudioPluginGL.MathHelpers;
using AudioPluginGL.UI;
using Hexa.NET.ImGui;
using Reverb.AudioEffectsTIA.Delays.Effects;

namespace Reverb.AudioEffectsTIA.AudioUnits;

public class StereoVibrato : StereoAudioUnit
{
    public Vibrato LeftVibrato;
    public Vibrato RightVibrato;
    
    public StereoVibrato() : base("Stereo Vibrato")
    {
        LeftVibrato = new Vibrato(_lfo);
        RightVibrato = new Vibrato(_lfo);
    }

    public override double[] ProcessStereoSample(double[] samples)
    {
        if (!Enabled)
        {
            return samples;
        }
        try
        {
            LeftVibrato.Lfo.SetBpm(Plugin.Instance.BPM);
            RightVibrato.Lfo.SetBpm(Plugin.Instance.BPM);
            var output = new double[] { LeftVibrato.ProcessSample(samples[0]), RightVibrato.ProcessSample(samples[1]) };
            return output;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return samples;
        }
    }

    private LFO _lfo = new LFO(8.5f, 0.012f, 0, LFOShape.Sine, 0, 130, 44100);
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
                LeftVibrato.Lfo = _lfo.Clone() as LFO;
                RightVibrato.Lfo = _lfo.Clone() as LFO;
            }
        }
        else
        {
            bool changed = ImGui.SliderFloat("Depth##" + GetHashCode(), ref _depth, 0.0f, 1.0f); // amplitude: from 0 to 1, later divided by 100 so it does not become too aggressive
            changed |= ImGui.SliderFloat("Rate##" + GetHashCode(), ref _rate, 0.2f, 20.0f);
            if (changed)
            {
                _lfo = new LFO(AudioSample.HzToSeconds(_rate), _depth / 100, 0, LFOShape.Sine, 0, Plugin.Instance.BPM, Plugin.Instance.SampleRate);
                
                LeftVibrato.Lfo = _lfo.Clone() as LFO;
                RightVibrato.Lfo = _lfo.Clone() as LFO;
            }
            if (ImGui.BeginPopup("Help##"+GetHashCode()))
            {
                ImGui.Text("Depth: The amplitude of the LFO, from 0 to 1");
                ImGui.Text("Rate: The frequency of the LFO, from 0.2 to 20 Hz");
                ImGui.Text("Delay: The delay before input gets to the output.\nTo use delay correction, go to the DAW settings and set the delay compensation to the same value as the delay here.\nSorry, but this is the only way to get rid of latency, as the library used for this VST does not support PDC.");
                ImGui.EndPopup();
            }
            ImGui.Text("Delay: " + _lfo.SweepWidth); // The delay before input gets to the output
            ImGui.SameLine();
            
            if (ImGui.Button("?##"+GetHashCode()))
            {
                ImGui.OpenPopup("Help##"+GetHashCode());
            }
            
            
        }
    }
    
    private void UpdateLfo()
    {
        
    }
}