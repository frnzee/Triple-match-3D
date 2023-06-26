using UnityEngine;
using Zenject;

public class CollectedItem : MonoBehaviour
{
    [Inject]
    public void Construct(string incomingName)
    {
        name = incomingName;
    }
    
    public class Factory : PlaceholderFactory<string, CollectedItem> { }
}
