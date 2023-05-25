using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    public float fitness;

    public DenseLayer layer1;
    public DenseLayer layer2;

    GameObject geneticAlgorithmGO;
    GeneticAlgorithm geneticAlgorithm;

    Movement movement;

    public int inputLength; //+1 extra ray    -    31
    float[] gameState;

    private void Awake() {
        inputLength = 2; //distance to closest obstacle, block or spike
        layer1 = new DenseLayer(inputLength, 3);
        layer2 = new DenseLayer(3, 1);

        gameState = new float[inputLength];

        movement = GetComponentInParent<Movement>();
        geneticAlgorithmGO = GameObject.Find("Genetic Algorithm");
        geneticAlgorithm = geneticAlgorithmGO.GetComponent<GeneticAlgorithm>();
    }

    public class DenseLayer {
        public float[] biases;
        public float[][] weights;
        public int neurons;
        public int inputs;
        public float[] output;


        public DenseLayer(int inputs, int neurons) { //__init__(inputs, neurons)
            float[] biases = new float[neurons];
            float[][] weights = new float[neurons][];
            for (int n = 0; n < neurons; n++) {
                weights[n] = new float[inputs];
                for (int i = 0; i < inputs; i++) {
                    weights[n][i] = Random.Range(-0.1f, 0.1f);
                }
            }

            this.inputs = inputs;
            this.neurons = neurons;
            this.weights = weights;
            this.biases = biases;
        }

        public void Forward(float[] inputValues) {
            float[] output = new float[neurons];
            for (int n = 0; n < neurons; n++) {
                for (int i = 0; i < inputs; i++) {
                    output[n] += inputValues[i] * weights[n][i]; //dot product of weights
                }
                output[n] += biases[n]; //biases
                output[n] = Mathf.Max(0, output[n]); // ReLU
            }
            this.output = output;
        }

    }

    void print<T>(T[] array) {
        Debug.Log(string.Join(" ", array));
    }

    float Sigmoid(float n) {
        return 1 / (1 + Mathf.Pow(2.718281828f, -n));
    }

    bool ForwardPass(float[] inputs) {
        layer1.Forward(inputs);
        layer2.Forward(layer1.output);
        bool output = Sigmoid(layer2.output[0]) > 0.5 ? true : false;
        return output;
    }

    float shootRay(int angle) {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 direction = rotation * Vector3.right;

        float maxDistance = 5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, ~(1 << 10)); //dont interact with other agents on layer 10

        float distance = 0;
        if (hit) {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
        } else {
            Debug.DrawRay(transform.position, direction * maxDistance, Color.green);
        }

        return distance;
    }

    void FixedUpdate() {
        GameObject closestSpike = null;
        int closestObstacleType = 0;

        float closestDistance = Mathf.Infinity;
        float searchRange = 15f; // Search for spikes within 10 units of the player
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, searchRange);
        foreach (Collider2D collider in nearbyColliders) {
            float playerX = transform.position.x;
            float spikeX = collider.transform.position.x;
            if (spikeX > playerX && spikeX - playerX < closestDistance) {
                closestSpike = collider.gameObject;
                closestDistance = spikeX - playerX;
                if (collider.CompareTag("Spike")) {
                    closestObstacleType = 0;
                    Debug.DrawRay(transform.position, Vector3.right * closestDistance, Color.red);
                } else if (collider.CompareTag("Block")) {
                    closestObstacleType = 1;
                    Debug.DrawRay(transform.position, Vector3.right * closestDistance, Color.blue);
                }
            }
        }


        gameState[0] = closestDistance;
        gameState[1] = closestObstacleType;   

        movement.clicking = ForwardPass(gameState);

        if (movement.runOver) {
            fitness = transform.position.x;
            returnInfoOnDeath();
            Destroy(transform.parent.gameObject);
        }
    }

    void returnInfoOnDeath() {
        GeneticAlgorithm.NNinfo agentInfo;
        agentInfo.weights1 = layer1.weights;
        agentInfo.biases1 = layer1.biases;
        agentInfo.weights2 = layer2.weights;
        agentInfo.biases2 = layer2.biases;
        agentInfo.fitness = fitness;
        geneticAlgorithm.addAgentInfo(agentInfo);
    }
}