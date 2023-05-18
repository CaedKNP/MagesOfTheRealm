namespace Assets._Scripts.Utilities
{
    public struct ConditionBase
    {
        public ConditionBase(Conditions conditions, float affectTime, float affectOnTick)
        {
            Conditions = conditions;
            AffectOnTick = affectOnTick;
            AffectTime = affectTime;
        }

        public Conditions Conditions { get; set; }

        public float AffectTime { get; set; }

        public float AffectOnTick { get; set;}
    }
}