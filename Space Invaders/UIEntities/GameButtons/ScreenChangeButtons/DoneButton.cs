using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ObjectModel.UIModel;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.UIEntities.GameButtons.ScreenChangeButtons
{
    public class DoneButton : ScreenChangeButton
    {
        private const string k_DoneButtonStaticText = "Done";

        public DoneButton(Menu i_ParentMenu, GameScreen i_SourceScreen) : base (i_ParentMenu, i_SourceScreen)
        {
            StaticText = k_DoneButtonStaticText;
        }
        
        protected override void ScreenChangeButton_Clicked()
        {
            this.SourceScreen.ExitScreen();
        }
    }
}
