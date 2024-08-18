using System.Numerics;
using AudioPluginGL.MathHelpers;
using Hexa.NET.ImGui;
using Hexa.NET.ImPlot;

namespace AudioPluginGL.UI;

/// <summary>
/// Extra ImGui functions, related to more musical UI elements.
/// For example, a slider that snaps to 1/4 notes, 1/8 notes, etc, they take a BPM and a sample rate.
/// </summary>
public static class ExtImGui
{
   
    
    public static bool SliderFloatSTR(string label, ref float value, float min, float max, SnapTo snap, float bpm, int sampleRate)
    {
        float snappedValue = AudioSample.SnapToRythm(value, snap, bpm, sampleRate);
        ImGui.Text(snap.ToString() + " : ");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(200); // To keep it from being too wide/ugly/small
        bool changed = ImGui.SliderFloat(label, ref snappedValue, min, max, "%.3f");
        value = AudioSample.SnapToRythm(snappedValue, snap, bpm, sampleRate);
        return changed;
    }

    private static Dictionary<string, SnapTo> _snapTos = new Dictionary<string, SnapTo>();
    private static Dictionary<string, TimeSnap> _timeSnaps = new Dictionary<string, TimeSnap>();

    public static bool SliderFloatUserSnap(string label, ref float value, float min, float max, float bpm,
        int sampleRate)
    {
        // We need a combo box to select the snap value
        if (!_snapTos.ContainsKey(label))
        {
            _snapTos[label] = SnapTo.None;
        }
        
        SnapTo snap = _snapTos[label];
        bool changed = SliderFloatSTR(label, ref value, min, max, snap, bpm, sampleRate);
        ImGui.SameLine();
        if (ImGui.BeginCombo("##snap", snap.ToString()))
        {
            foreach (SnapTo snapTo in Enum.GetValues(typeof(SnapTo)))
            {
                if (ImGui.Selectable(snapTo.ToString()))
                {
                    _snapTos[label] = snapTo;
                }
            }
            ImGui.EndCombo();
        }
        
        return changed;
    }
    
