using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.UIModel;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.UIEntities.GameButtons
{
    public class QuitButton : MenuButton
    {
        private const string k_QuitButtonStaticText = "Quit";
        
        public QuitButton(Menu i_ParentMenu) : base(i_ParentMenu)
        {
            StaticText = k_QuitButtonStaticText;
            Clicked += QuitButton_Clicked;
        }

        private void QuitButton_Clicked()
        {
            Game.Exit();
        }
    }
}
