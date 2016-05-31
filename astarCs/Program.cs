using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace astarCs
{
    public class AstarCsProgram
    {
        private static readonly Random RandomPoint = new Random(DateTime.Now.Millisecond);
        const int Row = 10;
        const int Column = 19;

        private static readonly byte[,] Map = new byte[Row, Column]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };
        static readonly List<long> RunTimeStat = new List<long>();
        static int _unreachableCount = 0;
        static void Main(string[] args)
        {
            var testTime = 30000;
            var stopWatch = new Stopwatch();
            while (testTime -- >0)
            {
                var startPoint = GetRandomPoint();
                var stopPoint = GetRandomPoint();

                //Debug.WriteLine("Start: " + startPoint + "   desti:  " + stopPoint);
                Map[startPoint.X, startPoint.Y] = 2;
                Map[stopPoint.X, stopPoint.Y] = 3;
                //Map[7, 2] = 2;
                //Map[4, 9] = 3;
                stopWatch.Restart();
                TestSearch(Map, Column, Row);
                stopWatch.Stop();
                if (testTime%10000 == 0)
                    Console.WriteLine(testTime / 10000);

                RunTimeStat.Add(stopWatch.ElapsedTicks);
                Map[startPoint.X, startPoint.Y] = 0;
                Map[stopPoint.X, stopPoint.Y] = 0;
            }
            Console.Clear();
            RunTimeStat.ForEach(Console.WriteLine);
            Console.WriteLine("Average: " + RunTimeStat.Average() + " ticks.   Unreachable Count: " + _unreachableCount +
                              "\nTotal cost time: " + RunTimeStat.Sum() + " ticks" +
                              "\nPress any key exit.");
            Console.ReadKey();
        }

        private static AsPoint GetRandomPoint()
        {
            while (true)
            {
                var x = RandomPoint.Next(Column - 1);
                var y = RandomPoint.Next(Row - 1);
                if (Map[y, x] == 0) 
                    return new AsPoint(y, x);
            }
        }

        private static void TestSearch(byte[,] map, int column, int row)
        {
            try
            {
                var route = new RouteSearch(column * row).GetRoute(map, column, row);
                //DrawRoute(route, map, column, row);
            }
            catch (NotReachableExcetion nrex)
            {
                _unreachableCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void DrawRoute(List<AsPoint> route, byte[,] map, int column, int row)
        {
            var tempMap = new byte[row, column];
            var tempRoute = new List<AsPoint>(route);
            Array.Copy(map, tempMap, row*column);
            if (tempRoute.Count >0)
                tempRoute.RemoveAt(0);
            if (tempRoute.Count > 1)
            {
                var lastPoint = tempRoute.Last();
                if(tempMap[lastPoint.X, lastPoint.Y] == 3)
                    tempRoute.RemoveAt(tempRoute.Count - 1);
            }
            tempRoute.ForEach(p => tempMap[p.X, p.Y] = 4);
            Console.Clear();
            for (var j = 0; j < row; j++)
            {
                for (var i = 0; i < column; i++)
                {
                    if (tempMap[j, i] == 0)
                        Console.Write("  ,");
                    else
                        Console.Write(tempMap[j, i] + " ,");
                }
                Console.WriteLine();
            }
        }
        public static void DrawRoute(List<AsPoint> closeList, List<AsPoint> route, byte[,] map, int column, int row)
        {
            var tempMap = new byte[row, column];
            Array.Copy(map, tempMap, row * column);
            //
            var tempCloseList = new List<AsPoint>(closeList);
            tempCloseList.RemoveAt(0);
            tempCloseList.ForEach(p => tempMap[p.X, p.Y] = 8);
            //
            var tempRoute = new List<AsPoint>(route);
            if (tempRoute.Count > 0)
                tempRoute.RemoveAt(0);
            if (tempRoute.Count > 1)
            {
                var lastPoint = tempRoute.Last();
                if (tempMap[lastPoint.X, lastPoint.Y] == 3)
                    tempRoute.RemoveAt(tempRoute.Count - 1);
            }
            tempRoute.ForEach(p => tempMap[p.X, p.Y] = 4);
            //
            Console.Clear();

            Console.Write("    ");
            for (var index = 0; index < column; index++)
            {
                Console.Write(index.ToString().PadLeft(2, ' ') + ",");
            }
            Console.Write("\n    ");
            for (var index = 0; index < column; index++)
            {
                Console.Write("___");
            }
            Console.WriteLine();
            for (var j = 0; j < row; j++)
            {
                Console.Write(j + " | ");
                for (var i = 0; i < column; i++)
                {
                    if (tempMap[j, i] == 0)
                        Console.Write("  ,");
                    else
                        Console.Write(tempMap[j, i] + " ,");
                }
                Console.WriteLine();
            }
        }
        public static void DrawRoute(NumHashlist closeList, List<AsPoint> route, byte[,] map, int column, int row)
        {
            var tempMap = new byte[row, column];
            Array.Copy(map, tempMap, row * column);
            //
            var tempCloseList = closeList.Where(p => p != null).ToList();// new List<AsPoint>(closeList);
            tempCloseList.RemoveAt(0);
            tempCloseList.ForEach(p => tempMap[p.X, p.Y] = 8);
            //
            var tempRoute = new List<AsPoint>(route);
            if (tempRoute.Count > 0)
                tempRoute.RemoveAt(0);
            if (tempRoute.Count > 1)
            {
                var lastPoint = tempRoute.Last();
                if (tempMap[lastPoint.X, lastPoint.Y] == 3)
                    tempRoute.RemoveAt(tempRoute.Count - 1);
            }
            tempRoute.ForEach(p => tempMap[p.X, p.Y] = 4);
            //
            Console.Clear();

            Console.Write("    ");
            for (var index = 0; index < column; index++)
            {
                Console.Write(index.ToString().PadLeft(2, ' ') + ",");
            }
            Console.Write("\n    ");
            for (var index = 0; index < column; index++)
            {
                Console.Write("___");
            }
            Console.WriteLine();
            for (var j = 0; j < row; j++)
            {
                Console.Write(j + " | ");
                for (var i = 0; i < column; i++)
                {
                    if (tempMap[j, i] == 0)
                        Console.Write("  ,");
                    else
                        Console.Write(tempMap[j, i] + " ,");
                }
                Console.WriteLine();
            }
        }
    }
}
