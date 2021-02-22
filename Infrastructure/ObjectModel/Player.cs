using System;
using Infrastructure.ObjectModel.UIModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{

    // This reusable class is a representation of a player in a game.
    public abstract class Player : Sprite
    {
        protected IInputManager m_InputManager;
        protected readonly Score r_Score;
        protected readonly LivesBar r_LivesBar;

        public LivesBar PlayerLivesBar
        {
            get { return r_LivesBar; }
        }

        public Score PlayerScore
        {
            get { return r_Score; }
        }

        protected Player(string i_AssetName, Game i_Game) : base(i_AssetName, i_Game)
        {
            r_Score = new Score(Game);
            r_LivesBar = new LivesBar(Game, this);
            Hit += PlayerLivesBar.RemoveNextLife;
        }
    }
}
