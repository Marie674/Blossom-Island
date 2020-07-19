using PixelCrushers;

// This class handles extra info on spawned objects.
// The SpawnedObjectManagerWithExtraInfo class is aware of this class.
// The MainObject class is an example subclass of this class.
public abstract class SpawnedObjectWithExtraInfo : SpawnedObject
{
    public abstract string GetExtraInfo();

    public abstract void ApplyExtraInfo(string s);

}

