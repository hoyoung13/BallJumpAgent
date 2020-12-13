using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class BallAgentLogic : Agent
{
    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform target;

    public override void OnEpisodeBegin(){
        //BallAgent 초기화
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(-9, 0.5f, 0);

        // 타겟의 위치가 움직입니다!
        target.localPosition = new Vector3(12 + Random.value * 8, Random.value * 3, Random.value * 10 - 5);
    }

    public override void CollectObservations(VectorSensor sensor){
        // 타겟과 Agent의 위치와 속도
        try{
            sensor.AddObservation(target.localPosition);
            sensor.AddObservation(this.transform.localPosition);
            sensor.AddObservation(rBody.velocity);
        }finally{
            
        }

    }

    public float speed = 20;

    public override void OnActionReceived(float[] vectorAction){
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];

        if(vectorAction[1]==2){
            controlSignal.z = 1;
        }
        else{
            controlSignal.z = -vectorAction[1];
        }
        
        if(this.transform.localPosition.z < 8.5){
            rBody.AddForce(controlSignal*speed);
        }

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }

    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
    }
}
