using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace WizardGrenade2
{
    public sealed class SoundManager
    {
        private SoundManager() { }
        private static readonly Lazy<SoundManager> lazyManager = new Lazy<SoundManager>(() => new SoundManager());
        public static SoundManager Instance { get => lazyManager.Value; }

        private Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();
        private Dictionary<string, SoundEffectInstance> _soundInstances = new Dictionary<string, SoundEffectInstance>();
        private List<Song> _songs = new List<Song>();

        public void LoadContent(ContentManager contentManager)
        {
            _sounds.Add("stone0", contentManager.Load<SoundEffect>(@"Audio/WQ_tile2"));
            _sounds.Add("stone1", contentManager.Load<SoundEffect>(@"Audio/WQ_tile3"));
            _sounds.Add("scroll", contentManager.Load<SoundEffect>(@"Audio/WQ_scroll"));
            _sounds.Add("clink", contentManager.Load<SoundEffect>(@"Audio/WQ_potiongone"));
            _sounds.Add("potion", contentManager.Load<SoundEffect>(@"Audio/WQ_potion_cork"));
            _sounds.Add("magic0", contentManager.Load<SoundEffect>(@"Audio/WQ_magic1"));
            _sounds.Add("magic1", contentManager.Load<SoundEffect>(@"Audio/WQ_magic_hit1"));
            _sounds.Add("magic2", contentManager.Load<SoundEffect>(@"Audio/WQ_magic_hit2"));
            _sounds.Add("magic3", contentManager.Load<SoundEffect>(@"Audio/WQ_magic_hit3"));
            _sounds.Add("magic4", contentManager.Load<SoundEffect>(@"Audio/WQ_magic_hit4"));
            _sounds.Add("magic5", contentManager.Load<SoundEffect>(@"Audio/WQ_magic_hit5"));
            _sounds.Add("magicChord", contentManager.Load<SoundEffect>(@"Audio/WQ_magic_hit_chord"));
            _sounds.Add("frost", contentManager.Load<SoundEffect>(@"Audio/WQ_frostray"));
            _sounds.Add("fireHit", contentManager.Load<SoundEffect>(@"Audio/FireHit"));
            _sounds.Add("fireBounce", contentManager.Load<SoundEffect>(@"Audio/fireBounce"));
            _sounds.Add("fireCharge", contentManager.Load<SoundEffect>(@"Audio/WQ_firecharge"));
            _sounds.Add("fireCast", contentManager.Load<SoundEffect>(@"Audio/WQ_fireball"));
            _sounds.Add("hiss", contentManager.Load<SoundEffect>(@"Audio/WQ_nocast"));
            _sounds.Add("wizardOh0", contentManager.Load<SoundEffect>(@"Audio/wizard_ooh2"));
            _sounds.Add("wizardOh1", contentManager.Load<SoundEffect>(@"Audio/wizard_ooh1"));
            _sounds.Add("wizardSad", contentManager.Load<SoundEffect>(@"Audio/wizard_nocast"));
            _sounds.Add("wizardJump", contentManager.Load<SoundEffect>(@"Audio/wizard_hit1"));
            _sounds.Add("wizardHit0", contentManager.Load<SoundEffect>(@"Audio/wizard_hit2"));
            _sounds.Add("wizardHit1", contentManager.Load<SoundEffect>(@"Audio/wizard_hit3"));
            _sounds.Add("wizardCast", contentManager.Load<SoundEffect>(@"Audio/wizard_cast"));
            _sounds.Add("arrowShot", contentManager.Load<SoundEffect>(@"Audio/ArrowShot"));
            _sounds.Add("arrowDraw", contentManager.Load<SoundEffect>(@"Audio/BowDraw"));
            _sounds.Add("Draw", contentManager.Load<SoundEffect>(@"Audio/DrawTheme"));
            _sounds.Add("Win", contentManager.Load<SoundEffect>(@"Audio/WinTheme"));
            _sounds.Add("frostCharge", contentManager.Load<SoundEffect>(@"Audio/IceCharge"));
            _sounds.Add("iceHit", contentManager.Load<SoundEffect>(@"Audio/IceHit"));

            _soundInstances.Add("frost", _sounds["frost"].CreateInstance());
            _soundInstances.Add("fireCharge", _sounds["fireCharge"].CreateInstance());
            _soundInstances.Add("clink", _sounds["clink"].CreateInstance());
            _soundInstances.Add("magic0", _sounds["magic0"].CreateInstance());
            _soundInstances.Add("fireCast", _sounds["fireCast"].CreateInstance());
            _soundInstances.Add("arrowShot", _sounds["arrowShot"].CreateInstance());
            _soundInstances.Add("arrowDraw", _sounds["arrowDraw"].CreateInstance());
            _soundInstances.Add("frostCharge", _sounds["frostCharge"].CreateInstance());
            _soundInstances.Add("Draw", _sounds["Draw"].CreateInstance());
            _soundInstances.Add("Win", _sounds["Win"].CreateInstance());
            _soundInstances.Add("fireHit", _sounds["fireHit"].CreateInstance());
            _soundInstances.Add("iceHit", _sounds["iceHit"].CreateInstance());

            _soundInstances["Draw"].IsLooped = false;
            _soundInstances["Win"].IsLooped = false;

            _songs.Add(contentManager.Load<Song>("WQ_main_theme_mp3"));
        }

        public void PlaySound(string soundEffect)
        {
            if (soundEffect != null && _sounds.ContainsKey(soundEffect))
                _sounds[soundEffect].Play();
        }

        public void PlaySoundInstance(string soundInstance)
        {
            if (soundInstance != null && _soundInstances.ContainsKey(soundInstance))
            {
                _soundInstances[soundInstance].Play();
            }
        }

        public void StopSoundInstance(string soundInstance)
        {
            if (soundInstance != null && _soundInstances.ContainsKey(soundInstance))
                _soundInstances[soundInstance].Stop();
        }

        public void PlaySong(int songNumber)
        {
            if (songNumber > _songs.Count)
                return;

            MediaPlayer.Play(_songs[songNumber]);
            MediaPlayer.IsRepeating = true;
        }

        public void StopMediaPlayer()
        {
            MediaPlayer.Stop();
        }
    }
}
