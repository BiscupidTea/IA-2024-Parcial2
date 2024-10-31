using IA_Library_FSM;
using IA_Library.Brain;

namespace IA_Library_FSM
{
    public enum MainBehaviours
    {
        Move,
        Eat,
    }

    public enum MainFlags
    {
        OnTransitionMove,
        OnTransitionEat
    }

    public class Agent
    {
        protected FSM<MainBehaviours, MainFlags> fsmController;
        protected (float, float) position;

        protected NeuralNetwork MainBrain;

        public virtual void StartAgent()
        {
            fsmController = new FSM<MainBehaviours, MainFlags>();
            
            fsmController.AddBehaviour<MoveState>(MainBehaviours.Move);
            fsmController.AddBehaviour<EatState>(MainBehaviours.Eat);
            
            fsmController.SetTransition(MainBehaviours.Move, MainFlags.OnTransitionEat, MainBehaviours.Eat);
            fsmController.SetTransition(MainBehaviours.Eat, MainFlags.OnTransitionMove, MainBehaviours.Move);
        }

        public void Update()
        {
            fsmController.Tick();
        }
    }

    public sealed class MoveState : State
    {
        public override BehavioursActions GetOnEnterbehaviour(params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public override BehavioursActions GetTickbehaviour(params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public override BehavioursActions GetOnExitbehaviour(params object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public sealed class EatState : State
    {
        public override BehavioursActions GetOnEnterbehaviour(params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public override BehavioursActions GetTickbehaviour(params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public override BehavioursActions GetOnExitbehaviour(params object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}