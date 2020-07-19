using UnityEngine;

public class RandomSpawnTest : MonoBehaviour
{
    public SpawnedObjectWithExtraInfo mainObject;

    public void SpawnInRandomLocation()
    {
        Instantiate(mainObject.gameObject, new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0), Quaternion.identity);
    }

    public void DespawnAll()
    {
        var mainObjects = FindObjectsOfType<SpawnedObjectWithExtraInfo>();
        foreach (var obj in mainObjects)
        {
            Destroy(obj.gameObject);
        }
    }
}
