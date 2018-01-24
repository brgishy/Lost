//-----------------------------------------------------------------------
    using UnityEngine.Events;
    using UnityEngine.UI;

    [RequireComponent(typeof(Toggle))]
        #pragma warning disable 0649
        #pragma warning restore 0649

        private Toggle toggle;
        {
            this.toggle = this.GetComponent<Toggle>();
            this.toggle.onValueChanged.AddListener(this.ToggleChanged);
            this.ToggleChanged(this.toggle.isOn);
        }
        {
            if (newValue)
            {
                this.onToggleOn.InvokeIfNotNull();
            }
            else
            {
                this.onToggleOff.InvokeIfNotNull();
            }
        }