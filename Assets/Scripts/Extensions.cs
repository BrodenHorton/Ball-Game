using UnityEngine;

public static class Extensions
{
    public static Transform GetParentOrSelf(this Transform self)
    {
        return self.parent ? self.parent : self;
    }
}
