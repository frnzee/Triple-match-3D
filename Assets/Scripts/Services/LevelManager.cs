using Services.Loading;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class LevelManager : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private SceneNames _sceneNames;
    
    [Inject]
    public void Construct(SceneNames sceneNames)
    {
        _sceneNames = sceneNames;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);
    }
}