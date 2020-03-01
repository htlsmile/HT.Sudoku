using System;

namespace HT.Sudoku
{
    public static class Sudoku
    {
        private static readonly Random random = new Random();
        private static readonly int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public static bool Solve(int[,] data, int i = 0, int j = 0)
        {
            i += j / 9;
            j %= 9;
            if (i == 9)
            {
                return true;
            }
            else if (data[i, j] == 0)
            {
                var r = random.Next(100);
                for (int v = 0; v < nums.Length; v++)
                {
                    data[i, j] = nums[(r + v) % nums.Length];
                    if (CheckValue(data, i, j) && Solve(data, i, j + 1))
                    {
                        return true;
                    }
                }
                data[i, j] = 0;
                return false;
            }
            else
            {
                return Solve(data, i, j + 1);
            }
        }

        public static bool CheckValue(int[,] data, int i, int j) => CheckRow(data, i, j) && CheckColum(data, i, j) && CheckRound(data, i, j);

        private static bool CheckRow(int[,] data, int i, int j)
        {
            for (int k = 0; k < data.GetLength(0); k++)
            {
                if (k == i)
                {
                    continue;
                }
                if (data[k, j] == data[i, j])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CheckColum(int[,] data, int i, int j)
        {
            for (int k = 0; k < data.GetLength(1); k++)
            {
                if (k == j)
                {
                    continue;
                }
                if (data[i, k] == data[i, j])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CheckRound(int[,] data, int i, int j)
        {
            for (int m = i / 3 * 3; m < (i / 3 + 1) * 3; m++)
            {
                for (int n = j / 3 * 3; n < (j / 3 + 1) * 3; n++)
                {
                    if (m == i && n == j)
                    {
                        continue;
                    }
                    if (data[m, n] == data[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
