    using UnityEngine;

    public interface ICarryable
    {
        void GetCarried(Vector2 carryingPos);
        bool IsGettingCarried();
    }
