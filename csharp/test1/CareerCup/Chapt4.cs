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
            
        }
        #endregion
    }


}
