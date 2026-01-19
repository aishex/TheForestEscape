using UnityEngine;

public class GemManager : MonoBehaviour
{
    public static string chosenSceneName = "";
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void PickRandomScene()
    {
        string[] allowedScenes = new string[] { "Forest1", "ForestUp", "ForestDown", "ForestRight" };
        int randomIndex = Random.Range(0, allowedScenes.Length);
        chosenSceneName = allowedScenes[randomIndex];
    }
}