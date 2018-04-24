using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare41_Game {
    class AudioManager {
        public enum AudioStates { InWave, BetweenWave };
        AudioStates audioState;

        Song wave, between;

        public AudioManager(ContentManager c) {
            audioState = AudioStates.BetweenWave;
            wave = c.Load<Song>("Audio/ingame");
            between = c.Load<Song>("Audio/ambiance");
        }

        public void UpdateState(AudioStates newState) {
            MediaPlayer.Stop();

            switch (newState) {
                case AudioStates.InWave:
                    MediaPlayer.Play(wave);
                    break;

                case AudioStates.BetweenWave:
                    MediaPlayer.Play(between);
                    break;
            }

            MediaPlayer.Volume = 0.3f;
            MediaPlayer.IsRepeating = true;
            audioState = newState;
        }
    }
}