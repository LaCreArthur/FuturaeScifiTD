using System;
using UnityEngine;

namespace ScriptableVariables
{
    public abstract class SOVar<T> : ScriptableObject
    {
        [SerializeField] T initialValue;
        [SerializeField] T previousValue;
        [SerializeField] protected T value;

        [SerializeField] bool isReadOnly;
        [SerializeField] bool isSaved;

        public event Action<T> OnChanged;

        public T Value
        {
            get => value;
            set {
                if (isReadOnly)
                {
                    Debug.LogWarning($"Cannot set {name}, it is a read-only variable", this);
                    return;
                }

                // returns if the value is the same, to avoid unnecessary event calls
                if (value.Equals(this.value)) return;

                previousValue = this.value;
                this.value = value;
                OnChanged?.Invoke(this.value);
            }
        }

        void OnEnable()
        {
            if (isSaved)
                Load();
            else
                Value = initialValue;
        }

        void OnDisable()
        {
            if (isSaved)
                Save();
        }

        protected virtual void Save() {}
        protected virtual void Load() {}
    }
}
