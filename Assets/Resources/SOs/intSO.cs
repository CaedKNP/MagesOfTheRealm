using UnityEngine;

[CreateAssetMenu]
public class intSO : ScriptableObject
{
    [SerializeField]
    private int _int;

    public int Int
    {
        get
        {
            return _int;
        }
        set
        {
            _int = value;
        }
    }
}