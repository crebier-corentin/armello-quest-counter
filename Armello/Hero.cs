using System.Collections.Generic;

namespace ArmelloLogTools.Armello
{
    public enum Hero
    {
        //Wolf
        Thane,
        River,
        Magna,
        Fang,

        //Rat
        Mercurio,
        Zosha,
        Sargon,
        Griotte,

        //Bear
        Sana,
        Brun,
        Ghor,
        Yordana,

        //Rabbit
        Amber,
        Barnaby,
        Elyssia,
        Hagrave,

        //Bandit
        Twiss,
        Sylas,
        Horace,
        Scarlet,

        //Dragon
        Volodar,
        Agniya,
        Oxana,
        Nazar,

        Unknown
    }

    public static class HeroHelpers
    {
        public static readonly Dictionary<string, Hero> HeroesHexId = new Dictionary<string, Hero>()
        {
            //Wolf
            ["0x9AC46BF3"] = Hero.Thane,
            ["0x9AC46BF4"] = Hero.River,
            ["0x9AC46BF5"] = Hero.Magna,
            ["0x9AC46BF6"] = Hero.Fang,
            //Rat
            ["0x04B158C6"] = Hero.Mercurio,
            ["0x04B158C7"] = Hero.Zosha,
            ["0x04B158C8"] = Hero.Sargon,
            ["0x04B158C9"] = Hero.Griotte,
            //Bear
            ["0x765CE8D5"] = Hero.Sana,
            ["0x765CE8D6"] = Hero.Brun,
            ["0x765CE8D7"] = Hero.Ghor,
            ["0x765CE8D8"] = Hero.Yordana,
            //Rabbit
            ["0xFE2E33BB"] = Hero.Amber,
            ["0xFE2E33BC"] = Hero.Barnaby,
            ["0xFE2E33BD"] = Hero.Elyssia,
            ["0xFE2E33BE"] = Hero.Hagrave,
            //Bandit
            ["0x94B1BC41"] = Hero.Twiss,
            ["0x94B1BC42"] = Hero.Sylas,
            ["0x94B1BC43"] = Hero.Horace,
            ["0x94B1BC44"] = Hero.Scarlet,
            //Dragon
            ["0xD1BBEF74"] = Hero.Volodar,
            ["0xD1BBEF75"] = Hero.Agniya,
            ["0xD1BBEF76"] = Hero.Oxana,
            ["0xD1BBEF77"] = Hero.Nazar
        };

        public static Hero HeroFromHexId(string hex)
        {
            return HeroesHexId.TryGetValue(hex, out var hero) ? hero : Hero.Unknown;
        }
    }
}