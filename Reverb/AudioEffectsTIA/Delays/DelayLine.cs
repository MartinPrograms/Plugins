namespace Reverb.AudioEffectsTIA.Delays;

public class DelayLine : ISampleProcessor
{
    // This is based on the Audio effects Theory and Implementation book by Joshua D. Reiss and Andrew P. McPherson
    private readonly double[] _buffer;
    private int _writeIndex;
    private int _readIndex;
    private int _delay;
    private double _gain;
    
    // Possible feedback extraction and injection point
    public double FeedbackValue { get; set; } // Could be used to extract the feedback buffer value
    public bool WriteFeedback { get; set; } = true; // if false, does not write feedback to the buffer, however does write to Feedback property
    public bool ReadFeedback { get; set; } = true; // if false, does not read feedback from the buffer, however does read from Feedback property
    public double FeedbackGain { get; set; } = 0.7; // This is the gain of the feedback signal, 1 is full feedback, 0 is no feedback
    
    public DelayLine(int delay, double gain)
    {
        _buffer = new double[delay];
        _delay = delay;
        _gain = gain;
    }
    
    public double ProcessSample(double sample)
    {
        double output = _buffer[_readIndex];
        if (WriteFeedback)
        {
            _buffer[_writeIndex] = sample + FeedbackValue * FeedbackGain;
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
        if (_writeIndex >= _delay)
        {
            _writeIndex = 0;
        }
        if (_readIndex >= _delay)
        {
            _readIndex = 0;
        }
        return output * _gain;
    }
    
    // This class is now done.
    // This class can be used to make the following effects: ping-pong delay, flanger, chorus, reverb, echo, slapback delay, and more.
}