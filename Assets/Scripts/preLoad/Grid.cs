using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Grid
{
    public static Music music; //Get the scripts we will use during the whole game and in all scenes
    public static UiManager UiManager;
    public static Game game;
    


    static Grid()
    {
        GameObject g;

        g = safeFind("__app"); //Find the __app object that contains all managers

        music = (Music)SafeComponent(g, "Music"); // Get reference of all managers in a static variable, accessible from all scripts.
        UiManager = (UiManager)SafeComponent(g, "UiManager");
        game = (Game)SafeComponent(g, "Game");
        
    }

    //the safe find of the __app gameObject, to handle errors
    private static GameObject safeFind(string s)
    {
        GameObject g = GameObject.Find(s);
        if (g == null) Woe("GameObject " + s + "  not on _preload.");
        return g;
    }

    //the safe find component from the __app gameObject, as well to handle errors
    private static Component SafeComponent(GameObject g, string s)
    {
        Component c = g.GetComponent(s);
        if (c == null) Woe("Component " + s + " not on _preload.");
        return c;
    }

    //Error message function, to show that there is a problem with the preload scene or the object
    //NOTE : For the managers to function, we must ALWAYS open the game from the preload scene first!!
    private static void Woe(string error)
    {
        Debug.Log(">>> Cannot proceed... " + error);
        Debug.Log(">>>You need to launch from scene 0, the preload scene..");
    }



}
