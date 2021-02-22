using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ObjectModel.UIModel;
using Microsoft.Xna.Framework;
using SpaceInvaders.GameScreens;

namespace SpaceInvaders.UIEntities.GameButtons
{
    public class PlayButton : ScreenChangeButton
    {
        private const string k_PlayButtonStaticText = "Play";
        
        public PlayButton(Menu i_ParentMenu, GameScreen i_SourceScreen) 
            : base(i_ParentMenu, i_SourceScreen)
        {
            StaticText = k_PlayButtonStaticText;
        }

        protected override void ScreenChangeButton_Clicked()
        {
            SourceScreen.ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game));
        }
    }
}
