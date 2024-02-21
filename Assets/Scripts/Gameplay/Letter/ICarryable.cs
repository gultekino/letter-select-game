    using UnityEngine;

    public interface ICarryable
    {
        void GetCarried(Slot carryingSlot);
        bool IsGettingCarried();
    }
