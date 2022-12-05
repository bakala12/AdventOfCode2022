using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day5 : AocDay<CratesStacks>
    {
        public Day5(IInputParser<CratesStacks> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(CratesStacks input)
        {
            var stacks = CreateMovableStacks(input);
            foreach (var move in input.Moves)
            {
                DoMove(stacks, move);
            }
            Console.WriteLine(string.Join("", stacks.Select(s => s.TryPeek(out char r) ? r.ToString() : "")));
        }

        protected override void Part2(CratesStacks input)
        {
            var stacks = CreateMovableStacks(input);
            foreach (var move in input.Moves)
            {
                DoMoveWithOrder(stacks, move);
            }
            Console.WriteLine(string.Join("", stacks.Select(s => s.TryPeek(out char r) ? r.ToString() : "")));
        }

        private static Stack<char>[] CreateMovableStacks(CratesStacks input)
        {
            var stacks = new Stack<char>[input.Stacks.Length];
            for (int i = 0; i < input.Stacks.Length; i++)
            {
                stacks[i] = new Stack<char>();
                for (int ind = input.Stacks[i].Items.Length - 1; ind >= 0; ind--)
                    stacks[i].Push(input.Stacks[i].Items[ind]);
            }
            return stacks;
        }

        private static void DoMove(Stack<char>[] stacks, StackMove move)
        {
            var from = stacks[move.From - 1];
            var to = stacks[move.To - 1];
            for (int q = 0; q < move.Quantity; q++)
                to.Push(from.Pop());
        }

        private static void DoMoveWithOrder(Stack<char>[] stacks, StackMove move)
        {
            var from = stacks[move.From - 1];
            var to = stacks[move.To - 1];
            var h = new Stack<char>();
            for (int q = 0; q < move.Quantity; q++)
                h.Push(from.Pop());
            foreach (var i in h)
                to.Push(i);
        }
    }
}