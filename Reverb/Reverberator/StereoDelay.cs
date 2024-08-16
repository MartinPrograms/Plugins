namespace Reverb.Reverberator;

public class StereoDelay : StereoAudioUnit
{
    public Delay LeftReverb;
    public Delay RightReverb;
    
    public StereoDelay(DelaySettings settings)
    {
        LeftReverb = new Delay(settings);
        RightReverb = new Delay(settings);
    }
    
    public override double[] ProcessStereoSample(double[] samples)
    {
        double leftSample = samples[0];
        double rightSample = samples[1];
        try
        {
            for (int i = 0; i < samples.Length; i += 2)
            {
                leftSample = LeftReverb.ProcessSample(leftSample);
                rightSample = RightReverb.ProcessSample(rightSample);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return new double[] { leftSample, rightSample };
    }
}