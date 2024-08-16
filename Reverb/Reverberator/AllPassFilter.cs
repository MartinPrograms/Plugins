using AudioPluginGL.MathHelpers;

namespace Reverb.Reverberator;

public class AllPassFilter : ISampleProcessor
{
    private readonly double[] _buffer;
    private int _writeIndex;
    private int _readIndex;
    private int _delay;
    private double _gain;

    public AllPassFilter(int delay, double gain)
    {
        _buffer = new double[delay];
        _delay = delay;
        _gain = gain;
    }

    public double ProcessSample(double sample)
    {
        double delayedSample = _buffer[_writeIndex];
        _buffer[_writeIndex] = sample + delayedSample * _gain;
        _writeIndex = (_writeIndex + 1) % _delay;
        _readIndex = (_writeIndex + _delay - 1) % _delay;
        return delayedSample - sample * _gain;
    }

    // This method does not further the _writeIndex or _readIndex, it just processes the sample without changing the state of the filter
    public double Process(double earlyOutput)
    {
        return earlyOutput + _buffer[_writeIndex] * _gain;
    }

    public void SetFeedback(float damping)
    {
        _gain = damping;
    }
}