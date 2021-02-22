using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Infrastructure.ObjectModel.UIModel
{
    public class LivesBar : CompositeDrawableComponent<Sprite>
    {
        private const float k_TextureOpacityXY = 0.5f;
        private readonly Vector2 r_TextureScales = new Vector2(0.5f);
        private readonly Sprite r_Player;
        private Sprite m_PlayerClone;
        private Vector2 m_Position;
        public event Action LivesChanged;

        public Sprite DefaultPlayer
        {
            get { return m_PlayerClone; }
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public LivesBar(Game i_Game, Player i_Player) : base(i_Game)
        {
            this.SpritesSortMode = SpriteSortMode.Immediate;
            this.BlendState = BlendState.NonPremultiplied;
            r_Player = i_Player;
        }

        public override void Initialize()
        {
            base.Initialize();
            createDefaultPlayer();
            initializeLives();
            setLivesPositions();
        }

        private void createDefaultPlayer()
        {
            m_PlayerClone = r_Player.ShallowClone();
            m_PlayerClone.Enabled = false;
            m_PlayerClone.Opacity = k_TextureOpacityXY;
            m_PlayerClone.Scales = r_TextureScales;
        }

        private void initializeLives()
        {
            for (int i = 0; i < r_Player.Lives; i++)
            {
                Sprite currentPlayerShip = DefaultPlayer.ShallowClone();
                this.Add(currentPlayerShip);
            }
        }

        private void setLivesPositions()
        {
            float startingXPos = Position.X;
            float startingYPos = Position.Y;
            float distanceBetweenLives = DefaultPlayer.Bounds.Width * 1.5f;
            for (int i = 0; i < this.Count; i++)
            {
                this.Components[i].Position = new Vector2(startingXPos - (i * distanceBetweenLives), startingYPos);
            }
        }

        public void ResetLivesPosition()
        {
            setLivesPositions();
        }

        public void RemoveNextLife()
        {
            Sprite playerShipToDispose = this.Components[r_Player.Lives - 1];
            playerShipToDispose.Visible = false;
            this.Remove(playerShipToDispose);
        }

        private void OnLivesChanged()
        {
            RemoveNextLife();

            if (LivesChanged != null)
            {
                LivesChanged.Invoke();
            }
        }
    }
}
