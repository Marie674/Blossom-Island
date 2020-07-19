using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlayerInteractionDetector : MonoBehaviour
{

    public InteractionPoint CurrentPoint;

    public InteractionPoint PreviousPoint;
    public List<InteractionPoint> ValidPoints = new List<InteractionPoint>();
    public List<InteractionPoint> PotentialPoints = new List<InteractionPoint>();

    public float TimeBeforeHold = 0.2f;
    protected float TimeHeld = 0;
    protected bool IsHolding = false;
    public KeyCode UseKey = KeyCode.Space;
    public string UseButton = "Interact";
    void LateUpdate()
    {
        // Exit if disabled or paused:
        if (!enabled || (Time.timeScale <= 0)) return;

        GetClosestValidPoint();

        // If the player presses the use key/button, send the OnUse message:
        if (CurrentPoint != null && IsUseButtonUp() && TimeHeld < TimeBeforeHold)
        {
            UseCurrentSelection();
        }

        if (IsUseButtonUp())
        {
            IsHolding = false;
            TimeHeld = 0;
        }

        if (IsUseButtonHeld())
        {
            TimeHeld += Time.deltaTime;
            if (TimeHeld >= TimeBeforeHold)
            {
                IsHolding = true;
            }
        }

        if (CurrentPoint != null && IsHolding)
        {
            HoldUseCurrentSelection();
        }
    }

    public void UseCurrentSelection()
    {
        if ((CurrentPoint != null) && (CurrentPoint.gameObject != null))
        {
            CurrentPoint.Interact();
        }
    }

    public virtual void HoldUseCurrentSelection()
    {
        if ((CurrentPoint != null) && (CurrentPoint.gameObject != null))
        {
            CurrentPoint.HoldUse();
        }
    }

    protected bool IsUseButtonDown()
    {
        return ((UseKey != KeyCode.None) && Input.GetKeyDown(UseKey))
            || (!string.IsNullOrEmpty(UseButton) && Input.GetButtonDown(UseButton));
    }

    protected virtual bool IsUseButtonHeld()
    {
        return ((UseKey != KeyCode.None) && Input.GetKey(UseKey))
            || (!string.IsNullOrEmpty(UseButton) && Input.GetButton(UseButton));
    }

    protected virtual bool IsUseButtonUp()
    {
        return ((UseKey != KeyCode.None) && Input.GetKeyUp(UseKey))
            || (!string.IsNullOrEmpty(UseButton) && Input.GetButtonUp(UseButton));
    }



    void OnTriggerStay2D(Collider2D pOther)
    {
        InteractionPoint point = pOther.GetComponent<InteractionPoint>();
        if (point == null)
        {
            return;
        }
        if (PotentialPoints.Contains(point) == false)
        {
            PotentialPoints.Add(point);
            GetValidPoints();
        }

    }

    void OnTriggerExit2D(Collider2D pOther)
    {
        InteractionPoint point = pOther.GetComponent<InteractionPoint>();

        if (point != null && PotentialPoints.Contains(point) == true)
        {
            // print(point.Target.gameObject.name);
            PotentialPoints.Remove(point);
            PotentialPoints = PotentialPoints.Where(item => item != null).ToList();
        }
        GetValidPoints();
        if (ValidPoints.Count < 1)
        {
            ClearPointHUD(PreviousPoint);
            PreviousPoint = null;
            ClearPointHUD(CurrentPoint);
            CurrentPoint = null;
        }

    }

    void GetValidPoints()
    {
        PotentialPoints = PotentialPoints.Where(item => item != null).ToList();

        ValidPoints.Clear();

        foreach (InteractionPoint point in PotentialPoints)
        {
            if (point.CheckFacing() == true)
            {
                ValidPoints.Add(point);
            }
        }
    }

    void GetClosestValidPoint()
    {
        GetValidPoints();
        ValidPoints = ValidPoints.Where(item => item != null).ToList();

        if (ValidPoints.Count < 1)
        {
            if (CurrentPoint != null)
            {
                ClearPointHUD(CurrentPoint);
                ClearPointHUD(PreviousPoint);
            }
            CurrentPoint = null;
            PreviousPoint = null;
            return;
        }
        float distance = 100000;

        foreach (InteractionPoint point in ValidPoints)
        {
            float pointDisance = Vector2.Distance(transform.position, point.transform.position);
            if (pointDisance <= distance)
            {
                distance = pointDisance;
                CurrentPoint = point;
            }
        }

        if (PreviousPoint != CurrentPoint)
        {
            CurrentPointChanged(PreviousPoint);
            PreviousPoint = CurrentPoint;
            if (CurrentPoint != null)
            {
                ShowPointHUD(CurrentPoint);
            }
        }

    }

    void ClearPointHUD(InteractionPoint pPoint)
    {
        ContextualHUD targetHUD = null;
        if (pPoint != null && pPoint.Target != null && pPoint.Target.GetComponent<ContextualHUD>() != null)
        {
            targetHUD = pPoint.Target.GetComponent<ContextualHUD>();
        }
        if (targetHUD != null)
        {
            targetHUD.Hide();
        }
    }

    void ShowPointHUD(InteractionPoint pPoint)
    {
        ContextualHUD targetHUD = null;
        if (pPoint != null && pPoint.Target != null && pPoint.Target.GetComponent<ContextualHUD>() != null)
        {
            targetHUD = pPoint.Target.GetComponent<ContextualHUD>();
        }
        if (targetHUD != null)
        {
            targetHUD.Show();
        }
    }

    void CurrentPointChanged(InteractionPoint pPreviousPoint)
    {
        ContextualHUD previousTargetHUD = null;
        ContextualHUD currentTargetHUD = null;

        if (pPreviousPoint != null)
        {
            if (PreviousPoint.Target != null && PreviousPoint.Target.GetComponent<ContextualHUD>() != null)
            {
                previousTargetHUD = pPreviousPoint.Target.GetComponent<ContextualHUD>();

            }
        }
        if (CurrentPoint != null)
        {
            if (CurrentPoint.Target != null && CurrentPoint.Target.GetComponent<ContextualHUD>() != null)
            {
                currentTargetHUD = CurrentPoint.Target.GetComponent<ContextualHUD>();

            }
        }

        if (pPreviousPoint != null && previousTargetHUD != null)
        {
            previousTargetHUD.Hide();
        }
        if (CurrentPoint != null && currentTargetHUD != null)
        {
            currentTargetHUD.Show();

        }

    }
}
