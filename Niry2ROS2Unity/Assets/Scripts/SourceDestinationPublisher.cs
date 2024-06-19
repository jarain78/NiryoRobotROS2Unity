using System;
using RosMessageTypes.Geometry;

using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.UrdfImporter;
using UnityEngine;
//using ShoulderPositionMessage = RosMessageTypes.Std.Float32MultiArrayMsg;
using JointStates = RosMessageTypes.Sensor.JointStateMsg;

using System.Collections;


public class SourceDestinationPublisherNiryo : MonoBehaviour
{
    const int k_NumRobotJoints = 7;

    public static readonly string[] LinkNames = { "/link1", "/link2", "/link3", "/link4", "/link5",  "/link6",  "/link7"};

    // Variables required for ROS communication
    [SerializeField]
    string m_TopicName = "/joint_states";

    [SerializeField]
    GameObject m_myPalletizer260;
    [SerializeField] private ArticulationBody[] robotJoints = new ArticulationBody[k_NumRobotJoints];

    // Robot Joints
    UrdfJointRevolute[] m_JointArticulationBodies;

    // ROS Connector
    ROSConnection m_Ros;

    void Start()
    {
        // Get ROS connection static instance
        m_Ros = ROSConnection.GetOrCreateInstance();

        // Subscribe to joint_state topic
        m_Ros.Subscribe<JointStates>(m_TopicName, getMessageJointState);

        m_JointArticulationBodies = new UrdfJointRevolute[k_NumRobotJoints];

        Debug.Log("N-Joints: " + k_NumRobotJoints);
       
        
    void Update()
    {
               
        
    }

    void getMessageJointState(JointStates jointPosMsg)
    {
        //Debug.Log(shoulderPosMsg.data);
       StartCoroutine(SetJointValues(jointPosMsg));
    }
    
    IEnumerator SetJointValues(JointStates message)
    {
        Debug.Log("Message Length: " + message.name.Length.ToString());
        

        for (int i = 0; i < message.name.Length; i++)
        {           
            var joint1XDrive = robotJoints[i].xDrive;
            float jointAngle = (float)(message.position[i]) * Mathf.Rad2Deg;            
            joint1XDrive.target = jointAngle;
            robotJoints[i].xDrive = joint1XDrive;            
            Debug.Log(jointAngle.ToString("F4") + " " + i.ToString());
        }
 
        yield return new WaitForSeconds(0.5f);
    }
}
}

