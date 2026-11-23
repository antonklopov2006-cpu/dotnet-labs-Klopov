namespace TestStatistics;

internal static class Program
{
    private const int MinScore = 0;
    private const int MaxScore = 100;
    private const int RandomFillCount = 10;

    private static void Main()
    {
        int[] scores = [];
        bool isRunning = true;

        while (isRunning)
        {
            PrintMenu();
            int choice = ReadInt("Your choice: ", 0, 6);
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    AddScore(ref scores);
                    break;
                case 2:
                    ShowAllScores(scores);
                    break;
                case 3:
                    ShowAverage(scores);
                    break;
                case 4:
                    ShowMinMax(scores);
                    break;
                case 5:
                    ShowDistribution(scores);
                    break;
                case 6:
                    FillRandom(ref scores);
                    break;
                case 0:
                    isRunning = false;
                    Console.WriteLine("Goodbye!");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("===== Test results statistics =====");
        Console.WriteLine("1. Add a result");
        Console.WriteLine("2. Show all results");
        Console.WriteLine("3. Show average score");
        Console.WriteLine("4. Show highest and lowest score");
        Console.WriteLine("5. Show distribution by category");
        Console.WriteLine($"6. Fill with {RandomFillCount} random results");
        Console.WriteLine("0. Exit");
    }

    // Reads an integer in [min, max]; repeats the prompt until the input is valid.
    private static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine($"Invalid input. Enter an integer from {min} to {max}.");
        }
    }

    private static bool HasData(int[] scores)
    {
        if (scores.Length == 0)
        {
            Console.WriteLine("No data");
            return false;
        }

        return true;
    }

    private static void AddScore(ref int[] scores)
    {
        int score = ReadInt($"Enter score ({MinScore}-{MaxScore}): ", MinScore, MaxScore);
        Array.Resize(ref scores, scores.Length + 1);
        scores[^1] = score;
        Console.WriteLine($"Score {score} added. Total results: {scores.Length}");
    }

    private static void FillRandom(ref int[] scores)
    {
        int oldLength = scores.Length;
        Array.Resize(ref scores, oldLength + RandomFillCount);

        for (int i = oldLength; i < scores.Length; i++)
        {
            scores[i] = Random.Shared.Next(MinScore, MaxScore + 1);
        }

        Console.WriteLine($"{RandomFillCount} random results added. Total results: {scores.Length}");
    }

    private static void ShowAllScores(int[] scores)
    {
        if (!HasData(scores))
        {
            return;
        }

        Console.WriteLine("--- All results ---");
        for (int i = 0; i < scores.Length; i++)
        {
            Console.WriteLine($"#{i + 1,-3} {scores[i],3}");
        }
    }

    private static void ShowAverage(int[] scores)
    {
        if (!HasData(scores))
        {
            return;
        }

        double average = ScoreStatistics.Average(scores);
        Console.WriteLine($"Average score: {average:F2} (results: {scores.Length})");
    }

    private static void ShowMinMax(int[] scores)
    {
        if (!HasData(scores))
        {
            return;
        }

        int max = scores.Max();
        int min = scores.Min();

        Console.WriteLine($"Highest score: {max}, position(s): {FormatPositions(ScoreStatistics.FindPositions(scores, max))}");
        Console.WriteLine($"Lowest score:  {min}, position(s): {FormatPositions(ScoreStatistics.FindPositions(scores, min))}");
    }

    private static void ShowDistribution(int[] scores)
    {
        if (!HasData(scores))
        {
            return;
        }

        int[] counts = ScoreStatistics.CountByCategory(scores);
        string[] names = ["Excellent (90-100)", "Good (75-89)", "Satisfactory (60-74)", "Unsatisfactory (<60)"];

        Console.WriteLine("--- Distribution by category ---");
        for (int i = 0; i < counts.Length; i++)
        {
            double percent = (double)counts[i] / scores.Length * 100;
            Console.WriteLine($"{names[i],-22} {counts[i],3} ({percent:F1}%)");
        }
    }

    // Positions are shown to the user starting from 1.
    private static string FormatPositions(List<int> indexes) =>
        string.Join(", ", indexes.Select(i => i + 1));
}
