namespace Reverb.Reverberator;

public class HighPassFilter : ISampleProcessor
{
    private double _alpha;
    private double _y1;
    private double _x1;

    public HighPassFilter(double cutoffFrequency)
    {
        // Calculate alpha based on sample rate and cutoff frequency
        _alpha = cutoffFrequency / (cutoffFrequency + Plugin.Instance.SampleRate);
        _y1 = 0;
        _x1 = 0;
    }

    public double ProcessSample(double sample)
    {
        var y0 = _alpha * (_y1 + sample - _x1);
        _y1 = y0;
        _x1 = sample;
        return sample - y0;
        
    }

    public void SetMix(float settingsHighDamping)
    {
        _alpha = settingsHighDamping;
    }
}