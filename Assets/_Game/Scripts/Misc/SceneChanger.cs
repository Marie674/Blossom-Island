using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    public string RequiredTag = "Player";

    [SerializeField]
    public string DestinationSceneName;


    [SerializeField]
    public string SpawnPointName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(RequiredTag)) return;
        UsePortal();
    }

    public virtual void Interact()
    {
        UsePortal();
    }
   protected void UsePortal()
    {
        LoadScene();
    }
    protected void LoadScene()
    {
        SaveSystem.LoadScene(string.IsNullOrEmpty(SpawnPointName) ? DestinationSceneName : DestinationSceneName + "@" + SpawnPointName);
    }

}
