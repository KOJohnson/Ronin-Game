using System;
using UnityEngine.Events;

namespace ThirdPersonMeleeSystem.Timers
{
    [Serializable]
    public class SerializedTimer
    {
        public float timerDuration;
        public float _timeElapsed;
        public bool resetOnComplete;
        public bool IsTimerComplete { get; private set; }
        public UnityEvent OnTimerComplete;

        public void Tick(float deltaTime)
        {
            if (IsTimerComplete) return;
            
            if (_timeElapsed < timerDuration)
            {
                _timeElapsed += deltaTime;
            }
            else
            {
                if (!IsTimerComplete)
                {
                    IsTimerComplete = true;
                    OnTimerCompleteEvent();
                    if (!resetOnComplete) return;
                    Reset();
                }
            }
        }

        public void Reset()
        {
            IsTimerComplete = false;
            _timeElapsed = 0f;
        }

        private void OnTimerCompleteEvent()
        {
            OnTimerComplete?.Invoke();
        }
    }
}