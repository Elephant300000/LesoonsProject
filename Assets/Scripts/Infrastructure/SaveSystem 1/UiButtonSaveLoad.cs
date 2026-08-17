using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class UiButtonSaveLoad : MonoBehaviour
{
    [Inject]
    private ISaveDataOrcestrator _saveDataOrcestrator;

    public void Load()
    {
          _saveDataOrcestrator.LoadGame(1).Forget();
    }
    public void Save()
    {
          _saveDataOrcestrator.SaveGame(1).Forget();
    }
    public void Load_2()
    {
         _saveDataOrcestrator.LoadGame(2).Forget();
    }
    public void Save_2()
    {
         _saveDataOrcestrator.SaveGame(2).Forget();
    } 

}
