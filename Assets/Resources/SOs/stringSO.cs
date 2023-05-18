using UnityEngine;

[CreateAssetMenu]
public class stringSO : ScriptableObject
{
    [SerializeField]
    private string _string;

    public string String
    {
        get
        {
            return _string;
        }
        set
        {
            _string = value;
        }
    }
}