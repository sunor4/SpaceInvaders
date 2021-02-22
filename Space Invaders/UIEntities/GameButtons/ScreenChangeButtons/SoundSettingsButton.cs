using Infrastructure.ObjectModel.Screens;
using Infrastructure.ObjectModel.UIModel;
using SpaceInvaders.GameScreens;

namespace SpaceInvaders.UIEntities.GameButtons.ScreenChangeButtons
{
    public class SoundSettingsButton : ScreenChangeButton
    {
        private const string k_SoundSettingsButtonStaticText = "Sound Settings";

        public SoundSettingsButton(Menu i_ParentMenu, GameScreen i_SourceScreen, GameScreen i_ScreenToTransitionTo = null) 
            : base(i_ParentMenu, i_SourceScreen, i_ScreenToTransitionTo)
        {
            StaticText = k_SoundSettingsButtonStaticText;
            if (ScreenToTransitionTo == null)
            {
                ScreenToTransitionTo = new SoundSettingsScreen(Game);
            }
        }
    }
}
