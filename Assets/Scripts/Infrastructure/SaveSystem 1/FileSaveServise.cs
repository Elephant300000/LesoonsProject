using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;


public class FileSaveServise: IFileSaveServise
{
    private readonly string SaveDirectory;

    public FileSaveServise(string RootPath)
    {
        this.SaveDirectory = Path.Combine(RootPath,"Saves");
        Directory.CreateDirectory(SaveDirectory);
    }

    async UniTask IFileSaveServise.Save(GameData Data, int number, CancellationToken token)
    {
        string newPath = CreateNewPath(number);
        string json = JsonUtility.ToJson(Data);
        using var stream = new FileStream(newPath, FileMode.Create, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream);
        await writer.WriteAsync(json).AsUniTask().AttachExternalCancellation(token);
        Debug.Log("save");
    }             
     async UniTask<GameData> IFileSaveServise.Load(int number, CancellationToken token)
    { 
        string newPath = CreateNewPath(number);
        if (!File.Exists(newPath)) 
        {
            return new GameData();
        }
        using var stream = new FileStream(newPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync().AsUniTask().AttachExternalCancellation(token);
        if(string.IsNullOrEmpty(json))
            return new GameData();

        return JsonUtility.FromJson<GameData>(json);
    }
     bool IFileSaveServise.HasSave(int numberSave)
     {
        return File.Exists(CreateNewPath(numberSave));
     }  
    private string CreateNewPath(int numberSave)
    {
        return Path.Combine(SaveDirectory, $"SaveData_{numberSave}.json");
    }
}

public interface IFileSaveServise
{
    bool HasSave(int numberSave);
    UniTask Save(GameData Data, int number, CancellationToken token);
    UniTask<GameData> Load(int number, CancellationToken token);

}

[System.Serializable]
public sealed class GameData
{
    public List<CubeV3> cubes = new();
}
[System.Serializable]
public sealed class CubeV3
{
    public float x;
    public float y;
    public float z;
    public int iD;
}