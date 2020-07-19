using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwayShader : MonoBehaviour
{
    public Color Tint = Color.white;
    public float Amplitude;

    public float Speed;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Get the current value of the material properties in the renderer.
        _renderer.GetPropertyBlock(_propBlock);
        // Assign our new value.
        _propBlock.SetColor("_Tint", Tint);

        _propBlock.SetFloat("_Amplitude", Amplitude);
        //        print(_propBlock.GetFloat("_Amplitude"));

        _propBlock.SetFloat("_Speed", Speed);
        // Apply the edited values to the renderer.
        _renderer.SetPropertyBlock(_propBlock);
    }
}