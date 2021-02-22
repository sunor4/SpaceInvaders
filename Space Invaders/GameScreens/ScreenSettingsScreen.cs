using Infrastructure.Screens;
using Microsoft.Xna.Framework;
using SpaceInvaders.UIEntities.GameButtons.InputGameButtons;
using SpaceInvaders.UIEntities.GameButtons.ScreenChangeButtons;

namespace SpaceInvaders.GameScreens
{
    public class ScreenSettingsScreen : MenuGameScreen
    {
        private const string k_HeadlineText = "Screen Settings";
        private WindowResizingButton m_WindowResizingButton;
        private FullScreenButton m_FullScreenButton;
        private MouseVisibilityButton m_MouseVisibilityButton;
        private DoneButton m_DoneButton;
        
        public ScreenSettingsScreen(Game i_Game) : base (i_Game, k_HeadlineText)
        {

        }

        protected override void CreateAllButtonsByOrder()
        {
            m_WindowResizingButton = new WindowResizingButton(ScreenMenu);
            m_FullScreenButton = new FullScreenButton(ScreenMenu);
            m_MouseVisibilityButton = new MouseVisibilityButton(ScreenMenu);
            m_DoneButton = new DoneButton(ScreenMenu, this);
        }

        protected override void AddComponentsToUI()
        {
            GameUI.Add(new Background(Game));
        }
    }
}
