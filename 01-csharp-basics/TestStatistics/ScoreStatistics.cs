namespace TestStatistics;

public static class ScoreStatistics
{
    public static double Average(int[] scores)
    {
        if (scores.Length == 0)
        {
            throw new ArgumentException("Array is empty.", nameof(scores));
        }

        int sum = 0;
        foreach (int score in scores)
        {
            sum += score;
        }

        return (double)sum / scores.Length;
    }

    // Returns zero-based indexes of all elements equal to value.
    public static List<int> FindPositions(int[] scores, int value)
    {
        var positions = new List<int>();
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == value)
            {
                positions.Add(i);
            }
        }

        return positions;
    }

    // Index 0 - excellent, 1 - good, 2 - satisfactory, 3 - unsatisfactory.
    public static int[] CountByCategory(int[] scores)
    {
        int[] counts = new int[4];
        foreach (int score in scores)
        {
            int category = score switch
            {
                >= 90 => 0,
                >= 75 => 1,
                >= 60 => 2,
                _ => 3
            };
            counts[category]++;
        }

        return counts;
    }
}
