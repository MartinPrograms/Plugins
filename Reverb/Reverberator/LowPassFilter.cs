namespace Reverb.Reverberator;

public class LowPassFilter : ISampleProcessor
{
    private double _alpha;
    private double _y1;

    public LowPassFilter(double cutoffFrequency)
    {
        // Calculate alpha based on sample rate and cutoff frequency
        // Frequency is in Hz, sample rate is in samples per second
        _alpha = 1 - Math.Exp(-2 * Math.PI * cutoffFrequency / Plugin.Instance.SampleRate);
        _y1 = 0;
    }

    public double ProcessSample(double sample)
    {
        var y0 = _alpha * sample + (1 - _alpha) * _y1;
        _y1 = y0;
        return y0;
    }
}