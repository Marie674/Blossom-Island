using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.RpgMapEditor;

public class PlayerCharacter : MonoBehaviour
{

    public enum CharacterDirection
    {
        Down,
        Right,
        Up,
        Left
    }
    private string CurrentArea = string.Empty;
    public float WalkSpeed = 4f;

    public float RunSpeed = 8f;

    public float SpeedMultiplier = 1f;

    float CurrentSpeed = 0f;
    public Vector3 Dir;
    public NeedBase EnergyNeed;

    public CharacterDirection Direction = CharacterDirection.Down;
    public delegate void PlayerDirectionChange();
    public static event PlayerDirectionChange OnPlayerDirectionChange;

    public List<PlayerTraitScriptableObject> Traits = new List<PlayerTraitScriptableObject>();
    public bool IsJumping = false;
    public bool IsSwimming = false;
    public bool IsUsingTool = false;
    public bool IsSheltered = false;
    public bool IsWet = false;
    public StatusEffectConditionBool WetBool;

    public StatusEffectConditionBool RunningBool;

    public List<Animator> Animators;

    public Animator ToolAnims;

    private float Horizontal;
    private float Vertical;

    public bool IsActing = false;

    public float PettingTime = 2.5f;

    public float MovementDamping = 5f;
    private bool IsCarrying;

    private bool IsRunning = false;

    bool IsApplicationQuitting = false;
    private void CheckCarrying()
    {

        IsCarrying = false;
        if (Toolbar.Instance.SelectedSlot.ReferencedItemStack != null)
        {
            if (Toolbar.Instance.SelectedSlot.ReferencedItemStack.ContainedItem.itemType != ItemSystem.ItemType.Tool)
            {
                IsCarrying = true;
            }
        }

        foreach (Animator anim in Animators)
        {
            anim.SetBool("Carrying", IsCarrying);
        }
    }

    public string GetCurrentArea()
    {
        if (CurrentArea == string.Empty)
        {
            CurrentArea = GameManager.Instance.LevelInfo.Name;
        }
        return CurrentArea;
    }

