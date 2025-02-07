using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System.Linq;
using System.Diagnostics;
public class MazeGenerator : MonoBehaviour
{
    public GameObject DeparturePoint;
    public GameObject Destination;
    public GameObject Wall;
    public int LongWallCount = 10;
    public float WallScale = 1f;
    public GameObject RightDark;
    public GameObject TopDark;

    private SpriteRenderer mRenderer;
    private Transform mOffsetTransform;
    private Transform mGroundTransform;

    private static readonly int MAXIMUM_LENGTH = 20;
    public static readonly int MAXIMUM_LEVEL = 31;
    public static readonly int MAXIMUM_STAGE = 20;

    private int mColumnCount;
    private int mRowCount;
    private int mRowPathCount;
    private int mColumnPathCount;

    private float mWallThickness;
    private float mSurroundingWallWidth;
    private float mSurroundingWallHeight;
    private float mWallLength;
    private float mMaxGroundWidth;
    private float mMaxGroundHeight;

    private int maxPathDepth;
    private int defaultCount = 5;

    private int mLevel;
    private int mStage;

    private Stack<GameObject> mTotalFixedObjects;
    private Dictionary<Link, GameObject> mTotalVariableWalls;
    private Stack<GameObject> mOpenObjects;
    private GridGraph mGridGraph;

