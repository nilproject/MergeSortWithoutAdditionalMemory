using System;
using System.Linq;

namespace sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random(777);
            var bytes = new byte[20];
            random.NextBytes(bytes);
            var i = 0;

            var a = bytes.GroupBy(x => i++ / 1).Select(x => x.Aggregate(0, (res, b) => (res << 8) + b)).ToArray();
            Array.Sort(a);

            a = bytes.GroupBy(x => i++ / 1).Select(x => x.Aggregate(0, (res, b) => (res << 8) + b)).ToArray();
            sort(a, 0, a.Length - 1);
        }

        private static void sort(int[] array, int start, int end)
        {
            for (var i = start + 1; i < end; i += 2)
            {
                if (array[i - 1] > array[i])
                {
                    var t = array[i];
                    array[i] = array[i - 1];
                    array[i - 1] = t;
                }
            }

            var d = 2;
            var len = end - start;
            while (d < len)
            {
                for (var i = start; i < end; i += d * 2)
                {
                    merge(array, i, i + d, Math.Min(end, i + d * 2 - 1));

                    validate(array, i, Math.Min(end, i + d * 2 - 1));
                }

                d *= 2;
            }
        }

        private static void validate(int[] array, int start, int end)
        {
            for (var i = start; i < end; i++)
            {
                if (array[i] > array[i + 1])
                    System.Diagnostics.Debugger.Break();
            }
        }

        private static void merge(int[] array, int start, int middle, int end)
        {
            var s0 = start;
            var s1 = middle;
            while (s1 <= end)
            {
                if (s0 >= middle)
                    break;

                if (s1 <= end && array[s0] > array[s1])
                {
                    var startValue = array[s0];
                    var t = array[s1];
                    array[s1] = array[s0];
                    array[s0] = t;

                    s0++;
                    s1++;

                    var moved = false;
                    while (s0 < middle && s1 <= end && array[s1 - 1] > array[s1])
                    {
                        if (startValue < array[s1])
                        {
                            merge(array, middle, s1, end);
                            break;
                        }

                        t = array[s1];
                        array[s1] = array[s0];
                        array[s0] = t;
                        s1++;
                        s0++;
                        moved = true;
                    }

                    if (s0 >= middle && moved)
                    {
                        if (s1 <= end)
                        {
                            s0 = middle;
                            middle = s1;
                            continue;
                        }
                    }

                    if (middle > s0)
                    {
                        s1 = middle;
                    }
                    else
                    {
                        middle++;
                    }
                }
                else
                    s0++;
            }
        }
    }
}
