using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint : MonoBehaviour
{

    public int Height = 0;
    public PlayerCharacter.CharacterDirection RequiredFacing;
    PlayerCharacter Player;
    public JumpPoint Target;
    public bool ActivePoint = false;
    public float JumpTime = 1f;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();

    }

    public void CheckJump()
    {
        if (ActivePoint == false)
        {
            return;
        }
        if (CheckConditions() == true)
        {
            float requiredEnergy = Mathf.Clamp(Target.Height - Height, 0, 10f);
            PlayerNeedManager.Instance.GetNeed("Energy").Change(-requiredEnergy);
            Jump();
        }
    }


    public bool CheckConditions()
    {
        if (Player.IsJumping == true)
        {
            return false;
        }
        if (Player.Direction != RequiredFacing)
        {
            return false;
        }
        if (Target.Height > Height + 1)
        {
            return false;
        }
        return true;

    }

    private void Jump()
    {
        Player.StartCoroutine("SetJumpCooldown", JumpTime);
        Vector3 newPos = Player.transform.position;
        newPos.x = Target.transform.position.x;
        newPos.y = Target.transform.position.y;
        Player.transform.position = newPos;

    }
}
