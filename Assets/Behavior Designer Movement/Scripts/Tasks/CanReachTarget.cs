using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the destination is reachable by the agent, using A*.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CanReachTarget : Conditional
    {

        public SharedVector2 targetPosition;

        // Returns success if the destination is valid, otherwise, failure
        public override TaskStatus OnUpdate()
        {
            var validDestination = false;
            var destination = targetPosition;
           
            validDestination = SamplePosition(destination.Value);

            if (validDestination==true) {
                // Return success if an object was found
                return TaskStatus.Success;
            }
            // An object is not within sight so return failure
            return TaskStatus.Failure;
        }

        protected bool SamplePosition(Vector3 position)
        {
            return AstarPath.active.GetNearest(position).node.Walkable;
        }

        // Reset the public variables
        public override void OnReset()
        {
            targetPosition = Vector2.zero;
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
           
        }

        public override void OnBehaviorComplete()
        {
            MovementUtility.ClearCache();
        }
    }
}