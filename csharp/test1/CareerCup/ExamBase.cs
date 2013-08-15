using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
    using System.Collections;

    class ExamBase
    {
        #region Assert Methods
        public static void AssertEqual(string s1, string s2)
        {
            if(s1.Equals(s2)) Console.Write("Pass\t"); else Console.Write("Fail\t");
            Console.WriteLine("s1={0}\ts2={1}",s1,s2);
        }

        public static void AssertEqual(bool s1, bool s2)
        {
            if (s1.Equals(s2)) Console.Write("Pass\t"); else Console.Write("Fail\t");
            Console.WriteLine("s1={0}\ts2={1}", s1, s2);
        }

        public static void AssertEqual(int s1, int s2)
        {
            if( s1 == s2 ) Console.Write("Pass\t"); else Console.Write("Fail\t");
            Console.WriteLine("s1={0}\ts2={1}", s1, s2);
        }
        #endregion

        #region Tool
        public static void PrintMatrix(int[,] matrix, int M, int N)
        {
            for (int i1 = 0; i1 < M; i1++)
            {
                for (int j1 = 0; j1 < N; j1++)
                    Console.Write(matrix[i1, j1] + "\t");
                Console.WriteLine();
            }
        }
        public static void PrintMatrix(int[][] matrix)
        {
            if (matrix == null) return;
            int M = matrix.Length;
            if (M < 1) return;
            int N = matrix[0].Length;
            if (N < 1) return;

            for (int i1 = 0; i1 < M; i1++)
            {
                for (int j1 = 0; j1 < N; j1++)
                    Console.Write(matrix[i1][j1] + "\t");
                Console.WriteLine();
            }
        }

        public static void PrintLinkedList(LinkedListNode header)
        {
            if (header == null) return;
            while (header != null)
            {
                Console.Write("{0}->", header.value);
                header = header.next;
            }
            Console.WriteLine("NULL");
        }        

        public static void PrintStack(Stack s)
        {
            int count = s.Count;
            if (s == null || s.Count < 1) return;
            Stack tmp = new Stack();
            while(s.Count>0)
            {
                Console.Write("{0}\t", (int)s.Peek());
                tmp.Push(s.Pop());
            }

            Console.WriteLine();

            while (tmp.Count > 0)
                s.Push(tmp.Pop());
        }

        public static void PrintTree(TreeNode root)
        {
            if (root == null) return;
            List<TreeNode> list = new List<TreeNode>();
            list.Add(root);
            int start = 0;

            bool hasmore = true;
            while (true)
            {
                int end = list.Count - 1;
                for (int i = start; i <= end; i++)
                {
                    TreeNode node = list[i];
                    Console.Write(node.value+"\t");
                    if(node.left != null) list.Add(node.left);
                    if(node.right!=null) list.Add(node.right);
                }
                Console.WriteLine();
                if (end == list.Count - 1) break;
                start = end + 1;
            }
        }
        #endregion
    }
}
