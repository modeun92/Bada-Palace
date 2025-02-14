using Assets.Scripts.BasicClasses;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
namespace Assets.Scripts
{
    public class GridGraph
    {
        public int Count { get => m_Count; set { m_Count = value; } }
        private List<Vector2Int>[,] mGrid;
        private int mColumnCount;
        private int mRowCount;
        private int m_Count;
        public List<Vector2Int> this[Vector2Int vec] => this[vec.x, vec.y];
        public List<Vector2Int> this[int x, int y] => mGrid[y, x];
        public GridGraph(int aColumnCount, int aRowCount)
        {
            Init(aColumnCount, aRowCount);
        }
        public GridGraph(int aColumnCount, int aRowCount, List<NodeLink> aLinks)
        {
            Init(aColumnCount, aRowCount);
            AddLinks(aLinks);
        }
        public bool TryGetValue(Vector2Int a_Vec, out List<Vector2Int> a_Value)
        {
            return TryGetValue(a_Vec.x, a_Vec.y, out a_Value);
        }
        public bool TryGetValue(int aColumn, int aRow, out List<Vector2Int> a_Value)
        {
            var l_IsInRange = aColumn < mColumnCount && aRow < mRowCount;
            if (l_IsInRange)
            {
                a_Value = this[aColumn, aRow];
            }
            else
            {
                a_Value = null;
            }
            return true;
        }
        public List<Vector2Int> GetDeadEnds()
        {
            var l_DeadEnds = new List<Vector2Int>();
            for (int lRow = 0; lRow < mRowCount; lRow++)
            {
                for (int lColumn = 0; lColumn < mColumnCount; lColumn++)
                {
                    if (this[lColumn, lRow].Count == 1)
                    {
                        l_DeadEnds.Add(new Vector2Int(lColumn, lRow));
                    }
                }
            }
            return l_DeadEnds;
        }
        public void AddLinks(List<NodeLink> aLinks)
        {
            foreach (var link in aLinks)
            {
                AddLink(link);
            }
        }
        public void AddLink(NodeLink a_Link)
        {
            AddLink(a_Link.Source, a_Link.Target);
        }
        public void AddLink(Vector2Int a_Source, Vector2Int a_Target)
        {
            if (TryGetValue(a_Source, out var l_Source))
            {
                l_Source.Add(a_Target);
            }
            if (TryGetValue(a_Target, out var l_Target))
            {
                l_Target.Add(a_Source);
            }
            m_Count++;
        }
        public List<(Vector2Int, int)> FindDeadEndsFrom(Vector2Int a_Node)
        {
            var l_Visited = new Stack<Vector2Int>();
            var l_DeadEnds = new List<(Vector2Int, int)>();
            int l_Length = 0;
            FindDeadEndsFrom(a_Node, l_Length, l_Visited, l_DeadEnds);
            return l_DeadEnds;
        }
        private void FindDeadEndsFrom(Vector2Int a_Node, int a_Length, Stack<Vector2Int> a_Visited, List<(Vector2Int, int)> a_DeadEnds)
        {
            if (!a_Visited.Contains(a_Node))
            {
                a_Visited.Push(a_Node);
                var l_NodeLinks = this[a_Node];
                if (a_Length > 0 && l_NodeLinks.Count == 1) // undiscovered and the dead-end
                {
                    a_DeadEnds.Add((a_Node, a_Length));
                }
                else
                {
                    ++a_Length;
                    foreach (var l_NodeLink in l_NodeLinks)
                    {
                        FindDeadEndsFrom(l_NodeLink, a_Length, a_Visited, a_DeadEnds);
                    }
                }
            }
        }
        private void Init(int aColumnCount, int aRowCount)
        {
            mColumnCount = aColumnCount;
            mRowCount = aRowCount;
            mGrid = new List<Vector2Int>[mRowCount, mColumnCount];
            for (int lRow = 0; lRow < mRowCount; lRow++)
            {
                for (int lColumn = 0; lColumn < mColumnCount; lColumn++)
                {
                    mGrid[lRow, lColumn] = new List<Vector2Int>();
                }
            }
        }
    }
}
