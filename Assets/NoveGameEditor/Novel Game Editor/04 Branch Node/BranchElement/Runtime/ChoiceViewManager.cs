using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoiceViewManager : MonoBehaviour
{
    [SerializeField]
    private ChoiceView ChoiceViewPrefab = null;
    [SerializeField]
    private Transform ChoiceViewParent = null;

    private HashSet<ChoiceView> _inactiveChoiceViews = new HashSet<ChoiceView>();

    public ChoiceView GetOrCreateChoiceView(int index, string text)
    {
        if (_inactiveChoiceViews.Count > 0)
        {
            var choiceView = _inactiveChoiceViews.First();
            _inactiveChoiceViews.Remove(choiceView);
            choiceView.gameObject.SetActive(true);
            choiceView.transform.SetAsLastSibling();
            choiceView.Initialize(index, text);
            return choiceView;
        }
        var instance = Instantiate(ChoiceViewPrefab, ChoiceViewParent);
        instance.Initialize(index, text);
        return instance;
    }

    public void ReturnChoiceView(ChoiceView choiceView)
    {
        choiceView.gameObject.SetActive(false);
        _inactiveChoiceViews.Add(choiceView);
    }
}