using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected ContentController contentController;
    protected TouchController touchController;

    public State(ContentController cc)
    {
        contentController = cc;
    }

    // Start is called before the first frame update
    public virtual IEnumerator Start()
    {
        yield break;
    }

    // Update is called once per frame
    public virtual void Touch()
    {
        return;
    }

    public virtual void Pinch()
    {
        return;
    }

    public virtual void NoTouch()
    {
        return;
    }

    public virtual void ContentLock_Click()
    {
        return;
    }
}
