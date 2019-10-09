namespace ArmelloLogTools.Armello
{
    public struct Player
    {
        public readonly int Id;
        public readonly string Name;
        public readonly Hero Hero;
        public int QuestCount;

        public Player(int id, string name, Hero hero)
        {
            Id = id;
            Name = name;
            Hero = hero;
            QuestCount = 1;
        }

        public Player(int id, string name, string heroHexId)
        {
            Id = id;
            Name = name;
            Hero = HeroHelpers.HeroFromHexId(heroHexId);
            QuestCount = 1;
        }

        public static Player Empty = new Player(-1, "", Hero.Unknown);
    }
}