using System;

namespace Assets._Scripts.Utilities
{
    [Serializable]
    public struct ConditionBase
    {
        public ConditionBase(Conditions conditions, float affectTime, float affectOnTick)
        {
            Condition = conditions;
            AffectOnTick = affectOnTick;
            AffectTime = affectTime;
        }

        public Conditions Condition;

        public float AffectTime;

        public float AffectOnTick;
    }
}