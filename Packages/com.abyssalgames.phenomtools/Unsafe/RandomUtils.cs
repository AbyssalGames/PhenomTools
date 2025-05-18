using System;

namespace PhenomTools.Utility
{
  public class RandomUtils
  {
    private static readonly Random Randy = new Random(unchecked((int)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));

    /// <summary>
    /// Generates a random float within the specified range.
    /// </summary>
    /// <param name="min">The minimum random value to return.</param>
    /// <param name="max">The maximum random value to return.</param>
    /// <returns>A random float whose value is within the specified range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    public static float NextFloat(float min, float max)
    {
      if (min > max)
        throw new ArgumentOutOfRangeException("min cannot be greater than max.");
      return min + (float)(Randy.NextDouble() * (max - min));
    }

    /// <summary>
    /// Generates a random float value.
    /// </summary>
    /// <returns>A random float whose value is greater than or equal to 0.0f, and less than 1.0f.</returns>
    public static float NextFloat() => (float)Randy.NextDouble();

    /// <summary>
    /// Generates a random double value.
    /// </summary>
    /// <returns>A random double whose value is greater than or equal to 0.0, and less than 1.0.</returns>
    public static double NextDouble() => Randy.NextDouble();

    /// <summary>
    /// Generates a random double within the specified range.
    /// </summary>
    /// <param name="min">The minimum random value to return.</param>
    /// <param name="max">The maximum random value to return.</param>
    /// <returns>A random float whose value is within the specified range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    public static double NextDouble(double min, double max)
    {
      if (min > max)
        throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
      return min + (NextDouble() * (max - min));
    }

    /// <summary>
    /// Returns a random boolean value.
    /// </summary>
    /// <returns>True or false, generated at random.</returns>
    public static bool NextBool() => Randy.Next() % 2 == 0;

    /// <summary>
    /// Returns a random integer value within the specified bounds.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the number returned.</param>
    /// <param name="max">The exclusive upper bound of the number returned.</param>
    /// <returns>A random integer within the specified bounds.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    public static int NextInt(int min, int max) => Randy.Next(min, max);

    /// <summary>
    /// Returns a random integer value.
    /// </summary>
    /// <returns>A random integer.</returns>
    public static int NextInt() => Randy.Next();

    /// <summary>
    /// Returns a random long integer value within the specified bounds.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the number returned.</param>
    /// <param name="max">The exclusive upper bound of the number returned.</param>
    /// <returns>A random long integer within the specified bounds.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    public static long NextLong(long min, long max)
    {
      if (min > max)
        throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max");
      else if (min == max)
        return min;
      else
        return min + (NextLong() % (max - min));
    }

    /// <summary>
    /// Returns a random long integer value.
    /// </summary>
    /// <returns>A random long integer.</returns>
    unsafe public static long NextLong()
    {
      double d = Randy.NextDouble();
      return *(long*)&d;
    }

    /// <summary>
    /// Returns a random unsigned integer value within the specified bounds.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the number returned.</param>
    /// <param name="max">The exclusive upper bound of the number returned.</param>
    /// <returns>A random unsigned integer within the specified bounds.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    public static uint NextUint(uint min, uint max)
    {
      if (min > max)
        throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
      else if (min == max)
        return min;
      else
        return min + (NextUint() % (max - min));
    }

    /// <summary>
    /// Returns a random unsigned integer value.
    /// </summary>
    /// <returns>A random unsigned integer.</returns>
    unsafe public static uint NextUint()
    {
      // Generate random int and reinterpret-cast it into an unsigned integer
      int r = Randy.Next();
      return *(uint*)&r;
    }

    /// <summary>
    /// Returns a random unsigned long integer value within the specified bounds.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the number returned.</param>
    /// <param name="max">The exclusive upper bound of the number returned.</param>
    /// <returns>A random unsigned long integer within the specified bounds.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    public static ulong NextULong(ulong min, ulong max)
    {
      if (min > max)
        throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
      else if (min == max)
        return min;
      else
        return min + (NextULong() % (max - min));
    }

    /// <summary>
    /// Returns a random unsigned long integer value.
    /// </summary>
    /// <returns>A random unsigned long integer.</returns>
    unsafe public static ulong NextULong()
    {
      double d = Randy.NextDouble();
      return *(ulong*)&d;
    }

    public static Tenum NextEnum<Tenum>() where Tenum : Enum
    {
      Array values = Enum.GetValues(typeof(Tenum));
      return (Tenum)values.GetValue(NextInt(0, values.Length - 1));
    }

    public static Tenum NextEnum<Tenum>(params Tenum[] excludes) where Tenum : Enum
    {
      Tenum[] values = (Tenum[])Enum.GetValues(typeof(Tenum));
      values = Array.FindAll(values, value => Array.IndexOf(excludes, value) == -1);
      if (values.Length == 0)
        throw new ArgumentException("No values can be picked at random due to provided excludes.");
      return (Tenum)values.GetValue(NextInt(0, values.Length - 1));
    }
  }
}