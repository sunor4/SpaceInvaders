// *** Guy Ronen � 2008-2011 ***//
// We were given permission to use this file as a part of XNA Monogame summer course IDC 2020 //
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
	public class RegisteredComponent : GameComponent
	{
		public RegisteredComponent(Game i_Game, int i_UpdateOrder)
			: base(i_Game)
		{
			this.UpdateOrder = i_UpdateOrder;
			Game.Components.Add(this); // self-register as a coponent
		}

		public RegisteredComponent(Game i_Game)
			: this(i_Game, int.MaxValue)
		{
		}
	}
}