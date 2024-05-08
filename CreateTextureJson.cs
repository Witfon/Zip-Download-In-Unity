using UnityEngine;
using UnityEditor;
using System.IO;
using LoadImageTools;
using System;

public class CreateTextureJson
{
    [MenuItem("Jobs/Generate Json Texture")]
    private static void GenerateTextureJsonFile()
    {
        /// This will output the final file in your Assets folder. You can change this to any other location.
        string jsonSavePath = Application.dataPath;

        /// Set this to the name of the texture asset that you want to convert into a text file.
        /// This texture asset should be put in Assets/Resources folder
        string textureAssetName = "Card Sprites";

        try
        {
            // Load sprite array from resources
            Sprite[] allSprites = Resources.LoadAll<Sprite>(textureAssetName);

            SpriteInfoLibrary spriteInfoLib = new();

            // Look at all sprite values and save them into a new spriteInfo class
            foreach (Sprite sprite in allSprites)
            {

                SpriteInfo spriteInfo = new()
                {
                    rect = sprite.rect,
                    ppu = sprite.pixelsPerUnit,
                    name = sprite.name
                };

                // Save the sprite info class to our sprite info library
                spriteInfoLib.allSpriteInfos.Add(spriteInfo);
            }

            // Load the big card sprite texture
            Texture2D texture = Resources.Load<Texture2D>("Card Sprites");

            // Convert the texture to bytes
            byte[] pngBytes = texture.EncodeToPNG();

            // Compress texture bytes to base64 and save them to our spriteInfoLib
            spriteInfoLib.textureAsBase64 = Convert.ToBase64String(pngBytes);

            spriteInfoLib.textureSize = new Vector2Int(texture.width, texture.height);



            // Convert spriteInfoLib to a JSON string
            string spriteInfoLibJson = EditorJsonUtility.ToJson(spriteInfoLib);

            // Save the JSON string as text file
            File.WriteAllText(jsonSavePath + "/textureData.txt", spriteInfoLibJson);

            Debug.Log("Texture data file generated successfully in " + jsonSavePath);
        }
        catch (System.Exception exception)
        {
            Debug.LogWarning(exception);
        }
    }
}
