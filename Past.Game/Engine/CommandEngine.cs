﻿using System.Linq;
using Past.Common.Data;
using Past.Game.Network;
using Past.Game.Network.Handlers.Basic;
using Past.Protocol.Enums;

namespace Past.Game.Engine
{
    public class CommandEngine
    {
        
        public static void Handle(Client client, string content)
        {
            string[] command = content.Split(' ');
            switch (command[0])
            {
                case ".help":
                    Command.Commands.Where(cmd => cmd.Value.Role <= client.Account.Role).ToList().ForEach(cmd => BasicHandler.SendTextInformationMessage(client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 16, new[] { $"{cmd.Value.Name}", $"{cmd.Value.Description}" }));
                    break;
                case ".save":
                    client.Character.Save();
                    break;
                case ".start":
                    client.Character.Teleport(client.Character.BreedData.StartMapId, client.Character.BreedData.StartDisposition.cellId);
                    break;
                case ".goname":
                    if (client.Account.Role >= GameHierarchyEnum.MODERATOR)
                    {
                        Client targetClient = Server.Clients.FirstOrDefault(target => target.Character.Name == command[1]);
                        if (targetClient != null && targetClient != client)
                        {
                            client.Character.Teleport(targetClient.Character.CurrentMapId, targetClient.Character.CellId);
                        }
                        else
                        {
                            BasicHandler.SendTextInformationMessage(client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 16, new[] { "Error", $"Can't found the character {command[1]} !" });
                        }
                    }
                    break;
                case ".go":
                    if (client.Account.Role >= GameHierarchyEnum.MODERATOR)
                    {
                        Map map = Map.Maps.FirstOrDefault(findMap => findMap.Value.Id == int.Parse(command[1])).Value;
                        if (map != null && map.Id != client.Character.CurrentMapId)
                        {
                            client.Character.Teleport(map.Id, client.Character.CellId);
                        }
                        else
                        {
                            BasicHandler.SendTextInformationMessage(client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 16, new[] { "Error", $"Can't found the map {command[1]} !" });
                        }
                    }
                    break;
                case ".levelup":
                    if (client.Account.Role >= GameHierarchyEnum.GAMEMASTER_PADAWAN)
                    {
                        client.Character.LevelUp();
                    }
                    break;
                default:
                    BasicHandler.SendTextInformationMessage(client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 16, new[] { "Error", $"Command {command[0]} not found !" });
                    break;
            }
        }
    }
}