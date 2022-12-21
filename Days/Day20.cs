using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day20 : AocDay<int[]>
    {
        public Day20(IInputParser<int[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(int[] input)
        {
            input = new int[] { 1,
2,
-3,
3,
-2,
0,
4};
            var list = new CyclicList(input);
            foreach(var item in input)
            {
                //list.Display();
                //Console.ReadLine();
                list.Move(item);
            }
            //list.Display();
            var r = list.ItemAtPosition(1000) + list.ItemAtPosition(2000) + list.ItemAtPosition(3000);
            Console.WriteLine(r);
        }

        protected override void Part2(int[] input)
        {
        }

        public class ListNode<T>
        {
            public T Value { get; }
            public ListNode<T>? Next { get; set; }
            public ListNode<T>? Prev { get; set; }

            public ListNode(T value)
            {
                Value = value;
            }
        }

        public class CyclicList
        {
            private readonly ListNode<int> head;
            private readonly int count;

            public CyclicList(int[] array)
            {
                if (array.Length == 0)
                    throw new ArgumentException("Empty array");
                head = new ListNode<int>(array[0]);
                count = array.Length;
                ListNode<int> prev = head;
                for(int i = 1; i < array.Length; i++)
                {
                    var next = new ListNode<int>(array[i]);
                    prev.Next = next;
                    next.Prev = prev;
                    prev = next;
                }
                prev.Next = head;
                head.Prev = prev;
            }

            public void Move(int item)
            {
                var node = Find(item);
                node.Prev!.Next = node.Next;
                node.Next!.Prev = node.Prev;
                while (item < 0)
                    item += count - 1;
                item %= (count - 1);
                var next = node.Next;
                for (int i = 0; i < item; i++)
                    next = next!.Next;
                next!.Prev!.Next = node;
                node.Prev = next.Prev;
                node.Next = next;
                next.Prev = node;
            }


            private ListNode<int> Find(int item)
            {
                if (head.Value == item)
                    return head;
                var n = head.Next;
                while (n != head)
                {
                    if (n!.Value == item)
                        return n;
                    n = n.Next;
                }
                throw new Exception("Element not found");
            }

            public int ItemAtPosition(int positionAfterZero)
            {
                var item = Find(0);
                for (int i = 0; i < positionAfterZero % count; i++)
                    item = item!.Next;
                return item!.Value;
            }

            public void Display()
            {
                var h = head;
                Console.Write(h.Value);
                Console.Write(" ");
                h = head.Next;
                while (h != head)
                {
                    Console.Write(h.Value);
                    Console.Write(" ");
                    h = h.Next;
                }
                Console.WriteLine();
            }
        }
    }
}