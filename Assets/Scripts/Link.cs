using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
namespace Assets.Scripts
{
    public class Link : IEquatable<Link>, IEqualityComparer<Link>
    {
        public Vector2D Start { get; set; }
        public Vector2D End { get; set; }
        public Link()
        {
            Start = new Vector2D(0, 0);
            End = new Vector2D(0, 0); ;
        }
        public Link(int aStartX, int aStartY, int aEndX, int aEndY)
        {
            Start = new Vector2D(aStartX, aStartY);
            End = new Vector2D(aEndX, aEndY); ;
        }
        public Link(Vector2D Start, Vector2D End)
        {
            this.Start = Start;
            this.End = End;
        }
        public bool Equals(Link other)
        {
            if (IsParallel(true, other))
            {
                return (Start.X == other.Start.X && End.X == other.End.X) || (Start.X == other.End.X && End.X == other.Start.X);
            }
            else if (IsParallel(false, other))
            {
                return (Start.Y == other.Start.Y && End.Y == other.End.Y) || (Start.Y == other.End.Y && End.Y == other.Start.Y);
            }
            else
            {
                return false;
            }
        }
        public bool IsParallel(bool IsHorizontally)
        {
            if (IsHorizontally)
            {
                return Start.Y == End.Y;
            }
            else
            {
                return Start.X == End.X;
            }
        }
        private bool IsParallel(bool IsHorizontally, Link other)
        {
            if (IsHorizontally)
            {
                return Start.Y == other.Start.Y && Start.Y == other.End.Y && IsParallel(IsHorizontally);
            }
            else
            {
                return Start.X == other.Start.X && Start.X == other.End.X && IsParallel(IsHorizontally);
            }
        }
        public override string ToString()
        {
            return string.Format("({0}>{1})", Start, End);
        }

        public bool Equals(Link x, Link y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Link obj)
        {
            return 2222;
        }
    }
}
