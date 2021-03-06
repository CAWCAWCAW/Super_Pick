﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hooks;
using TShockAPI;
using Terraria;

namespace SuperPick
{
    [APIVersion( 1,12)]
    public class SuperPick : TerrariaPlugin
    {
        private bool[] players;

        public override string Author
        {
            get { return "Zack"; }
        }

        public override string Description
        {
            get { return "Instant Mining"; }
        }

        public override string Name
        {
            get { return "Super Pick"; }
        }

        public override Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        public SuperPick(Main game)
            : base(game)
        {
            players = new bool[byte.MaxValue];
            for( int i = 0; i < players.Length; i++ )
            {
                players[i] = false;
            }
        }

        public override void Initialize()
        {
            Hooks.ServerHooks.Leave += OnLeave;
            GetDataHandlers.TileEdit += OnTileEdit;
            Commands.ChatCommands.Add( new Command("superpick", Toggle, "superpick", "sp"));
        }

        private void OnLeave( int who )
        {
            players[who] = false;
        }

        private void OnTileEdit( object sender, GetDataHandlers.TileEditEventArgs args )
        {
            if (players[args.Player.Index])
            {
                switch( args.EditType )
                {
                    case 0:
                    case 4:
                        WorldGen.KillTile(args.X, args.Y);
                        TSPlayer.All.SendTileSquare(args.X, args.Y, 1);
                        break;
                    case 2:
                        WorldGen.KillWall(args.X, args.Y);
                        TSPlayer.All.SendTileSquare(args.X, args.Y, 1);
                        break;
                    case 6:
                        WorldGen.KillWire(args.X, args.Y);
                        TSPlayer.All.SendTileSquare(args.X, args.Y, 1);
                        break;
                }
            }
        }

        private void Toggle( CommandArgs args )
        {
            players[args.Player.Index] = (!players[args.Player.Index]);
            args.Player.SendMessage(String.Format("You have {0} superpick.", (players[args.Player.Index] ? "enabled" : "disabled")), Color.Green);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerHooks.Leave -= OnLeave;
                GetDataHandlers.TileEdit -= OnTileEdit;
            }

            base.Dispose(disposing);
        }
    }
}
