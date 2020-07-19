using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCursorBase : MonoBehaviour
{

    protected SpriteRenderer Sprite;
    
    public float MaxDistance;
    
    void Start(){
        Sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void LateUpdate()
    {
        Position();
    }
   protected virtual void Position(){
        Vector3 mousePos = Input.mousePosition;
	    Vector3 pos = Camera.main.ScreenToWorldPoint (mousePos);
        pos.z = -8f;
        transform.position = pos;
   }
   
}
