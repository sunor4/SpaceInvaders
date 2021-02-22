using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.UIModel;

namespace SpaceInvaders.UIEntities.GameButtons.InputGameButtons
{
    public class WindowResizingButton : InputGameButton<string>
    {
        private const string k_WindowResizingButtonText = "Allow Window Resizing: ";
        public WindowResizingButton(Menu i_ParentMenu) : base(i_ParentMenu)
        {
            StaticText = k_WindowResizingButtonText;
        }

        protected override void SetValuesInList()
        {
            this.InputList.Add("Off");
            this.InputList.Add("On");
        }

        protected override void InputGameButton_InputChanged()
        {
            const int trueInputIndex = 1;
            bool isResizingAllowed = (CurrentInputIndex == trueInputIndex);
            Game.Window.AllowUserResizing = isResizingAllowed;
        }
    }
}