    private Stack<Vector2D> mVisitedNodes;
    private List<Link> mMazePaths;
    private int mTotalPathCount;
    private System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        InitGenerator();
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
    private void InitGenerator()
    {
        mGroundTransform = GetComponent<Transform>();
        mOffsetTransform = mGroundTransform.GetChild(0);
        mRenderer = GetComponent<SpriteRenderer>();

        BuildBasement(MAXIMUM_LENGTH, MAXIMUM_LENGTH);
    }
    private void BuildBasement(int aColumnCount, int aRowCount)
    {
        mTotalFixedObjects = new Stack<GameObject>();
        mTotalVariableWalls = new Dictionary<Link, GameObject>(new Link());
        mOpenObjects = new Stack<GameObject>();

        CalculateBasicInfos(aColumnCount, aRowCount);
        BuildGround();
        BuildBaseWalls(aColumnCount, aRowCount);
        BuildDarkness();
    }
    private void CalculateBasicInfos(int aColumnCount, int aRowCount)
    {
        mWallThickness = WallManager.CONTENT_LENGTH;
        mWallLength = mWallThickness * LongWallCount;

        mMaxGroundWidth = (mWallLength * aColumnCount) + (mWallThickness * (aColumnCount - 1));
        mMaxGroundHeight = (mWallLength * aRowCount) + (mWallThickness * (aRowCount - 1));

        float wingSize = mWallThickness * 2;
        mSurroundingWallWidth = mMaxGroundWidth + wingSize;
        mSurroundingWallHeight = mMaxGroundHeight + wingSize;
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
    private void BuildBaseWalls(int aColumnCount, int aRowCount)
    {
        var xTopBottom = mSurroundingWallWidth / 2f - mWallThickness;
        var yLeftRight = mSurroundingWallHeight / 2f - mWallThickness;

        var lYBottomXLeft = -(mWallThickness / 2f);

        mTotalFixedObjects.Push(InstantiateWall(xTopBottom, mMaxGroundHeight - lYBottomXLeft, mSurroundingWallWidth, mWallThickness));//top
        mTotalFixedObjects.Push(InstantiateWall(xTopBottom, lYBottomXLeft, mSurroundingWallWidth, mWallThickness));//bottom

        mTotalFixedObjects.Push(InstantiateWall(lYBottomXLeft, yLeftRight, mWallThickness, mSurroundingWallHeight));//left
        mTotalFixedObjects.Push(InstantiateWall(mMaxGroundWidth - lYBottomXLeft, yLeftRight, mWallThickness, mSurroundingWallHeight));//right

        BuildFixedJointWalls(aColumnCount, aRowCount);
        BuildVariableWalls(aColumnCount, aRowCount);
    }
    private void BuildFixedJointWalls(int aColumnCount, int aRowCount)
    {
        var lColumnLimit = aColumnCount - 1;
        var lRowLimit = aRowCount - 1;
        for (int lRowIndex = 0; lRowIndex < lRowLimit; lRowIndex++) // horizontal
        {
            for (int lColumnIndex = 0; lColumnIndex < lColumnLimit; lColumnIndex++)
            {
                var jointPath = new Link(lColumnIndex, lRowIndex, lColumnIndex + 1, lRowIndex + 1);
                mTotalFixedObjects.Push(InstantiateWall(jointPath));
            }
        }
    }
    private void BuildVariableWalls(int aColumnCount, int aRowCount)
    {
        for (int lRowIndex = 0; lRowIndex < aRowCount; lRowIndex++) // horizontal
        {
            var lColumnLimit = aColumnCount - 1;
            var lRowLimit = aRowCount - 1;
            for (int lColumnIndex = 0; lColumnIndex < aColumnCount; lColumnIndex++)
            {
                if (lRowIndex < lRowLimit)
                {
                    var link = new Link(lColumnIndex, lRowIndex, lColumnIndex, lRowIndex + 1);
                    mTotalVariableWalls.Add(link, InstantiateWall(link));
                }
                if (lColumnIndex < lColumnLimit)
                {
                    var link = new Link(lColumnIndex, lRowIndex, lColumnIndex + 1, lRowIndex);
                    mTotalVariableWalls.Add(link, InstantiateWall(link));
                }
            }
        }
    }
    private GameObject InstantiateWall(Link aPath)
    {
        int lColumnCount = 1;
        int lRowCount = 1;
        if (aPath.IsParallel(true))
        {
            lRowCount = LongWallCount;
        }
        else if (aPath.IsParallel(false))
        {
            lColumnCount = LongWallCount;
        }
        return InstantiateWall(aPath, lColumnCount, lRowCount);
    }
    private GameObject InstantiateWall(Link aPath, int aColumnCount, int aRowCount)
    {
        var instance = Instantiate(Wall, new Vector3(), Quaternion.identity, mOffsetTransform);
        instance.GetComponent<Transform>().localPosition = GetAxis(aPath);
        instance.GetComponent<WallManager>().SetSize(aColumnCount, aRowCount);
        return instance;
    }
    private GameObject InstantiateWall(float aX, float aY, float aWidth, float aHeight)
    {
        var instance = Instantiate(Wall, new Vector3(), Quaternion.identity, mOffsetTransform);
        instance.GetComponent<Transform>().localPosition = new Vector3(aX, aY, 0);
        instance.GetComponent<WallManager>().SetSize(aWidth, aHeight);
        return instance;
    }

    private void AdjustDarkness(int aColumnCount, int aRowCount)
    {
        var lWidth = (mWallLength * aColumnCount) + (mWallThickness * (aColumnCount - 1));
        var lHeight = (mWallLength * aRowCount) + (mWallThickness * (aRowCount - 1));
        TopDark.GetComponent<Transform>().localPosition = new Vector3(mMaxGroundWidth / 2f, lHeight + 5f + mWallThickness);
        RightDark.GetComponent<Transform>().localPosition = new Vector3(lWidth + 5f + mWallThickness, mMaxGroundHeight / 2f);
    }

    public void Generate(int aLevel, int aStage)
    {
        mLevel = aLevel;
        mStage = aStage;

        SetRandomWithSeedNumber();

        var increment = (mLevel - 1) / 2f;

        mColumnCount = defaultCount + (int)Mathf.Round(increment);
        mRowCount = defaultCount + (int)Mathf.Ceil(increment);

        AdjustDarkness(mColumnCount, mRowCount);

        maxPathDepth = (int)(Mathf.Sqrt(mColumnCount * mRowCount) * 1.2f);

        mColumnPathCount = mColumnCount - 1;
        mRowPathCount = mRowCount - 1;

        mTotalPathCount = ((mColumnPathCount * mRowCount) + (mColumnCount * mRowPathCount)) - (mColumnPathCount * mRowPathCount);

        mVisitedNodes = new Stack<Vector2D>();
        mMazePaths = new List<Link>();

        
        Generate();
    }
    private void SetRandomWithSeedNumber()
    {
        int seedNumber = (mLevel * 10000) + mStage;
        if (seedNumber % 2 == 0)
        {
            seedNumber += 777;
        }
        random = new System.Random(seedNumber);
    }
    private void Generate()
    {
        mVisitedNodes.Clear();
        mMazePaths.Clear();
        CloseWalls();

        int x = random.Next(0, mColumnPathCount);
        int y = random.Next(0, mRowPathCount);
        var vec = new Vector2D(x, y);
        while (mMazePaths.Count < mTotalPathCount)
        {
            int depth = 0;
            if (!Exists(vec))
            {
                mVisitedNodes.Push(vec);
            }
            while (depth <= maxPathDepth)
            {
                var roundVecs = vec.GetSuffledRoundVector2Ds(random);
                var hasUpdated = false;
                foreach (var roundVec in roundVecs)
                {
                    if (Available(roundVec))
                    {
                        mVisitedNodes.Push(roundVec);
                        mMazePaths.Add(new Link(vec, roundVec));
                        vec = roundVec;
                        depth++;
                        hasUpdated = true;
                        break;
                    }
                }
                if (!hasUpdated)
                { break; }
            }
            int ranIdx = random.Next(0, mVisitedNodes.Count - 1);
            vec = mVisitedNodes.ElementAt(ranIdx);
        }
        foreach (var lPath in mMazePaths)
        {
            OpenWall(lPath);
        }
        mGridGraph = new GridGraph(mColumnCount, mRowCount, mMazePaths);

        LocateDeparturePointAndDestinationRandomly();
    }
    private void OpenWall(Link aPath)
    {
        GameObject wall;
        if (mTotalVariableWalls.TryGetValue(aPath, out wall))
        {
            wall.GetComponent<WallManager>().ActivateToggle(false);
            mOpenObjects.Push(wall);
        }
        else
        {
        }
    }
    private void CloseWalls()
    { 
        while (mOpenObjects.Count != 0)
        {
            var wall = mOpenObjects.Pop();
            wall.GetComponent<WallManager>().ActivateToggle(true);
        }
    }
    private bool Exists(Vector2D vec)
    {
        return mVisitedNodes.Any((el) => { return el.Equals(vec); });
    }
    private bool Available(Vector2D vec)
    {
        var isInRange = (vec.X >= 0 && vec.Y >= 0 && vec.Y < mRowCount && vec.X < mColumnCount);
        return isInRange && !Exists(vec);
    }
    private void LocateDeparturePointAndDestinationRandomly()
    {
        var deadEnds = mGridGraph.GetDeadEnds().ToArray();
        var destinationIndex = random.Next(0, deadEnds.Length - 1);

        var departurePointIndex = destinationIndex;
        while (departurePointIndex == destinationIndex)
        {
            departurePointIndex = random.Next(0, deadEnds.Length - 1);
        }

        LocateDestination(deadEnds[destinationIndex]);
        LocateDeparturePoint(deadEnds[departurePointIndex]);
    }
    private void LocateDeparturePoint(Vector2D vec)
    {
        var x = GetAxis(vec.X);
        var y = GetAxis(vec.Y);
        DeparturePoint.GetComponent<Transform>().localPosition = new Vector3(x, y, 0f);
    }
    private void LocateDestination(Vector2D vec)
    {
        var x = GetAxis(vec.X);
        var y = GetAxis(vec.Y);
        Destination.GetComponent<Transform>().localPosition = new Vector3(x, y, 0f);
    }
    private Vector3 GetAxis(Link aPath)
    {
        var lX = (GetAxis(aPath.Start.X) + GetAxis(aPath.End.X)) / 2f;
        var lY = (GetAxis(aPath.Start.Y) + GetAxis(aPath.End.Y)) / 2f;
        return new Vector3(lX, lY, 0f);
    }
    private float GetAxis(int aIndex)
    {
        return (mWallLength * aIndex) + (mWallThickness * aIndex) + (mWallLength / 2f);
    }
}

