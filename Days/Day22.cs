﻿using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day22 : AocDay<MapWithPassword>
    {
        public Day22(IInputParser<MapWithPassword> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(MapWithPassword input)
        {
            var start = FindStart(input.Map);
            var (pos, facing) = Move(input.Map, start, Facing.Up, new TurnInfo(TurnType.Right, input.InitialMove));
            foreach (var m in input.Moves)
                (pos, facing) = Move(input.Map, pos, facing, m);
            var pass = 1000 * (pos.Item1 + 1) + 4 * (pos.Item2 + 1) + (int)facing;
            Console.WriteLine(pass);
        }

        protected override void Part2(MapWithPassword input)
        {
            //BuildCube(input.Map);
            var start = FindStart(input.Map);
            var (pos, facing) = Move2(input.Map, start, Facing.Up, new TurnInfo(TurnType.Right, input.InitialMove));
            foreach (var m in input.Moves)
                (pos, facing) = Move2(input.Map, pos, facing, m);
            var pass = 1000 * (pos.Item1 + 1) + 4 * (pos.Item2 + 1) + (int)facing;
            Console.WriteLine(pass);
        }

        private static (int, int) FindStart(char[,] map)
        {
            int x = 0;
            for (int i = 0; i < map.GetLength(1); i++)
                if (map[0, i] == '.')
                {
                    x = i;
                    break;
                }
            return (0, x);
        }

        public enum Facing
        {
            Right = 0,
            Down = 1,
            Left = 2,
            Up = 3
        }

        private static Facing Turn(Facing facing, TurnType turn)
        {
            return facing switch
            {
                Facing.Left => turn == TurnType.Left ? Facing.Down : Facing.Up,
                Facing.Right => turn == TurnType.Left ? Facing.Up : Facing.Down,
                Facing.Up => turn == TurnType.Left ? Facing.Left : Facing.Right,
                Facing.Down => turn == TurnType.Left ? Facing.Right : Facing.Left,
                _ => throw new Exception("Invalid facing")
            };
        }

        private static (int, int) NextPosition((int, int) pos, Facing facing)
        {
            var (y, x) = pos;
            return facing switch
            {
                Facing.Right => (y, x + 1),
                Facing.Left => (y, x - 1),
                Facing.Up => (y - 1, x),
                Facing.Down => (y + 1, x),
                _ => throw new Exception("Invalid move")
            };
        }

        private static (int, int) CoerceIndexes((int, int) pos, char[,] map)
        {
            var (y, x) = pos;
            if (x < 0) x = map.GetLength(1) - 1;
            if (x >= map.GetLength(1)) x = 0;
            if (y < 0) y = map.GetLength(0) - 1;
            if (y >= map.GetLength(0)) y = 0;
            return (y, x);
        }

        private static (int, int) SingleMove((int, int) pos, Facing facing, char[,] map)
        {
            var (y, x) = CoerceIndexes(NextPosition(pos, facing), map);
            while (map[y, x] == ' ')
                (y, x) = CoerceIndexes(NextPosition((y, x), facing), map);
            if (map[y, x] == '#')
                return pos;
            return (y, x);
        }

        private static ((int, int), Facing) Move(char[,] map, (int, int) pos, Facing facing, TurnInfo turnInfo)
        {
            facing = Turn(facing, turnInfo.Turn);
            for (int step = 0; step < turnInfo.Move; step++)
                pos = SingleMove(pos, facing, map);
            return (pos, facing);
        }

        private static ((int, int), Facing) Move2(char[,] map, (int, int) pos, Facing facing, TurnInfo turnInfo)
        {
            facing = Turn(facing, turnInfo.Turn);
            for (int step = 0; step < turnInfo.Move; step++)
                pos = SingleMove2(pos, facing, map);
            return (pos, facing);
        }

        private static (int, int) SingleMove2((int, int) pos, Facing facing, char[,] map)
        {
            //TODO: implement
            return pos;
        }

            //Example 2
            // . A B
            // . C .
            // D E .
            // F . .
            //Example 1
            //. . A .
            //B C D .
            //. . E F

        //1 - 6, 2 - 5, 3 - 4
        
    }
}