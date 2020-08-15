using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTuft : HarvestObject
{
    public float ShakeTime = 0.75f;
    public float ShakeAmplitude = 0.02f;
    public float ShakeSpeed = 12;

    bool IsShaking = false;

    void Start()
    {
        CurrentHealth = Random.Range(0.0f, 1.0f);
    }
    public override bool Hit()
    {
        if (base.Hit() == true)
        {
            Shake();
            return true;
        }
        return false;
    }
    void OnTriggerEnter2D(Collider2D pOther)
    {
        if (pOther.gameObject.tag == "Player" && pOther.isTrigger == false)
        {
            Shake();
        }
    }

    private void Shake()
    {
        // StartCoroutine("DoSway");
    }
    IEnumerator DoSway()
    {
        SwayShader shader = GetComponent<SwayShader>();
        float initialAmp = shader.Amplitude;
        float initialSpeed = shader.Speed;

        shader.Amplitude = ShakeAmplitude;
        shader.Speed = ShakeSpeed;
        yield return new WaitForSeconds(ShakeTime);
        shader.Amplitude = initialAmp;
        shader.Speed = initialSpeed;
        StopCoroutine("DoSway");
    }
}
