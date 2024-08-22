using AudioPluginGL.UI;

namespace AudioPluginGL.MathHelpers;

public static class AudioSample
{
    public static double Mix(double a, double b, float t)
    {
        return Mix(a, b, (double) t);
    }
    
    public static double Mix(double a, double b, double t)
    {
        return a * (1 - t) + b * t;
    }
    
    public static double Mix(double a, double b, float dryLevel, float wetLevel)
    {
        return Mix(a, b, (double) dryLevel, (double) wetLevel);
    }
    
    public static double Mix(double a, double b, double dryLevel, double wetLevel)
    {
        return a * dryLevel + b * wetLevel;
    }
    
    public static int SecondsToSamples(float seconds, int sampleRate)
    {
        return (int) (seconds * sampleRate);
    }
    
    public static float SamplesToSeconds(int samples, int sampleRate)
    {
        return (float) samples / sampleRate;
    }

    public static float SnapToRythm(float seconds, SnapTo snap, float bpm, int sampleRate)
    {
        float beatsPerSecond = bpm / 60;
        float secondsPerBeat = 1 / beatsPerSecond;
        float samplesPerBeat = secondsPerBeat * sampleRate;
        float samplesPerSnap = samplesPerBeat / (int) snap;
        float samples = seconds * sampleRate;
        float snappedSamples = (int) (samples / samplesPerSnap) * samplesPerSnap;
        return snappedSamples / sampleRate;
    }
    
    public static float SnapToMS(SnapTo snap, float bpm)
    {
        // Converts a snap value to milliseconds, for example 1/4 note at 120 BPM 
        float beatsPerSecond = bpm / 60;
        float secondsPerBeat = 1 / beatsPerSecond;
        float msPerBeat = secondsPerBeat * 1000;
        return msPerBeat / (int) snap;
    }
    
    public static float SnapToSeconds(SnapTo snap, float bpm)
    {
        // Converts a snap value to seconds, for example 1/4 note at 120 BPM 
        float beatsPerSecond = bpm / 60;
        float secondsPerBeat = 1 / beatsPerSecond;
        return secondsPerBeat / (int) snap;
    }
    
    public static float SnapToSamples(SnapTo snap, float bpm, int sampleRate)
    {
        // Converts a snap value to samples, for example 1/4 note at 120 BPM 
        float beatsPerSecond = bpm / 60;
        float secondsPerBeat = 1 / beatsPerSecond;
        float samplesPerBeat = secondsPerBeat * sampleRate;
        return samplesPerBeat / (int) snap;
    }

    public static float SecondsToHz(float seconds)
    {
        return 1.0f / seconds;
    }

    public static float HzToSeconds(float hz)
    {
        return 1.0f / hz;
    }

    public static float ValueFromPercentage(float maxOscAmplitude, float amp)
    {
        return maxOscAmplitude * amp;
    }
    
    public static double ValueFromPercentage(double maxOscAmplitude, double amp)
    {
        return maxOscAmplitude * amp;
    }

    public static float FullRotationToDegrees(float phase)
    {
        return (float) (phase * 360);
    }

    public static float DegreesToFullRotation(float degPhase)
    {
        return degPhase / 360;
    }

    // A is the number to round up, B is the number to round up to
    public static int RoundUp(int a, int b)
    {
        return (int) Math.Ceiling((double) a / b) * b;
    }
}