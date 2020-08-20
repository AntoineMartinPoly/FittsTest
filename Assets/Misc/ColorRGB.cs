using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorRGB : MonoBehaviour
{
    // Basic colors
    private Color orange =          new Color(1, 0.5f, 0, 1);
    private Color yellow =          new Color(1, 1, 0, 1);
    private Color lessGreen =       new Color(0.5f, 1, 0, 1);
    private Color turquoiseGreen =  new Color(0, 1, 0.5f, 1);
    private Color turquoise =       new Color(0, 1, 1, 1);
    private Color turquoiseBlue =   new Color(0, 0.5f, 1, 1);
    private Color purple =          new Color(0.5f, 0, 1, 1);
    private Color pink =            new Color(1, 0, 1, 1);
    private Color magenta =         new Color(1, 0, 0.5f, 1);
    private Stack<Color> colorStack = new Stack<Color>();
    public HashSet<int> redIntervalToRemove = new HashSet<int>();
    public HashSet<int> greenIntervalToRemove = new HashSet<int>();
    public HashSet<int> blueIntervalToRemove = new HashSet<int>();


    // Materials to be modified in gameplay
    private Material[] materials;

    // Initial colors to be restore when the game closes
    private List<Color> initialColors;

    public float interval = 0.05f;

    // Used to call a method each second
    private int nextUpdate = 1;


    private void Start()
    {
        // On load les ressources du folder Assets/Resources/... il faut donc que les Materials s'y trouvent prealablement
        materials = Resources.LoadAll<Material>("Material");

        // On les garde en mémoire pour les réassigner à la fin de la simulation
        foreach(Material material in materials){
            initialColors.Add(material.color);
        }

        colorStack.Push(Color.red);
        colorStack.Push(Color.green);
        colorStack.Push(Color.blue);
        colorStack.Push(pink);
        colorStack.Push(yellow);
        colorStack.Push(turquoise);
        //colorStack.Push(orange);
        //colorStack.Push(lessGreen);
        //colorStack.Push(turquoiseGreen);
        //colorStack.Push(turquoiseBlue);
        //colorStack.Push(purple);
        //colorStack.Push(magenta);

    }

    void Update()
    {
        // Degrade Graphics each second
        //if (Time.time >= nextUpdate)
        //{
        //    Debug.Log(Time.time + ">=" + nextUpdate);
        //    nextUpdate = Mathf.FloorToInt(Time.time) + 1;
        //    ReduceRedSlowly();
        //}

        // Degrade Graphics by keyboard input
        switch (Input.inputString)
        {
            case "f":
                AddColorToBeRemoveToInterval(colorStack.Pop());
                foreach (Material material in materials)
                {
                    material.color = RemoveColorIntervalFromColor(material.color);
                }
                break;
            case "g":
                interval += 0.05f;
                foreach (Material material in materials)
                {
                    material.color = RemoveColorIntervalFromColor(material.color);
                }
                break;

        }
    }

    void OnApplicationQuit()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = initialColors[i];
        }
    }

    void AddColorToBeRemoveToInterval(Color colorToBeRemoved)
    {
        AddIntervalToHashSet(redIntervalToRemove, RGBFloatToInt(colorToBeRemoved.r));
        AddIntervalToHashSet(greenIntervalToRemove, RGBFloatToInt(colorToBeRemoved.g));
        AddIntervalToHashSet(blueIntervalToRemove, RGBFloatToInt(colorToBeRemoved.b));
    }

    void AddIntervalToHashSet(HashSet<int> colorInterval, int RGBvalue)
    {
        int rgbInterval = RGBFloatToInt(interval);
        for (int i = (RGBvalue - rgbInterval < 0 ? 0 : RGBvalue - rgbInterval) ; i <= (RGBvalue + rgbInterval > 255 ? 255 : RGBvalue + rgbInterval); i++)
        {
            colorInterval.Add(i);
        }
    }

    Color RemoveColorIntervalFromColor(Color colorSource)
    {
        colorSource.r = FindNewRGBValue(RGBFloatToInt(colorSource.r), redIntervalToRemove);
        colorSource.g = FindNewRGBValue(RGBFloatToInt(colorSource.g), greenIntervalToRemove);
        colorSource.b = FindNewRGBValue(RGBFloatToInt(colorSource.b), blueIntervalToRemove);
        return colorSource;
    }

    float FindNewRGBValue(int RGBValue, HashSet<int> nonValidRGB)
    {
        while (nonValidRGB.Contains(RGBValue))
        {
            RGBValue++;
            if(RGBValue > 255)
            {
                RGBValue = 255;
                while (nonValidRGB.Contains(RGBValue)) RGBValue--;
                return RGBIntToFloat(RGBValue);
            }
        }
        return RGBIntToFloat(RGBValue);
    }

    int RGBFloatToInt(float rgbFloat)
    {
        return (int)(rgbFloat * 255);
    }

    float RGBIntToFloat(int rgbInt)
    {
        return rgbInt / 255f;
    }
}
