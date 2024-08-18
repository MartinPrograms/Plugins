
using Hexa.NET.ImGui;

namespace Reverb.Reverberator;

public class StereoDelay : StereoAudioUnit
{
    public Delay LeftDelay;
    public Delay RightDelay;
    public bool SyncSettings = true; // Means left and right delay settings are the same
    
    public StereoDelay(DelaySettings settings) : base("Stereo Delay")
    {
        LeftDelay = new Delay(settings.Clone());
        RightDelay = new Delay(LeftDelay.Settings); // Don't clone, because we want to sync settings
    }
    
    public override double[] ProcessStereoSample(double[] samples)
    {
        if (!Enabled)
        {
            return samples;
        }
        
        double leftSample = samples[0];
        double rightSample = samples[1];
        try
        {
            for (int i = 0; i < samples.Length; i += 2)
            {
                leftSample = LeftDelay.ProcessSample(leftSample);
                rightSample = RightDelay.ProcessSample(rightSample);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return new double[] { leftSample, rightSample };
    }
    private DelaySettings _settings;
    public override void DrawUserInterface()
    {
        base.DrawUserInterface();
        
        ImGui.Checkbox("Sync Settings", ref SyncSettings);
        
        if (SyncSettings)
        {
            if (LeftDelay.Settings != RightDelay.Settings)
            {
                RightDelay.SetSettings(LeftDelay.Settings);
            }
            
            LeftDelay.DrawUserInterface();
            if (LeftDelay.OnSettingsChanged == null)
            {
                LeftDelay.OnSettingsChanged += OnSettingsChanged;
            }
        }
        else
        {
            if (LeftDelay.OnSettingsChanged != null)
            {
                LeftDelay.OnSettingsChanged -= OnSettingsChanged;
            }
            
            if (LeftDelay.Settings == RightDelay.Settings)
            {
                RightDelay.SetSettings(LeftDelay.Settings.Clone());
            }
            
            ImGui.Text("Left Delay");
            LeftDelay.DrawUserInterface();
            ImGui.Text("Right Delay");
            RightDelay.DrawUserInterface();
        }
        
        if (ImGui.Button("Reset##" + GetHashCode()))
        {
            _settings = new DelaySettings();
            LeftDelay = new Delay(_settings.Clone());
            RightDelay = new Delay(_settings.Clone());
        }
    }

    private void OnSettingsChanged()
    {
        if (SyncSettings)
        {
            RightDelay.SetSettings(LeftDelay.Settings.Clone());
        }
    }
}