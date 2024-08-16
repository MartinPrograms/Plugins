namespace Reverb.Reverberator;

public class DelayLine : ISampleProcessor, IReadableSample, IWritableSample
{ 
    private readonly double[] _buffer;
    private int _writeIndex;
    private int _readIndex;
    private int _delay;

    public DelayLine(int delay)
    {
        _buffer = new double[delay];
        _delay = delay;
    }

    public double ProcessSample(double sample)
    {
        _buffer[_writeIndex] = sample;
        _writeIndex = (_writeIndex + 1) % _delay;
        _readIndex = (_writeIndex + _delay - 1) % _delay;
        return _buffer[_readIndex];
    }
    
    public double ReadSample()
    {
        return _buffer[_readIndex];
    }
    
    public void WriteSample(double sample)
    {
        _buffer[_writeIndex] = sample;
        _writeIndex = (_writeIndex + 1) % _delay;
        _readIndex = (_writeIndex + _delay - 1) % _delay;
    }

    public void SetDelay(int settingsSampleRate)
    {
        _delay = settingsSampleRate;
    }
}