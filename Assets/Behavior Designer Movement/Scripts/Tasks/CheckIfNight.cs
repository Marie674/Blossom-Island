using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Blossoms
{
    [TaskDescription("Check to see if the destination is reachable by the agent, using A*.")]
    [TaskCategory("Blossoms")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CheckIfNight : Conditional
    {
        [RequiredField]
        public SharedBool storeValue;

        // Returns success if nighttime, otherwise, failure
        public override TaskStatus OnUpdate()
        {

            bool isNight = false;
            if(DayPhaseManager.Instance.CurrentDayPhase.Name == DayPhaseManager.DayPhaseNames.Night || DayPhaseManager.Instance.CurrentDayPhase.Name == DayPhaseManager.DayPhaseNames.Dawn)
            {
                isNight = true;
            }
            storeValue.Value = isNight;
            if (isNight==true) {
                // Return success if an object was found
                return TaskStatus.Success;
            }
            // An object is not within sight so return failure
            return TaskStatus.Failure;
        }


        // Reset the public variables
        public override void OnReset()
        {
            
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
           
        }

        public override void OnBehaviorComplete()
        {
            
        }
    }
}