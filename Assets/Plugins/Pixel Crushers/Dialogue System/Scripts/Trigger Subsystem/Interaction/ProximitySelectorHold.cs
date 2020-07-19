// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem.UnityGUI;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This component implements a proximity-based selector that allows the player to move into
    /// range and use a usable object. 
    /// 
    /// To mark an object usable, add the Usable component and a collider to it. The object's
    /// layer should be in the layer mask specified on the Selector component.
    /// 
    /// The proximity selector tracks the most recent usable object whose trigger the player has
    /// entered. It displays a targeting reticle and information about the object. If the target
    /// is in range, the inRange reticle texture is displayed; otherwise the outOfRange texture is
    /// displayed.
    /// 
    /// If the player presses the use button (which defaults to spacebar and Fire2), the targeted
    /// object will receive an "OnUse" message.
    /// 
    /// You can hook into SelectedUsableObject and DeselectedUsableObject to get notifications
    /// when the current target has changed.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class ProximitySelectorHold : ProximitySelector
    {
        public float TimeBeforeHold = 0.2f;
        protected float TimeHeld = 0;

        protected bool IsHolding = false;


        /// <summary>
        /// Sends an OnUse message to the current selection if the player presses the use button.
        /// </summary>
        protected override void Update()
        {
            // Exit if disabled or paused:
            if (!enabled || (Time.timeScale <= 0)) return;

            // If the currentUsable went missing (was destroyed or we changed scene), tell listeners:
            if (toldListenersHaveUsable && currentUsable == null)
            {
                SetCurrentUsable(null);
                OnDeselectedUsableObject(null);
                toldListenersHaveUsable = false;
            }

            // If the player presses the use key/button, send the OnUse message:
            if (IsUseButtonUp() && TimeHeld < TimeBeforeHold)
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
                //                print(TimeHeld);
                if (TimeHeld >= TimeBeforeHold)
                {
                    IsHolding = true;
                }
            }

            if (IsHolding)
            {
                HoldUseCurrentSelection();
            }
        }


        /// <summary>
        /// Calls OnUse on the current selection.
        /// </summary>
        public override void UseCurrentSelection()
        {
            if ((currentUsable != null) && (currentUsable.gameObject != null) && (Time.time >= timeToEnableUseButton))
            {
                currentUsable.OnUseUsable();
                var fromTransform = (actorTransform != null) ? actorTransform : this.transform;
                if (broadcastToChildren)
                {
                    currentUsable.gameObject.BroadcastMessage("OnUse", fromTransform, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    currentUsable.gameObject.SendMessage("OnUse", fromTransform, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        /// <summary>
        /// Calls OnUse on the current selection.
        /// </summary>
        public virtual void HoldUseCurrentSelection()
        {
            if ((currentUsable != null) && (currentUsable.gameObject != null) && (Time.time >= timeToEnableUseButton))
            {
                currentUsable.OnUseUsable();

                currentUsable.SendMessage("HoldUse", SendMessageOptions.DontRequireReceiver);

                var fromTransform = (actorTransform != null) ? actorTransform : this.transform;
                if (broadcastToChildren)
                {
                    currentUsable.gameObject.BroadcastMessage("HoldUse", fromTransform, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    currentUsable.gameObject.SendMessage("HoldUse", fromTransform, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        /// <summary>
        /// Checks whether the player has just pressed the use button.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the use button/key is down; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsUseButtonDown()
        {
            if (DialogueManager.IsDialogueSystemInputDisabled()) return false;
            if (enableTouch && IsTouchDown()) return true;
            return ((useKey != KeyCode.None) && Input.GetKeyDown(useKey))
                || (!string.IsNullOrEmpty(useButton) && DialogueManager.GetInputButtonDown(useButton));
        }

        /// <summary>
        /// Checks whether the player is holding down the use button.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the use button/key is down; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsUseButtonHeld()
        {
            if (DialogueManager.IsDialogueSystemInputDisabled()) return false;
            if (enableTouch && IsTouchDown()) return true;
            return ((useKey != KeyCode.None) && Input.GetKey(useKey))
                || (!string.IsNullOrEmpty(useButton) && Input.GetButton(useButton));
        }

        /// <summary>
        /// Checks whether the player has just released the use button.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the use button/key is up; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsUseButtonUp()
        {
            return ((useKey != KeyCode.None) && Input.GetKeyUp(useKey))
                || (!string.IsNullOrEmpty(useButton) && Input.GetButtonUp(useButton));
        }



    }

}
