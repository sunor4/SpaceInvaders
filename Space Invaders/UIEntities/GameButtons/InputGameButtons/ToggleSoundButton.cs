using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.UIModel;

namespace SpaceInvaders.UIEntities.GameButtons.InputGameButtons
{
    public class ToggleSoundButton : InputGameButton<string>
    {
        private const string k_ToggleSoundButtonStaticText = "Toggle Sound: ";
        public ToggleSoundButton(Menu i_ParentMenu) : base (i_ParentMenu)
        {
            StaticText = k_ToggleSoundButtonStaticText;
            (Game as BaseGame).SoundManager.MuteChangedIndirectly += ToggleSoundButton_MuteChanged;
        }

        private void ToggleSoundButton_MuteChanged(bool i_IsMute)
        {
            CurrentInputIndex = (i_IsMute) ? 1 : 0;
        }

        protected override void SetValuesInList()
        {
            this.InputList.Add("On");
            this.InputList.Add("Off");
        }

        protected override void InputGameButton_InputChanged()
        {
           (Game as BaseGame).SoundManager.ToggleMute();
        }
    }
}
