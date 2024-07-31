using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour {

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {

            string folderPath = "Assets/Screenshots/";

            if(!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            var screenshotName ="Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png";
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 2);
            Debug.Log(folderPath + screenshotName);
        }
    }
}
