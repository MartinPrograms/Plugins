namespace Reverb;

public interface ISampleProcessor
{
    public double ProcessSample(double sample);
}

public interface IStereoSampleProcessor
{
    public double[] ProcessStereoSample(double[] samples);
}

public interface IReadableSample
{
    public double ReadSample();
}

public interface IWritableSample
{
    public void WriteSample(double sample);
}