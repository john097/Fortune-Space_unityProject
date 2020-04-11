using UnityEngine;
using BehaviorDesigner.Runtime;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the any objects are within sight of the agent.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=11")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CanSeeObject : Conditional
    {
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
        [Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
#endif
        [Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
        public SharedTransform targetObject;
        [Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        
        [Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat fieldOfViewAngle = 90;
        [Tooltip("The distance that the agent can see ")]
        public SharedFloat viewDistance = 1000;
        [Tooltip("The offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The object that is within sight")]
        public SharedTransform objectInSight;

        public mon_attack A;
        public mon_skill B;

        private BattleManager MCS_manager;

        Transform ProtectPoint;
        Transform Player;


        //BOSSÂß¼­ÅÐ¶¨±äÁ¿
        public BehaviorTree behaviortree;
        public BOSS_SIMPLEMOVE C;
        public SharedBool UsingLaser;

        public SharedBool usingskill;
      
        public SharedBool lv2;
        public SharedBool dash;

        public override void OnStart()
        {
            //A.attack_finished = true;
            behaviortree = gameObject.GetComponent<BehaviorTree>();
            MCS_manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();

            usingskill = (SharedBool)behaviortree.GetVariable("USING_SKILL");

            UsingLaser = (SharedBool)behaviortree.GetVariable("Laser_Using");
            lv2 = (SharedBool)behaviortree.GetVariable("LV2_SPEED_RUNNING");
            dash = (SharedBool)behaviortree.GetVariable("DASH_RUNNING");

            base.OnStart();
        }

        // Returns success if an object was found otherwise failure
        public override TaskStatus OnUpdate()
        {
            if (MCS_manager.Protect_Room_Battle)
            {
                ProtectPoint = GameObject.Find("ProtectPoint").transform;
                targetObject.Value = ProtectPoint;

            }
            else
            {
                Player = GameObject.Find("Actor").transform;
                targetObject.Value = Player;
            }

#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            if (usePhysics2D) {
                // If the target object is null then determine if there are any objects within sight based on the layer mask
                if (targetObject.Value == null) {
                    
                    objectInSight.Value = MovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask);
                } else { // If the target is not null then determine if that object is within sight
                    objectInSight.Value = MovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value);
                }
            } else {
#endif
                
                // If the target object is null then determine if there are any objects within sight based on the layer mask
                if (targetObject.Value == null) {
                    
                    objectInSight.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask);
                } else { // If the target is not null then determine if that object is within sight
                    objectInSight.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value);
                }
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            }
#endif
            if (gameObject.tag == "BOSS")
            {
                if (/*lv2.Value || */UsingLaser.Value /*|| dash.Value*/)
                {
                    return TaskStatus.Failure;
                }
            }
            

            if (objectInSight.Value != null ) {
                // Return success if an object was found
                //A.attack_finished.Value = true;
                //B.skill_finished.Value = false;

                if (gameObject.tag=="BOSS")
                {
                    if (!UsingLaser.Value)
                    {
                        C.SpeedUpRemove_ATK_DIS();
                    }
                }

                return TaskStatus.Success;
            }
            if (gameObject.tag == "BOSS")
            {
                if (usingskill.Value)
                {
                    return TaskStatus.Success;
                }
            }

            if(gameObject.tag != "BOSS"&& !A.attack_finished.Value)
            {
                return TaskStatus.Success;
            }

            // An object is not within sight so return failure
            
            return TaskStatus.Failure;
        }

       

        // Reset the public variables
        public override void OnReset()
        {
            fieldOfViewAngle = 90;
            viewDistance = 1000;
            offset = Vector3.zero;
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
            if (fieldOfViewAngle == null || viewDistance == null) {
                return;
            }

            bool is2D = false;
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            is2D = usePhysics2D;
#endif      
            
            MovementUtility.DrawLineOfSight(Owner.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, is2D);
            
        }
    }
}