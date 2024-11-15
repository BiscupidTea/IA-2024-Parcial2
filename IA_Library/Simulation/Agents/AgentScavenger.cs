using System.Collections.Generic;
using System.Numerics;
using IA_Library.Brain;

namespace IA_Library_FSM
{
    public class AgentScavenger : Agent
    {
        private Brain flockingBrain;
        private Brain moveToFoodBrain;
        private Brain eatBrain;

        private float rotation;

        public AgentScavenger()
        {
            fsmController.AddBehaviour<MoveToEatScavengerState>(Behaviours.MoveToFood,
                onTickParameters: () => { return new object[] { mainBrain, moveToFoodBrain }; });
            fsmController.AddBehaviour<EatScavengerState>(Behaviours.Eat,
                onTickParameters: () => { return new object[] { mainBrain, eatBrain }; });


            fsmController.SetTransition(Behaviours.MoveToFood, Flags.OnTransitionEat,
                Behaviours.Eat);

            fsmController.SetTransition(Behaviours.Eat, Flags.OnTransitionMoveToEat,
                Behaviours.MoveToFood);
        }

        public override void Update(float deltaTime)
        {
            fsmController.Tick();
        }

        public override void ChooseNextState(float[] outputs)
        {
            throw new System.NotImplementedException();
        }

        public override void MoveTo(Vector2 direction)
        {
            throw new System.NotImplementedException();
        }

        public override Vector2 GetNearestFoodPosition()
        {
            throw new System.NotImplementedException();
        }
    }

    //todo:Terminar el move de esto
    public class MoveToEatScavengerState : MoveState
    {
        private float previousDistance;
        private float MinEatRadius;

        public override BehavioursActions GetOnEnterBehaviour(params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public override BehavioursActions GetTickBehaviour(params object[] parameters)
        {
            BehavioursActions behaviour = new BehavioursActions();

            float[] outputs = parameters[0] as float[];
            position = (Vector2)(parameters[1]);
            Vector2 nearFoodPos = (Vector2)parameters[2];
            MinEatRadius = (float)(parameters[3]);
            
            behaviour.AddMultitreadableBehaviours(0, () =>
            {
                List<Vector2> newPositions = new List<Vector2> { nearFoodPos };
                float distanceFromFood = GetDistanceFrom(newPositions);

                if (distanceFromFood < MinEatRadius)
                {
                    brain.FitnessReward += 1;
                }
                else if (distanceFromFood > MinEatRadius)
                {
                    brain.FitnessMultiplier -= 0.05f;
                }
            });

            return behaviour;
        }

        public override BehavioursActions GetOnExitBehaviour(params object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }

    public class EatScavengerState : EatState
    {
        protected float MinEatRadius;

        public override BehavioursActions GetOnEnterBehaviour(params object[] parameters)
        {
            brain = parameters[0] as Brain;
            return default;
        }

        public override BehavioursActions GetTickBehaviour(params object[] parameters)
        {
            BehavioursActions behaviour = new BehavioursActions();
            
            float[] outputs = parameters[0] as float[];
            float currentDistance = (float)parameters[1];
            AgentHerbivore herbivore = parameters[2] as AgentHerbivore;
            bool maxEaten = (bool)parameters[3];
            int currentFood = (int)parameters[4];
            int maxEating = (int)parameters[5];

            behaviour.AddMultitreadableBehaviours(0, () =>
            {
                if (herbivore == null)
                {
                    return;
                }

                if (outputs[0] >= 0f)
                {
                    if (currentDistance <= MinEatRadius && !maxEaten)
                    {
                        if (herbivore.CanBeEaten())
                        {
                            herbivore.EatPiece();
                            currentFood++;

                            brain.FitnessReward += 20;

                            if (currentFood == maxEating)
                            {
                                brain.FitnessReward += 30;
                            }
                        }
                    }
                    else if (maxEaten || currentDistance > MinEatRadius)
                    {
                        brain.FitnessMultiplier -= 0.05f;
                    }
                }
                else
                {
                    if (currentDistance <= MinEatRadius && !maxEaten)
                    {
                        brain.FitnessMultiplier -= 0.05f;
                    }
                    else if (maxEaten)
                    {
                        brain.FitnessMultiplier += 0.10f;
                    }
                }
            });
            return behaviour;
        }

        public override BehavioursActions GetOnExitBehaviour(params object[] parameters)
        {
            brain.ApplyFitness();
            return default;
        }
    }
}