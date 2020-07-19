using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetism : MonoBehaviour
{
    public Transform Target;
    public float Range = 2f;
    bool Reached = false;
    public float delayTime = 0.5f;

    bool delay = true;
    private void Start()
    {
        Target = GameManager.Instance.Player.transform;
        StartCoroutine("DoDelay");
    }

    IEnumerator DoDelay()
    {
        yield return new WaitForSeconds(delayTime);
        delay = false;
        StopCoroutine("DoDelay");
    }


    void Update()
    {
        if (delay == true)
        {
            return;
        }
        if (Target == null)
        {
            return;
        }
        if (Reached)
        {
            return;
        }
        Vector2 pos = transform.position;
        Vector2 targetPos = Target.transform.position;
        if (Vector2.Distance(pos, targetPos) > Range)
        {
            return;
        }
        targetPos.y += 0.5f;

        Vector2 newPos = Vector2.MoveTowards(pos, targetPos, Time.deltaTime * 4f);
        transform.position = newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Target.gameObject)
        {
            Reached = true;
        }
    }



}
