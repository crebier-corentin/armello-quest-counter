using System.Collections.Generic;
using ArmelloLogTools.Armello;

namespace ArmelloLogTools
{
    public class Interpreter
    {
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>(4);

        public IReadOnlyDictionary<int, Player> Players => _players;

        private int _currentPlayerId = 0;

        private Player CurrentPlayer
        {
            get => _players[_currentPlayerId];
            set => _players[_currentPlayerId] = value;
        }

        public void ProcessEvents(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                ProcessEvent(@event);
            }
        }

        private void ProcessEvent(Event @event)
        {
            switch (@event.Type)
            {
                //New game, reset players
                case EventType.LoadGame:
                    _players.Clear();
                    _currentPlayerId = 0;
                    break;

                //Add player to _players
                case EventType.LoadPlayer:
                    var playerEvent = (LoadPlayerEvent) @event;
                    _players[playerEvent.Player.Id] = playerEvent.Player;
                    break;

                //New turn, update currentPlayerId
                case EventType.StartTurn:
                    var turnEvent = (StartTurnEvent) @event;
                    _currentPlayerId = turnEvent.PlayerId;
                    break;

                //Quest complete, increment player's quest counter
                case EventType.CompleteQuest:
                    var player = CurrentPlayer;
                    player.QuestCount++;
                    CurrentPlayer = player;
                    break;
            }
        }
    }
}