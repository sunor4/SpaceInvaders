using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class Background : Sprite
    {
        private const string k_SpriteFileName = @"GameAssets\BG_Space01_1024x768";

        public Background(Game i_Game) : base(k_SpriteFileName, i_Game)
        {
            TintColor = Color.White;
        }

        protected override void SetInitialPosition()
        {
            Position = Vector2.Zero;
        }

        public override void Initialize()
        {
            base.Initialize();
            Scales = (Game as BaseGame).ResolutionManager.ScaledResolutionMultiplier;
        }

        protected override void Sprite_ResolutionChanged()
        {
            Scales = (Game as BaseGame).ResolutionManager.ScaledResolutionMultiplier;
        }
    }
}
