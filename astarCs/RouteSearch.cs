using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace astarCs
{
    public class NotReachableExcetion : Exception
    {
        public NotReachableExcetion(string msg) : base(msg)
        {
        }
    }

    public class RouteSearch
    {
        private const byte AccessiblePoint = 0;
        private const byte StartPoint = 2;
        private const byte DestiPoint = 3;
        private const byte BarrierPoint = 1;

        //private readonly List<AsPoint> _closeList, _openList;
        //private readonly NumList _closeList, _openList;
        private readonly NumHashlist _closeList, _openList;
        private readonly Stack<IEnumerable<AsPoint>> _reachableStepsStack;
        private int _maximumX, _maximumY;
        private byte[,] _map;

        public RouteSearch(int capacity)
        {
            //this._openList = new List<AsPoint>();
            //this._closeList = new List<AsPoint>();
            //
            this._openList = new NumHashlist(capacity);
            this._closeList = new NumHashlist(capacity);
            this._reachableStepsStack = new Stack<IEnumerable<AsPoint>>();            
        }

        public List<AsPoint> GetRoute( byte[,] map, int width, int height)
        {
            AsPoint.Width = width;
            AsPoint.Height = height;
            this._map = map;
            AsPoint startPoint = new AsPoint(), destiPoint = new AsPoint();
            this._maximumY = width;
            this._maximumX = height;
            Enumerable.Range(0, height).ToList().ForEach((n) =>
            {
                Enumerable.Range(0, width).ToList().ForEach(m =>
                {
                    if (map[n, m] == AccessiblePoint)
                        this._openList.Add(new AsPoint(n, m));
                    else if (map[n, m] == StartPoint)
                        startPoint = new AsPoint(n, m);
                    else if (map[n, m] == DestiPoint)
                    {
                        this._openList.Add(new AsPoint(n, m));
                        destiPoint = new AsPoint(n, m);
                    }
                });
            });
            this._closeList.AddRange(new[] { startPoint });
            
            var route = new List<AsPoint> {startPoint};
            return AstarSearchAlgorithm(ref route, startPoint, destiPoint, false);
        }

        private AsPoint[] GetReachableSteps(AsPoint p)
        {
            var temp = new[]
            {
                //new AsPoint(p.X - 1, p.Y + 1), new AsPoint(p.X, p.Y + 1), new AsPoint(p.X + 1, p.Y + 1),
                //new AsPoint(p.X - 1, p.Y),     new AsPoint(p.X - 1, p.Y),
                //new AsPoint(p.X - 1, p.Y - 1), new AsPoint(p.X, p.Y - 1), new AsPoint(p.X + 1, p.Y - 1)
                new AsPoint(p.X, p.Y + 1),
                new AsPoint(p.X - 1, p.Y), new AsPoint(p.X + 1, p.Y),
                new AsPoint(p.X, p.Y - 1),
            };
            for (var i = 0; i < 4; i++)
            {
                if (!IsPointAvailible(temp[i]))
                    temp[i] = null;
            }            
            return temp;
        }

        private bool IsPointAvailible(AsPoint p)
        {
            if (p.X < 0 || p.X >= this._maximumX || p.Y < 0 || p.Y >= this._maximumY)
                return false;
            return this._openList.Contains(p) && !this._closeList.Contains(p);
            
        }

        private List<AsPoint> AstarSearchAlgorithm(ref List<AsPoint> route, AsPoint startPoint, AsPoint destiPoint, bool revert)
        {
            AsPoint nextStep;
            if (revert)
            {
                if (this._openList.Count == 0 || this._reachableStepsStack.Count == 0 || route.Count == 0)
                    throw new NotReachableExcetion("Not reachable destination.");
                this._openList.AddRange(this._reachableStepsStack.Pop());
                route.RemoveAt(route.Count - 1);
                startPoint = route.Last();
                revert = false;
            }

            AsPoint[] nextPossibleSteps = GetReachableSteps(startPoint);
            //reached
            if (nextPossibleSteps.Contains(destiPoint))
            {
                route.Add(destiPoint);
                return route;
            }
            //get blocked and revert
            if (nextPossibleSteps.All(p=>p==null))
            {
                revert = true;
            }
            else
            {
                nextStep = Decision(nextPossibleSteps, destiPoint);
                //Debug.WriteLine("Next Step: " + nextStep);
                this._reachableStepsStack.Push(nextPossibleSteps.SkipWhile(p =>
                {
                    if (p == null) return true;
                    return p.HashCode == nextStep.HashCode;
                }));
                this._closeList.Add(nextStep);
                route.Add(nextStep);
                startPoint = nextStep;
            }

            //AstarCsProgram.DrawRoute(this._closeList, route, _map, this._maximumY, this._maximumX);
            return AstarSearchAlgorithm(ref route, startPoint, destiPoint, revert);
        }

        private AsPoint Decision(IEnumerable<AsPoint> points, AsPoint destiPoint)
        {
            return points.OrderBy(p =>
            {
                if (p == null)
                    return int.MaxValue;
                this._openList.Remove(p);
                return (p.X - destiPoint.X)*(p.X - destiPoint.X) + (p.Y - destiPoint.Y)*(p.Y - destiPoint.Y);
            })
            .First();
        }

        private AsPoint Decision(AsPoint[] points, AsPoint destiPoint)
        {
            var minimuxIndex = 0;
            var distanceSquare = 0;
            var minDistanceSqure = int.MaxValue;

            for (var i = 0; i < points.Length; i++)
            {
                if (points[i] == null)
                    continue;
                distanceSquare =
                   (points[i].X - destiPoint.X) * (points[i].X - destiPoint.X) +
                   (points[i].Y - destiPoint.Y) * (points[i].Y - destiPoint.Y);
                if (distanceSquare < minDistanceSqure)
                {
                    minDistanceSqure = distanceSquare;
                    minimuxIndex = i;
                }
            }
            return points[minimuxIndex];
        }
    }
}