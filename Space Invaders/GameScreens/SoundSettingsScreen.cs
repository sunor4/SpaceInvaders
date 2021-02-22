using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.Screens;
using Microsoft.Xna.Framework;
using SpaceInvaders.UIEntities.GameButtons.InputGameButtons;
using SpaceInvaders.UIEntities.GameButtons.ScreenChangeButtons;

namespace SpaceInvaders.GameScreens
{
    public class SoundSettingsScreen : MenuGameScreen
    {
        private const string k_HeadlineText = "Sound Settings";
        private ToggleSoundButton m_ToggleSoundButton;
        private BgVolumeButton m_BgVolumeButton;
        private SeVolumeButton m_SeVolumeButton;
        private DoneButton m_DoneButton;
        
        public SoundSettingsScreen(Game i_Game) : base (i_Game, k_HeadlineText)
        {
            
        }

        protected override void CreateAllButtonsByOrder()
        {
            m_ToggleSoundButton = new ToggleSoundButton(ScreenMenu);
            m_BgVolumeButton = new BgVolumeButton(ScreenMenu);
            m_SeVolumeButton = new SeVolumeButton(ScreenMenu);
            m_DoneButton = new DoneButton(ScreenMenu, this);
        }

        protected override void AddComponentsToUI()
        {
            GameUI.Add(new Background(Game));
        }
    }
}
