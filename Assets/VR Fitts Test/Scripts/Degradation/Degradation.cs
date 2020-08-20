using UnityEngine;
using UnityEngine.UI;

public abstract class Degradation : MonoBehaviour
{
    public InputField rate;
    public InputField step;
    public bool active;
    public bool autoDegradation;

    protected float nextUpdate;

    public abstract float ValueToDegrade { get; set; }

    private void Start()
    {
        SetDefaultValues();
        nextUpdate = 1 / float.Parse(rate.text);
        rate.onValueChanged.AddListener((newRate) => { nextUpdate = 1 / float.Parse(newRate); });
    }

    public virtual void Update()
    {
        // Degrade Graphics each second
        if (active && autoDegradation && Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1 / float.Parse(rate.text);
            ValueToDegrade -= float.Parse(step.text);
        }
    }

    public void SetActive(bool _active, bool _autoDegradation)
    {
        active = _active;
        autoDegradation = _autoDegradation;
    }

    // Called with manual degradation with the +/- button
    public virtual void IncrementDegradation()
    {
        ValueToDegrade -= float.Parse(step.text);
    }

    public virtual void DecrementDegradation()
    {
        ValueToDegrade += float.Parse(step.text);
    }

    protected abstract void SetDefaultValues();
}
