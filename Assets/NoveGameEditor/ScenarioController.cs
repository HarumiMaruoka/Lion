using Glib.NovelGameEditor;
using System.Collections;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    [SerializeField]
    private NovelRunner _novelGameController;
    [SerializeField]
    private NovelNodeGraph _nodeGraph;

    public void Begin()
    {
        _novelGameController.Play(_nodeGraph);
    }
}