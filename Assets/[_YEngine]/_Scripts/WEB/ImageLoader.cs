using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;


public class ImageLoader : MonoBehaviour
{

    Image image;
    [SerializeField] Sprite defaultSprite;
    bool isloading;

    private void Awake()
    {
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
    }

    public void LoadImage(string url)
    {
        StartCoroutine(LoadPhotoCR(url));
    }

    internal IEnumerator LoadPhotoCR(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        isloading = true;
        yield return request.SendWebRequest();
        isloading = false;

        if (string.IsNullOrEmpty(request.error))
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.preserveAspect = true;
        }
        else
        {
            isloading = false;
            image.sprite = defaultSprite;
        }
    }
}
