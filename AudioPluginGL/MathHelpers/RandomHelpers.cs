namespace AudioPluginGL.MathHelpers;

public class RandomHelpers
{
    private static Random random = new Random(365); // 365 is a cool number B)
    
    public static double NextDouble()
    {
        return random.NextDouble();
    }
    
    public static double NextDouble(double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }
    
    public static int NextInt(int min, int max)
    {
        return random.Next(min, max);
    }
    
    public static float NextFloat()
    {
        return (float) random.NextDouble();
    }
    
    public static float NextFloat(float min, float max)
    {
        return (float) random.NextDouble() * (max - min) + min;
    }
    
    public static float NextFloat(float min, float max, int precision)
    {
        float value = NextFloat(min, max);
        return (float) Math.Round(value, precision);
    }

    private static Dictionary<int, List<double>> _randoms = new Dictionary<int, List<double>>();
    public static int Double(int i, int index)
    {
        if (!_randoms.ContainsKey(i))
        {
            _randoms[i] = new List<double>(){-1.0};
        }

        if (_randoms[i][(int)index] == -1.0)
        {
            // means we haven't generated this random number yet
            _randoms[i][(int) index] = NextDouble();
        }
        return (int) (_randoms[i][(int) index]);
    }
}