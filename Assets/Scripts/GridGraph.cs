using Assets.Scripts.BasicClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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
        public List<Vector2Int> this[int x, int y] => mGrid[x, y];
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
            var l_Result = aColumn < mColumnCount && aRow < mRowCount;
            if (l_Result)
            {
                a_Value = this[aRow, aColumn];
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
                    if (mGrid[lRow, lColumn].Count == 1)
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
