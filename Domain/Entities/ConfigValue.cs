namespace FriMav.Domain.Entities
{
    public enum ConfigurationType
    {
        Int,
        String,
        Decimal,
    }

    public class ConfigValue : Entity
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public ConfigurationType Type { get; set; }
        public int IntValue { get; set; }
        public string StringValue { get; set; }
        public decimal DecimalValue { get; set; }
    }
}
