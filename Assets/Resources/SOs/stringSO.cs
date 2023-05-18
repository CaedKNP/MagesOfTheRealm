using UnityEngine;

[CreateAssetMenu]
public class stringSO : ScriptableObject
{
    [SerializeField]
    private string _string = "RedMage";

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