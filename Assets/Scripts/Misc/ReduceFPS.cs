using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReduceFPS : MonoBehaviour
{
    //private int desiredSamplingRate = 50;
    //private int nextUpdate = 1;
    //private int fps = -1;
    private void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
        //Time.fixedDeltaTime = 1 / desiredSamplingRate;
        //Time.maximumDeltaTime = 0.1f;
    }
    private void Update()
    {
        //if (QualitySettings.vSyncCount != 0) QualitySettings.vSyncCount = 0;
        //if (Application.targetFrameRate != fps) Application.targetFrameRate = fps;

        // Degrade Graphics each second
        //if (Time.time >= nextUpdate)
        //{
        //    Debug.Log(Time.time + ">=" + nextUpdate);
        //    nextUpdate = Mathf.FloorToInt(Time.time) + 1;
        //    ReduceMaxDeltaTimeSlowly();
        //}

        // Degrade Graphics by keyboard input
        switch (Input.inputString)
        {
            case "f":
                //Debug.Log("Augmented fps to : " + ++fps);
                //Application.targetFrameRate = fps;
                Time.fixedDeltaTime -= 0.001f;
                Debug.Log(Time.fixedDeltaTime);
                break;
            case "g":
                //Debug.Log("Reduced fps to : " + --fps);
                //Application.targetFrameRate = fps;
                Time.fixedDeltaTime += 0.001f;
                Debug.Log(Time.fixedDeltaTime);
                break;
            case "r":
                Debug.Log("reloaded level");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "q":
                Time.fixedDeltaTime = 0.1f;
                //Time.maximumDeltaTime = 0.1f;
                break;
            
        }
    }

    void ReduceMaxDeltaTimeSlowly()
    {
        Time.fixedDeltaTime += 0.0005f;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.02f);
    }
}
