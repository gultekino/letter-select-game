    public interface ISelecable
    {
        bool IsSelectable();
        void TrySelect();
        void UpdateVisuals();   
        void OnMouseDown();
        
    }