    public static bool DragIntTimeSnapToSeconds(string label, ref float value, float bpm, int min = 1, int max = 64)
    {
        if (!_timeSnaps.ContainsKey(label))
        {
            _timeSnaps[label] = TimeSnap.Half;
        }
        
        TimeSnap timeSnap = _timeSnaps[label];
        ImGui.Text($"{label.Split("##")[0]} : "); // remove the identifier
        ImGui.SameLine();
        ImGui.SetNextItemWidth(80); // To keep it from being too wide/ugly/small
        bool changed = ImGui.DragInt($"##{label}", ref timeSnap.Numerator, 0.1f, min, max);
        ImGui.SameLine();
        ImGui.Text("/");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(80); // To keep it from being too wide/ugly/small
        changed |= ImGui.DragInt($"##{label}denominator", ref timeSnap.Denominator, 0.1f, min, max);
        value = timeSnap.ToSeconds(bpm);
        return changed;
    }
    public static bool DragIntTimeSnap(string label, ref TimeSnap snap, float bpm, int min = 1, int max = 64)
    {
        if (!_timeSnaps.ContainsKey(label))
        {
            _timeSnaps[label] = TimeSnap.Half;
        }
        
        TimeSnap timeSnap = _timeSnaps[label];
        ImGui.Text($"{label.Split("##")[0]} : "); // remove the identifier
        ImGui.SameLine();
        ImGui.SetNextItemWidth(80); // To keep it from being too wide/ugly/small
        bool changed = ImGui.DragInt($"##{label}", ref timeSnap.Numerator, 0.1f, min, max);
        ImGui.SameLine();
        ImGui.Text("/");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(80); // To keep it from being too wide/ugly/small
        changed |= ImGui.DragInt($"##{label}denominator", ref timeSnap.Denominator, 0.1f, min, max);
        snap = timeSnap;
        return changed;
    }

    public static void LfoVisualizer(string label, LFO leftVibratoLfo)
    {
        float[] samples = leftVibratoLfo.GetSamples();
        
        if (ImPlot.BeginSubplots(label, 1,1, new Vector2(200, 100), ImPlotSubplotFlags.NoResize | ImPlotSubplotFlags.NoTitle))
        {
            ImPlot.SetNextAxesToFit();
            if (ImPlot.BeginPlot("", ImPlotFlags.NoLegend)){
                ImPlot.SetupAxis(ImAxis.X1, ImPlotAxisFlags.NoDecorations); // get rid of the x axis (too many numbers, looks really bad)
                ImPlot.SetupAxis(ImAxis.Y1, "Intensity", ImPlotAxisFlags.None);
                ImPlot.PlotLine("LFO", ref samples[0], samples.Length-1);
                ImPlot.EndPlot();
            }
        }
    }
    
    public static bool SliderHz(string label, ref float value, float min, float max)
    {
        float hz = AudioSample.SecondsToHz(value);
        bool changed = ImGui.SliderFloat(label, ref hz, min, max);
        value = AudioSample.HzToSeconds(hz);
        return changed;
    }
    
    public static void LfoEditor(string label, ref LFO lfo, float bpm, int sampleRate)
    {
        if (ImGui.CollapsingHeader(label))
        {
            ImGui.BeginColumns("##" + label, 2, ImGuiOldColumnFlags.NoResize);
            bool useSnap = lfo.Snap != null;
            ImGui.Checkbox("Use Snap", ref useSnap);
            if (useSnap)
            {
                if (lfo.Snap == null)
                {
                    lfo.Snap = new TimeSnap(1, 4);
                    lfo.Update(bpm, sampleRate);
                }
            }
            else
            {
                lfo.Snap = null;
            }
            if (lfo.Snap != null)
            {
                if (DragIntTimeSnap("Frequency##"+label, ref lfo.Snap, bpm))
                {
                    lfo.Update(bpm, sampleRate);
                }
            }
            else
            {
                ImGui.Text("Frequency (Hz)");
                if (SliderHz("##frequency"+label, ref lfo.Frequency, 0.001f, 1000.0f)) // above 1khz is basically a generator
                {
                    lfo.Update(bpm, sampleRate);
                }
            }
            ImGui.Text("Frequency (Hz): " + AudioSample.SecondsToHz(lfo.Frequency) + "\nSweepWidth (samples): " + lfo.SweepWidth);
            ImGui.Text("Amplitude");
            ImGui.SliderFloat("##amplitude"+label, ref lfo.Amplitude, 0.0f, 1.0f);
            ImGui.Text("Offset");
            ImGui.SliderFloat("##offset"+label, ref lfo.Offset, -1.0f, 1.0f);
            ImGui.Text("Phase");
            ImGui.SliderFloat("##phase"+label, ref lfo.Phase, 0.0f, 1.0f);
            ImGui.Text("Shape");
            ImGui.SameLine();
            if (ImGui.BeginCombo("##shape"+label, lfo.Shape.ToString()))
            {
                foreach (LFOShape shape in Enum.GetValues(typeof(LFOShape)))
                {
                    if (ImGui.Selectable(shape.ToString()))
                    {
                        lfo.Shape = shape;
                    }
                }
                ImGui.EndCombo();
            }
           
            if (ImGui.Button("Randomize##" + label))
            {
                if (lfo.Snap != null)
                {
                    lfo = new LFO(lfo.Snap, (float) MathF.Sin((float) DateTime.Now.Ticks), (float) MathF.Cos((float) DateTime.Now.Ticks), (LFOShape) (DateTime.Now.Ticks % 5), (float) MathF.Tan((float) DateTime.Now.Ticks), bpm, sampleRate);
                }
                else
                {
                    var random = new Random();
                    lfo = new LFO(AudioSample.HzToSeconds(random.NextSingle() * 40), random.NextSingle(), random.NextSingle(), (LFOShape) random.Next(5), random.NextSingle(), bpm, sampleRate);
                }
            }
            
            ImGui.SameLine();
            if (ImGui.Button("Invert##" + label))
            {
                lfo.Amplitude = -lfo.Amplitude;
            }
            
            ImGui.SameLine();

            if (ImGui.Button("Reset##" + label))
            {
                lfo = new LFO(lfo.Snap, 1.0f, 0.0f, LFOShape.Sine, 0.0f, bpm, sampleRate);
            }
            
            ImGui.NextColumn();
            LfoVisualizer(label, lfo);
            ImGui.EndColumns();
            // ezpz
            
            // That is a lot of buttons, but it's a lot of fun to play with :) (especially the randomize button :D)
        }
    }
}
