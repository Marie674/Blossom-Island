using UnityEngine;
using Game.Blossoms;
namespace BehaviorDesigner.Runtime.Tasks.Blossoms
{
    [TaskCategory("NPCs")]
    [TaskDescription("Stores the origin position. Returns Success.")]
    public class GetOriginPosition : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("Can the target GameObject be empty?")]
        [RequiredField]
        public SharedVector2 storeValue;

        private Vector2 targetPosition;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                targetPosition = currentGameObject.transform.position;
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (targetPosition == null)
            {
                Debug.LogWarning("Position is null");
                return TaskStatus.Failure;
            }

            storeValue.Value = targetPosition;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            storeValue = Vector2.zero;
        }
    }
}