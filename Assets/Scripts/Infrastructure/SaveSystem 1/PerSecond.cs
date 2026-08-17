using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerSecond : MonoBehaviour, IDataPeresistance
{
    private int id;
    private Vector3 vector;
    private int amount;
    private Transform tr;
    private void Awake()
    {
        tr = transform;
        id = GetHashCode();
    }
    public void OnLoaGame(GameData game)
    {

        var cubeData = game.cubes.FirstOrDefault(cube => cube.iD == id);

        vector = new Vector3(cubeData.x, cubeData.y, cubeData.z);
        tr.localPosition = vector;


    }

    public void OnSaveGame(GameData game)
    {
        var cubeData = game.cubes.FirstOrDefault(cube => cube.iD == id);
        cubeData.x = tr.localPosition.x;
        cubeData.y = tr.localPosition.y;
        cubeData.z = tr.localPosition.z;
    }
}