    public void SetCurrentArea(string pAreaName)
    {
        CurrentArea = pAreaName;
    }
    public CharacterDirection GetAnimDir(Vector2 vDir)
    {
        //print(vDir);
        if (Mathf.Abs(vDir.x - vDir.y) <= 0.1f)
        {
            return Direction;
        }
        float angle360 = Vector3.Cross(-Vector2.up, vDir).z >= 0f ? Vector2.Angle(-Vector2.up, vDir) : 360f - Vector2.Angle(-Vector2.up, vDir);
        int dirIdx = Mathf.RoundToInt((angle360 * 4) / 360f) % 4;

        if (IsRunning == false && CurrentSpeed < WalkSpeed * SpeedMultiplier)
        {
            return Direction;
        }
        else if (IsRunning == true && CurrentSpeed < RunSpeed * SpeedMultiplier)
        {
            return Direction;

        }
        return (CharacterDirection)(dirIdx);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    protected void OnEnable()
    {
        GameInputManager.ObserveAxis("Horizontal");
        GameInputManager.ObserveAxis("Vertical");

        GameInputManager.Register(OnInputEvent);
        Toolbar.Instance.OnSelectedSlotItemChanged += CheckCarrying;
    }

    protected void OnDisable()
    {
        GameInputManager.Unregister(OnInputEvent);
        if (!IsApplicationQuitting)
            Toolbar.Instance.OnSelectedSlotItemChanged -= CheckCarrying;

    }

    void OnApplicationQuit()
    {
        IsApplicationQuitting = true;
    }
    protected void OnInputEvent(GameInputManager.EventData data)
    {
        if (data.used) return;

        if (data.axis == "Horizontal")
        {
            Horizontal = data.value;
            data.used = true;
        }
        if (data.axis == "Vertical")
        {
            Vertical = data.value;
            data.used = true;
        }


    }

    public bool IsMoving
    {
        get { return Dir.sqrMagnitude > 0; }
    }

    void FixedUpdate()
    {

        //Set move direction
        Dir = new Vector3(Mathf.Round(Horizontal / 0.15f) * 0.15f, Mathf.Round(Vertical / 0.15f) * 0.15f, 0);
        Dir.z = 0f;

        //If moving input down
        if (Dir.sqrMagnitude > 0f)
        {

            //If run button is down, set speed to run speed and running bool to true
            if (Input.GetButton("Run") && EnergyNeed.CurrentValue > 0)
            {
                CurrentSpeed += (RunSpeed - CurrentSpeed) / Mathf.Pow(2f, Time.deltaTime);
                IsRunning = true;
                RunningBool.MetBool = true;

            }
            //If run button isn't down, set speed to walk speed and running bool to false
            else
            {
                RunningBool.MetBool = false;
                IsRunning = false;
                CurrentSpeed += (WalkSpeed - CurrentSpeed) / Mathf.Pow(2f, Time.deltaTime);
            }

        }
        // If moving input not down, set running bool to false and tamper off speed
        else
        {
            RunningBool.MetBool = false;
            CurrentSpeed /= Mathf.Pow(2f, Time.deltaTime * MovementDamping);
        }

        // Set z value to reflect X and Y position (walk in front and behinf objects)
        float zPos = ((transform.position.y / 100) - (transform.position.x / 1000)) - 5f;
        //Set the new position
        Vector3 newPos = new Vector3(transform.position.x + Dir.x * (CurrentSpeed * (Time.deltaTime)), transform.position.y + (Dir.y * CurrentSpeed * (Time.deltaTime)), zPos);

        //If character is not acting, move the position
        if (!IsActing)
        {
            Direction = GetAnimDir(Dir);

            transform.position = newPos;

            UpdateAnimation();
        }

    }


    public bool DoAction(string pAnimTrigger, float pTime, Vector2 pPos, float pExitdelay = 0, string pToolTrigger = "", bool pTools = false)
    {
        if (IsActing || IsCarrying)
        {
            return false;
        }
        IsActing = true;


        Vector2 dir = ((Vector2)transform.position - pPos).normalized;

        foreach (Animator anim in Animators)
        {

            anim.SetBool("Acting", true);
        }
        ToolAnims.SetBool("Acting", true);


        float xDir = dir.x;
        float yDir = dir.y;

        if (Mathf.Abs(xDir) > Mathf.Abs(yDir))
        {
            if (xDir >= 0)
            {
                ChangeFacing(CharacterDirection.Left);
            }
            else
            {
                ChangeFacing(CharacterDirection.Right);
            }
        }
        else
        {
            if (yDir <= 0)
            {
                ChangeFacing(CharacterDirection.Up);

            }
            else
            {
                ChangeFacing(CharacterDirection.Down);

            }
        }

        StartCoroutine(ActionTimer(pTime, pExitdelay));

        foreach (Animator anim in Animators)
        {
            anim.SetTrigger(pAnimTrigger);
        }
        if (pTools == true)
        {
            ToolAnims.SetTrigger(pToolTrigger);
        }

        return true;
    }

    public IEnumerator ActionTimer(float pTime, float pDelay = 0f)
    {


        yield return new WaitForSeconds(pTime);
        foreach (Animator anim in Animators)
        {

            anim.SetBool("Acting", false);
        }
        ToolAnims.SetBool("Acting", false);

        yield return new WaitForSeconds(pDelay);
        IsActing = false;

    }


    void ChangeFacing(CharacterDirection pDir)
    {
        bool left = false;
        bool right = false;
        bool down = false;
        bool up = false;

        switch (pDir)
        {
            case CharacterDirection.Down:
                down = true;
                break;
            case CharacterDirection.Left:
                left = true;
                break;
            case CharacterDirection.Right:
                right = true;
                break;
            case CharacterDirection.Up:
                up = true;
                break;
            default:
                return;

        }
        foreach (Animator anim in Animators)
        {
            anim.SetBool("Down", down);
            anim.SetBool("Left", left);
            anim.SetBool("Right", right);
            anim.SetBool("Up", up);
        }
        ToolAnims.SetBool("Down", down);
        ToolAnims.SetBool("Left", left);
        ToolAnims.SetBool("Right", right);
        ToolAnims.SetBool("Up", up);
        Direction = pDir;
    }
    void UpdateAnimation()
    {

        bool left = false;
        bool right = false;
        bool down = false;
        bool up = false;

        switch (Direction)
        {
            case CharacterDirection.Down:
                down = true;
                left = false;
                right = false;
                up = false;
                break;
            case CharacterDirection.Left:
                left = true;
                down = false;
                right = false;
                up = false;
                break;
            case CharacterDirection.Right:
                right = true;
                left = false;
                down = false;
                up = false;
                break;
            case CharacterDirection.Up:
                up = true;
                left = false;
                right = false;
                down = false;
                break;
            default:
                return;

        }
        foreach (Animator anim in Animators)
        {
            anim.SetBool("Down", down);
            anim.SetBool("Left", left);
            anim.SetBool("Right", right);
            anim.SetBool("Up", up);
            anim.SetFloat("Speed", (int)CurrentSpeed);
            if (IsActing == false)
            {
                float speed = CurrentSpeed / 4;
                if (speed < 1)
                {
                    speed = 1f;
                }
                anim.speed = speed;
            }
            else
            {
                anim.speed = 1f;
            }
        }
    }

}
