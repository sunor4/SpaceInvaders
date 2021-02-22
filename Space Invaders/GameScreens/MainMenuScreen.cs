using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using SpaceInvaders.UIEntities.GameButtons;
using SpaceInvaders.UIEntities.GameButtons.InputGameButtons;
using SpaceInvaders.UIEntities.GameButtons.ScreenChangeButtons;
using System;
using Infrastructure.ObjectModel.UIModel;
using Infrastructure.Screens;

namespace SpaceInvaders.GameScreens
{
    public class MainMenuScreen : MenuGameScreen
    {
        private const string k_HeadlineText = "Main Menu";
        private SoundSettingsScreen m_SoundSettingsScreen;
        private ScreenSettingsScreen m_ScreenSettingsScreen;
        private ScreenSettingsButton m_ScreenSettingsButton;
        private PlayerNumberButton m_PlayerNumberButton;
        private SoundSettingsButton m_SoundSettingsButton;
        private PlayButton m_PlayButton;
        private QuitButton m_QuitButton;

        public MainMenuScreen(Game i_Game) : base(i_Game, k_HeadlineText)
        {
            // Create Screens
            m_SoundSettingsScreen = new SoundSettingsScreen(Game);
            m_ScreenSettingsScreen = new ScreenSettingsScreen(Game);
        }

        protected override void CreateAllButtonsByOrder()
        {
            // Create Buttons
            m_ScreenSettingsButton = new ScreenSettingsButton(ScreenMenu, this, m_ScreenSettingsScreen);
            m_PlayerNumberButton = new PlayerNumberButton(ScreenMenu);
            m_SoundSettingsButton = new SoundSettingsButton(ScreenMenu, this, m_SoundSettingsScreen);
            m_PlayButton = new PlayButton(ScreenMenu, this);
            m_QuitButton = new QuitButton(ScreenMenu);
        }

        protected override void AddComponentsToUI()
        {
            GameUI.Add(new Background(Game));
        }
    }
}
