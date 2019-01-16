using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour
{
    public int resWidth = 2550;
    public int resHeight = 3300;

    private bool takeHiResShot = false;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",  //사진이 생성되는 파일 폴더입니다.
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    void LateUpdate()
    {
		takeHiResShot |= (ButtonControlManager.Btntype == ButtonType.Screenshot);
        //takeHiResShot |= Input.GetKeyDown("k");  //윈도우 테스트용이므로, 모바일 테스트시 GetMouseDown(0)과 같은 touch를 Input으로 받는 코드로 바꿔주시면됩니다.
        if (takeHiResShot)
        {
            Camera camera = this.GetComponent<Camera>();
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;
			ButtonControlManager.Btntype = ButtonType.NULL;
        }
    }
}