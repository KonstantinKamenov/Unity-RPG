using UnityEngine;

namespace RPG.Utils
{
    public class LazyValue<T>
    {
        private T _val;
        public T val
        {
            get
            {
                if (!isInitialized)
                {
                    _val = initializer();
                    isInitialized = true;
                }
                return _val;
            }
            set
            {
                isInitialized = true;
                _val = value;
            }
        }
        private bool isInitialized = false;
        public delegate T Initializer();
        private Initializer initializer;

        public LazyValue(Initializer initializer)
        {
            this.initializer = initializer;
        }

        public void Evaluate()
        {
            isInitialized = true;
            _val = initializer();
        }
    }
}