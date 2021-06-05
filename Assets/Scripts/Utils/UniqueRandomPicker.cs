using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UniqueRandomPicker<TContainer, TValue> 
    where TValue : class 
    where TContainer : IList<TValue>
{
    private TContainer _container;
    private HashSet<TValue> _usedValues = new HashSet<TValue>();

    public UniqueRandomPicker() { }

    public UniqueRandomPicker(TContainer container)
    {
        _container = container;
    }

    public void SetContainer(TContainer container) => _container = container;

    public TValue GetNextUniqueRandomElement()
    {
        HashSet<TValue> triedElements = new HashSet<TValue>();
        TValue result = null;

        do
        {
            result = _container[UnityEngine.Random.Range(0, _container.Count)];
            triedElements.Add(result);

            if (triedElements.Count == _container.Count)
            {
                throw new Exception(
                    "There is no unique element to get from list collection.");
            }
        } while (_usedValues.Contains(result));

        _usedValues.Add(result);
        return result;
    }
}
