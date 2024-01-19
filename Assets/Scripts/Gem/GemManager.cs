using System.Collections.Generic;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    private static GemManager _current;
    public static GemManager Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
    }

    [SerializeField]
    private Gem _gemPrefab;
    [SerializeField]
    private int _capacity;

    private HashSet<Gem> _activeGems = new HashSet<Gem>();
    private Stack<Gem> _inactiveGems = new Stack<Gem>();

    public IEnumerable<Gem> ActiveGems => _activeGems;
    public IEnumerable<Gem> InactiveGems => _inactiveGems;

    private void OnDestroy()
    {
        _current = null;
    }

    public void CreateGem(Vector3 position)
    {
        if (_activeGems.Count >= _capacity) return;

        Gem gem = null;
        if (_inactiveGems.Count == 0)
        {
            if (!_gemPrefab)
            {
                Debug.Log("EnemyPrefab is not assigned! Please assign it in the Inspector.");
                return;
            }
            gem = Instantiate(_gemPrefab, transform);
        }
        else
        {
            gem = _inactiveGems.Pop();
        }
        _activeGems.Add(gem);
        gem.gameObject.SetActive(true);
        gem.Initialize(position);
        gem.OnDead += DeleteGem;
    }

    public void DeleteGem(Gem gem)
    {
        gem.OnDead -= DeleteGem;
        gem.gameObject.SetActive(false);
        _activeGems.Remove(gem);
        _inactiveGems.Push(gem);
    }
}