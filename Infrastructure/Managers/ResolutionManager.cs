using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using SpaceInvaders;
using System;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Infrastructure.Managers
{
    public class ResolutionManager : GameService, IResolutionManager
    {
        private Vector2 m_OriginalResolution = Vector2.Zero;
        private Vector2 m_ClientResolution = Vector2.Zero;
        private Vector2 m_FullScreenResolution = Vector2.Zero;
        private Matrix m_TransformMatrix = Matrix.Identity;
        public event Action ResolutionChanged;

        public Vector2 MouseScale
        {
            get { return ClientResolution / OriginalResolution; }
        }

        // get { return m_TransformMatrix; }
        // private set { m_TransformMatrix = value; }
        public Matrix TransformMatrix
        {
            get { return m_TransformMatrix; }
            private set { m_TransformMatrix = value; }
        }

        // Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height
        //((Game as BaseGame).GraphicsDevice.Viewport.Width, (Game as BaseGame).GraphicsDevice.Viewport.Height)
        public Vector2 ClientResolution
        {
            get { return new Vector2((Game as BaseGame).GameWindowBounds.Width, (Game as BaseGame).GameWindowBounds.Height); }
        }                             

        public Vector2 OriginalResolution
        {
            get { return m_OriginalResolution; }
            private set { m_OriginalResolution = value; }
        }

        public Vector2 FullScreenResolution
        {
            get { return m_FullScreenResolution; }
        }

        public Vector2 ScaledResolutionMultiplier
        {
            get { return ClientResolution / OriginalResolution; }
        }

        public ResolutionManager(Game i_Game) : base(i_Game)
        {
            Game.Window.ClientSizeChanged += OnResolutionChange;
        }

        //private void ScreensManager_ScreenChange()
        //{
        //    OriginalResolution = ClientResolution;
        //    updateTransformMatrix();
        //}

        public override void Initialize()
        {
            base.Initialize();
            getClientScreenResolution();
            getFullScreenResolution();
            updateTransformMatrix();
        }

        private void getClientScreenResolution()
        {
            OriginalResolution = ClientResolution;
        }

        private void getFullScreenResolution()
        {
            m_FullScreenResolution.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            m_FullScreenResolution.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }

        public void ToggleFullScreen()
        {
            (Game as BaseGame).GameGraphicsDeviceManager.ToggleFullScreen();
        }

        private void setFullScreen()
        {
            OriginalResolution = ClientResolution;
            Vector2 scale = FullScreenResolution / ClientResolution;
            TransformMatrix = Matrix.CreateScale(scale.X, scale.Y, 1f);
        }

        private void setOriginalScreenResolution()
        {
            TransformMatrix = Matrix.CreateScale(m_OriginalResolution.X, m_OriginalResolution.Y, 1f);
        }

        public void ToggleResizeScreen()
        {
            Game.Window.AllowUserResizing = !Game.Window.AllowUserResizing;
        }

        private void applyChanges()
        {
            (Game as BaseGame).GameGraphicsDeviceManager.ApplyChanges();
        }

        private void OnResolutionChange(object sender, EventArgs e)
        {
            updateTransformMatrix();

            if (ResolutionChanged != null)
            {
                ResolutionChanged.Invoke();
            }

            applyChanges();
        }

        private void updateTransformMatrix()
        {
            TransformMatrix = Matrix.CreateScale(ScaledResolutionMultiplier.X, ScaledResolutionMultiplier.Y, 1f);
            applyChanges();
        }

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(IResolutionManager), this);
        }
    }
}
