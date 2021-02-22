using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Infrastructure.Managers
{
    public class XactSoundManager : GameService, ISoundManager
    {
        private string[] m_CategoryNames = {"BGMusic", "Music"};
        private AudioEngine m_AudioEngine;
        private SoundBank m_SoundBank;
        private WaveBank m_WaveBank;
        private readonly string[] r_CueList;
        private readonly Dictionary<string, Cue> r_CuesCollection;
        private readonly Dictionary<string, float> r_CategoryCollection;
        private float m_Volume = 1f;
        private bool m_IsMuted = false;
        public event Action<bool> MuteChangedIndirectly;

        public float Volume
        {
            get { return m_Volume; }
            private set
            {
                value = MathHelper.Clamp(value, 0, 1);
                m_Volume = value;
            }
        }

        public Dictionary<string, Cue> CuesCollection
        {
            get { return r_CuesCollection; }
        }

        public Dictionary<string, float> CategoryCollection
        {
            get { return r_CategoryCollection; }
        }

        public XactSoundManager(Game i_Game, string[] i_CueList, int i_UpdateOrder) : base(i_Game, i_UpdateOrder)
        {
            r_CuesCollection = new Dictionary<string, Cue>();
            r_CategoryCollection = new Dictionary<string, float>();
            r_CueList = i_CueList;
        }

        public XactSoundManager(Game i_Game, string[] i_CueList) : this(i_Game, i_CueList, int.MaxValue)
        {

        }

        private void OnMuteChangedIndirectly()
        {
            if (MuteChangedIndirectly != null)
            {
                MuteChangedIndirectly.Invoke(m_IsMuted);
            }
        }

        public void Play(string i_CueName)
        { 
            CuesCollection.TryGetValue(i_CueName, out Cue cueToPlay);
            if (cueToPlay != null)
            {
                CuesCollection[i_CueName].Play();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_AudioEngine = new AudioEngine("Content/Ex03.xgs");
            m_SoundBank = new SoundBank(m_AudioEngine, "Content/Sound Bank.xsb");
            m_WaveBank = new WaveBank(m_AudioEngine, "Content/Wave Bank.xwb");
            initCueCollection();
            initCategoryCollection();
        }

        private void initCategoryCollection()
        {
            const float k_InitialVolume = 1f;
            foreach (string categoryName in m_CategoryNames)
            {
                r_CategoryCollection.Add(categoryName, k_InitialVolume);
            }
        }

        private void initCueCollection()
        {
            for (int i = 0; i < r_CueList.Length; i++)
            {
                string currentCueName = r_CueList[i];
                CuesCollection.Add(currentCueName, this.m_SoundBank.GetCue(currentCueName));
            }
        }

        public void Mute(string i_CategoryName)
        {
            const float k_MuteVolumeValue = 0f;
            m_AudioEngine.GetCategory(i_CategoryName).SetVolume(k_MuteVolumeValue);
        }

        public void MuteAll()
        {
            foreach (string categoryName in m_CategoryNames)
            {
                Mute(categoryName);
            }
            m_IsMuted = true;
        }

        public void Stop(string i_CueName)
        {
            CuesCollection.TryGetValue(i_CueName, out Cue cueToStop);
            if (cueToStop != null)
            {
                CuesCollection[i_CueName].Stop(AudioStopOptions.Immediate);
            }
        }

        public void UnMute(string i_CategoryName)
        {
           SetVolume(i_CategoryName, r_CategoryCollection[i_CategoryName]);
        }

        public void UnMuteAll()
        {
            m_IsMuted = false;
            foreach (string categoryName in m_CategoryNames)
            {
                UnMute(categoryName);
            }
        }

        public void ToggleMute()
        {
            if (m_IsMuted)
            {
                UnMuteAll();
            }
            else
            {
                MuteAll();
            }
        }

        public void ToggleMuteIndirectly()
        {
            ToggleMute();
            OnMuteChangedIndirectly();
        }

        public void Pause(string i_CueName)
        {
            CuesCollection.TryGetValue(i_CueName, out Cue cueToPause);
            if (cueToPause != null)
            {
                CuesCollection[i_CueName].Pause();
            }
        }

        public void Resume(string i_CueName)
        {
            CuesCollection.TryGetValue(i_CueName, out Cue cueToResume);
            if (cueToResume != null)
            {
                CuesCollection[i_CueName].Resume();
            }
        }

        public void SetVolume(string i_CategoryName, float i_ValueToSet)
        {
            Volume = i_ValueToSet;
            r_CategoryCollection[i_CategoryName] = Volume;
            if (!m_IsMuted)
            {
                m_AudioEngine.GetCategory(i_CategoryName).SetVolume(Volume);
            }
        }

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(ISoundManager), this);
        }
    }
}
