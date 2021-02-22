// *** Guy Ronen © 2008-2011 ***//
// We were given permission to use this file as a part of XNA Monogame summer course IDC 2020 //

using System;
using System.Numerics;
using Infrastructure.ObjectModel.Animators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ServiceInterfaces;
using SpaceInvaders;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Infrastructure.ObjectModel
{
    public class Sprite : LoadableDrawableComponent
    {
        public event Action Death;

        public event Action Hit;

        private int m_Lives = 1;
        private bool m_IsAlreadyDead = false;

        public bool IsAlreadyDead
        {
            get { return m_IsAlreadyDead; }
            private set
            {
                if (value == true)
                {
                    zeroBounds();
                }
                else 
                {
                    SetBounds();
                }

                m_IsAlreadyDead = value;
            }
        }

        protected int m_MaxTextureCells = 1;
        private int m_CurrentCellIndex = 0;

        public int CurrentCellIndex
        {
            get { return m_CurrentCellIndex; }
            set
            {
                if (value > m_MaxTextureCells)
                {
                    m_CurrentCellIndex = value % m_MaxTextureCells;
                }
                else
                {
                    m_CurrentCellIndex = value;
                }
            }
        }

        public virtual int Lives
        {
            get { return m_Lives; }
            set
            {
                m_Lives = MathHelper.Clamp(value, 0, int.MaxValue);

                // If we're dead, we assume there is not way to heal.
                if (m_Lives == 0)
                {
                    IsAlreadyDead = true;
                    OnDeath();
                }
            }
        }

        protected CompositeAnimator m_Animations;

        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        private Color[] m_Pixels;

        public Color[] Pixels
        {
            get 
            {
                if (m_Pixels == null)
                {
                    m_Pixels = new Color[Texture.Width * Texture.Height];
                    Texture.GetData(m_Pixels);
                    Texture = new Texture2D(Game.GraphicsDevice, Texture.Width, Texture.Height);
                    Texture.SetData(m_Pixels);
                }

                return m_Pixels;
            }
        }

        private Texture2D m_Texture;

        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)Width,
                    (int)Height);
            }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        protected Rectangle m_SourceRectangle;

        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(m_Texture.Width / 2), (float)(m_Texture.Height / 2));
            }
        }

        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(m_SourceRectangle.Width / 2), (float)(m_SourceRectangle.Height / 2)); }
        }

        protected float m_Rotation = 0;

        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        protected Vector2 m_Scales = Vector2.One;

        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;

                    // Notify the Collision Detection mechanism:
                    OnPositionChanged();
                }
            }
        }

        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set
            {
                if (m_WidthBeforeScale != value)
                {
                    m_WidthBeforeScale = value / m_Scales.X;
                    OnSizeChanged();
                }
            }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set
            {
                if (m_HeightBeforeScale != value)
                {
                    m_HeightBeforeScale = value / m_Scales.Y;
                    OnSizeChanged();
                }
            }
        }

        protected float m_WidthBeforeScale;

        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        protected float m_HeightBeforeScale;

        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }

        protected Vector2 m_Position = Vector2.Zero;

        public Vector2 Position
        {
            get { return m_Position; }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    OnPositionChanged();
                }
            }
        }

        public Vector2 m_PositionOrigin = Vector2.Zero;

        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        public Vector2 m_RotationOrigin = Vector2.Zero;

        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        protected Vector2 PositionForDraw
        {
            get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
        }

        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

        protected Color m_TintColor = Color.White;

        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        protected float m_LayerDepth;

        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        protected SpriteEffects m_SpriteEffects = SpriteEffects.None;

        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }

        protected SpriteSortMode m_SortMode = SpriteSortMode.Immediate;
        public SpriteSortMode SortMode
        {
            get { return m_SortMode; }
            set { m_SortMode = value; }
        }

        protected BlendState m_BlendState = BlendState.NonPremultiplied;
        public BlendState BlendState
        {
            get { return m_BlendState; }
            set { m_BlendState = value; }
        }

        protected SamplerState m_SamplerState = null;
        public SamplerState SamplerState
        {
            get { return m_SamplerState; }
            set { m_SamplerState = value; }
        }

        protected DepthStencilState m_DepthStencilState = null;
        public DepthStencilState DepthStencilState
        {
            get { return m_DepthStencilState; }
            set { m_DepthStencilState = value; }
        }

        protected RasterizerState m_RasterizerState = null;
        public RasterizerState RasterizerState
        {
            get { return m_RasterizerState; }
            set { m_RasterizerState = value; }
        }

        protected Effect m_Shader = null;
        public Effect Shader
        {
            get { return m_Shader; }
            set { m_Shader = value; }
        }

        protected Matrix m_TransformMatrix = Matrix.Identity;
        public Matrix TransformMatrix
        {
            get { return m_TransformMatrix; }
            set { m_TransformMatrix = value; }
        }

        protected Vector2 m_Velocity = Vector2.Zero;

        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        private float m_AngularVelocity = 0;

        /// <summary>
        /// Radians per Second on X Axis
        /// </summary>
        /// 
        public float AngularVelocity
        {
            get { return m_AngularVelocity; }
            set { m_AngularVelocity = value; }
        }

        protected Vector2 CenterOfViewport
        {
            get
            {
                return new Vector2((float)Game.GraphicsDevice.Viewport.Width / 2, (float)Game.GraphicsDevice.Viewport.Height / 2);
            }
        }

        public Sprite(string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
            (Game as BaseGame).ResolutionManager.ResolutionChanged += Sprite_ResolutionChanged;
        }

        public Sprite(string i_AssetName, Game i_Game, int i_CallsOrder)
            : this(i_AssetName, i_Game, i_CallsOrder, i_CallsOrder)
        {
        }

        public Sprite(string i_AssetName, Game i_Game)
            : this(i_AssetName, i_Game, int.MaxValue)
        {
        }

        /// <summary>
        /// Default initialization of bounds
        /// </summary>
        /// <remarks>
        /// Derived classes are welcome to override this to implement their specific boudns initialization
        /// </remarks>
        protected override void InitBounds()
        {
            // default initialization of bounds
            SetBounds();
            SetInitialPosition();
            InitSourceRectangle();
            InitOrigins();
        }

        protected virtual void SetBounds()
        {
            WidthBeforeScale = Texture.Width;
            HeightBeforeScale = Texture.Height;
        }

        private void zeroBounds()
        {
            Width = 0;
            Height = 0;
        }

        protected virtual void InitOrigins()
        {
            PositionOrigin = Vector2.Zero;
        }

        protected virtual void InitSourceRectangle()
        {
            m_SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Animations = new CompositeAnimator(this);
            InitAnimations();
        }
        protected virtual void Sprite_ResolutionChanged()
        {
        }

        protected virtual void InitAnimations()
        {
        }

        protected virtual void SetInitialPosition()
        {
        }

        public void SetPositionWithoutCollision(Vector2 i_Position)
        {
            m_Position = i_Position;
        }

        protected bool m_UseSharedBatch = false;

        protected SpriteBatch m_SpriteBatch;

        public SpriteBatch SpriteBatch
        {
            protected get { return m_SpriteBatch; }
            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        protected override void LoadContent()
        {
            if (AssetName != String.Empty)
            {
                m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
            }

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

            base.LoadContent();
        }

        /// <summary>
        /// Basic movement logic (position += velocity * totalSeconds)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <remarks>
        /// Derived classes are welcome to extend this logic.
        /// </remarks>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.Animations != null)
            {
                this.Animations.Update(gameTime);
            }
        }

        class DeviceStates
        {
            public BlendState BlendState;
            public SamplerState SamplerState;
            public DepthStencilState DepthStencilState;
            public RasterizerState RasterizerState;
        }

        DeviceStates m_SavedDeviceStates = new DeviceStates();
        protected void saveDeviceStates()
        {
            m_SavedDeviceStates.BlendState = GraphicsDevice.BlendState;
            m_SavedDeviceStates.SamplerState = GraphicsDevice.SamplerStates[0];
            m_SavedDeviceStates.DepthStencilState = GraphicsDevice.DepthStencilState;
            m_SavedDeviceStates.RasterizerState = GraphicsDevice.RasterizerState;
        }

        private void restoreDeviceStates()
        {
            GraphicsDevice.BlendState = m_SavedDeviceStates.BlendState;
            GraphicsDevice.SamplerStates[0] = m_SavedDeviceStates.SamplerState;
            GraphicsDevice.DepthStencilState = m_SavedDeviceStates.DepthStencilState;
            GraphicsDevice.RasterizerState = m_SavedDeviceStates.RasterizerState;
        }

        protected bool m_SaveAndRestoreDeviceState = false;
        public bool SaveAndRestoreDeviceState
        {
            get { return m_SaveAndRestoreDeviceState; }
            set { m_SaveAndRestoreDeviceState = value; }
        }

        /// <summary>
        /// Basic texture draw behavior, using a shared/own sprite batch
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (!m_UseSharedBatch)
            {
                if (SaveAndRestoreDeviceState)
                {
                    saveDeviceStates();
                }

                m_SpriteBatch.Begin(
                    SortMode, BlendState, SamplerState,
                    DepthStencilState, RasterizerState, Shader, TransformMatrix);
            }

            if (this.Texture != null) 
            {
                m_SpriteBatch.Draw(m_Texture, this.PositionForDraw,
                    this.SourceRectangle, this.TintColor,
                    this.Rotation, this.RotationOrigin, this.Scales,
                    SpriteEffects.None, this.LayerDepth);
            }

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();

                if (SaveAndRestoreDeviceState)
                {
                    restoreDeviceStates();
                }
            }

            base.Draw(gameTime);
        }

        public Sprite ShallowClone()
        {
            return this.MemberwiseClone() as Sprite;
        }

        // TODO 04:
        protected override void DrawBoundingBox()
        {
            // not implemented yet
        }

        // -- end of TODO 04

        // TODO 14: Implement a basic collision detection between two ICollidable2D objects:
        public virtual bool CheckCollision(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;
            if (!this.IsAlreadyDead && source != null)
            {
                collided = source.Bounds.Intersects(this.Bounds) || source.Bounds.Contains(this.Bounds);
                if (collided && i_Source is ICollidable2D)
                {
                    // If the "Outer" bounds intersect, we check if the "inner" pixels intersect.
                    collided = CheckPixelCollision(i_Source as ICollidable2D);
                }
            }

            return collided;
        }

        // -- end of TODO 14

        // This method checks whether a pixel collision occured.
        public bool CheckPixelCollision(ICollidable2D i_Source)
        {
            bool collided = false;
            Rectangle impactPos = GetImpactRectangle(i_Source);

            // For each single pixel in the intersecting rectangle
            for (int y = impactPos.Y; y < impactPos.Y + impactPos.Height; ++y)
            {
                for (int x = impactPos.X; x < impactPos.X + impactPos.Width; ++x)
                {
                    // Get the color from each texture
                    Color thisPixel = Pixels[x - this.Bounds.X + ((y - this.Bounds.Y) * this.Texture.Width)];
                    Color sourcePixel = i_Source.Pixels[x - i_Source.Bounds.X + ((y - i_Source.Bounds.Y) * i_Source.Texture.Width)];

                    // If both colors aren't transparent -> there's a collision
                    if (thisPixel.A != 0 && sourcePixel.A != 0) 
                    {
                        collided = true;
                        break;
                    }
                }
            }

            return collided;
        }

        // TODO 15: Implement a basic collision reaction between two ICollidable2D objects
        public virtual void Collided(ICollidable i_Collidable)
        {
        }

        // This method actually erases the relevant colliding pixels (Not needed for all collisions)
        public void ErasePixelCollision(ICollidable i_Collidable)
        {
            ICollidable2D other = i_Collidable as ICollidable2D;
            Rectangle impactPos = GetImpactRectangle(other);

            for (int y = impactPos.Y; y < impactPos.Y + impactPos.Height; y++)
            {
                for (int x = impactPos.X; x < impactPos.X + impactPos.Width; x++)
                {
                    int thisCurrentPixelLocation = x - this.Bounds.X + ((y - this.Bounds.Y) * this.Texture.Width);
                    int otherCurrentPixelLocation =
                        x - other.Bounds.X + ((y - other.Bounds.Y) * other.Texture.Width);

                    if (this.Pixels[thisCurrentPixelLocation].A != 0 && other.Pixels[otherCurrentPixelLocation].A != 0)
                    {
                        this.Pixels[thisCurrentPixelLocation].A = 0;
                    }
                }
            }

            this.Texture.SetData(Pixels);
        }

        // This method receives the impact rectangle between two collidables.
        protected Rectangle GetImpactRectangle(ICollidable2D i_Source)
        {
            int x1 = Math.Max(this.Bounds.X, i_Source.Bounds.X);
            int x2 = Math.Min(this.Bounds.X + this.Bounds.Width, i_Source.Bounds.X + i_Source.Bounds.Width);
            int y1 = Math.Max(this.Bounds.Y, i_Source.Bounds.Y);
            int y2 = Math.Min(this.Bounds.Y + this.Bounds.Height, i_Source.Bounds.Y + i_Source.Bounds.Height);
            
            Point topLeftImpactPos = new Point(x1, y1);
            Point BottomRightImpactPos = new Point(x2, y2);

            Rectangle result = new Rectangle(topLeftImpactPos, BottomRightImpactPos - topLeftImpactPos);
            return result;
        }

        public void OnHit()
        {
            if (Hit != null)
            {
                Hit.Invoke();
            }
        }

        public void OnDeath()
        {
            if (Death != null)
            {
                Death.Invoke();
            }
        }

        // This method "revives" the sprite and sets it to its initial position.
        public virtual void Respawn()
        {
            Enabled = true;
            IsAlreadyDead = false;
            SetInitialPosition();
        }

        // Set default behaviour when getting hit.
        public virtual void TakeDamage()
        {
            Lives--;
        }
    }
}