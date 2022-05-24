namespace ConsoleScratch.Options
{
    internal class GameConfigurations
    {
        public const string Guest = nameof(Guest);
        public const string Personalize = nameof(Personalize);

        public int PlayerCount { get; set; }
        public bool ControllerSupport { get; set; }
        public string PlayerName { get; set; }

    }
}
