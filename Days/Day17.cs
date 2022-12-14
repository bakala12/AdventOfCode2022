using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day17 : AocDay<LeftRight[]>
    {
        public Day17(IInputParser<LeftRight[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(LeftRight[] input)
        {
            var well = new BlockWell();
            var moves = new LazyQueue<LeftRight>(input);
            int count = 2022;
            foreach (var block in BlocksCollection.GetBlocks().Take(count))
                well.AddBlockAndMove(block, moves);
            Console.WriteLine(well.GetWellHeight());
        }

        protected override void Part2(LeftRight[] input)
        {
            var well = new BlockWell();
            var moves = new LazyQueue<LeftRight>(input);
            List<(int index, int height)> positions = new();
            while (true)
            {
                foreach (var block in BlocksCollection.GetBlocks())
                {
                    well.AddBlockAndMove(block, moves);
                    if (block == BlocksCollection.Fifth)
                    {
                        positions.Add((moves.Index, well.GetWellHeight()));
                        if (positions.Count < 1000)
                            continue;

                        (int position, int length) = FindLongestDuplicate(positions);
                        int difference = positions[position + length].height - positions[position].height;
                        int basePlaced = position * 5;
                        int duplicatePlaced = length * 5;
                        long repeats = (1000000000000L - basePlaced) / duplicatePlaced;
                        int leftOver = (int)(1000000000000L - basePlaced - repeats * duplicatePlaced);
                        int baseCount = positions[position + leftOver / 5 - 1].height;
                        Console.WriteLine($"{baseCount + repeats * difference}");
                        return;
                    }
                }
            }

        }

        private (int index, int length) FindLongestDuplicate(List<(int index, int height)> positions)
        {
            int n = positions.Count;
            int[,] data = new int[n + 1, n + 1];

            int length = 0;
            int index = 0;
            for (int i = 1; i <= n; i++)
            {
                for (int j = i + 1; j <= n; j++)
                {
                    if (positions[i - 1].index == positions[j - 1].index && data[i - 1, j - 1] < (j - i))
                    {
                        data[i, j] = data[i - 1, j - 1] + 1;

                        if (data[i, j] > length)
                        {
                            length = data[i, j];
                            index = Math.Max(i, index);
                        }
                    }
                    else
                    {
                        data[i, j] = 0;
                    }
                }
            }

            return (length > 0 ? index - length : -1, length);
        }

        /// <summary>
        /// Block well has 7 width. Each row is integer.
        /// Each line has first bit and nineth bit set (as those are borders)
        /// 1 0000 0001 (in binary)
        /// </summary>
        public class BlockWell
        {
            private const int EmptyLine = 0b100000001;
            private List<int> BlockLines = new();

            public int GetWellHeight() => BlockLines.Count;

            public void AddBlockAndMove(int[] block, LazyQueue<LeftRight> moves)
            {
                for (int i = 0; i < 3; i++)
                    BlockLines.Add(EmptyLine);
                for (int i = block.Length - 1; i >= 0; i--)
                    BlockLines.Add(block[i] | EmptyLine);
                int[] blockCopy = new int[block.Length];
                Array.Copy(block, blockCopy, block.Length);
                MoveBlock(blockCopy, moves);
            }

            private void MoveBlock(int[] block, LazyQueue<LeftRight> moves)
            {
                int depth = 0;
                bool canMoveDown = true;
                while (canMoveDown)
                {
                    RemoveBlock(block, depth);
                    Func<int, int> modifier = moves.Dequeue() switch
                    {
                        LeftRight.Left => l => l << 1,
                        _ => l => l >> 1
                    };
                    if (CanMove(block, modifier, depth))
                        MoveBlockHorizontally(block, modifier);
                    canMoveDown = depth + block.Length < BlockLines.Count && CanMove(block, i => i, depth + 1);
                    if (canMoveDown)
                        depth++;
                    PutBlock(block, depth);
                    var last = BlockLines.Last();
                    if (last == 0 || last == EmptyLine)
                    {
                        BlockLines.RemoveAt(BlockLines.Count - 1);
                        depth = 0;
                    }
                }
            }

            private static void MoveBlockHorizontally(int[] block, Func<int, int> modifier)
            {
                for (int i = 0; i < block.Length; i++)
                    block[i] = modifier(block[i]);
            }

            private void RemoveBlock(int[] block, int depth)
            {
                for (int i = 0; i < block.Length; i++)
                    BlockLines[BlockLines.Count - 1 - depth - i] ^= block[i];
            }

            private void PutBlock(int[] block, int depth)
            {
                for (int i = 0; i < block.Length; i++)
                    BlockLines[BlockLines.Count - 1 - depth - i] |= block[i];
            }

            private bool CanMove(int[] block, Func<int, int> modifier, int depth)
            {
                for (int i = 0; i < block.Length; i++)
                {
                    if (BlockLines.Count - 1 - depth - i < 0)
                        return false;
                    var l = BlockLines[BlockLines.Count - 1 - depth - i] & modifier(block[i]);
                    if (l != 0)
                        return false;
                }
                return true;
            }

            public void Display()
            {
                for (int i = BlockLines.Count - 1; i >= 0; i--)
                {
                    Console.Write('|');
                    for (int m = 7; m >= 1; m--)
                        Console.Write((BlockLines[i] & (1 << m)) != 0 ? '#' : '.');
                    Console.WriteLine('|');
                }
                Console.WriteLine("---------");
            }
        }

        public static class BlocksCollection
        {
            public static int[] First = new int[1]
            {
            0b000111100,
            };

            public static int[] Second = new int[3]
            {
            0b000010000,
            0b000111000,
            0b000010000
            };

            public static int[] Third = new int[3]
            {
            0b000001000,
            0b000001000,
            0b000111000
            };

            public static int[] Forth = new int[4]
            {
            0b000100000,
            0b000100000,
            0b000100000,
            0b000100000
            };

            public static int[] Fifth = new int[2]
            {
            0b000110000,
            0b000110000
            };

            public static IEnumerable<int[]> GetBlocks()
            {
                while (true)
                {
                    yield return First;
                    yield return Second;
                    yield return Third;
                    yield return Forth;
                    yield return Fifth;
                }
            }
        }

        public class LazyQueue<T>
        {
            private readonly T[] _array;
            private int _ind = 0;

            public int Index => _ind;

            public LazyQueue(T[] array)
            {
                _array = array;
            }

            public T Dequeue()
            {
                if (_ind == _array.Length)
                    _ind = 0;
                return _array[_ind++];
            }
        }
    }
}