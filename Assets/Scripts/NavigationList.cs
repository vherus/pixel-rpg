using System.Collections;
using System.Collections.Generic;

public class NavigationList<T> : List<T>
{
    private int _currentIndex = 0;
    public int CurrentIndex {
        get {
            if (_currentIndex > Count - 1 || _currentIndex < 0) {
                _currentIndex = 0;
            }
            
            return _currentIndex;
        }
        set { _currentIndex = value; }
    }

    public T Next() {
        CurrentIndex++;
        return this[CurrentIndex];
    }

    public T Previous() {
        CurrentIndex--;
        return this[CurrentIndex]; 
    }

    public T Current() {
        return this[CurrentIndex];
    }
}
