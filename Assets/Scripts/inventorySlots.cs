using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class inventorySlots : MonoBehaviour
{

    public GameObject movingItem;
    public Image movingItemImage;
    public playerScript playersScript;

    float itemImageVertical;
    float itemImageHorizontal;
    float itemImageSpeed = 5f;

    public Image emptyImage;

    public Canvas canvas;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
      /*  if(playersScript.inventoryOpen == true)
        {

            Vector2 pos;


            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
            if (movingItemImage != null)
            {
                movingItemImage.transform.position = canvas.transform.TransformPoint(pos);
            }
        } */

    }
}
