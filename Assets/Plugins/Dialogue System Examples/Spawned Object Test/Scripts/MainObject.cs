using UnityEngine;

// This subclass of SpawnedObjectWithExtraInfo does a few things:
// 1. Records extra info (whether it has a child or not).
// 1. If clicked, it instantiates a child GameObject.
public class MainObject : SpawnedObjectWithExtraInfo
{
    public GameObject childPrefab;

    private GameObject myChild = null; // My child GameObject, if any.

    public override string GetExtraInfo()
    {
        return (myChild != null) ? "1" : "0"; // Currently just return 1 or 0 depending on whether object has a child.
    }

    public override void ApplyExtraInfo(string s)
    {
        if (s == "1") InstantiateChild();
    }

    public void InstantiateChild()
    {
        if (myChild == null) myChild = Instantiate(childPrefab, transform);
    }

    private void OnMouseDown()
    {
        InstantiateChild();
    }
}
