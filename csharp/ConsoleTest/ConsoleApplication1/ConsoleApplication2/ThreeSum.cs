namespace ConsoleApplication2
{
    using System;
    using System.Collections;

    public class ThreeSum
    {
        public void Find3Sum(int[] array)
        {
            // sort the array
            Array.Sort(array);
            foreach (var e in array)
            {
                Console.Write(string.Empty + e + " ");
            }
            
            Console.WriteLine();

            for (int i = 0; i < array.Length; i++)
            {
                this.Find2Sum(array, -array[i], i);
            } 
        }

        public void Find2Sum(int[] array, int sum, int ignoreIndex)
        {
            int h = ignoreIndex + 1;
            int t = array.Length - 1;
            int x = array[ignoreIndex];

            while (h < t)
            {
                int u = array[h] + array[t];
                if (u < sum || h == ignoreIndex)
                {
                    h++;
                    continue;
                }

                if (u > sum || t == ignoreIndex)
                {
                    t--;
                    continue;
                }

                Console.WriteLine("(" + array[h] + "," + array[t] + "," + x + ") "+ ignoreIndex);

                h++;
            }
        }
    


}
}