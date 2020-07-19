﻿using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    public class FollowObjectBehaviour : MonoBehaviour
    {

        public float DampTime = 0.15f;
        public Transform Target;

        private Vector3 velocity = Vector3.zero;
        private Camera m_camera;

        void Start()
        {
            m_camera = GetComponent<Camera>();
        }

        void LateUpdate()
        {
            if (Target)
            {
                Vector3 point = m_camera.WorldToViewportPoint(Target.position);
                Vector3 delta = Target.position - m_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                Vector3 destination = transform.position + delta;
                destination.z = -15f;
                transform.position = new Vector3(transform.position.x, transform.position.y, destination.z);
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
            }
        }
    }
}
