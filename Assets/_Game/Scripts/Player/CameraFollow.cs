using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float DampTime = 0.15f;
    public Transform Target;

    private Vector2 Velocity = Vector2.zero;
    private Camera Camera;

    void Start()
    {
        Camera = GetComponent<Camera>();
        if (Target == null)
        {
            Target = GameObject.FindWithTag("Player").transform;
        }
    }

    void FixedUpdate()
    {
        if (Target)
        {
            Vector2 point = Camera.WorldToViewportPoint(Target.position);
            Vector2 delta = Target.position - Camera.ViewportToWorldPoint(new Vector2(0.5f, 0.5f)); //(new Vector3(0.5, 0.5, point.z));
            Vector2 destination = (Vector2)transform.position + delta;
            Vector2 smooth = Vector2.SmoothDamp((Vector2)transform.position, destination, ref Velocity, DampTime);
            transform.position = new Vector3(smooth.x, smooth.y, transform.position.z);
        }
    }
}
