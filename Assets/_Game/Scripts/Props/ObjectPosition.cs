using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPosition : MonoBehaviour
{

    public bool AdjustXY = true;
    public bool AdjustZ = true;
    public float FixedZ = 0f;

    public float Units = 1f;

    public bool AdjustOnUpdate = false;

    // Use this for initialization
    void Start()
    {

        if (AdjustXY == true)
        {
            AdjustXYPos();
        }
        if (AdjustZ == true)
        {
            AdjustZPos();
        }
    }

    void Update()
    {
        if (AdjustOnUpdate == true)
        {
            AdjustPositions();
        }
    }

    public void AdjustPositions()
    {
        if (AdjustXY == true)
        {
            AdjustXYPos();
        }
        if (AdjustZ == true)
        {
            AdjustZPos();
        }
        else
        {
            // Vector3 newPos = transform.position;
            // newPos.x = Mathf.Round(newPos.x * 10f) / 10f;
            // newPos.y = Mathf.Round(newPos.y * 10f) / 10f;
            // transform.position = newPos;
        }
    }
    //
    //	void Awake(){
    //		AdjustPositions ();
    //	}
    //
    void AdjustXYPos()
    {

        // Get sprite extents (half of the sprite size)
        Vector3 spriteExtents = GetComponent<SpriteRenderer>().bounds.extents;
        // Get sprite center
        Vector3 spriteCenter = GetComponent<SpriteRenderer>().bounds.center;
        // Get top left point of sprite (this is what we want to snap to the 0.32 grid, since the sprite has a size multiple of Units and 1 unit = 100px)
        Vector3 spriteTopLeft = new Vector3(spriteCenter.x - spriteExtents.x, spriteCenter.y + spriteExtents.y, 0f);
        // Calculate what position the top left of the sprite would be if it were snapped to Units
        Vector3 snapPos = new Vector3(Units * Mathf.Round(spriteTopLeft.x / Units), Units * Mathf.Round(spriteTopLeft.y / Units), 0f);
        // Calculate the difference to apply to the actual pivot, which is different than the top left corner of the sprite
        Vector3 posDifference = spriteTopLeft - snapPos;
        // Apply the calculated difference
        Vector3 newPos = transform.position - posDifference;

        // Apply the new position
        transform.position = newPos;
    }

    void AdjustZPos()
    {

        float zPos = ((transform.position.y / 100) - (transform.position.x / 1000)) - 5f;
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, zPos);
        transform.position = newPos;

    }

}
