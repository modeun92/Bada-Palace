using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class TestClassCoroutine : MonoBehaviour
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
        //public static void ExecuteThis(GameObject obj, DelegatedBoolean isReady, DelegatedFunction function)
        //{
        //    obj.GetComponent<MonoBehaviour>().StartCoroutine(ActivateToggleCoroutine(isReady, function));
        //}
        //private static WaitUntil WaitUntilInitialized(DelegatedBoolean isReady)
        //{
        //    return new WaitUntil(() => isReady());
        //}
        //private static IEnumerator ActivateToggleCoroutine(DelegatedBoolean isReady, DelegatedFunction function)
        //{
        //    yield return WaitUntilInitialized(isReady);
        //    function();
        //}
    }
}
public delegate void DelegatedFunction();
public delegate bool DelegatedBoolean();
