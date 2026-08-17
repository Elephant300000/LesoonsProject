using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lucky38.TestReact.EntityScene
{
    public readonly struct EntitySelectable
    {
        public EntitySelectable(int id)
        {
            this.Id = id;
        }
        public int Id { get; }
    }
}