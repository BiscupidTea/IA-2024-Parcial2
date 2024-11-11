namespace IA_Library_FSM
{
    public class AgentScavenger : Agent
    {
        protected FSM<Behaviours, Flags> fsmController;

        protected Brain mainBrain;
        protected Brain flockingBrain;
        protected Brain moveFoodBrain;
        protected Brain eatBrain;

        public override void StartAgent()
        {
            fsmController.AddBehaviour<MoveToEatScavengerState>(Behaviours.MoveToFood,
                onTickParameters: () => { return new object[] { mainBrain, moveFoodBrain }; });
            fsmController.AddBehaviour<EatScavengerState>(Behaviours.Eat,
                onTickParameters: () => { return new object[] { mainBrain, eatBrain }; });


            fsmController.SetTransition(Behaviours.MoveToFood, Flags.OnTransitionEat,
                Behaviours.Eat);

            fsmController.SetTransition(Behaviours.Eat, Flags.OnTransitionMoveToEat,
                Behaviours.MoveToFood);
        }

        public override void Update()
        {
            fsmController.Tick();
        }
    }

    public class MoveToEatScavengerState : MoveState
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

    public class EatScavengerState : EatState
    {
        protected float MinEatRadius;
            
        public override BehavioursActions GetOnEnterbehaviour(params object[] parameters)
        {
            float posX = (float)(parameters[0]);
            float posY = (float)(parameters[1]);
            float nearFoodRadius = (float)(parameters[2]);
            bool hasEatenFood = (bool)parameters[3];
            //insert parameters to brain;
            
            return default;
        }

        public override BehavioursActions GetTickbehaviour(params object[] parameters)
        {
            float[] outputs = parameters[0] as float[];
            float posX = (float)(parameters[1]);
            float posY = (float)(parameters[2]);
            float nearFoodRadius = (float)(parameters[3]);
            bool hasEatenFood = (bool)parameters[4];
            //brain output;

            if (outputs[0] >= 0f)
            {
                if (nearFoodRadius < MinEatRadius && !hasEatenFood)
                {
                    //Eat+
                    //Fitness+
                    //If comi 5
                    // fitness skyrocket
                    hasEaten = true;
                }
                else if (hasEatenFood)
                {
                    //Fitness-
                }
                else if (nearFoodRadius > MinEatRadius)
                {
                    //Fitness-
                }
            }
            else
            {
                if (nearFoodRadius < MinEatRadius && !hasEatenFood)
                {
                    //fitness-
                }
                else if (hasEatenFood)
                {
                    //Fitness+   
                }
            }

            return default;
        }

        public override BehavioursActions GetOnExitbehaviour(params object[] parameters)
        {
            return default;
        }
    }
}