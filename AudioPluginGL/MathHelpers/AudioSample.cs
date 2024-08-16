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
}