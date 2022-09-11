using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class defaultButtonScript : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Sprite clickSprite;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Image>().sprite = defaultSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseOver()
    {
        Debug.Log("sus");
        if (this.gameObject.GetComponent<Image>() != null)
        {
            Debug.Log("mgous");
            this.gameObject.GetComponent<Image>().sprite = hoverSprite;
        }
    }
    void OnMouseExit()
    {
        if (this.gameObject.GetComponent<Image>() != null)
        {
            this.gameObject.GetComponent<Image>().sprite = defaultSprite;
        }
    }
    void OnMouseDown()
    {
        if (this.gameObject.GetComponent<Image>() != null)
        {
            this.gameObject.GetComponent<Image>().sprite = defaultSprite;
        }
    }
}
