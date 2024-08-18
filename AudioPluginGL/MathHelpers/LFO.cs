namespace AudioPluginGL.MathHelpers;

public class LFO : ICloneable
{
    public TimeSnap? Snap;
    public float Frequency; // Disregarded if Snap is set, is in seconds.
    public float Amplitude;
    public float Offset;
    public LFOShape Shape;
    public float Phase;
    /// <summary>
    /// The current value of the LFO. Always between -1 and 1.
    /// </summary>
    public double Value { get; private set; } // range -1 to 1
    public int SweepWidth; // Width of the sweep in samples.
    
    public LFO(TimeSnap snap, float amplitude, float offset, LFOShape shape, float phase, float bpm, int sampleRate)
    {
        Snap = snap;
        Amplitude = amplitude;
        Offset = offset;
        Shape = shape;
        Phase = phase;

        Frequency = snap.ToSeconds(bpm);
        SweepWidth = (int) (Frequency * sampleRate);
    }
    
    public LFO(float frequency, float amplitude, float offset, LFOShape shape, float phase, float bpm, int sampleRate)
    {
        Frequency = frequency;
        Amplitude = amplitude;
        Offset = offset;
        Shape = shape;
        Phase = phase;

        SweepWidth = (int) (Frequency * sampleRate);
        if (SweepWidth < 1)
        {
            SweepWidth = 1;
        }
    }
    
    private int _phaseIndex = 0;
    public void Update(float bpm, int sampleRate)
    {
        if (Snap != null)
        {
            Frequency = Snap.ToSeconds(bpm);
        }
        
        SweepWidth = (int) (Frequency * sampleRate);
        Value = GetValue(((double) _phaseIndex / SweepWidth) + Phase);
        
        _phaseIndex++;
        if (_phaseIndex >= SweepWidth)
        {
            _phaseIndex = 0;
        }
    }
    
    private double GetValue(double phase)
    {
        switch (Shape)
        {
            case LFOShape.Sine:
                return Sine(phase) * Amplitude + Offset;
            case LFOShape.Triangle:
                return Triangle(phase) * Amplitude + Offset;
            case LFOShape.Sawtooth:
                return Sawtooth(phase) * Amplitude + Offset;
            case LFOShape.Square:
                return Square(phase) * Amplitude + Offset;
            case LFOShape.Noise:
                return Noise(phase) * Amplitude + Offset;
            default:
                return 0;
        }
    }
    
    public float[] GetSamples()
    {
        int maxSamples = 1000; // Limit to 1000 samples.
        
        int sampleCount = SweepWidth > maxSamples ? maxSamples : SweepWidth;
        float[] samples = new float[sampleCount];
        float howManyToIncrament = (float) SweepWidth / (float) sampleCount; // So if our sample count is 1000 and our sweep width is 10000, we want to increment by 10.
        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = (float) GetValue(((double) i * howManyToIncrament / SweepWidth) + Phase);
        }
        
        return samples;
    }

    public void SetBpm(float instanceBpm)
    {
        Frequency = Snap == null ? Frequency : Snap.ToSeconds(instanceBpm);
    }
    
    public static double Sine(double phase)
    {
        return Math.Sin(2 * Math.PI * phase);
    }
    
    public static double Triangle(double phase)
    {
        // We need a triangle that repeats, and does not infinitely increase. 
        double x = phase * 4;
        double y = 0;
        
        x = x % 4;
        if (x < 1)
        {
            y = x;
        }
        else if (x < 3)
        {
            y = 2 - x;
        }
        else
        {
            y = x - 4;
        }
        
        return y;
    }
    
    public static double Sawtooth(double phase)
    {
        // Same here, we need a sawtooth that repeats.
        double x = phase * 2;
        
        x = x % 2;
        double y = 0;
        if (x < 1)
        {
            y = x;
        }
        else
        {
            y = x - 2;
        }
        
        return y;
    }
    
    public static double Square(double phase)
    {
        // Same here, we need a square that repeats, so if phase is 1.5, we want 1, if phase is 2.5, we want 0.
        double x = phase * 2;
        double y = 0;
        if (x % 2 < 1)
        {
            y = 1;
        }
        
        return y;
    }
    
    private static List<double> _noiseValues = new List<double>();
    public static double Noise(double phase)
    {
        if (_noiseValues.Count == 0)
        {
            for (int i = 0; i < 1000; i++)
            {
                _noiseValues.Add(GenerateNoiseValue());
            }
        }
        
        int index = (int) (phase * 1000) % 1000; // modulo so i t never goes out of bounds.
        
        return _noiseValues[index];
        
    }
    
    private static double GenerateNoiseValue()
    {
        return 2 * (new Random().NextDouble() - 0.5);
    }
    


    public object Clone()
    {
        if (Snap != null)
        {
            return new LFO(Snap.Clone(), Amplitude, Offset, Shape, Phase, 130, 44100);
        }
        else
        {
            return new LFO(Frequency, Amplitude, Offset, Shape, Phase, 130, 44100);
        }
    }
}

public enum LFOShape
{
    Sine,
    Triangle,
    Sawtooth,
    Square,
    Noise
}