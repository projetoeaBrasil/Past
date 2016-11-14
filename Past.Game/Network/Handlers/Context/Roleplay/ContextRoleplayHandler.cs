﻿using Past.Common.Database.Record;
using Past.Common.Utils;
using Past.Game.Network.Handlers.Basic;
using Past.Game.Network.Handlers.Character;
using Past.Game.Network.Handlers.Inventory;
using Past.Protocol.Messages;
using Past.Protocol.Types;
using System.Linq;

namespace Past.Game.Network.Handlers.Context.Roleplay
{
    public class ContextRoleplayHandler
    {
        public static void HandleMapInformationsRequestMessage(Client client, MapInformationsRequestMessage message)
        {
            client.Send(new MapComplementaryInformationsDataMessage(client.Character.Map.Id, (short)client.Character.Map.SubAreaId, new HouseInformations[0], new GameRolePlayActorInformations[0], new InteractiveElement[0], new StatedElement[0], new MapObstacle[0], new FightCommonInformations[0]));
            client.Character.CurrentMap.AddClient(client);
        }

        public static void HandleChangeMapMessage(Client client, ChangeMapMessage message)
        {
            short cell = client.Character.CellId;
            if (client.Character.Map.TopNeighbourId == message.mapId)
                cell += 532;
            if (client.Character.Map.BottomNeighbourId == message.mapId)
                cell -= 532;
            if (client.Character.Map.LeftNeighbourId == message.mapId)
                cell += 13;
            if (client.Character.Map.RightNeighbourId == message.mapId)
                cell -= 13;
            client.Character.CellId = cell;
            client.Character.CurrentMapId = message.mapId;
            client.Character.CurrentMap.RemoveClient(client);
            client.Send(new CurrentMapMessage(message.mapId));
        }

        public static void HandleEmotePlayRequestMessage(Client client, EmotePlayRequestMessage message)
        {
            BasicHandler.SendTextInformationMessage(client, Protocol.Enums.TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 0, new string[] { message.emoteId.ToString() });
        }

        public static void HandleSpellUpgradeRequestMessage(Client client, SpellUpgradeRequestMessage message)
        {
            CharacterSpellRecord spell = client.Character.Spells.FirstOrDefault(x => x.SpellId == message.spellId);
            if (spell != null && client.Character.SpellsPoints > 0)
            {
                client.Character.SpellsPoints--;
                client.Send(new SpellUpgradeSuccessMessage(spell.SpellId, spell.Level++));
                CharacterHandler.SendCharacterStatsListMessage(client);
                InventoryHandler.SendSpellListMessage(client);
            }
            else
            {
                client.Send(new SpellUpgradeFailureMessage());
            }
        }

        public static void HandleSpellMoveMessage(Client client, SpellMoveMessage message)
        {
            ConsoleUtils.Write(ConsoleUtils.Type.DEBUG, $"{message.spellId}");
        }

        public static void SendEmoteListMessage(Client client, sbyte[] emoteIds)
        {
            client.Send(new EmoteListMessage(emoteIds));
        }
    }
}