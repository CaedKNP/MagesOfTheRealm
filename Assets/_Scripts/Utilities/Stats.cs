using System;
using UnityEngine;

namespace Assets._Scripts.Utilities
{
    /// <summary>
    /// Keeping base stats as a struct on the scriptable keeps it flexible and easily editable.
    /// </summary>
    [Serializable]
    public struct Stats
    {
        [SerializeField]
        int _MaxHp;
        public int MaxHp { get { return _MaxHp; } set { _MaxHp = value; } } 

        int _CurrentHp;
        public int CurrentHp { get { return _CurrentHp; } set { _CurrentHp = value; } }

        [SerializeField]
        float _Armor;
        public float Armor { get { return _Armor; } set { _Armor = value; } } //if u getting damaged for some (dmg) u get (dmg) * (armor)

        [SerializeField]
        float _MovementSpeed;
        public float MovementSpeed { get { return _MovementSpeed; } set { _MovementSpeed = value; } }

        [SerializeField]
        float _DmgModifier;
        public float DmgModifier { get { return _DmgModifier; } set { _DmgModifier = value; } }

        [SerializeField]
        float _CooldownModifier;
        public float CooldownModifier { get { return _CooldownModifier; } set { _CooldownModifier = value; } }
    }
}