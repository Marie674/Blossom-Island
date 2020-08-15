using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneWarp : MonoBehaviour
{
    public string SceneName;
    public string SpawnPoint;

    public PlayerCharacter.CharacterDirection Facing;

    void OnTriggerEnter2D(Collider2D pOther)
    {
        if (pOther.isTrigger == false && pOther.gameObject.tag == "Player")
        {
            Warp();
        }
    }

    protected void Warp()
    {
        GameManager.Instance.LoadScene(SceneName, SpawnPoint, Facing);
    }
}
