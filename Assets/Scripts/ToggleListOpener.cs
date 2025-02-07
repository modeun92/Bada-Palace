using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class ToggleListOpener : MonoBehaviour
{
    public float openingUnit;
    private bool toggle;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        toggle = false;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.transform.localScale = new Vector3(0f, 1f, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        float xScale = rectTransform.transform.localScale.x;
        if (toggle)
        {
            if (xScale < 1f)
            {
                xScale += openingUnit;
            }
            else
            {
                if (xScale == 1f)
                {
                    return;
                }
                else
                {
                    xScale = 1f;
                }
            }
        }
        else
        {
            if (xScale > 0f)
            {
                xScale -= openingUnit;
            }
            else
            {
                if (xScale == 0f)
                {
                    return;
                }
                else
                {
                    xScale = 0f;
                }
            }
        }
        rectTransform.transform.localScale = new Vector3(xScale, 1f, 1f);
    }
    public void Toggle()
    {
        toggle = !toggle;
    }
}
