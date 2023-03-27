using System;
using System.ComponentModel;

namespace Assets._Scripts.Utilities
{
    [Serializable]
    public struct Stats
    {
        [DefaultValue(5)]
        public int MaxHp { get; set; }

        [DefaultValue(5)]
        public int CurrentHp { get; set; }

        [DefaultValue(1.0f)]
        public float Armor { get; set; } //if u getting damaged for some (dmg) u get (dmg) * (armor)

        [DefaultValue(1.0f)]
        public float MovementSpeed { get; set; }

        [DefaultValue(1.0f)]
        public float DmgModifier { get; set; }

        [DefaultValue(1.0f)]
        public float CooldownModifier { get; set; }
    }
}
