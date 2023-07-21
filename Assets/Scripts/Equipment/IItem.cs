using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IItem
{
    void Activate();
    void InActivate();
    IItem PickUp();
    void Drop();
    string GetItemName();
    Sprite getItemSprite();
}

