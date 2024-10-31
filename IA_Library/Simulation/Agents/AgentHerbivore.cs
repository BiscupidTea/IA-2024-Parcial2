using IA_Library.Brain;

namespace IA_Library_FSM
{
    public enum HerbivoreBehaviours
    {
        Move,
        Eat,
    }

    public enum HerbivoreFlags
    {
        OnTransitionMove,
        OnTransitionEat
    }
    
    public class AgentHerbivore : Agent
    {
        protected FSM<HerbivoreBehaviours, HerbivoreFlags> fsmController;
        
        protected NeuralNetwork MainBrain;
        protected NeuralNetwork SenseBrain;
        protected NeuralNetwork MoveBrain;
        protected NeuralNetwork EatBrain;

        public override void StartAgent()
        {
            fsmController = new FSM<HerbivoreBehaviours, HerbivoreFlags>();
            
            fsmController.AddBehaviour<MoveHerbivoreState>(HerbivoreBehaviours.Move);
            fsmController.AddBehaviour<EatHerbivoreState>(HerbivoreBehaviours.Eat);
            
            fsmController.SetTransition(HerbivoreBehaviours.Move, HerbivoreFlags.OnTransitionEat, HerbivoreBehaviours.Eat);
            fsmController.SetTransition(HerbivoreBehaviours.Eat, HerbivoreFlags.OnTransitionMove, HerbivoreBehaviours.Move);
        }
        
        public override void Update()
        {
            fsmController.Tick();
        }
    }
    
    public class MoveHerbivoreState : State
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
    
    public class EatHerbivoreState : State
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