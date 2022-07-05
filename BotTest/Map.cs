using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Map<T1, T2> : Dictionary<T1, T2> {
    private T2 defaultValue;
    public Map(T2 defaultValue = default(T2)) => this.defaultValue = defaultValue;

    public T2 this[T1 key] {
        get { 
            if (!ContainsKey(key)) {
                Add(key, defaultValue);
            }
            return base[key];
        }
        set { 
            if (!ContainsKey(key)) {
                Add(key, value);
            } else {
                base[key] = value;
            }
        }
    }
}
