using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class SaveDataOrcestrator: ISaveDataOrcestrator, IDisposable
{
    
    public IFileSaveServise fileSaveServise;
    private SemaphoreSlim slimLocked = new(1,1);
    private GameData gameData;
    private readonly List<IDataPeresistance> persitances = new();
    private CancellationTokenSource tokenSource = new();
    
    public SaveDataOrcestrator(List<IDataPeresistance> persitances, IFileSaveServise fileSaveServise)
    {                                                 
        this.fileSaveServise = fileSaveServise;
        this.persitances = persitances;
        Debug.Log(persitances.Count);
    }

    async UniTask ISaveDataOrcestrator.SaveGame(int number)
     {
        await slimLocked.WaitAsync(tokenSource.Token);
        try
        {
            gameData = new();
            foreach (var persitance in persitances)
            {
                var cube = new CubeV3();
                cube.iD = persitance.GetHashCode();
                gameData.cubes.Add(cube);
                persitance.OnSaveGame(gameData);
            }
            Debug.Log("SaveGameData" + gameData.cubes.Count);
            await fileSaveServise.Save(gameData, number,tokenSource.Token);
        }
        catch(Exception e)
        { 
            Debug.LogError($"Ошибка сохранения в {e.Message}");
        }
        finally 
        { 
            slimLocked.Release(); 
        }
    }
     async UniTask ISaveDataOrcestrator.LoadGame(int number)
     {
        await slimLocked.WaitAsync(tokenSource.Token);
        try
        {
            if (!fileSaveServise.HasSave(number))
            {
                Debug.Log("Отсутсвует сохранение");
                return;
            }
            gameData = await fileSaveServise.Load(number,tokenSource.Token);
            foreach (var persitance in persitances)
            {
                persitance.OnLoaGame(gameData);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка загрузки в {e.Message}");
        }
        finally
        {
            slimLocked.Release();
        }
    }
     void ISaveDataOrcestrator.Register(IDataPeresistance peresistance)
     {
        if (!persitances.Contains(peresistance))
            persitances.Add(peresistance);
     }

    public void Dispose()
    {
        tokenSource.Cancel();
        tokenSource.Dispose();
    }
}
public interface IDataPeresistance
{
    void OnSaveGame(GameData game);
    void OnLoaGame(GameData game);
}
partial interface ISaveDataOrcestrator
{
    UniTask SaveGame(int number);
    UniTask LoadGame(int number);
    void Register(IDataPeresistance peresistance);
}