# Zip-Download-In-Unity
A guide on how to download zip-like files in Unity WebGL apps

## Why do I get an error when downloading files in my WebGL build?
Downloading anything more complex than an image or a text file in Unity WebGL apps will throw you an error when you are running the app outside your Unity editor. This happens because of CORS, an internet security measure.

## Why would I want to download zip-like files in my Unity game?
Let's say you ae making a card game. You want to be able to add new cards to the game. These cards require things like card stats and card images. You also need to keep the extra data of each card sprite like their name, so you can assign them to the proper card.

The hard way of doing this is to make a new build of your game with new card stats and images every time you want to add new cards. This is very tedious.

An easier way to do this is to host your files on a cloud service, like GitHub, Dropbox, or Google Drive, and download all card data from the cloud.
You can save your card stats as a JSON text file and you card images as individual sprites in a zip file, or if you don't want to tank your game's performance from rendering each sprite as a separate texture, a Sprite Atlas with all your sprites as a Unity asset bundle.

You won't have any trouble downloading these files in your editor, but you will get an error when trying to download your zip/tar/rar/assetbundle... file in your build. You'll only be able to successfully download basic text and image files.

Mass-downloading all your assets as separate files is also not a good option, because it will most likely trigger DDOS protection on your cloud service and make it unavailable for some time.

If you don't need the extra sprite data, like their names and the ability to cut multiple sprites from your downloaded texture to avoid tanking performance, you don't need to read further.
You can keep using Unity's WebRequest to download everything you need.

## How can I download zip-like files in my Unity WebGL app?
There are three solutions that you can use to avoid getting CORS errors when downloading archive files:

### Solution 1
Use a public CORS proxy.

You can get around CORS protection by attaching a CORS proxy link to your download link. Here is one service that is currently available and that is very easy to use: https://corsproxy.io/ 

Here is an example how you can convert your link into a CORS proxy link using the service above:
```
string url = "https://corsproxy.io/?"+ System.Web.HttpUtility.HtmlDecode("https://example-link.com");
```

**Pros:**
- Very easy to use

**Cons:**
- Can shut down at any time
- Won't work for users in certain countries or with certain ISPs (internet service providers)

### Solution 2
Make your own CORS proxy server.

You can make your own proxy server by using this: https://github.com/Zibri/cloudflare-cors-anywhere 

**Pros:**
- Will work anywhere
- You have full control over the server, no risk of randomly shutting down

**Cons:**
- Requires you to own a domain, which means a small yearly subscription fee
- Requires web dev skills and a CloudFlare account to follow the linked tutorial above

### Solution 3
Use my scripts to convert your texture and sprites into a text file, send it over the internet, then convert it back into a texture with sprites in your game.

1. Download all C# scripts from this GitHub repository.
2. Create a folder in your Assets called Scripts. Then create a folder in Scripts called Editor.
3. Put `Load Image Tools` and `Download Sprites Example` in Scripts folder. Put `Create Texture Json` in Editor folder.
4. Create a folder in your Assets called Resources. Put the texture with all your sprites that you want to send over the internet in Resources folder.
5. Click on your texture and enable `Read/Write`. Also make sure all your sprites are sliced properly.
6. In the top-left corner, where you have File/Edit/Assets... buttons, click on Jobs > Generate Json Texture.
7. A text file should appear in your Assets folder called `textureData`. If you can't see it, open your Assets folder in explorer, then open that file. This should update the folder in Unity.
8. This is the file that contains all your texture and sprite info. Upload it to a cloud storage service like GitHub and copy its link (raw link in case of GitHub). Make sure the link works and instantly downloads this file.
9. In your Unity project scene, create an empty game object and attach `Download Sprites Example` script to it.
10. Paste the link from step 8 into the `url` box of the script. Play the scene.
11. All your sprites should have downloaded into a dictionary in this script. For demonstration purposes, you can also see them in a list if you look at the script in the editor.

**What happens under the hood**

The script reads all the sprite values and saves them into a class. This is needed so we can rebuild these sprites later. It then takes the texture where these sprites came from, and converts it into a byte array.
That byte array is then compressed into a base64 string to save space. Your base64 texture data string and your sprite info are then converted into a JSON text file that is saved in your assets folder.

When you download this file in your game, the download example script takes your base64 string and converts it back into bytes, and bytes into your texture.
It then takes slices that texture into sprites using the sprite data from the downloaded file.

Feel free modify these scripts to suit your needs.

**Pros:**
- Will work anywhere
- Completely free

**Cons:**
- Slightly larger file size
- Provided example only works for textures/sprites. You can in theory apply the same principle and convert any collection of assets into a byte array JSON file, and then put it back together into your assets.
