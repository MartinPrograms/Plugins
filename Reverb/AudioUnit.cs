namespace Reverb;

public abstract class AudioUnit : ISampleProcessor
{
    public AudioUnit()
    {
    }

    public abstract double ProcessSample(double sample);
}

public abstract class StereoAudioUnit : IStereoSampleProcessor
{
    public StereoAudioUnit()
    {
    }

    public abstract double[] ProcessStereoSample(double[] samples);
}