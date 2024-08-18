namespace AudioPluginGL.MathHelpers;
public class TimeSnap
{
    public int Numerator; // 4 + denominator 4 would be a quarter note
    public int Denominator;
    
    public TimeSnap(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }
    
    public static TimeSnap Whole => new TimeSnap(1, 1);
    public static TimeSnap Half => new TimeSnap(1, 2);
    public static TimeSnap Quarter => new TimeSnap(1, 4);
    public static TimeSnap Eighth => new TimeSnap(1, 8);
    public static TimeSnap Sixteenth => new TimeSnap(1, 16);
    public static TimeSnap ThirtySecond => new TimeSnap(1, 32);
    public static TimeSnap SixtyFourth => new TimeSnap(1, 64);
    
    public float ToSeconds(float bpm)
    {
        float beatsPerSecond = bpm / 60;
        float secondsPerBeat = 1 / beatsPerSecond;
        float secondsPerSnap = secondsPerBeat * Numerator / Denominator;
        return secondsPerSnap;
    }
    
    public float ToMS(float bpm)
    {
        return ToSeconds(bpm) * 1000;
    }
    
    public float ToSamples(float bpm, int sampleRate)
    {
        return AudioSample.SecondsToSamples(ToSeconds(bpm), sampleRate);
    }
    
    public float ToHz(float bpm)
    {
        return 1 / ToSeconds(bpm);
    }
    
    public static TimeSnap FromSeconds(float seconds, float bpm)
    {
        float beatsPerSecond = bpm / 60;
        float secondsPerBeat = 1 / beatsPerSecond;
        float snapsPerBeat = secondsPerBeat / seconds;
        return new TimeSnap(1, (int) snapsPerBeat);
    }
    
    public static TimeSnap FromMS(float ms, float bpm)
    {
        return FromSeconds(ms / 1000, bpm);
    }
    
    public static TimeSnap FromSamples(int samples, int sampleRate, float bpm)
    {
        return FromSeconds(AudioSample.SamplesToSeconds(samples, sampleRate), bpm);
    }
    
    public override string ToString()
    {
        return Numerator + "/" + Denominator;
    }

    public TimeSnap Clone()
    {
        return new TimeSnap(Numerator, Denominator);
    }
}

public enum SnapTo : int
{
    None = 0, 
    Whole = 1,
    Half = 2,
    Quarter = 4,
    Eighth = 8,
    Sixteenth = 16,
    ThirtySecond = 32,
    SixtyFourth = 64,
}