using UnityEngine;

public class Places
{
    //Variables
    public Transform position;
    private bool occupied = false;

    public bool Occupied
    {
        get { return occupied; }
        set { occupied = value; }
    }

    public Places(Transform pos)
    {
        position = pos;
        occupied = false;
    }
}