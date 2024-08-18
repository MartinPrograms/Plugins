using System.Numerics;

namespace AudioPluginGL.MathHelpers;

public static class FFT
{
    // Short Time Fourier Transform
    // STFT{x[n]} = X[m, k] = sum(n=0 to N-1) x[n] * w[n] * exp(-j * 2 * pi * m * n / N)
    public static double STFT(double[] x, int m, int k)
    {
        double sum = 0;
        for (int n = 0; n < x.Length; n++)
        {
            sum += x[n] * MathWindow.Hamming(n, x.Length) * Complex.Exp(-Complex.ImaginaryOne * 2 * Math.PI * m * n / x.Length).Real;
        }
        return sum;
    }
}