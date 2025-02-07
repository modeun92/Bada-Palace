using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickResizer : MonoBehaviour
{
    [SerializeField]
    public float HeightRatio = 0.1f;
    [SerializeField]
    public int ScreenHeight = 0;
    [SerializeField]
    public int ScreenWidth = 0;
    private float mWidth;
    private float mHeight;
    RectTransform mTransform;
    // Start is called before the first frame update
    void Start()
    {
        mTransform = GetComponent<RectTransform>();
        ScreenHeight = Screen.height;
        ScreenWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
