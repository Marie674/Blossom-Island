using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.RpgMapEditor;
public class JumpArea : MonoBehaviour
{


    public enum JumpDirection
    {
        Vertical,
        Horizontal
    }

    public JumpDirection Direction;

    public float Distance = 0.32f;
    PlayerCharacter Player;
    bool ActiveJumpArea = false;


    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();
    }

    void OnTriggerStay2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player")
        {
            if (ActiveJumpArea == false)
            {
                ActiveJumpArea = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player")
        {
            ActiveJumpArea = false;

        }
    }

    public void Interact()
    {
        Vector3 pos = Player.transform.position;

        switch (Direction)
        {
            case JumpDirection.Vertical:
                switch (Player.Direction)
                {
                    case PlayerCharacter.CharacterDirection.Down:
                        if (Player.transform.position.y > transform.position.y)
                        {
                            pos.y -= Distance;
                        }
                        break;
                    case PlayerCharacter.CharacterDirection.Up:
                        if (Player.transform.position.y <= transform.position.y)
                        {
                            pos.y += Distance;
                        }
                        break;
                    default:
                        return;
                }
                break;
            case JumpDirection.Horizontal:
                switch (Player.Direction)
                {
                    case PlayerCharacter.CharacterDirection.Left:
                        pos.x -= Distance;
                        break;
                    case PlayerCharacter.CharacterDirection.Right:
                        pos.x += Distance;
                        break;
                    default:
                        return;
                }
                break;
            default:
                break;
        }
        Player.transform.position = pos;
    }


}
