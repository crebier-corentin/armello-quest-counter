namespace ArmelloLogTools.Armello
{
    public abstract class Event
    {
        public abstract EventType Type { get; }
    }

    public class LoadGameEvent : Event
    {
        public override EventType Type => EventType.LoadGame;
    }

    public class LoadPlayerEvent : Event
    {
        public override EventType Type => EventType.LoadPlayer;

        public Player Player;

        public LoadPlayerEvent(int id, string name, string heroHexId)
        {
            Player = new Player(id, name, heroHexId);
        }
    }

    public class StartTurnEvent : Event
    {
        public override EventType Type => EventType.StartTurn;

        public int PlayerId;

        public StartTurnEvent(int playerId)
        {
            PlayerId = playerId;
        }
    }

    public class CompleteQuestEvent : Event
    {
        public override EventType Type => EventType.CompleteQuest;
    }
}