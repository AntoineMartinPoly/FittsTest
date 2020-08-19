using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script attached to toggle of each degradation
public class toggleDegradation : MonoBehaviour
{
    Toggle toggle;

    List<GameObject> removablesObject;

    private void Start()
    {
        toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(toggle);
        });

        removablesObject = new List<GameObject>();

        foreach (Component o in transform.parent.GetComponentsInChildren<Component>())
        {
            if (o.CompareTag("DegradationRemovable"))
                removablesObject.Add(o.gameObject);
        }
    }

    void ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            foreach(GameObject o in removablesObject)
            {
                o.SetActive(true);
            }
        } else
        {
            foreach (GameObject o in removablesObject)
            {
                o.SetActive(false);
            }
        }
    }
}
