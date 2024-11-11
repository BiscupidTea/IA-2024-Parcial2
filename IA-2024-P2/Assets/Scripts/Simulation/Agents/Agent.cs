using System.Collections.Generic;
using System.Numerics;
using IA_Library;
using IA_Library_FSM;
using IA_Library.Brain;

namespace IA_Library_FSM
{
    public enum Behaviours
    {
        MoveToFood,
        MoveEscape,
        Eat,
        Death,
        Corpse
    }

    public enum Flags
    {
        None,
        OnTransitionMoveToEat,
        OnTransitionMoveEscape,
        OnTransitionEat,
    }
    
    public class Agent
    {
        protected (float, float) position;
        
        public virtual void StartAgent()
        {

        }

        public virtual void Update()
        {
            
        }
    }
    
    public abstract class MoveState : State
    {

    }
    
    public abstract class EatState : State
    {
        protected int totalFoodEaten = 0;
        protected bool hasEaten = false;
    }
}