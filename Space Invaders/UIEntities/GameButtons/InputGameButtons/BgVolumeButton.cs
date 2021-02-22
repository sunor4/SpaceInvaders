using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.UIModel;

namespace SpaceInvaders.UIEntities.GameButtons.InputGameButtons
{
    public class BgVolumeButton : InputGameButton<int>
    {
        private const string k_BgVolumeButtonStaticText = "Background Volume: ";
        private const float k_NumOfInputJumps = 10;
        private const float k_MaxInputValue = 100;
        private const float k_ValueOfOneJump = k_MaxInputValue / k_NumOfInputJumps;

        public BgVolumeButton(Menu i_ParentMenu) : base(i_ParentMenu)
        {
            StaticText = k_BgVolumeButtonStaticText;
        }

        protected override void SetValuesInList()
        {
            for (int i = 0; i <= k_NumOfInputJumps; i++)
            {
                InputList.Add((int)(i * k_ValueOfOneJump));
            }
        }

        protected override void SetInitialValue()
        {
            CurrentInputIndex = InputList.Count - 1;
        }

        protected override void InputGameButton_InputChanged()
        {
            float newVolume = (float)(k_ValueOfOneJump * CurrentInputIndex / k_MaxInputValue);
            (Game as BaseGame).SoundManager.SetVolume("BGMusic", newVolume);
        }
    }
}
