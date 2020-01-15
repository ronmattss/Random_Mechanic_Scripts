using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for Unit actions

namespace PlayerScripts
{
    interface IUnit 
    {
        bool IsIdle();
        void MoveTo(Vector3 toPosition);
        // TODO animation method

    }
}