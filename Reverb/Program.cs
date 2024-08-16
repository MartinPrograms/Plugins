using System.Runtime.InteropServices;
using AudioPluginGL.UI;
using AudioPlugSharp;
using AudioPlugSharpHost;
using ImGuiNET;
using Reverb.Reverberator;
using Logger = StupidSimpleLogger.Logger;
namespace Reverb;

public class Plugin : AudioPluginOpenGL
{
    public int SampleRate;
    public static Plugin Instance;
    
    public Plugin()
    {
        Company = "MARTIN!";
        Website = "martiiin.net";
        Contact = "no@no.com";
        PluginName = "Reverberator";
        PluginCategory = "Fx";
        PluginVersion = "1.0.0";

        // Unique 64bit ID for the plugin
        PluginID = 0xA57745596BFC6EF8;

        HasUserInterface = true;
        EditorWidth = 400;
        EditorHeight = 350;
    }

    private AudioIOPort stereoInput;
    private AudioIOPort stereoOutput;

    private StereoDelay _reverb;

    private DelaySettings _settings = new DelaySettings();
    
    public override void Initialize()
    {
        base.Initialize();
        NativeMethods.AllocConsole();

        Instance = this;

        SampleRate = (int)Host.SampleRate;
        if (SampleRate == 0)
        {
            SampleRate = 44100; // nasty bug workaround :(
        }
        Console.WriteLine("Sample rate: " + SampleRate);
        
        InputPorts = new AudioIOPort[] { stereoInput = new AudioIOPort("Stereo Input", EAudioChannelConfiguration.Stereo) };
        OutputPorts = new AudioIOPort[] { stereoOutput = new AudioIOPort("Stereo Output", EAudioChannelConfiguration.Stereo) };
        _reverb = new StereoDelay(_settings);
    }

    private double[] _debugSamples = new double[64];
    private int _debugIndex;

    public override void Process()
    {
        base.Process();
        
        ReadOnlySpan<double> inSamplesR = stereoInput.GetAudioBuffer(1);
        ReadOnlySpan<double> inSamplesL = stereoInput.GetAudioBuffer(0);

        Span<double> outLeftSamples = stereoOutput.GetAudioBuffer(0);
        Span<double> outRightSamples = stereoOutput.GetAudioBuffer(1);

        int currentSample = 0;
        int nextSample = 0;

        do
        {
            nextSample = Host.ProcessEvents();  // Handle sample-accurate parameters - see the SimpleExample plugin for a simpler, per-buffer parameter approach
            
            for (int i = currentSample; i < nextSample; i++)
            {
                double[] samples = new double[2];
                samples[0] = (inSamplesL[i]);
                samples[1] = (inSamplesR[i]);
                
                samples = _reverb.ProcessStereoSample(samples);
                
                outLeftSamples[i] = samples[0];
                outRightSamples[i] = samples[1];
            }

            currentSample = nextSample;
        }
        while (nextSample < inSamplesR.Length); // Continue looping until we hit the end of the buffer
    }

    public override void RenderUI(float dt)
    {
        ImGui.Text("Martin's exquisite reverberation engine 9000");
        if (ImGui.Button("Dump Logs"))
        {
            Console.WriteLine("DUMPING!!!");
            foreach (var log in StupidSimpleLogger.Logger.Logs)
            {
                Console.WriteLine(log.Format());
            }
            
            // Output the debug samples
            Console.WriteLine("Debug samples:");
            foreach (var sample in _debugSamples)
            {
                Console.Write(sample);
            }
            Console.WriteLine();
        }
        
        ImGui.Separator();
        
        ImGui.Text("Settings");
        ImGui.DragFloat("Delay", ref _settings.Delay, 0.01f, 0.01f, 10f);
        ImGui.DragFloat("Feedback", ref _settings.Feedback, 0.01f, 0.01f, 1.0f);
        
        ImGui.DragFloat("High Pass Frequency", ref _settings.HighPassFrequency, 50f, 20f, 20000f);
        ImGui.DragFloat("Low Pass Frequency", ref _settings.LowPassFrequency, 50f, 20f, 20000f);
        
        ImGui.DragFloat("High Damping", ref _settings.HighDamping, 0.01f, 0.01f, 1.0f);
        ImGui.DragFloat("Low Damping", ref _settings.LowDamping, 0.01f, 0.01f, 1.0f);
        
        ImGui.DragFloat("Wet Level", ref _settings.WetLevel, 0.01f, 0.01f, 1.0f);
        ImGui.DragFloat("Dry Level", ref _settings.DryLevel, 0.01f, 0.01f, 1.0f);

        if (ImGui.Button("Apply Settings Right"))
        {
            _reverb.RightReverb = new Delay(_settings);
        }
        ImGui.SameLine(); 
        if (ImGui.Button("Apply Settings Left"))
        {
            _reverb.LeftReverb = new Delay(_settings);
        }
        ImGui.SameLine();
        if (ImGui.Button("Apply Settings Both"))
        {
            _reverb = new StereoDelay(_settings);
        }
    }

    [STAThread]
    public static void Main(string[] args)
    {
        Console.WriteLine("Why the FUCK are you running this exe?");

        WindowsFormsHost<Plugin> host = new WindowsFormsHost<Plugin>(new Plugin());
        host.Run();
    }
}

internal static class NativeMethods
{
    // http://msdn.microsoft.com/en-us/library/ms681944(VS.85).aspx
    /// <summary>
    /// Allocates a new console for the calling process.
    /// </summary>
    /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
    /// <remarks>
    /// A process can be associated with only one console,
    /// so the function fails if the calling process already has a console.
    /// </remarks>
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern int AllocConsole();

    // http://msdn.microsoft.com/en-us/library/ms683150(VS.85).aspx
    /// <summary>
    /// Detaches the calling process from its console.
    /// </summary>
    /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
    /// <remarks>
    /// If the calling process is not already attached to a console,
    /// the error code returned is ERROR_INVALID_PARAMETER (87).
    /// </remarks>
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern int FreeConsole();
}