using System;
using System.Diagnostics;

namespace Miner
{
    /// <summary>
    /// Класс-помощник. Позволяет захватывать время
    /// </summary>
    public class Helper
    {
        private Stopwatch _stopwatch;

        private int _frameCounter;

        private int _fps;
        private bool _game;
        public bool Game { get => _game; }
        //fps
        public int FPS { get => _fps; }

        private long _previousFPSMeasurementTime;

        private float _time;
        //время
        public float Time { get => _time; }

        public Helper()
        {
            _stopwatch = new Stopwatch();
            Reset();
        }

        /// <summary>
        /// Сброс таймера
        /// </summary>
        public void Reset()
        {
            _stopwatch.Reset();
            _frameCounter = 0;
            _fps = 0;
            _game = true;
            _previousFPSMeasurementTime = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Start();
        }

        /// <summary>
        /// Получение тика и обновление fps и времени
        /// </summary>
        public void Tick()
        {
            long ticks = _stopwatch.Elapsed.Ticks;
            _time = (float)ticks / TimeSpan.TicksPerSecond;

            _frameCounter++;
            if (_stopwatch.ElapsedMilliseconds - _previousFPSMeasurementTime >= 1000)
            {
                _fps = _frameCounter;
                _frameCounter = 0;
                _previousFPSMeasurementTime = _stopwatch.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Остановка таймера
        /// </summary>
        public void Stop()
        {
            _stopwatch.Stop();
        }
    }
}
