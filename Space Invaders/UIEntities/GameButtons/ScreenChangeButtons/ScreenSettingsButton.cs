using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ObjectModel.UIModel;
using Microsoft.Xna.Framework;
using SpaceInvaders.GameScreens;

namespace SpaceInvaders.UIEntities.GameButtons.ScreenChangeButtons
{
    public class ScreenSettingsButton : ScreenChangeButton
    {
        private const string k_ScreenSettingsButtonStaticText = "Screen Settings";

        public ScreenSettingsButton(Menu i_ParentMenu, GameScreen i_SourceScreen, GameScreen i_ScreenToTransitionTo = null) 
            : base(i_ParentMenu, i_SourceScreen, i_ScreenToTransitionTo)
        {
            StaticText = k_ScreenSettingsButtonStaticText;
            if (ScreenToTransitionTo == null)
            {
                ScreenToTransitionTo = new ScreenSettingsScreen(Game);
            }
        }
    }
}
