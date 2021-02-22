using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.UIModel;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.UIEntities.GameButtons.InputGameButtons
{
    public class MouseVisibilityButton : InputGameButton<string>
    {
        private const string k_MouseVisibilityButtonButtonText = "Mouse Visibility: ";
        public MouseVisibilityButton(Menu i_ParentMenu) : base (i_ParentMenu)
        {
            StaticText = k_MouseVisibilityButtonButtonText;
        }

        protected override void SetValuesInList()
        {
            this.InputList.Add("Visible");
            this.InputList.Add("Invisible");
        }

        protected override void InputGameButton_InputChanged()
        {
            const int trueInputIndex = 0;
            bool isMouseVisible = (CurrentInputIndex == trueInputIndex);
            Game.IsMouseVisible = isMouseVisible;
        }
    }
}
