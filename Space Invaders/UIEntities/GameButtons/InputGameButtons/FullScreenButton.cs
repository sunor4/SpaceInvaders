using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.UIModel;

namespace SpaceInvaders.UIEntities.GameButtons.InputGameButtons
{
    public class FullScreenButton : InputGameButton<string>
    {
        private const string k_FullScreenButtonStaticText = "Full Screen: ";
        public FullScreenButton(Menu i_ParentMenu) : base(i_ParentMenu)
        {
            StaticText = k_FullScreenButtonStaticText;
        }

        protected override void SetValuesInList()
        {
            this.InputList.Add("Off");
            this.InputList.Add("On");
        }

        protected override void InputGameButton_InputChanged()
        {
            (Game as BaseGame).ResolutionManager.ToggleFullScreen();
        }
    }
}
