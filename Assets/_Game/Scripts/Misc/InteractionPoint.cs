using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    public GameObject Target;

    public bool Up = false;
    public bool Down = false;
    public bool Left = false;
    public bool Right = false;

    List<PlayerCharacter.CharacterDirection> AllowedFacings = new List<PlayerCharacter.CharacterDirection>();
    PlayerCharacter Player;

    void Start()
    {
        AllowedFacings.Clear();
        if (Up == true)
        {
            AllowedFacings.Add(PlayerCharacter.CharacterDirection.Up);
        }
        if (Down == true)
        {
            AllowedFacings.Add(PlayerCharacter.CharacterDirection.Down);
        }
        if (Left == true)
        {
            AllowedFacings.Add(PlayerCharacter.CharacterDirection.Left);
        }
        if (Right == true)
        {
            AllowedFacings.Add(PlayerCharacter.CharacterDirection.Right);
        }
        Player = GameManager.Instance.Player;
    }

    public bool CheckFacing()
    {
        if (Player != null && AllowedFacings.Contains(Player.Direction))
        {
            return true;
        }
        return false;
    }
    public void Interact()
    {
        if (Player != null && Target != null && AllowedFacings.Contains(Player.Direction))
        {
            Target.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void HoldUse()
    {
        if (Player != null && Target != null && AllowedFacings.Contains(Player.Direction))
        {
            Target.SendMessage("HoldUse", SendMessageOptions.DontRequireReceiver);
        }
    }
}
