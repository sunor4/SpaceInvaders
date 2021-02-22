using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.UIModel;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.UIEntities.GameButtons.InputGameButtons
{
    public class PlayerNumberButton : InputGameButton<string>
    {
        private const string k_PlayerSettingsButtonText = "Players: ";
        public PlayerNumberButton(Menu i_ParentMenu) : base (i_ParentMenu)
        {
            StaticText = k_PlayerSettingsButtonText;
        }

        protected override void SetValuesInList()
        {
            this.InputList.Add("One");
            this.InputList.Add("Two");
            CurrentInputIndex = (Game as BaseGame).NumOfPlayers - 1;
        }

        protected override void InputGameButton_InputChanged()
        {
            int numOfPlayers = CurrentInputIndex + 1;
            (Game as BaseGame).NumOfPlayers = numOfPlayers;
        }
    }
}
