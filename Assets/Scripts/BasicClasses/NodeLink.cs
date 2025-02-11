using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.BasicClasses
{
    public class NodeLink
    {
        public NodeLink(int x1, int y1, int x2, int y2)
        {
            m_Source = new Vector2Int(x1, y1);
            m_Target = new Vector2Int(x2, y2);
        }
        public NodeLink(Vector2Int v1, Vector2Int v2)
        {
            m_Source = v1;
            m_Target = v2;
        }
        private Vector2Int m_Source;
        private Vector2Int m_Target;
        public Vector2Int Source { get => m_Source; set => m_Source = value; }
        public Vector2Int Target { get => m_Target; set => m_Target = value; }
        public bool IsHorizontal()
        { return HasDirections(Vector2Int.left, Vector2Int.right); }
        public bool IsVertical()
        { return HasDirections(Vector2Int.up, Vector2Int.down); }
        private bool HasDirections(Vector2Int a, Vector2Int b)
        {
            var l_Gap = Source - Target;
            return l_Gap == a || l_Gap == b;
        }

        public override string ToString()
        {
            return $"Link from {Source} to {Target}";
        }
    }

    class NodeLinkComparer : EqualityComparer<NodeLink>
    {
        public override bool Equals(NodeLink a, NodeLink b)
        {
            return a.Source.Equals(b.Source) && a.Target.Equals(b.Target) ||
             a.Source.Equals(b.Target) && a.Target.Equals(b.Source);
        }

        public override int GetHashCode(NodeLink a_Link)
        {
            int hCode = 
                Math.Min(a_Link.Source.x, a_Link.Target.x) ^
                Math.Min(a_Link.Source.y, a_Link.Target.y) ^
                Math.Max(a_Link.Source.x, a_Link.Target.x) ^
                Math.Max(a_Link.Source.y, a_Link.Target.y);
            return hCode.GetHashCode();
        }
    }
}
