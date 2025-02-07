using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Vector2D : IEquatable<Vector2D>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Vector2D(Vector2D vec)
        {
            X = vec.X;
            Y = vec.Y;
        }
        public bool Equals(Vector2D other)
        {
            return X == other.X && Y == other.Y;
        }
        public Vector2D[] GetRoundVector2Ds()
        {
            var roundVecs = new Vector2D[]
            {
            new Vector2D(X, Y - 1),
            new Vector2D(X, Y + 1),
            new Vector2D(X - 1, Y),
            new Vector2D(X + 1, Y)
            };
            return roundVecs;
        }
        public Vector2D[] GetSuffledRoundVector2Ds(System.Random r)
        {
            var roundVecs = GetRoundVector2Ds();
            int size = roundVecs.Length;
            int maxRange = size - 1;
            for (int i = 0; i < size; i++)
            {
                var ranIdx = r.Next(0, maxRange);
                var temp = roundVecs[i];
                roundVecs[i] = roundVecs[ranIdx];
                roundVecs[ranIdx] = temp;
            }
            return roundVecs;
        }
        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}
