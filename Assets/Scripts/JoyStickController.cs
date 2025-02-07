using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Vector2 NormalizedMovingVelocity;
    public bool JoyStickState;

    private RectTransform Transform;
    private RectTransform HolderTransform;
    private float Radius;
    // Start is called before the first frame update
    void Start()
    {
        var Image = GetComponent<Image>();
        Image.alphaHitTestMinimumThreshold = 0.5f;
        Transform = GetComponent<RectTransform>();
        Radius = Transform.rect.width * 0.4f;
        HolderTransform = transform.parent.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        var gap = eventData.position - eventData.pressPosition;
        ControlJoyStick(gap);
    }
    public void SetLocation(bool aIsRightSide)
    {
        float x = Mathf.Abs(HolderTransform.anchoredPosition.x);
        float y = Mathf.Abs(HolderTransform.anchoredPosition.y);
        Vector2 anchorMin; Vector2 anchorMax; Vector3 rectPosition;
        if (aIsRightSide)
        {
            anchorMin = new Vector2(1, 0);
            anchorMax = new Vector2(1, 0);
            rectPosition = new Vector3(-x, y, 0);
        }
        else
        {
            anchorMin = new Vector2(0, 0);
            anchorMax = new Vector2(0, 0);
            rectPosition = new Vector3(x, y, 0);
        }
        HolderTransform.anchorMin = anchorMin;
        HolderTransform.anchorMax = anchorMax;
        HolderTransform.anchoredPosition = rectPosition;
    }
    private void ControlJoyStick(Vector2 gap)
    {
        var length = gap.magnitude;
        var joyStickLocation = new Vector3(gap.x, gap.y);
        if (length > Radius)
        {
            joyStickLocation.Normalize();
            joyStickLocation.x *= Radius;
            joyStickLocation.y *= Radius;
            gap.Normalize();
        }
        else
        {
            gap.x = joyStickLocation.x / Radius;
            gap.y = joyStickLocation.y / Radius;
        }
        NormalizedMovingVelocity = gap;
        Transform.localPosition = joyStickLocation;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //touch sound
        JoyStickState = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //
        JoyStickState = false;
        Transform.localPosition = new Vector3(0f, 0f);
    }
}