using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
    using System.Collections;

    class Chapt3 : ExamBase
    {
        public static void Test3_2()
        {
            StackWithMin swm = new StackWithMin();
            swm.Push(3);
            AssertEqual(3, swm.Min());
            swm.Push(5);
            AssertEqual(3, swm.Min());            
            swm.Push(1);
            AssertEqual(1, swm.Min());
        }

        public static void Test3_3()
        {
            SetOfStack ss = new SetOfStack();
            ss.Push(1);
            ss.Push(2);
            AssertEqual(2, ss.Peek());
            ss.Push(3);
            ss.Push(4);
            AssertEqual(4, ss.Peek());

            AssertEqual(4, ss.Pop());

            AssertEqual(3, ss.Pop());

            AssertEqual(2, ss.Pop());

            AssertEqual(1, ss.Pop());

            AssertEqual(true, ss.IsEmpty());

            ss.Push(1);
            ss.Push(2);
            ss.Push(3);
            ss.Push(4);
            AssertEqual(4, ss.Peek());
        }

        public static void Test3_4()
        {
            var q = new MyQueue();
            q.Enqueue2(1);
            q.Enqueue2(2);
            q.Enqueue2(3);
            AssertEqual(1, q.Dequeue2());
            AssertEqual(2, q.Dequeue2());
            q.Enqueue2(4);
            AssertEqual(3, q.Dequeue2());

        }
    }

    //3.2
    public class StackWithMin
    {
	    Stack buffer = new Stack();
	    Stack minSequence = new Stack();
	
	    public void Push(int v)
	    {
		    buffer.Push(v);
		
		    if(minSequence.Count<1) {
			    minSequence.Push(v);
		    }
		    else
		    {
		        int currMin = this.Min();
			    if(v<=currMin) minSequence.Push(v);
		    }
	    }
	
	    public int Pop()
	    {
		    if(buffer.Count<1) throw new InvalidOperationException("Stack is empty");
		    int num = (int)buffer.Pop();
	        int currMin = (int)minSequence.Peek();
		
		    if(num==currMin) minSequence.Pop();
		
		    return num;
	    }	
	
	    public int Min()
	    {
            if (buffer.Count < 1) throw new InvalidOperationException("Stack is empty");
            if (minSequence.Count < 1) throw new InvalidOperationException("errror");
		
		    return (int)minSequence.Peek();
		
	    }
    }
    
    //3.3
    public class SetOfStack
    {
        List<Stack> stacks = new List<Stack>();
        int stackNum = 0;
        int threshold = 3;

        public bool IsEmpty()
        {
            if (stackNum < 1) return true;
            else return false;
        }

        public void Push(int v)
        {
            if (stackNum < 1 ||
                (stackNum > 0 && stacks[stackNum - 1].Count == threshold))
            {
                if(stacks.Count < stackNum+1)
                    stacks.Add(new Stack());
                stackNum++;
            }

            stacks[stackNum - 1].Push(v);
        }

        public int Pop()
        {
            if (IsEmpty()) throw new InvalidOperationException("Cannot pop from empty stack");

            int v = (int)stacks[stackNum - 1].Pop();
            if (stacks[stackNum - 1].Count < 1) stackNum--;

            return v;
        }

        public int Peek()
        {
            if (IsEmpty()) throw new InvalidOperationException("Cannot peak from empty stack");

            int v = (int)stacks[stackNum - 1].Peek();
            return v;
        }
    }

    //3.4
    public class MyQueue
    {
        Stack s1 = new Stack();
        Stack s2 = new Stack();

        int flag = 1;
        public void Enqueue(int v)
        {
            s1.Push(v);
        }


        public int Dequeue()
        {
            if (s1.Count < 1) throw new Exception("Queue is empty");
            Move(s1, s2);
            int v = (int)s2.Pop();
            Move(s2, s1);

            return v;
        }

        public void Enqueue2(int v)
        {
            if (flag != 1)
            {
                Move(s2, s1);
                flag = 1;
            }

            s1.Push(v);
        }

        public int Dequeue2()
        {
            if (flag == 1)
            {
                Move(s1, s2);
                flag = 2;
            }

            return (int)s2.Pop();
        }

        private void Move(Stack from, Stack to)
        {
            while (from.Count>0)
            {
                to.Push(from.Pop());
            }
        }
    }
}
