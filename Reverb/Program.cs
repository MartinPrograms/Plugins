using System.Reflection;
using System.Runtime.InteropServices;
using AudioPluginGL.MathHelpers;
using AudioPluginGL.UI;
using AudioPlugSharp;
using AudioPlugSharpHost;
using Hexa.NET.ImGui;
using Reverb.AudioEffectsTIA.AudioUnits;
using Reverb.Reverberator;
using Logger = StupidSimpleLogger.Logger;
namespace Reverb;

public class Plugin : AudioPluginOpenGL
{
    public int SampleRate;
    public static Plugin Instance;
    public float BPM;

    public int CurrentDelay { get; private set; } // This will be returned to the DAW, so it can adjust the delay time. Otherwise, it introduces latency
    
    public Plugin()
    {
        Company = "Martin";
        Website = "https://martiiin.net";
        Contact = "martin@martiiin.net";
        PluginName = "Reverberator";
        PluginCategory = "Fx";
        PluginVersion = "1.0.0";

        // Unique 64bit ID for the plugin
        PluginID = 0xA57745596BFC6EF8;

        HasUserInterface = true;
        EditorWidth = 800;
        EditorHeight = 450;
    }

    private AudioIOPort stereoInput;
    private AudioIOPort stereoOutput;

    private List<StereoAudioUnit> _audioUnits = new List<StereoAudioUnit>();

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

        BPM = (float)Host.BPM;
        Console.WriteLine("BPM: " + BPM);
        if (BPM == 0)
        {
            BPM = 120; // nasty bug workaround :(
        }

        InputPorts = new AudioIOPort[]
            { stereoInput = new AudioIOPort("Stereo Input", EAudioChannelConfiguration.Stereo) };
        OutputPorts = new AudioIOPort[]
            { stereoOutput = new AudioIOPort("Stereo Output", EAudioChannelConfiguration.Stereo) };

        // get all the types in the assembly
        var types = Assembly.GetExecutingAssembly().GetTypes();
        
        // find all the types that inherit from StereoAudioUnit
        foreach (var type in types)
        {
            if (type.IsSubclassOf(typeof(StereoAudioUnit)))
            {
                var audioUnit = (StereoAudioUnit)Activator.CreateInstance(type);
                audioUnit.Enabled = false;
                _audioUnits.Add(audioUnit);
            }
        }
    }

    private double[] _debugSamples = new double[64];
    private int _debugIndex;

    public int CurrentOffset { get; private set; }

    public override void Process()
    {
        base.Process();
        BPM = (float)Host.BPM;
        SampleRate = (int)Host.SampleRate;
        
        if (SampleRate == 0)
        {
            SampleRate = 44100; // nasty bug workaround :(
        }
        
        if (BPM == 0)
        {
            BPM = 120; // nasty bug workaround :(
        }
        
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
                
                CurrentOffset = i;
                
                // Process the samples
                foreach (var audioUnit in _audioUnits)
                {
                    samples = audioUnit.ProcessStereoSample(samples);

                }

                outLeftSamples[i] = samples[0];
                outRightSamples[i] = samples[1];
            }

            currentSample = nextSample;
        }
        while (nextSample < inSamplesR.Length); // Continue looping until we hit the end of the buffer
    }

    public override void RenderUI(float dt)
    {
        ImGui.Text("Martin's exquisite effects extravaganza 9000!");
        ImGui.SameLine();
        if (ImGui.Button("Website"))
        {
            System.Diagnostics.Process.Start("https://martiiin.net");
        }
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
            
            Console.WriteLine("BPM: " + BPM);
            Console.WriteLine("Sample rate: " + SampleRate);
            
            Console.WriteLine("Audio units:");
            foreach (var audioUnit in _audioUnits)
            {
                Console.WriteLine(audioUnit.Name);
            }
        }
        
        ImGui.Separator();
        
        foreach (var audioUnit in _audioUnits)
        {
            if (ImGui.CollapsingHeader(audioUnit.Name+"##"+audioUnit.GetHashCode()))
                audioUnit.DrawUserInterface();
            ImGui.Separator();
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