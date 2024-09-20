using System;
using System.Collections.Generic;
using System.IO;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace Miner.DirectX
{
    /// <summary>
    /// Класс реализует звуки и фоновую музыку
    /// </summary>
    public class SoundsControl : IDisposable
    {
        //путь к музыке
        private string _musicPath;
        //путь к звукам объектов
        private string[] _soundsPath;
        //путь к звукам шагов
        private string[] _steps;
        //путь к звукам разрушения стен
        private string[] _wall;

        //ядро для воспроизведения музыки
        private XAudio2 _audioMusic;
        private MasteringVoice _voiceMusic;

        //ядро для воспроизведения звуков
        private XAudio2 _audioSounds;
        private MasteringVoice _voiceSounds;

        private SourceVoice _music;
        private SourceVoice _sound;

        private SoundStream _sourceMusic;

        private SoundStream _wallWoodDamage;
        private SoundStream _wallStoneDamage;
        private SoundStream _wallStonePartDamage;

        private SoundStream _sourceBoom;
        private SoundStream _sourceDead;
        private SoundStream _sourceMineInst;

        private SoundStream _sourceStep1;
        private SoundStream _sourceStep2;
        private SoundStream _sourceStep3;
        private SoundStream _sourceStep4;

        private List<SoundStream> _sourceStep;

        private AudioBuffer _bufferMusic;
        private List<AudioBuffer> _bufferSounds;
        private List<AudioBuffer> _bufferSteps;
        private List<AudioBuffer> _bufferWalls;

        private Random _rd;

        public SoundsControl(float musicVolume, float soundsVolume)
        {
            _musicPath = @"..\..\..\Resources\sounds\background.wav";

            _soundsPath = new string[3];
            _soundsPath[0] = @"..\..\..\Resources\sounds\boom.wav";
            _soundsPath[1] = @"..\..\..\Resources\sounds\dead.wav";
            _soundsPath[2] = @"..\..\..\Resources\sounds\mineInst.wav";

            _steps = new string[4];
            _steps[0] = @"..\..\..\Resources\sounds\steps\step1.wav";
            _steps[1] = @"..\..\..\Resources\sounds\steps\step2.wav";
            _steps[2] = @"..\..\..\Resources\sounds\steps\step3.wav";
            _steps[3] = @"..\..\..\Resources\sounds\steps\step4.wav";

            _wall = new string[3];
            _wall[0] = @"..\..\..\Resources\sounds\walls\wood.wav";
            _wall[1] = @"..\..\..\Resources\sounds\walls\stone.wav";
            _wall[2] = @"..\..\..\Resources\sounds\walls\stoneLive.wav";

            _rd = new Random();
            _sourceStep = new List<SoundStream>();
            _bufferSounds = new List<AudioBuffer>();
            _bufferSteps = new List<AudioBuffer>();
            _bufferWalls = new List<AudioBuffer>();

            _audioMusic = new XAudio2();

            _voiceMusic = new MasteringVoice(_audioMusic);
            _voiceMusic.SetVolume(musicVolume);
            _audioMusic.StartEngine();

            _audioSounds = new XAudio2();

            _voiceSounds = new MasteringVoice(_audioSounds);
            _voiceSounds.SetVolume(soundsVolume);
            _audioSounds.StartEngine();

            MusicLoad();
            SoundLoad();
            WallLoad();
            StepLoad();
        }

        /// <summary>
        /// Загрузка фоновой музыки
        /// </summary>
        private void MusicLoad()
        {
            using (_sourceMusic = new SoundStream(File.OpenRead(_musicPath)))
            {

                _bufferMusic = new AudioBuffer
                {
                    Stream = _sourceMusic.ToDataStream(),
                    AudioBytes = (int)_sourceMusic.Length,
                    Flags = BufferFlags.EndOfStream
                };
            }
        }

        /// <summary>
        /// Загрузка звуков объектов
        /// </summary>
        private void SoundLoad()
        {
            using (_sourceBoom = new SoundStream(File.OpenRead(_soundsPath[0])))
            {

                _bufferSounds.Add(new AudioBuffer
                {
                    Stream = _sourceBoom.ToDataStream(),
                    AudioBytes = (int)_sourceBoom.Length,
                    Flags = BufferFlags.EndOfStream
                });
            }

            using (_sourceDead = new SoundStream(File.OpenRead(_soundsPath[1])))
            {

                _bufferSounds.Add(new AudioBuffer
                {
                    Stream = _sourceDead.ToDataStream(),
                    AudioBytes = (int)_sourceDead.Length,
                    Flags = BufferFlags.EndOfStream
                });
            }

            using (_sourceMineInst = new SoundStream(File.OpenRead(_soundsPath[2])))
            {

                _bufferSounds.Add(new AudioBuffer
                {
                    Stream = _sourceMineInst.ToDataStream(),
                    AudioBytes = (int)_sourceMineInst.Length,
                    Flags = BufferFlags.EndOfStream
                });
            }
        }

        /// <summary>
        /// Загрузка звуков шагов
        /// </summary>
        private void StepLoad()
        {
            using (_sourceStep1 = new SoundStream(File.OpenRead(_steps[0])))
            {
                _bufferSteps.Add(new AudioBuffer
                {
                    Stream = _sourceStep1.ToDataStream(),
                    AudioBytes = (int)_sourceStep1.Length,
                    Flags = BufferFlags.EndOfStream
                });
                _sourceStep.Add(_sourceStep1);
            }


            using (_sourceStep2 = new SoundStream(File.OpenRead(_steps[1])))
            {
                _bufferSteps.Add(new AudioBuffer
                {
                    Stream = _sourceStep2.ToDataStream(),
                    AudioBytes = (int)_sourceStep2.Length,
                    Flags = BufferFlags.EndOfStream
                });
                _sourceStep.Add(_sourceStep2);
            }

            using (_sourceStep3 = new SoundStream(File.OpenRead(_steps[2])))
            {
                _bufferSteps.Add(new AudioBuffer
                {
                    Stream = _sourceStep3.ToDataStream(),
                    AudioBytes = (int)_sourceStep3.Length,
                    Flags = BufferFlags.EndOfStream
                });
                _sourceStep.Add(_sourceStep3);
            }

            using (_sourceStep4 = new SoundStream(File.OpenRead(_steps[3])))
            {
                _bufferSteps.Add(new AudioBuffer
                {
                    Stream = _sourceStep4.ToDataStream(),
                    AudioBytes = (int)_sourceStep4.Length,
                    Flags = BufferFlags.EndOfStream
                });
                _sourceStep.Add(_sourceStep4);
            }

        }

        /// <summary>
        /// Загрузка звуков уничтожения стен
        /// </summary>
        private void WallLoad()
        {
            using (_wallWoodDamage = new SoundStream(File.OpenRead(_wall[0])))
            {

                _bufferWalls.Add(new AudioBuffer
                {
                    Stream = _wallWoodDamage.ToDataStream(),
                    AudioBytes = (int)_wallWoodDamage.Length,
                    Flags = BufferFlags.EndOfStream
                });
            }

            using (_wallStoneDamage = new SoundStream(File.OpenRead(_wall[1])))
            {

                _bufferWalls.Add(new AudioBuffer
                {
                    Stream = _wallStoneDamage.ToDataStream(),
                    AudioBytes = (int)_wallStoneDamage.Length,
                    Flags = BufferFlags.EndOfStream
                });
            }

            using (_wallStonePartDamage = new SoundStream(File.OpenRead(_wall[2])))
            {

                _bufferWalls.Add(new AudioBuffer
                {
                    Stream = _wallStonePartDamage.ToDataStream(),
                    AudioBytes = (int)_wallStonePartDamage.Length,
                    Flags = BufferFlags.EndOfStream
                });
            }
        }

        /// <summary>
        /// Воспроизведение фоновой музыки
        /// </summary>
        public void Music()
        {
            _music = new SourceVoice(_audioMusic, _sourceMusic.Format);
            _music.SubmitSourceBuffer(_bufferMusic, _sourceMusic.DecodedPacketsInfo);
            _music.Start();
        }

        /// <summary>
        /// Воспроизведение звуков шагов
        /// </summary>
        public void Step()
        {
            SoundStream source = _sourceStep[_rd.Next(_sourceStep.Count)];
            AudioBuffer buffer = _bufferSteps[_rd.Next(_sourceStep.Count)];
            _sound = new SourceVoice(_audioSounds, source.Format);
            _sound.SubmitSourceBuffer(buffer, source.DecodedPacketsInfo);
            _sound.Start();
        }

        /// <summary>
        /// Воспроизведение звуков разрушения слабой стены
        /// </summary>
        public void WallWoodDamage()
        {
            _sound = new SourceVoice(_audioSounds, _wallWoodDamage.Format);
            _sound.SubmitSourceBuffer(_bufferWalls[0], _wallWoodDamage.DecodedPacketsInfo);
            _sound.Start();
        }

        /// <summary>
        /// Воспроизведение звуков нанесения ущерба средней стене
        /// </summary>
        public void WallStonePartDamage()
        {
            _sound = new SourceVoice(_audioSounds, _wallStonePartDamage.Format);
            _sound.SubmitSourceBuffer(_bufferWalls[2], _wallStonePartDamage.DecodedPacketsInfo);
            _sound.Start();
        }

        /// <summary>
        /// Воспроизведение звуков уничтожения средней стены
        /// </summary>
        public void WallStoneDamage()
        {
            _sound = new SourceVoice(_audioSounds, _wallStoneDamage.Format);
            _sound.SubmitSourceBuffer(_bufferWalls[1], _wallStoneDamage.DecodedPacketsInfo);
            _sound.Start();
        }

        /// <summary>
        /// Воспроизведение звука взрыва
        /// </summary>
        public void Boom()
        {
            _sound = new SourceVoice(_audioSounds, _sourceBoom.Format);
            _sound.SubmitSourceBuffer(_bufferSounds[0], _sourceBoom.DecodedPacketsInfo);
            _sound.Start();
        }

        /// <summary>
        /// Воспроизведение звука смерти персонажа
        /// </summary>
        public void Dead()
        {
            _sound = new SourceVoice(_audioSounds, _sourceDead.Format);
            _sound.SubmitSourceBuffer(_bufferSounds[1], _sourceDead.DecodedPacketsInfo);
            _sound.Start();
        }

        /// <summary>
        /// Воспроизведение звука минирования
        /// </summary>
        public void Mine()
        {
            _sound = new SourceVoice(_audioSounds, _sourceMineInst.Format);
            _sound.SubmitSourceBuffer(_bufferSounds[2], _sourceMineInst.DecodedPacketsInfo);
            _sound.Start();
        }

        /// <summary>
        /// Возвращает значение о воспроизведении фоновой музыки (true - проигрывает, false - не проигрывает)
        /// </summary>
        /// <returns>Возвращает значение о воспроизведении фоновой музыки</returns>
        public bool GetMusicRepeat()
        {
            if (_music.State.BuffersQueued == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Останавливает воспроизведение музыки и всех звуков
        /// </summary>
        public void Stop()
        {
            try
            {
                _music.Stop();
                _sound.Stop();
            }
            catch { }
        }

        /// <summary>
        /// Освобождение неуправляемых ресурсов
        /// </summary>
        public void Dispose()
        {
            Utilities.Dispose(ref _audioMusic);
            Utilities.Dispose(ref _audioSounds);
            Utilities.Dispose(ref _sound);
            Utilities.Dispose(ref _music);
        }
    }
}
