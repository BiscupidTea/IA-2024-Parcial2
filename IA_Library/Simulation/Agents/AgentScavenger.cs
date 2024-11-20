using System;
using System.Collections.Generic;
using System.Numerics;
using IA_Library;
using IA_Library.Brain;

namespace IA_Library_FSM
{
    public class AgentScavenger : Agent
    {
        public Brain flockingBrain = new Brain();

        public Vector2 position;
        public Vector2 Direction;
        public float speed;
        public float rotation;

        public float minEatRadius;

        public AgentScavenger(Simulation simulation, GridManager gridManager) : base(simulation, gridManager)
        {
            Action<Vector2> onMove;
            
            fsmController.AddBehaviour<MoveToEatScavengerState>(Behaviours.MoveToFood,
                onEnterParameters: () => { return new object[] { mainBrain, flockingBrain }; },
                onTickParameters: () =>
                {
                    return new object[]
                    {
                        mainBrain.outputs, flockingBrain.outputs, position, GetNearestFoodPosition(), onMove = MoveTo
                    };
                });

            fsmController.ForcedState(Behaviours.MoveToFood);
        }

        public override void Update(float deltaTime)
        {
            fsmController.Tick();
            MoveTo(Direction);
        }

        public override void ChooseNextState(float[] outputs)
        {
            throw new System.NotImplementedException();
        }

        public override void MoveTo(Vector2 direction)
        {
            position = direction;
        }

        public override void SettingBrainUpdate(float deltaTime)
        {
            Vector2 nearFoodPos = GetNearestFoodPosition();
            mainBrain.inputs = new[] { position.X, position.Y, minEatRadius, nearFoodPos.X, nearFoodPos.Y };
        }

        public override Vector2 GetNearestFoodPosition()
        {
            return simulation.GetNearestDeadHerbivorePosition(position);
        }

        private List<Vector2> GetNearestAgents()
        {
            return simulation.GetNearestScavengersPositions(position, 3);
        }
    }

    public class MoveToEatScavengerState : MoveState
    {
        public Brain flockingBrain;
        private Vector2 direction;
        
        public override BehavioursActions GetOnEnterBehaviour(params object[] parameters)
        {
            brain = parameters[0] as Brain;
            flockingBrain = parameters[1] as Brain;
            return default;
        }

        public override BehavioursActions GetTickBehaviour(params object[] parameters)
        {
            BehavioursActions behaviour = new BehavioursActions();

            float[] outputsMove = parameters[0] as float[];
            float[] outputsFlocking = parameters[1] as float[];
            
            float rotation = (float)(parameters[2]);
            float minEatRadius = (float)(parameters[3]);
            
            position = (Vector2)(parameters[4]);
            direction = (Vector2)(parameters[5]);
            
            float speed = (float)(parameters[6]);
            
            Vector2 nearFoodPos = (Vector2)parameters[7];
            var onMove = parameters[8] as Action<Vector2[]>;

            //Rotation
            behaviour.AddMultitreadableBehaviours(0, () =>
            {
                float leftValue = outputsMove[0];
                float rightValue = outputsMove[1];
                
                float netRotationValue = leftValue - rightValue;
                float turnAngle = netRotationValue * MathF.PI / 180;

                var rotationMatrix = new Matrix3x2(
                    MathF.Cos(turnAngle), MathF.Sin(turnAngle),
                    -MathF.Sin(turnAngle), MathF.Cos(turnAngle),
                    0, 0
                );

                direction = Vector2.Transform(direction, rotationMatrix);
                direction = Vector2.Normalize(direction);
                rotation += netRotationValue;
            });
            
            //Calculate Next Position
            behaviour.AddMultitreadableBehaviours(1, () =>
            {
                Vector2[] FinalPosition = new []{Vector2.Zero};
                
                
                FinalPosition[0] = Vector2.Zero;
                onMove.Invoke(FinalPosition);
            });

            //fitness
            behaviour.AddMultitreadableBehaviours(2, () =>
            {
                float distanceFromFood = Vector2.Distance(position, nearFoodPos);

                if (distanceFromFood < minEatRadius)
                {
                    brain.FitnessReward += 1;
                }
                else if (distanceFromFood > minEatRadius)
                {
                    brain.FitnessMultiplier -= 0.05f;
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