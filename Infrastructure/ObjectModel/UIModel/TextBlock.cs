using System;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Infrastructure.ObjectModel
{
    public class TextBlock : Sprite
    {
        private const string k_FontAssetName = @"Fonts\Consolas";
        private SpriteFont m_Font;
        private string m_Text = "";
        private string m_StaticText = "";
        private bool m_CenterText = false;
        public event Action TextChanged;

        public Vector2 TextCenter
        {
            get
            {
                Vector2 stringSize = m_Font.MeasureString(TextToDraw) * Scales;
                return stringSize / 2;
            }
        }

        // Text that would always appear before the actual Text. 
        public string StaticText
        {
            get { return m_StaticText; }
            set { m_StaticText = value; }
        }

        public string Text
        {
            get { return m_Text; } 
            set
            {
                if (m_Text != value)
                {
                    m_Text = value;
                    if (this.m_IsInitialized)
                    {
                        OnTextChanged();
                    }
                }
            }
        }

        public string TextToDraw
        {
            get { return m_StaticText + m_Text; }
        }

        public SpriteFont Font
        {
            get { return m_Font; }
            private set { m_Font = value; }
        }
        public TextBlock(Game i_Game, string i_TextToShow, Vector2 i_Position, bool i_CenterText = false) : base(k_FontAssetName, i_Game)
        {
            TintColor = Color.HotPink;
            Text = i_TextToShow;
            Position = i_Position;
            m_CenterText = i_CenterText;
        }

        public TextBlock(Game i_Game, string i_TextToShow, bool i_CenterText = false) : this(i_Game, i_TextToShow, Vector2.Zero, i_CenterText)
        {

        }

        public TextBlock(Game i_Game, bool i_CenterText = false) : this(i_Game, string.Empty, Vector2.Zero, i_CenterText)
        {
            
        }

        protected override void InitBounds()
        {
            SetTextBounds();
        }

        protected override void LoadContent()
        {
            Font = ContentManager.Load<SpriteFont>(AssetName);
            
            if (m_SpriteBatch == null)
            {
                m_SpriteBatch =
                    Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    m_UseSharedBatch = false;
                }
            }
        }

        protected virtual void SetTextBounds()
        {
            Vector2 stringSize = Font.MeasureString(TextToDraw);
            WidthBeforeScale = stringSize.X;
            HeightBeforeScale = stringSize.Y;
            
            if (m_CenterText)
            {
                PositionOrigin = TextCenter;
            }
        }

        public void OnTextChanged()
        {
            SetTextBounds();
            if (TextChanged != null)
            {
                TextChanged.Invoke();
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
            SpriteBatch.DrawString(Font, TextToDraw, PositionForDraw, TintColor, Rotation, RotationOrigin, Scales,
                SpriteEffects, LayerDepth);
        }
    }
}
