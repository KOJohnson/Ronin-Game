using ThirdPersonMeleeSystem.Structs;

namespace ThirdPersonMeleeSystem.Interfaces
{
    public interface IFinisherTarget
    {
        public void Finisher(AnimationData finisherAnimation, int damage);
    }
}