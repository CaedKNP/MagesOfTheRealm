using System;
using UnityEngine;

namespace Assets._Scripts.Utilities
{
    [Serializable]
    public struct ConditionBase
    {
        public ConditionBase(Conditions conditions, float affectTime, float affectOnTick)
        {
            Conditions = conditions;
            AffectOnTick = affectOnTick;
            AffectTime = affectTime;
        }

        [SerializeField]
        public Conditions Conditions;

        [SerializeField]
        public float AffectTime;

        [SerializeField]
        public float AffectOnTick;
    }
}