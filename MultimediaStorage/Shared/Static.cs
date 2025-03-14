namespace Shared
{
    public static class Static
    {
        private static AppSettings _settings;
        public static AppSettings Settings
        {
            get { return _settings; }
            set
            {
                if (_settings != value)
                {
                    _settings = value;
                }
                else
                    throw new InvalidOperationException("AppSettings can't be re-initialized");
            }
        }
    }
}
