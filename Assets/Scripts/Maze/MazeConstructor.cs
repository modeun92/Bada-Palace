using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Maze;
using System.Collections;
using Assets.Scripts.BasicClasses;
using System.Linq;

namespace Assets.Scripts.Maze
{
    public class MazeConstructor : MonoBehaviour
    {
        [SerializeField]
        private GameObject Source;
        [SerializeField]
        private GameObject Target;
        [SerializeField]
        private GameObject Wall;
        [SerializeField]
        private GameObject RightDark;
        [SerializeField]
        private GameObject TopDark;

        private const int MAXIMUM_LENGTH = 20;
        private const int MINIMUM_SIZE = 5;

        private int m_Level;
        private int m_Stage;
        private System.Random m_Random;

        private int m_RowCount;
        private int m_ColumnCount;

        public int LongWallCount = 10;
        public float WallScale = 1f;

        private SpriteRenderer mRenderer;
        private Transform mOffsetTransform;
        private Transform mGroundTransform;

        private float mWallThickness;
        private float mSurroundingWallWidth;
        private float mSurroundingWallHeight;
        private float mWallLength;
        private float mMaxGroundWidth;
        private float mMaxGroundHeight;

        private Matrix4x4 m_FixedWallTransform;
        private Matrix4x4 m_VariableWallTransform;
        private int maxPathDepth;
        private int defaultCount = 5;

