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



    }

    public class TreeNode
    {
        public int value;
        public TreeNode left;
        public TreeNode right;
    }
}
