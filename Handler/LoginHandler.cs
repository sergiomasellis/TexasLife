﻿using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using TexasLife.Database;
using TexasLife.Events;
using TexasLife.Player;

namespace TexasLife.Handler
{
    public static class TLLoginHandler
    {
        public static void FinishLogin(Client client)
        {
            int client_id = client.GetData("ID");

            TLPlayerStats playerStats = TLDatabase.GetByID<TLPlayerStats>(client_id);

            if(playerStats == null)
            {
                playerStats = new TLPlayerStats();
                playerStats._id = client_id;
                TLDatabase.Upsert(playerStats);
            }

            client.Position = playerStats.GetLastPosition();

            NAPI.Entity.SetEntityTransparency(client, 255);
            NAPI.Entity.SetEntityInvincible(client, false);
            NAPI.ClientEvent.TriggerClientEvent(client, "playerLoggedIn");

            UpdateMoneyEvent.Update_Money(client);
        }
    }
}
