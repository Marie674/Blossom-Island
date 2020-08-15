using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
public class RandomXFlip : MonoBehaviour
{

    bool IsInit = false;
    bool Flipped = false;
    void Start()
    {
        if (DialogueLua.DoesVariableExist(gameObject.name + transform.position.x + transform.position.y + SceneManager.GetActiveScene().name + "Init"))
        {
            IsInit = DialogueLua.GetVariable(gameObject.name + transform.position.x + transform.position.y + SceneManager.GetActiveScene().name + "Init").asBool;

        }
        if (IsInit == false)
        {
            Init();
        }
        if (DialogueLua.DoesVariableExist(gameObject.name + transform.position.x + transform.position.y + SceneManager.GetActiveScene().name + "Flipped"))
        {
            Flipped = DialogueLua.GetVariable(gameObject.name + transform.position.x + transform.position.y + SceneManager.GetActiveScene().name + "Flipped").asBool;

        }
        if (Flipped == true)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void Init()
    {
        IsInit = true;
        DialogueLua.SetVariable(gameObject.name + transform.position.x + transform.position.y + SceneManager.GetActiveScene().name + "Init", IsInit);

        int rand = Random.Range(0, 2);
        if (rand == 1)
        {
            Flipped = true;
            DialogueLua.SetVariable(gameObject.name + transform.position.x + transform.position.y + SceneManager.GetActiveScene().name + "Flipped", Flipped);
        }

    }

}
