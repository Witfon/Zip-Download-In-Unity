using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LoadImageTools;

public class DownloadSpritesExample : MonoBehaviour
{
    // This is where all of our downloaded sprites will end up.
    // Request sprite by its name.
    public Dictionary<string, Sprite> allSprites;

    // For example purposes. Will display all downloaded sprites
    public List<Sprite> allSpritesList;

    // Set this to your download link
    public string url;

    void Start()
    {
        StartCoroutine(LoadSprites());
    }

    IEnumerator LoadSprites()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {

            //Waiting for our web request to come back with data
            yield return webRequest.SendWebRequest();

            // If our web request succeeded, continue on
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Convert JSON text from our web request into a SpriteInfoLibrary
                SpriteInfoLibrary spriteInfoLib = JsonUtility.FromJson<SpriteInfoLibrary>(webRequest.downloadHandler.text);

                // Get a dictionary with all of our sprites
                allSprites = ConversionTools.SpritesFromJson(spriteInfoLib);

                // EXAMPLE
                // Shove all downloaded sprites into a public list so you can see them in editor
                foreach (KeyValuePair<string, Sprite> entry in allSprites)
                {
                    allSpritesList.Add(entry.Value);
                }
            }
            else
            {
                Debug.LogWarning("Error while loading sprites from web.\nURL: " + url + "\nError: " + webRequest.error);
            }

            webRequest.Dispose();
        }
    }
}
