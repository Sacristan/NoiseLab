[System.Serializable]
public class BoundaryInt
{
    public int min;
    public int max;

    public int GetRandomValueWithinBounds()
    {
        return UnityEngine.Random.Range(min, max + 1);
    }
}