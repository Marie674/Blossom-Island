using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Fence : MonoBehaviour
{
    public string Name;

    public SpriteRenderer Sprite;
    public Sprite Single;
    public Sprite Down;
    public Sprite DownLeft;
    public Sprite DownRight;
    public Sprite DownLeftRight;
    public Sprite DownUp;
    public Sprite DownLeftUp;
    public Sprite DownRightUp;

    public Sprite Left;
    public Sprite LeftRight;
    public Sprite LeftUp;
    public Sprite LeftUpRight;

    public Sprite Right;
    public Sprite RightUp;
    public Sprite Up;
    public Sprite AllDirections;

    private bool isQuitting = false;

    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        GetSurroundings();
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void GetSurroundings(bool ChangeTouchedFences = true, bool setSprite = true)
    {
        bool Up = false;
        bool Left = false;
        bool Right = false;
        bool Down = false;
        List<Fence> TouchedFences = new List<Fence>();
        Vector2 downPos = transform.position;
        downPos.y -= 1;
        RaycastHit2D[] hits = Physics2D.RaycastAll(downPos, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<Fence>() != null)
            {
                Fence fence = hit.collider.gameObject.GetComponent<Fence>();
                if (!TouchedFences.Contains(fence) && fence.Name == this.Name)
                {
                    TouchedFences.Add(fence);
                }
                Down = true;
            }
        }

        Vector2 leftPos = transform.position;
        leftPos.x -= 1;
        hits = Physics2D.RaycastAll(leftPos, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<Fence>() != null)
            {
                Fence fence = hit.collider.gameObject.GetComponent<Fence>();
                if (!TouchedFences.Contains(fence) && fence.Name == this.Name)
                {
                    TouchedFences.Add(fence);
                }
                Left = true;
            }
        }

        Vector2 rightPos = transform.position;
        rightPos.x += 1;
        hits = Physics2D.RaycastAll(rightPos, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<Fence>() != null)
            {
                Fence fence = hit.collider.gameObject.GetComponent<Fence>();
                if (!TouchedFences.Contains(fence) && fence.Name == this.Name)
                {
                    TouchedFences.Add(fence);
                }
                Right = true;
            }


        }

        Vector2 upPos = transform.position;
        upPos.y += 1;
        hits = Physics2D.RaycastAll(upPos, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<Fence>() != null)
            {
                Fence fence = hit.collider.gameObject.GetComponent<Fence>();
                if (!TouchedFences.Contains(fence) && fence.Name == this.Name)
                {
                    TouchedFences.Add(fence);
                }
                Up = true;
            }

        }

        SetSprite(Down, Left, Right, Up);

        if (ChangeTouchedFences)
        {
            TouchedFences = TouchedFences.Where(x => x != null).ToList();
            foreach (Fence fence in TouchedFences)
            {
                fence.GetSurroundings(false);
            }
        }


    }


    private void SetSprite(bool pDown, bool pLeft, bool pRight, bool pUp)
    {
        if (pDown == false && pLeft == false && pRight == false && pUp == false)
        {
            Sprite.sprite = Single;
        }
        else if (pDown == true && pLeft == false && pRight == false && pUp == false)
        {
            Sprite.sprite = Down;
        }
        else if (pDown == true && pLeft == true && pRight == false && pUp == false)
        {
            Sprite.sprite = DownLeft;
        }
        else if (pDown == true && pLeft == false && pRight == true && pUp == false)
        {
            Sprite.sprite = DownRight;
        }
        else if (pDown == true && pLeft == true && pRight == true && pUp == false)
        {
            Sprite.sprite = DownLeftRight;
        }
        else if (pDown == true && pLeft == false && pRight == false && pUp == true)
        {
            Sprite.sprite = DownUp;
        }
        else if (pDown == true && pLeft == true && pRight == false && pUp == true)
        {
            Sprite.sprite = DownLeftUp;
        }
        else if (pDown == true && pLeft == false && pRight == true && pUp == true)
        {
            Sprite.sprite = DownRightUp;
        }
        else if (pDown == false && pLeft == true && pRight == false && pUp == false)
        {
            Sprite.sprite = Left;
        }
        else if (pDown == false && pLeft == true && pRight == false && pUp == true)
        {
            Sprite.sprite = LeftUp;
        }
        else if (pDown == false && pLeft == true && pRight == true && pUp == false)
        {
            Sprite.sprite = LeftRight;
        }
        else if (pDown == false && pLeft == true && pRight == true && pUp == true)
        {
            Sprite.sprite = LeftUpRight;
        }
        else if (pDown == false && pLeft == false && pRight == true && pUp == false)
        {
            Sprite.sprite = Right;
        }
        else if (pDown == false && pLeft == false && pRight == true && pUp == true)
        {
            Sprite.sprite = RightUp;
        }
        else if (pDown == false && pLeft == false && pRight == false && pUp == true)
        {
            Sprite.sprite = Up;
        }
        else if (pDown == true && pLeft == true && pRight == true && pUp == true)
        {
            Sprite.sprite = AllDirections;
        }
    }

    void OnDestroy()
    {
        if (isQuitting)
        {
            return;
        }
        GetSurroundings(true, false);
    }
}
