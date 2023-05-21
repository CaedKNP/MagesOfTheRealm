using UnityEngine;

namespace Assets.Resources.SOs
{
    [CreateAssetMenu]
    public class intSO : ScriptableObject
    {
        [SerializeField]
        private int _int = 0;

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
}