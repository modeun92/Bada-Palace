using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject destination;
    public GameObject mermaid;
    public Rigidbody2D rigidbody2D;
    public float move_speed = 40f;
    // Start is called before the first frame update
    void Start()
    {
        mermaid = GameObject.Find("Mermaid");
        transform.position = mermaid.transform.position;
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BeUsed(Item item)
    {
        if (item.Type == ItemType.FISH)
        {
            destination = GameObject.Find("Destination(Clone)");
            rigidbody2D = GetComponent<Rigidbody2D>();
            var vecX = destination.transform.position.x - transform.position.x;
            var vecY = destination.transform.position.y - transform.position.y;
            if (vecX < 0f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            float length = Mathf.Sqrt((vecX * vecX) + (vecY * vecY));
            var stickPercent = move_speed / length;
            float xPercent = vecX * stickPercent;
            float yPercent = vecY * stickPercent;

            Vector2 vec = new Vector2(xPercent, yPercent);
            rigidbody2D.velocity = vec;
            Invoke("Disappear", 2f);
        }
        else if (item.Type == ItemType.STARFISH)
        {
        }
    }
    private void Disappear()
    {
        Destroy(GameObject.Find("Fish(Clone)"));
    }
}
