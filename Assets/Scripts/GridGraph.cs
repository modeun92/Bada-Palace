using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts
{
    class GridGraph
    {
        private List<Vector2D>[,] mGrid;
        private int mColumnCount;
        private int mRowCount;
        public GridGraph(int aColumnCount, int aRowCount)
        {
            Init(aColumnCount, aRowCount);
        }
        public GridGraph(int aColumnCount, int aRowCount, List<Link> aLinks)
        {
            Init(aColumnCount, aRowCount);
            InsertLinks(aLinks);
        }
        public List<Vector2D> GetLinks(int aColumn, int aRow)
        {
            return mGrid[aColumn, aRow];
        }
        public List<Vector2D> GetDeadEnds()
        {
            var lDeadEnds = new List<Vector2D>();
            for (int lRow = 0; lRow < mRowCount; lRow++)
            {
                for (int lColumn = 0; lColumn < mColumnCount; lColumn++)
                {
                    if (mGrid[lRow, lColumn].Count == 1)
                    {
                        lDeadEnds.Add(new Vector2D(lColumn, lRow));
                    }
                }
            }
            return lDeadEnds;
        }
        public void InsertLinks(List<Link> aLinks)
        {
            foreach (var link in aLinks)
            {
                GetGridValue(link.Start).Add(new Vector2D(link.End));
                GetGridValue(link.End).Add(new Vector2D(link.Start));
            }
        }
        private void Init(int aColumnCount, int aRowCount)
        {
            mColumnCount = aColumnCount;
            mRowCount = aRowCount;
            mGrid = new List<Vector2D>[mRowCount, mColumnCount];
            for (int lRow = 0; lRow < mRowCount; lRow++)
            {
                for (int lColumn = 0; lColumn < mColumnCount; lColumn++)
                {
                    mGrid[lRow, lColumn] = new List<Vector2D>();
                }
            }
        }
        private List<Vector2D> GetGridValue(Vector2D vec)
        {
            return mGrid[vec.Y, vec.X];
        }
    }
}
