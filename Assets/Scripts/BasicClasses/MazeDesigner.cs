using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BasicClasses
{
    internal class MazeDesigner
    {
        private static readonly Vector2Int[] s_Directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        private static int m_ColumnCount;
        private static int m_RowCount;
        private static int m_ColumnPathCount;
        private static int m_RowPathCount;

        private static System.Random m_Random;

        private static Stack<Vector2Int> m_VisitedNodes;
        private static int m_TotalPathCount;
        private static GridGraph m_GridGraph;
        private static int m_MaxPathDepth;

        public static void Generate(int a_ColumnCount, int a_RowCount, System.Random a_Random,
            out GridGraph o_GridGraph, out List<NodeLink> o_Links)
        {
            m_Random = a_Random;
            m_RowCount = a_RowCount;
            m_ColumnCount = a_ColumnCount;

            Init();

            o_GridGraph = new GridGraph(m_ColumnCount, m_RowCount);
            o_Links = new List<NodeLink>();

            int x = m_Random.Next(0, m_ColumnPathCount);
            int y = m_Random.Next(0, m_RowPathCount);

            var l_Source = new Vector2Int(x, y);
            do
            {
                int l_Depth = 0;
                if (!HasVisited(l_Source))
                {
                    m_VisitedNodes.Push(l_Source);
                }
                while (l_Depth <= m_MaxPathDepth)
                {
                    var l_HasUpdated = false;
                    var l_ShuffledDirections = GetShuffledDirections();
                    foreach (var l_Direction in l_ShuffledDirections)
                    {
                        var l_Target = l_Source + l_Direction;
                        if (IsAvailable(l_Target))
                        {
                            m_VisitedNodes.Push(l_Target);
                            var l_Link = new NodeLink(l_Source, l_Target);
                            o_Links.Add(l_Link);
                            o_GridGraph.AddLink(l_Link);
                            l_Source = l_Target;
                            l_Depth++;
                            l_HasUpdated = true;
                            break;
                        }
                    }
                    if (!l_HasUpdated)
                    { break; }
                }
                int l_RandomIndex = m_Random.Next(0, m_VisitedNodes.Count - 1);
                l_Source = m_VisitedNodes.ElementAt(l_RandomIndex);
            } while (o_GridGraph.Count < m_TotalPathCount);
        }
        private static void Init()
        {
            m_VisitedNodes = new Stack<Vector2Int>();

            m_MaxPathDepth = (int)(Mathf.Sqrt(m_ColumnCount * m_RowCount) * 1.2f);

            m_ColumnPathCount = m_ColumnCount - 1;
            m_RowPathCount = m_RowCount - 1;

            m_TotalPathCount = ((m_ColumnPathCount * m_RowCount) + (m_ColumnCount * m_RowPathCount)) - (m_ColumnPathCount * m_RowPathCount);
        }
        private static Vector2Int[] GetShuffledDirections()
        {
            var l_ShuffledDirections = s_Directions;
            for (var i = 0; i < l_ShuffledDirections.Length; i++)
            {
                var l_RandomIndex = m_Random.Next(0, 3);
                var temp = l_ShuffledDirections[i];
                l_ShuffledDirections[i] = l_ShuffledDirections[l_RandomIndex];
                l_ShuffledDirections[l_RandomIndex] = temp;
            }
            return l_ShuffledDirections;
        }
        private static bool IsAvailable(Vector2Int vec)
        {
            return IsInRange(vec) && !HasVisited(vec);
        }
        private static bool IsInRange(Vector2Int vec)
        {
            return vec.x >= 0 && vec.y >= 0 && vec.x < m_ColumnCount && vec.y < m_RowCount;
        }
        private static bool HasVisited(Vector2Int vec)
        {
            return m_VisitedNodes.Any((el) => { return el.Equals(vec); });
        }
    }
}
