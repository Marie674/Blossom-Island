using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "AddressableSpriteLoader", menuName = ("Data/AddressableSpriteLoader"))]
public class AddressableSpriteLoader : ScriptableObject
{
    private Sprite TargetSprite;
    public void LoadSprite(string pAddress, Sprite pTarget)
    {
        TargetSprite = pTarget;
        Addressables.LoadAssetAsync<Sprite>(pAddress).Completed += SpriteLoaded;
    }

    private void SpriteLoaded(AsyncOperationHandle<Sprite> pObj)
    {
        switch (pObj.Status)
        {
            case AsyncOperationStatus.Succeeded:
                SetSprite(pObj.Result);
                break;
            case AsyncOperationStatus.Failed:
                Debug.LogError("Sprite failed to load");
                break;
            default:
                break;
        }
    }

    private void SetSprite(Sprite pSprite)
    {
        if (TargetSprite != null)
        {
            TargetSprite = pSprite;
        }
        else
        {
            Debug.LogError("No valid renderer");
        }
    }
}
