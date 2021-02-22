using Infrastructure.ObjectModel.Screens;
using Infrastructure.ObjectModel.UIModel;
using Microsoft.Xna.Framework;
using System;

namespace Infrastructure.Screens
{
    public abstract class MenuGameScreen : GameScreen
    {
        private Menu r_ScreenMenu;

        public Menu ScreenMenu
        {
            get { return r_ScreenMenu; }
        }

        public MenuGameScreen(Game i_Game, string i_HeadlineText) : base (i_Game)
        {
            this.StateChanged += MenuGameScreen_StateChanged;
            r_ScreenMenu = new Menu(Game, i_HeadlineText);
            createMenuButtons();
            AddExtraComponents();
            this.Add(r_ScreenMenu);
        }

        public MenuGameScreen(Game i_Game) : this(i_Game, string.Empty)
        {

        }

        private void MenuGameScreen_StateChanged(object sender, StateChangedEventArgs e)
        {
            this.ScreenMenu.CurrentHighlightedButtonIndex = 0;
        }

        protected override void CompositeDrawableComponent_ResolutionChanged()
        {
            SetInitialMenuPosition();
            r_ScreenMenu.Initialize();
        }

        protected virtual void AddExtraComponents()
        {
        }

        private void createMenuButtons()
        {
            CreateAllButtonsByOrder();
            SetInitialMenuPosition();
        }

        protected virtual void SetInitialMenuPosition()
        {
            Vector2 menuPosition = this.CenterOfViewport - new Vector2(0, 90f);
            r_ScreenMenu.InitialPosition = menuPosition;
        }

        protected abstract void CreateAllButtonsByOrder();
    }
}
