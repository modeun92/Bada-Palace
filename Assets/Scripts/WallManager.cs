using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public static readonly float CONTENT_LENGTH = 0.988f;
    public static readonly float BORDER_LENGTH = 0.036f;
    public bool IsActivated;

    private Transform mTransform;
    private SpriteRenderer mRenderer;
    private BoxCollider2D mCollider;
    private bool mIsInitialized = false;
    private object locker = new object();
    // Start is called before the first frame update
    void Start()
    {
        CheckInit();
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void ActivateToggle(bool aToggle)
    {
        IsActivated = aToggle;
        CheckInit();
        mRenderer.enabled = aToggle;
        mCollider.enabled = aToggle;
    }
    public void SetConfig(float aScale, int aWidth, int aHeight)
    {
        SetScale(aScale);
        SetSize(aWidth, aHeight);
    }
    public void SetScale(float aScale)
    {
        CheckInit();
        mTransform.localScale = new Vector3(aScale, aScale, 1);
    }
    public void SetSize(int aColumnCount, int aRowCount)
    {
        float lWidth = CONTENT_LENGTH * aColumnCount;
        float lHeight = CONTENT_LENGTH * aRowCount;
        SetSize(lWidth, lHeight);
    }
    public void SetSize(float aWidth, float aHeight)
    {
        CheckInit();
        mCollider.size = new Vector2(aWidth, aHeight);
        mRenderer.size = new Vector2(aWidth + BORDER_LENGTH, aHeight + BORDER_LENGTH);
    }
    private void CheckInit()
    {
        lock (locker)
        {
            if (!mIsInitialized)
            {
                Initiate();
            }
        }
    }
    private void Initiate()
    {
        mTransform = GetComponent<Transform>();
        mRenderer = GetComponent<SpriteRenderer>();
        mCollider = GetComponent<BoxCollider2D>();
        mIsInitialized = true;
        IsActivated = true;
    }
    //public static readonly float CONTENT_LENGTH = 0.988f;
    //public static readonly float BORDER_LENGTH = 0.036f;
    //public bool IsActivated;

    //private Transform mTransform;
    //private SpriteRenderer mRenderer;
    //private BoxCollider2D mCollider;
    //private bool mIsInitialized = false;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    mTransform = GetComponent<Transform>();
    //    mRenderer = GetComponent<SpriteRenderer>();
    //    mCollider = GetComponent<BoxCollider2D>();
    //    IsActivated = true;
    //    mIsInitialized = true;
    //}
    //// Update is called once per frame
    //void Update()
    //{
    //}
    //public void ActivateToggle(bool aToggle)
    //{
    //    IsActivated = aToggle;
    //    StartCoroutine(ActivateToggleCoroutine(aToggle));
    //}
    //public void SetConfig(float aScale, int aWidth, int aHeight)
    //{
    //    SetScale(aScale);
    //    SetSize(aWidth, aHeight);
    //}
    //public void SetScale(float aScale)
    //{
    //    StartCoroutine(SetScaleCoroutine(aScale));
    //}
    //public void SetSize(int aColumnCount, int aRowCount)
    //{
    //    float lWidth = CONTENT_LENGTH * aColumnCount;
    //    float lHeight = CONTENT_LENGTH * aRowCount;
    //    SetSize(lWidth, lHeight);
    //}
    //public void SetSize(float aWidth, float aHeight)
    //{
    //    StartCoroutine(SetSizeCoroutine(aWidth, aHeight));
    //}
    //private IEnumerator SetScaleCoroutine(float aScale)
    //{
    //    yield return WaitUntilInitialized();
    //    mTransform.localScale = new Vector3(aScale, aScale, 1);
    //}
    //private IEnumerator ActivateToggleCoroutine(bool aToggle)
    //{
    //    yield return WaitUntilInitialized();
    //    mRenderer.enabled = aToggle;
    //    mCollider.enabled = aToggle;
    //}
    //private IEnumerator SetSizeCoroutine(float aWidth, float aHeight)
    //{
    //    yield return WaitUntilInitialized();
    //    mCollider.size = new Vector2(aWidth, aHeight);
    //    mRenderer.size = new Vector2(aWidth + BORDER_LENGTH, aHeight + BORDER_LENGTH);
    //}
    //private WaitUntil WaitUntilInitialized()
    //{
    //    return new WaitUntil(() => mIsInitialized);
    //}
}