        private Stack<GameObject> mTotalFixedObjects;
        private Dictionary<NodeLink, GameObject> mTotalVariableWalls;
        private Stack<GameObject> mOpenObjects;

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void PutAside(bool state)
        {
            var x = 0f;
            var y = 0f;
            if (state)
            {
                x = 10000f;
                y = 10000f;
            }
            mGroundTransform.localPosition = new Vector3(x, y, 0f);
        }
        private void InitRandom()
        {
            int l_SeedNumber = (m_Level * 10000) + m_Stage;
            if (l_SeedNumber % 2 == 0)
            {
                l_SeedNumber += 777;
            }
            m_Random = new System.Random(l_SeedNumber);
        }
        private void Init()
        {
            mGroundTransform = GetComponent<Transform>();
            mOffsetTransform = mGroundTransform.GetChild(0);
            mRenderer = GetComponent<SpriteRenderer>();


            BuildBasement(MAXIMUM_LENGTH, MAXIMUM_LENGTH);
        }
        private void ReinitMazeInfo()
        {
            Debug.Log($"MAZE : ReinitMazeInfo Level = {m_Level}, Stage = {m_Stage}.");
            InitRandom();
            SetSize();
            CloseOpenWalls();
        }
        private void SetSize()
        {
            var l_Increment = (m_Level - 1) / 2f;
            m_RowCount = MINIMUM_SIZE + (int)Mathf.Round(l_Increment);
            m_ColumnCount = MINIMUM_SIZE + (int)Mathf.Ceil(l_Increment);
        }
        private void BuildBasement(int aColumnLength, int aRowLength)
        {
            mTotalFixedObjects = new Stack<GameObject>();
            mTotalVariableWalls = new Dictionary<NodeLink, GameObject>(new NodeLinkComparer());
            mOpenObjects = new Stack<GameObject>();

            CalculateBasicInfos(aColumnLength, aRowLength);
            BuildGround();
            BuildBaseWalls(aColumnLength, aRowLength);
            BuildDarkness();
            AdjustDarkness(aColumnLength, aRowLength);
        }
        private void CalculateBasicInfos(int aColumnLength, int aRowLength)
        {
            mWallThickness = WallManager.CONTENT_LENGTH;
            mWallLength = mWallThickness * LongWallCount;

            mMaxGroundWidth = (mWallLength * aColumnLength) + (mWallThickness * (aColumnLength - 1));
            mMaxGroundHeight = (mWallLength * aRowLength) + (mWallThickness * (aRowLength - 1));

            float wingSize = mWallThickness * 2;
            mSurroundingWallWidth = mMaxGroundWidth + wingSize;
            mSurroundingWallHeight = mMaxGroundHeight + wingSize;
            var l_WallThicknessHalf = mWallThickness / 2f;

            var l_WallScaleUnit = mWallLength + mWallThickness;
            var l_FixedWallOffset = (-l_WallThicknessHalf) + l_WallScaleUnit;
            var l_VariableWallOffset = (-l_WallThicknessHalf) + l_WallScaleUnit / 2f;

            m_FixedWallTransform = new Matrix4x4()
            {
                m00 = l_WallScaleUnit,
                m11 = l_WallScaleUnit,
                m02 = l_FixedWallOffset,
                m12 = l_FixedWallOffset
            };
            m_VariableWallTransform = new Matrix4x4()
            {
                m00 = l_WallScaleUnit,
                m11 = l_WallScaleUnit,
                m02 = l_VariableWallOffset,
                m12 = l_VariableWallOffset
            };
        }
        private void BuildGround()
        {
            mRenderer.size = new Vector2(mMaxGroundWidth, mMaxGroundHeight);
            var xOffset = -mMaxGroundWidth / 2f;
            var yOffset = -mMaxGroundHeight / 2f;
            mOffsetTransform.localPosition = new Vector3(xOffset, yOffset, 0f);
        }
        private void BuildDarkness()
        {
            TopDark.GetComponent<SpriteRenderer>().size = new Vector2(mSurroundingWallWidth, 10f);
            RightDark.GetComponent<SpriteRenderer>().size = new Vector2(10f, mSurroundingWallHeight);
        }
        private void BuildBaseWalls(int a_ColumnLength, int a_RowLength)
        {
            var xTopBottom = mSurroundingWallWidth / 2f - mWallThickness;
            var yLeftRight = mSurroundingWallHeight / 2f - mWallThickness;

            var lYBottomXLeft = -(mWallThickness / 2f);

            mTotalFixedObjects.Push(InstantiateWall(xTopBottom, mMaxGroundHeight - lYBottomXLeft, mSurroundingWallWidth, mWallThickness));//top
            mTotalFixedObjects.Push(InstantiateWall(xTopBottom, lYBottomXLeft, mSurroundingWallWidth, mWallThickness));//bottom

            mTotalFixedObjects.Push(InstantiateWall(lYBottomXLeft, yLeftRight, mWallThickness, mSurroundingWallHeight));//left
            mTotalFixedObjects.Push(InstantiateWall(mMaxGroundWidth - lYBottomXLeft, yLeftRight, mWallThickness, mSurroundingWallHeight));//right

            BuildFixedJointWalls(a_ColumnLength, a_RowLength);
            BuildVariableWalls(a_ColumnLength, a_RowLength);
        }
        private void BuildFixedJointWalls(int a_ColumnLength, int a_RowLength)
        {
            var lColumnLimit = a_ColumnLength - 1;
            var lRowLimit = a_RowLength - 1;
            for (int lRowIndex = 0; lRowIndex < lRowLimit; lRowIndex++) // vertical
            {
                for (int lColumnIndex = 0; lColumnIndex < lColumnLimit; lColumnIndex++)
                {
                    mTotalFixedObjects.Push(InstantiateWall(lColumnIndex, lRowIndex));
                }
            }
        }
        private void BuildVariableWalls(int aColumnCount, int aRowCount)
        {
            for (int lRowIndex = 0; lRowIndex < aRowCount; lRowIndex++) // horizontal
            {
                for (int lColumnIndex = 0; lColumnIndex < aColumnCount; lColumnIndex++)
                {
                    var l_Source = new Vector2Int(lColumnIndex, lRowIndex);
                    if (lRowIndex < aRowCount - 1)
                    {
                        var l_Link = new NodeLink(l_Source, l_Source + Vector2Int.up);
                        mTotalVariableWalls.Add(l_Link, InstantiateWall(l_Link));
                    }
                    if (lColumnIndex < aColumnCount - 1)
                    {
                        var l_Link = new NodeLink(l_Source, l_Source + Vector2Int.right);
                        mTotalVariableWalls.Add(l_Link, InstantiateWall(l_Link));
                    }
                }
            }
        }
        private GameObject InstantiateWall(NodeLink a_Link)
        {
            int l_ColumnLength = 1;
            int l_RowLength = 1;
            if (a_Link.IsHorizontal())
            {
                l_RowLength = LongWallCount;
            }
            else if (a_Link.IsVertical())
            {
                l_ColumnLength = LongWallCount;
            }
            var instance = Instantiate(Wall, new Vector3(), Quaternion.identity, mOffsetTransform);
            instance.GetComponent<Transform>().localPosition = m_VariableWallTransform * GetMiddlePoint(a_Link);
            instance.GetComponent<WallManager>().SetSize(l_ColumnLength, l_RowLength);
            return instance;
        }
        private GameObject InstantiateWall(int x, int y)
        {
            Vector3 l_Vec = new Vector3(x, y, 1);
            int l_ColumnLength = 1;
            int l_RowLength = 1;
            var instance = Instantiate(Wall, new Vector3(), Quaternion.identity, mOffsetTransform);
            instance.GetComponent<Transform>().localPosition = m_FixedWallTransform * l_Vec;
            instance.GetComponent<WallManager>().SetSize(l_ColumnLength, l_RowLength);
            return instance;
        }
        private GameObject InstantiateWall(float aX, float aY, float aWidth, float aHeight)
        {
            var instance = Instantiate(Wall, new Vector3(), Quaternion.identity, mOffsetTransform);
            instance.GetComponent<Transform>().localPosition = new Vector3(aX, aY, 0);
            instance.GetComponent<WallManager>().SetSize(aWidth, aHeight);
            return instance;
        }

