using AudioPluginGL.MathHelpers;

namespace Reverb.AudioEffectsTIA.Delays;

public class FractionalDelayLine
{
    // This is based on the Audio effects Theory and Implementation book by Joshua D. Reiss and Andrew P. McPherson
    private readonly double[] _buffer;
    private int _writeIndex;
    private int _readIndex;
    private int _delay;
    private double _gain;
    private double _fractionalDelay;

    // Possible feedback extraction and injection point
    public double FeedbackValue { get; set; } // Could be used to extract the feedback buffer value

    public bool WriteFeedback { get; set; } =
        true; // if false, does not write feedback to the buffer, however does write to Feedback property

    public bool ReadFeedback { get; set; } =
        true; // if false, does not read feedback from the buffer, however does read from Feedback property

    public double FeedbackGain { get; set; } =
        0.7; // This is the gain of the feedback signal, 1 is full feedback, 0 is no feedback

    public FractionalDelayLine(int delay, double gain, double fractionalDelay, bool writeFeedback = true,
        bool readFeedback = true, int buffSize = 1)
    {
        _buffer = new double[buffSize];
        if (delay < 1)
        {
            delay = 1;
        }

        if (delay > buffSize)
        {
            delay = buffSize;
        }

        _delay = delay;
        _gain = gain;
        _fractionalDelay = fractionalDelay;
        WriteFeedback = writeFeedback;
        ReadFeedback = readFeedback;
    }

    public double CurrentDelay
    {
        get => _fractionalDelay;
        set { _fractionalDelay = value; }
    }

    private double Mod(double a, double b)
    {
        return a - b * Math.Floor(a / b);
    }

    public bool Flange = false;
    public double FlangeDepth = 0.0;
    public int SweepWidth = 0; // in samples
    public bool UseSweepWidthAsBufferLength = false;
    public double ProcessSample(double sample)
    {
        double dpw = _writeIndex;
        int bufferLength = _buffer.Length;
        if (UseSweepWidthAsBufferLength)
        {
            bufferLength = SweepWidth;
        }
        // Here, we assume _fractionalDelay is a small value that scales to a shorter delay time
        double dpr = Mod(dpw - (_fractionalDelay * Plugin.Instance.SampleRate) + bufferLength - 3, bufferLength);

        // Interpolation
        double readIndex = Math.Floor(dpr);
        double frac = dpr - readIndex;

        double output = 0;
   
        if (!Flange)
            output = AudioSample.Mix(_buffer[(int)readIndex], _buffer[(int)Mod(readIndex + 1, bufferLength)], frac);
        else
            output = sample + FlangeDepth * AudioSample.Mix(_buffer[(int)readIndex], _buffer[(int)Mod(readIndex + 1, bufferLength)], frac);
        
        if (Flange)
            FeedbackValue = output; // do the thing
        
        if (WriteFeedback)
        {
            _buffer[_writeIndex] = sample + (FeedbackValue * FeedbackGain);
        }
        else
        {
            _buffer[_writeIndex] = sample;
        }

        if (ReadFeedback)
        {
            FeedbackValue = output;
        }

        _writeIndex++;
        _readIndex++;

        if (_writeIndex >= bufferLength)
        {
            _writeIndex = 0;
        }

        if (_readIndex >= bufferLength)
        {
            _readIndex = 0;
        }

        // The output is the delayed sample, modulated by gain
        return output * _gain;
    }


    public int GetBufferLength()
    {
        return _buffer.Length;
    }
}