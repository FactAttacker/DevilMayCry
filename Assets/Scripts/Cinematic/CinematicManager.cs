using UnityEngine;
using System.Collections;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager instance;

    public delegate void WallBreakDel();
    public WallBreakDel wallBreakFunc;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {

    }

    public void OnWallBreak()
    {
        wallBreakFunc();
    }

    void Update()
    {

    }
}
