using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ObjectModel.UIModel
{
    public class UI : CompositeDrawableComponent<Sprite>
    {
        public UI(Game i_Game) : base (i_Game)
        {
            this.BlendState = BlendState.AlphaBlend;
            this.SpritesSortMode = SpriteSortMode.BackToFront;
        }

    }
}
