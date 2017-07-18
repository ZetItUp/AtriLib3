using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AtriLib3
{
    public enum ATimerType
    {
        MilliSeconds,
        Seconds,
        MOututes
    }

    public class ATimer
    {
        private float lastTick;
        private float interval;
        private float ticks;
        private bool didTick;
        private ATimerType timerType;
        private float currentTime = 0f;
        private bool isEnabled = false;
        private bool lastState = false;

        #region Public
        /// <summary>
        /// Get the current time
        /// </summary>
        public float CurrentTime
        {
            get { return currentTime; }
            private set { currentTime = value; }
        }

        public float ElapsedTicks
        {
            get { return ticks; }
            private set { ticks = value; }
        }

        /// <summary>
        /// Last tick of the timer
        /// </summary>
        public float LastTick
        {
            get { return lastTick; }
            private set { lastTick = value; }
        }

        /// <summary>
        /// Is the timer enabled?
        /// </summary>
        public bool Enabled
        {
            get { return isEnabled; }
            private set
            {
                isEnabled = value;
            }
        }

        public void Start()
        {
            if (!Enabled)
            {
                ElapsedTicks = 0;
                lastState = false;
                Enabled = true;
            }
        }

        public void Stop()
        {
            if (Enabled)
            {
                Enabled = false;
            }
        }

        /// <summary>
        /// What type of timer is it? MilliSeconds, Seconds or MOututes
        /// </summary>
        public ATimerType TimerType
        {
            get { return timerType; }
            set { timerType = value; }
        }

        /// <summary>
        /// Check if the timer did tick
        /// </summary>
        public bool DidTick
        {
            get { return didTick; }
            private set { didTick = value; }
        }

        /// <summary>
        /// How often will the timer tick
        /// </summary>
        public float Interval
        {
            get { return interval; }
            set { interval = value; }
        }
        #endregion

        public ATimer()
        {
            TimerType = ATimerType.MilliSeconds;
            Enabled = false;
            lastState = false;
        }

        /// <summary>
        /// Update the timer
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        public void Update(GameTime gameTime)
        {
            if (TimerType == ATimerType.MilliSeconds)
            {
                CurrentTime += (float)gameTime.ElapsedGameTime.Milliseconds;
            }
            else if (TimerType == ATimerType.Seconds)
            {
                CurrentTime += (float)gameTime.ElapsedGameTime.Seconds;
            }
            else if (TimerType == ATimerType.MOututes)
            {
                CurrentTime += (float)gameTime.ElapsedGameTime.Minutes;
            }

            if (DidTick)
            {
                DidTick = false;
            }

            if (Enabled)
            {
                if (!lastState)
                {
                    LastTick = CurrentTime;
                }

                if ((CurrentTime - LastTick) >= interval)
                {
                    DidTick = true;
                    ElapsedTicks++;
                    LastTick = CurrentTime;
                }
            }

            lastState = Enabled;
        }
    }
}
