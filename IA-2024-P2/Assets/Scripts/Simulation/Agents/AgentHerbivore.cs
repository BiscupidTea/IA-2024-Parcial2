using IA_Library.Brain;

namespace IA_Library_FSM
{
    public class AgentHerbivore : Agent
    {
        protected FSM<Behaviours, Flags> fsmController;

        protected Brain mainBrain;
        protected Brain escapeBrain;
        protected Brain moveFoodBrain;
        protected Brain eatBrain;
        
        public override void StartAgent()
        {
            fsmController = new FSM<Behaviours, Flags>();

            fsmController.AddBehaviour<MoveToEatHerbivoreState>(Behaviours.MoveToFood,
                onTickParameters: () => { return new object[] { mainBrain, moveFoodBrain }; });
            fsmController.AddBehaviour<MoveToEscapeHerbivoreState>(Behaviours.MoveEscape,
                onTickParameters: () => { return new object[] { mainBrain, escapeBrain }; });
            fsmController.AddBehaviour<EatHerbivoreState>(Behaviours.Eat,
                onTickParameters: () => { return new object[] { mainBrain, eatBrain }; });

            fsmController.AddBehaviour<DeathHerbivoreState>(Behaviours.Death);

            fsmController.SetTransition(Behaviours.MoveToFood, Flags.OnTransitionMoveEscape,
                Behaviours.MoveEscape);
            fsmController.SetTransition(Behaviours.MoveToFood, Flags.OnTransitionEat,
                Behaviours.Eat);

            fsmController.SetTransition(Behaviours.MoveEscape, Flags.OnTransitionMoveToEat,
                Behaviours.MoveToFood);
            fsmController.SetTransition(Behaviours.MoveEscape, Flags.OnTransitionEat,
                Behaviours.Eat);

            fsmController.SetTransition(Behaviours.Eat, Flags.OnTransitionMoveToEat,
                Behaviours.MoveToFood);
            fsmController.SetTransition(Behaviours.Eat, Flags.OnTransitionMoveEscape,
                Behaviours.MoveEscape);
        }

        public override void Update()
        {
            fsmController.Tick();
        }
    }

    public class MoveToEatHerbivoreState : MoveState
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

    public class MoveToEscapeHerbivoreState : MoveState
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

    public class EatHerbivoreState : EatState
    {
        public override BehavioursActions GetOnEnterbehaviour(params object[] parameters)
        {
            float posX = (float)(parameters[0]);
            float posY = (float)(parameters[1]);
            float nearFoodX = (float)(parameters[2]);
            float nearFoodY = (float)(parameters[3]);
            bool hasEatenFood = (bool)parameters[4];
            //parameters[5] insert parameters to brain;


            return default;
        }

        public override BehavioursActions GetTickbehaviour(params object[] parameters)
        {
            float[] outputs = parameters[0] as float[];
            float posX = (float)(parameters[1]);
            float posY = (float)(parameters[2]);
            float nearFoodX = (float)(parameters[3]);
            float nearFoodY = (float)(parameters[4]);
            bool hasEatenFood = (bool)parameters[5];
            // parameters[6] as Brain;

            if (outputs[0] >= 0f)
            {
                if (posX == nearFoodX && posY == nearFoodY && !hasEatenFood)
                {
                    //TODO: Eat++
                    //Fitness ++
                    //If comi 5
                    // fitness skyrocket
                    hasEaten = true;
                }
                else if (hasEatenFood)
                {
                    //Todo: Fitness*-
                }
                else if (posX != nearFoodX || posY != nearFoodY)
                {
                    //TODO: Fitness--
                }
            }
            else
            {
                if (posX == nearFoodX && posY == nearFoodY && !hasEatenFood)
                {
                    //TODO: fitness--
                }
                else if (hasEatenFood)
                {
                    //Todo: Fitness++   
                }
            }

            return default;
        }

        public override BehavioursActions GetOnExitbehaviour(params object[] parameters)
        {
            return default;
        }
    }

    public class DeathHerbivoreState : State
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