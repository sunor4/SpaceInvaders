// *** Guy Ronen © 2008-2011 ***//
// We were given permission to use this file as a part of XNA Monogame summer course IDC 2020 //
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public abstract class GameService : RegisteredComponent
    {
        public GameService(Game i_Game, int i_UpdateOrder)
            : base(i_Game, i_UpdateOrder)
        {
            RegisterAsService(); // self-regsiter as a service
        }

        public GameService(Game i_Game)
            : base(i_Game)
        {
            RegisterAsService(); // self-regsiter as a service
        }

        /// <summary>
        /// This method register this component as a service in the game.
        /// It should be overriden in derived classes
        ///     if they want to register it with an interface 
        /// </summary>
        protected virtual void RegisterAsService()
        {
            this.Game.Services.AddService(this.GetType(), this);
        }
    }
}
