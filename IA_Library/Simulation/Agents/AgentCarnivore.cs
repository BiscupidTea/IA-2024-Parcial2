using System;
using System.Collections.Generic;
using System.Numerics;
using IA_Library.Brain;

namespace IA_Library_FSM
{
    public class AgentCarnivore : Agent
    {
        public AgentCarnivore()
        {
            fsmController.AddBehaviour<MoveToEatCarnivoreState>(Behaviours.MoveToFood);
            fsmController.AddBehaviour<EatCarnivoreState>(Behaviours.Eat);
            
            fsmController.SetTransition(Behaviours.MoveToFood, Flags.OnTransitionEat, Behaviours.Eat);
            fsmController.SetTransition(Behaviours.Eat, Flags.OnTransitionMoveToEat, Behaviours.MoveToFood);
        }

        public override void Update(float deltaTime)
        {
            fsmController.Tick();
        }

        public override void ChooseNextState(float[] outputs)
        {
            throw new NotImplementedException();
        }

        public override void MoveTo(Vector2 direction)
        {
            throw new NotImplementedException();
        }

        public override Vector2 GetNearestFoodPosition()
        {
            throw new NotImplementedException();
        }
    }

    public class MoveToEatCarnivoreState : MoveState
    {
        private int movesPerTurn = 2;
        private float previousDistance;

        public override BehavioursActions GetOnEnterBehaviour(params object[] parameters)
        {
            brain = parameters[0] as Brain;
            positiveHalf = Neuron.Sigmoid(0.5f, brain.p);
            negativeHalf = Neuron.Sigmoid(-0.5f, brain.p);
            return default;
        }

        public override BehavioursActions GetTickBehaviour(params object[] parameters)
        {
            BehavioursActions behaviour = new BehavioursActions();

            float[] outputs = parameters[0] as float[];
            position = (Vector2)parameters[1];
            Vector2 nearFoodPos = (Vector2)parameters[2];
            var onMove = parameters[3] as Action<Vector2[]>;
            AgentHerbivore herbivore = parameters[4] as AgentHerbivore;
            behaviour.AddMultitreadableBehaviours(0, () =>
            {
                if (position == nearFoodPos)
                {
                    herbivore.ReceiveDamage();
                }

                Vector2[] direction = new Vector2[movesPerTurn];
                for (int i = 0; i < direction.Length; i++)
                {
                    direction[i] = GetDir(outputs[i]);
                }

                foreach (Vector2 dir in direction)
                {
                    onMove.Invoke(direction);
                    position += dir;
                }

                List<Vector2> newPositions = new List<Vector2> { nearFoodPos };
                float distanceFromFood = GetDistanceFrom(newPositions);
                if (distanceFromFood <= previousDistance)
                {
                    brain.FitnessReward += 20;
                    brain.FitnessMultiplier += 0.05f;
                }
                else
                {
                    brain.FitnessMultiplier -= 0.05f;
                }

                previousDistance = distanceFromFood;
            });
            return behaviour;
        }

        public override BehavioursActions GetOnExitBehaviour(params object[] parameters)
        {
            brain.ApplyFitness();
            return default;
        }
    }

    public class EatCarnivoreState : EatState
    {
        public override BehavioursActions GetOnEnterBehaviour(params object[] parameters)
        {
            brain = parameters[0] as Brain;
            return default;
        }

        public override BehavioursActions GetTickBehaviour(params object[] parameters)
        {
            BehavioursActions behaviour = new BehavioursActions();
            
            float[] outputs = parameters[0] as float[];
            position = (Vector2)parameters[1];
            Vector2 nearFoodPos = (Vector2)parameters[2];
            bool hasEatenEnoughFood = (bool)parameters[3];
            int counterEating = (int)parameters[4];
            int maxEating = (int)parameters[5];
            var onHasEatenEnoughFood = parameters[6] as Action<bool>;
            var onEaten = parameters[7] as Action<int>;
            AgentHerbivore herbivore = parameters[8] as AgentHerbivore;
            behaviour.AddMultitreadableBehaviours(0, () =>
            {
                if (herbivore == null)
                {
                    return;
                }

                if (outputs[0] >= 0f)
                {
                    if (position == nearFoodPos && !hasEatenEnoughFood)
                    {
                        if (herbivore.CanBeEaten())
                        {
                            onEaten(++counterEating);
                            
                            brain.FitnessReward += 20;
                            
                            if (counterEating == maxEating)
                            {
                                brain.FitnessReward += 30;
                                onHasEatenEnoughFood.Invoke(true);
                            }
                        }
                    }
                    else if (hasEatenEnoughFood || position != nearFoodPos)
                    {
                        brain.FitnessMultiplier -= 0.05f;
                    }
                }
                else
                {
                    if (position == nearFoodPos && !hasEatenEnoughFood)
                    {
                        brain.FitnessMultiplier -= 0.05f;
                    }
                    else if (hasEatenEnoughFood)
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