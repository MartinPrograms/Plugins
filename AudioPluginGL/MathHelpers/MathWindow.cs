namespace AudioPluginGL.MathHelpers;

public static class MathWindow
{
    public static double Hamming(int n, int N)
    {
        return 0.54 - 0.46 * Math.Cos(2 * Math.PI * n / N);
    }
}