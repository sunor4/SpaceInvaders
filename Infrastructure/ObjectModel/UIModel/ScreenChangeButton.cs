using Infrastructure.ObjectModel.Screens;
using SpaceInvaders;
using System;

namespace Infrastructure.ObjectModel.UIModel
{
    public class ScreenChangeButton : MenuButton
    {
        private const string k_SoundString = "MenuMove";
        private readonly GameScreen r_SourceScreen;
        private GameScreen m_ScreenToTransitionTo;

        protected GameScreen ScreenToTransitionTo
        {
            get { return m_ScreenToTransitionTo; }
            set { m_ScreenToTransitionTo = value; }
        }

        protected GameScreen SourceScreen
        {
            get { return r_SourceScreen; }
        }

        public ScreenChangeButton(Menu i_ParentMenu, GameScreen i_SourceScreen, GameScreen i_ScreenToTransitionTo = null) 
            : base(i_ParentMenu)
        {
            r_SourceScreen = i_SourceScreen;
            m_ScreenToTransitionTo = i_ScreenToTransitionTo;
            Clicked += ScreenChangeButton_Clicked;
        }

        protected virtual void ScreenChangeButton_Clicked()
        {
            SourceScreen.ScreensManager.SetCurrentScreen(ScreenToTransitionTo);
        }
    }
}
