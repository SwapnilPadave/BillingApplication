namespace BA.Entities.Infra
{
    [AttributeUsage(AttributeTargets.Class |
                     AttributeTargets.Struct
      | AttributeTargets.Field | AttributeTargets.Property)]
    public class IncludeAttribute : Attribute
    {
        public IncludeAttribute()
        {
        }
    }
}
