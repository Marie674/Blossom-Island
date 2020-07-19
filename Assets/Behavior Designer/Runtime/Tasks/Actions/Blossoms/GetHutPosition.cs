using UnityEngine;
using Game.Blossoms;
namespace BehaviorDesigner.Runtime.Tasks.Blossoms
{
    [TaskCategory("Blossoms")]
    [TaskDescription("Stores the position of the hut. Returns Success.")]
    public class GetHutPosition : Action
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
            if (currentGameObject != prevGameObject) {
                targetPosition = currentGameObject.GetComponent<BlossomData>().HutPosition;
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (targetPosition == null) {
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