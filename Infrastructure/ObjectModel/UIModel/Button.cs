using System;
using System.Windows.Forms;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Infrastructure.ObjectModel
{
    public class Button : Sprite
    {
        private Keys m_ClickKeyboardKey = Keys.Enter;
        private eInputButtons m_ClickMouseButton = eInputButtons.Left;
        private bool m_IsHighlighted = false;
        private IInputManager m_InputManager;
        private Color m_OriginalColor;
        private Color m_HighlightColor = Color.Yellow;
        public event EventHandler Highlighted;
        public event Action HighlightChanged;
        public event Action Clicked;
        public event Action PropertyChanged;
        private TextBlock m_ButtonText;

        public string Text
        {
            get { return m_ButtonText.Text; }
            set { m_ButtonText.Text = value; }
        }

        public string StaticText
        {
            get { return m_ButtonText.StaticText; }
            protected set 
            {
                if (value != m_ButtonText.StaticText)
                {
                    m_ButtonText.StaticText = value;
                    if (this.m_IsInitialized)
                    {
                        SetBounds();
                    }
                }
            }
        }

        public eInputButtons ClickMouseButton
        {
            get { return m_ClickMouseButton; }
            protected set { m_ClickMouseButton = value; }
        }

        public Keys ClickKeyboardKey
        {
            get { return m_ClickKeyboardKey; }
            protected set { m_ClickKeyboardKey = value; }
        }

        public bool IsHighlighted
        {
            get { return m_IsHighlighted; }
            set
            {
                if (value != m_IsHighlighted)
                {
                    m_IsHighlighted = value;
                    OnHighlightChanged();
                }
            }
        }

        public Color HighlightColor
        {
            get { return m_HighlightColor; }
            set { m_HighlightColor = value; }
        }

        public IInputManager InputManager
        {
            get { return m_InputManager; }
        }

        public Button(string i_AssetName, Game i_Game) : base(i_AssetName, i_Game)
        {
            m_ButtonText = new TextBlock(Game);
            this.TintColor = m_OriginalColor = m_ButtonText.TintColor;
            subscribeToEvents();
        }

        public Button(Game i_Game) : this(String.Empty, i_Game)
        {

        }

        private void subscribeToEvents()
        {
            HighlightChanged += Button_HighlightChanged;
            Clicked += Button_Clicked;
        }

        protected virtual void Button_Clicked()
        {

        }

        protected virtual void Button_HighlightChanged()
        {
            this.TintColor = IsHighlighted ? HighlightColor : m_OriginalColor;
            if (this.IsHighlighted)
            {
                OnHighlighted();
            }
        }

        public override void Initialize()
        {
            m_ButtonText.Initialize();
            base.Initialize();
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
        }

        protected override void InitBounds()
        {
            if (Texture != null)
            {
                base.InitBounds();
            }
            else
            {
                SetBounds();
                base.InitSourceRectangle();
                InitOrigins();
            }
        }

        // Set the button origin to be in the center
        protected override void InitOrigins()
        {
            PositionOrigin = (Texture != null) ? TextureCenter : m_ButtonText.TextCenter;
        }

        protected override void SetBounds()
        {
            if (this.Texture != null)
            {
                base.SetBounds();
            }
            else
            {
                WidthBeforeScale = m_ButtonText.WidthBeforeScale;
                HeightBeforeScale = m_ButtonText.HeightBeforeScale;
                PositionOrigin = m_ButtonText.PositionOrigin;
            }
        }

        private void OnClicked()
        {
            if (Clicked != null)
            {
                Clicked.Invoke();
            }
        }

        private void OnHighlightChanged()
        {
            if (HighlightChanged != null)
            {
                HighlightChanged.Invoke();
            }
        }

        // Only handles cases of highlighting for outer subscribers
        private void OnHighlighted()
        {
            if (Highlighted != null)
            {
                Highlighted.Invoke(this, EventArgs.Empty);
            }
        }

        // Handles case of changing a property the buttion is in charge of (sound volume,
        // number of players, etc...).
        private void OnPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke();
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_ButtonText.Update(i_GameTime);
            Rectangle mouseBounds = InputManager.MouseBounds;
            bool isMouseIntersects = mouseBounds.Intersects(this.Bounds);

            if (InputManager.DidMouseMoveThisFrame)
            {
                getMouseInput(isMouseIntersects);
            }

            bool mouseClicked = InputManager.ButtonPressed(ClickMouseButton) && isMouseIntersects;
            bool keyboardPressed = InputManager.KeyPressed(ClickKeyboardKey);

            if (IsHighlighted && (mouseClicked || keyboardPressed))
            {
                OnClicked();
            }
        }

        private void getMouseInput(bool i_IsIntersects)
        {
            bool shouldChangeState = /*(IsHighlighted && !i_IsIntersects) || */(!IsHighlighted && i_IsIntersects);

            if (shouldChangeState)
            {
                IsHighlighted = true;
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
            SpriteBatch.DrawString(m_ButtonText.Font, m_ButtonText.TextToDraw, PositionForDraw, TintColor, Rotation,
                RotationOrigin, Scales,
                SpriteEffects, LayerDepth);
        }
    }
}