        private void AdjustDarkness(int aColumnLength, int aRowLength)
        {
            var lWidth = (mWallLength * aColumnLength) + (mWallThickness * (aColumnLength - 1));
            var lHeight = (mWallLength * aRowLength) + (mWallThickness * (aRowLength - 1));
            TopDark.GetComponent<Transform>().localPosition = new Vector3(mMaxGroundWidth / 2f, lHeight + 5f + mWallThickness);
            RightDark.GetComponent<Transform>().localPosition = new Vector3(lWidth + 5f + mWallThickness, mMaxGroundHeight / 2f);
        }
        public void Generate(int aLevel, int aStage)
        {
            m_Level = aLevel;
            m_Stage = aStage;
            ReinitMazeInfo();
            MazeDesigner.Generate(m_ColumnCount, m_RowCount, m_Random,
                out var l_GridGraph, out var l_Links);
            OpenWalls(l_Links);
            SetMission(l_GridGraph);
        }

        private void OpenWalls(List<NodeLink> a_Links)
        {
            foreach (var l_Link in a_Links)
            {
                OpenWall(l_Link);
            }
            Debug.Log($"OpenWalls done.");
        }
        private void OpenWall(NodeLink a_Link)
        {
            GameObject wall;
            if (mTotalVariableWalls.TryGetValue(a_Link, out wall))
            {
                wall.gameObject.SetActive(false);
                mOpenObjects.Push(wall);
            }
            else
            {
                Debug.Log($"Wall doesn't exist! : {a_Link}");
            }
        }
        private void CloseOpenWalls()
        {
            while (mOpenObjects.Count != 0)
            {
                var wall = mOpenObjects.Pop();
                wall.gameObject.SetActive(true);
            }
        }
        private void SetMission(GridGraph a_GridGraph)
        {
            Vector2Int[] l_DeadEnds = a_GridGraph.GetDeadEnds().ToArray();
            var l_SourceIndex = m_Random.Next(0, l_DeadEnds.Length - 1);
            
            var l_OtherDeadEnds = a_GridGraph.FindDeadEndsFrom(l_DeadEnds[l_SourceIndex]).OrderBy((x) => x.Item2);
            var l_Count = l_OtherDeadEnds.Count();
            int l_TargetIndex = m_Random.Next(l_Count / 2, l_Count - 1);
            var l_Target = l_OtherDeadEnds.ElementAt(l_TargetIndex).Item1;
            
            LocateObject(Source, l_DeadEnds[l_SourceIndex]);
            LocateObject(Target, l_Target);
        }
        private void LocateObject(GameObject a_Object, Vector2Int vec)
        {
            var l_Vec = m_VariableWallTransform * new Vector3(vec.x, vec.y, 1);
            a_Object.GetComponent<Transform>().localPosition = l_Vec;
        }
        private Vector3 GetMiddlePoint(NodeLink aPath)
        {
            Vector2 l_MiddlePoint = aPath.Source + aPath.Target;
            l_MiddlePoint *= 0.5f;
            return new Vector3(l_MiddlePoint.x, l_MiddlePoint.y, 1);
        }
        private float GetAxis(int aIndex)
        {
            return (mWallLength * aIndex) + (mWallThickness * aIndex) + (mWallLength / 2f);
        }
    }
}