using UnityEngine;

public struct Cell // A struct is a copy of the data, not a reference to it
{
    public Vector3Int position;
    public Type type;
    public int number;
    public bool revealed;
    public bool flagged;
    public bool exploded;
    
    public enum Type
    {
        Invalid,
        Empty,
        Mine,
        Number,
    }
}
