using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Audio;

namespace Infrastructure.ServiceInterfaces
{
    public interface ISoundManager
    {
        public event Action<bool> MuteChangedIndirectly;
        Dictionary<string, float> CategoryCollection { get; }
        Dictionary<string, Cue> CuesCollection { get; }
        void Play(string i_CueName);
        void Mute(string i_CategoryName);
        void MuteAll();
        void Stop(string i_CueName);
        void UnMute(string i_CategoryName);
        void UnMuteAll();
        void ToggleMute();
        void Pause(string i_CueName);
        void Resume(string i_CueName);
        void SetVolume(string i_CategoryName, float i_ValueToSet);
    }

    public enum eGameSounds
    {
        SSGunShot = 0,
        EnemyGunShot,
        EnemyKill,
        MotherShipKill,
        BarrierHit,
        GameOver,
        LevelWin,
        LifeDie,
        MenuMove,
        BGMusic
    }
}
