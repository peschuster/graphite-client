namespace Graphite.Wcf
{
    public sealed class MetricSetting
    {
        public MetricSetting(bool enable)
            : this(enable, null, null)
        {
        }

        public MetricSetting(bool enable, string fixedKey)
            : this(enable, fixedKey, null)
        {
        }

        public MetricSetting(bool enable, string fixedKey, string keyPrefix)
        {
            this.Enable = enable;
            this.FixedKey = fixedKey;
            this.KeyPrefix = keyPrefix;
        }

        public bool Enable { get; set; }

        public string FixedKey { get; set; }

        public string KeyPrefix { get; set; }
    }
}
