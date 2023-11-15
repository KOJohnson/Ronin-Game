using System;

namespace ThirdPersonMeleeSystem.Timers
{
    public class Timer
    {
        public event Action OnTimerComplete;
        public float timerDuration;
        public float TimeElapsed { get; private set; }
        public bool IsTimerComplete { get; private set; }
        
        public Timer()
        {
        }
        
        public Timer(float duration)
        {
            timerDuration = duration;
        }
        
        public void Tick(float deltaTime)
        {
            if (IsTimerComplete) return;
            
            if (TimeElapsed < timerDuration)
            {
                TimeElapsed += deltaTime;
            }
            else
            {
                if (!IsTimerComplete)
                {
                    IsTimerComplete = true;
                    OnTimerCompleteEvent();
                }
            }
        }

        public void SetDuration(float duration)
        {
            timerDuration = duration;
        }

        public void Reset()
        {
            IsTimerComplete = false;
            TimeElapsed = 0f;
        }

        private void OnTimerCompleteEvent()
        {
            OnTimerComplete?.Invoke();
        }
    }
}