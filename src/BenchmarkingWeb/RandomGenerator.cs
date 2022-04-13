using System.Text;

namespace BenchmarkingWeb
{
    internal class RandomGenerator{
        private static Random ran = new Random();

        private const string b = "abcdefghijklmnopqrstuvwxyz0123456789          !@#$%^&*~";

        private static int len = b.Length;

        public string GetRandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(b[ran.Next(len)]);
            }
            return sb.ToString();
        }

        public int GetRandomInt(int min, int max) => ran.Next(min, max);
    }
}
