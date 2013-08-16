using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
    using System.Collections;

    class Chapt4 : ExamBase
    {
#region 4.1
        private bool CheckTree(TreeNode node, int depth, ref int min, ref int max)
        {
            if (node == null) throw new Exception("Incorrect input, node cannnot be null");
            if (IsLeaf(node))
                return CheckDepth(depth, ref min, ref max);
            else
            {
                bool leftCheck = false;
                bool rightCheck = false;
                if (node.left != null) leftCheck = CheckTree(node.left, depth + 1, ref min, ref max);
                if (node.right != null) rightCheck = CheckTree(node.right, depth + 1, ref min, ref max);
                return (leftCheck && rightCheck);
            }
        }

        public bool CheckBalanced(TreeNode tr)
        {
	        if(tr==null) return false;

	        int minDepth = -1;
	        int maxDepth = -1;
	        return CheckTree(tr, 0, ref minDepth, ref maxDepth);
        }



        private bool CheckDepth(int depth, ref int min, ref int max)
        {
            if (min == -1 || max == -1)
            {
                min = depth;
                max = depth;
                return true;
            }

            if (depth > max) max = depth;
            if (depth < min) min = depth;

            if (max - min > 1) return false;
            else return true;
        }

        private bool IsLeaf(TreeNode node)
        {
            if (node == null) throw new Exception("Incorrect input, the node cannot be null");

            return (node.left == null && node.right == null);
        }

        public int MaxDepth(TreeNode root)
        {
            if (root == null) return 0;
            return 1 + Math.Max(this.MaxDepth(root.left), this.MaxDepth(root.right));
        }
        public int MinDepth(TreeNode root)
        {
            if (root == null) return 0;
            return 1 + Math.Min(this.MinDepth(root.left), this.MinDepth(root.right));
        }

        public bool IsBalanced(TreeNode root)
        {
            return (MaxDepth(root) - this.MinDepth(root) <= 1);


        }
#endregion

        #region 4.3
        public TreeNode ArrayToTree(int[] ar)
        {
	        if(ar==null || ar.Length<1) return null;
	        TreeNode root = new TreeNode();

	        ConvertToTree(ar, root, 0, ar.Length-1);

	        return root;
        }

        public TreeNode ConvertToTree2(int[] ar, int start, int end)
        {
            if (start > end) return null;
            int mid = (start + end)/2;
            TreeNode node = new TreeNode(mid);
            node.left = ConvertToTree2(ar, start, mid - 1);
            node.right = ConvertToTree2(ar, mid + 1, end);
            return node;
        }

        public void ConvertToTree(int[] ar, TreeNode root, int start, int end)
        {
	        if(start == end) {
		        root.value = ar[start];
		        root.left=null;
		        root.right=null;
		        return;
	        }

	        int mid = (start+end)/2;
	        root.value = ar[mid];
	        if(start<mid){
		        root.left = new TreeNode();
		        ConvertToTree(ar, root.left, start, mid-1);
	        }
	        if(end>mid){
		        root.right = new TreeNode();
		        ConvertToTree(ar, root.right, mid+1, end);
	        }
        }

        public static void Test4_3()
        {
            int[] ar = new int[]{1,2,3};
            Chapt4 cp = new Chapt4();
            TreeNode root = cp.ArrayToTree(ar);
            PrintTree(root);
        }
        #endregion

        #region 4.5
        public TreeNode Succeed(TreeNode node)
        {
            if (node == null) return null;
            if (node.right != null)
            {
                TreeNode sr = node.right;
                while (sr.left != null) sr = sr.left;
                return sr;
            }
            else
            {
                TreeNode parent = node.parent;
                if (parent == null) return null;

                if (parent.left == node) return parent;
                if (parent.right == node)
                {
                    TreeNode grandp = parent.parent;
                    if (grandp == null) return null;
                    return grandp;
                }
                return null;
            }
        }
        #endregion

        #region 4.6

        public TreeNode CommonAncestor(TreeNode n1, TreeNode n2)
        {
            if (n1 == null || n2 == null) return null;
            int dep1 = GetDepth(n1);
            int dep2 = GetDepth(n2);
            if (dep1 > dep2) n1 = GoUp(n1, dep1 - dep2);
            if (dep2 > dep1) n2 = GoUp(n2, dep2 - dep1);

            while (n1 != n2)
            {
                n1 = n1.parent;
                n2 = n2.parent;
            }

            return n1;
        }

        public int GetDepth(TreeNode node)
        {
            if (node == null) return 0;
            int d = 0;
            while (node.parent != null)
            {
                d++;
                node = node.parent;
            }

            return d;
        }

        public TreeNode GoUp(TreeNode node, int steps)
        {
            if (node == null || steps < 1) throw new ArgumentException("inpust incorrect.");
            while (steps > 0)
            {
                if (node == null) throw new ArgumentException("the steps incorrect");
                steps--;
                node = node.parent;
            }

            return node;
        }
        #endregion

        #region 4.7
        public bool IsSubTree(TreeNode t1, TreeNode t2)
        {
            if (t1 == null || t2 == null) return false;
            if (t1.value == t2.value)
            {
                if (EqualTree(t1, t2)) return true;
            }

            return (IsSubTree(t1.left, t2) || IsSubTree(t1.right, t2));
        }

        public bool EqualTree(TreeNode t1, TreeNode t2)
        {
            if (t1 == null && t2 == null) return true;
            if (t1 == null || t2 == null) return false;
            if (t1.value != t2.value) return false;

            return (EqualTree(t1.left, t2.left) && EqualTree(t1.right, t2.right));
        }
        #endregion

        #region 4.8
        public void PathSum(TreeNode root, int sum)
        {
            if (root == null) return;
            List<int> path = new List<int>();

            FindPath(root, sum, path);
        }

        public void FindPath(TreeNode t, int sum, List<int> currPath)
        {
            if (t == null || currPath == null) throw new ArgumentException("Input incorrect");

            CheckAndPrint(currPath, t, sum);

            currPath.Add(t.value);
            FindPath(t.left, sum, currPath);
            FindPath(t.right, sum, currPath);
        }

        public void CheckAndPrint(List<int> currPath, TreeNode t, int sum)
        {
	        int pathsum = t.value;
	        for(int i=currPath.Count-1;i>=0;i--)
	        {
		        pathsum+=currPath[i];
	            if (pathsum == sum) PrintPath(currPath, i);
	        }
        }
        public void PrintPath(List<int> path, int start)
        {
            for(int i = start; i<path.Count;i++)
                Console.Write(path[i]);
            Console.WriteLine();
        }
        #endregion
    }


}
