using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    public float fitness;

    public DenseLayer layer1;
    public DenseLayer layer2;
    GameObject Player;
    GameObject geneticAlgorithmGO;
    GeneticAlgorithm geneticAlgorithm;

    Movement movement;

    void print<T>(T[] array) {
        Debug.Log(string.Join(" ", array));

    }

    float Sigmoid(float n) {
        return 1 / (1 + Mathf.Pow(2.718281828f, -n));
    }
    // Start is called before the first frame update
    void Start() {
        Player = GameObject.Find("Player");
        movement = Player.GetComponent<Movement>();
        geneticAlgorithmGO = GameObject.Find("Genetic Algorithm");
        geneticAlgorithm = geneticAlgorithmGO.GetComponent<GeneticAlgorithm>();

        layer1 = new DenseLayer(3, 2);
        layer2 = new DenseLayer(2, 1);


        Debug.Log(ForwardPass(new float[] { 1, -1, 1 })); //make sure martches size of input layer
        Debug.Log(layer2.output[0] + " Raw output");
        Debug.Log(Sigmoid(layer1.output[0]) + " Sigmoid of output");
        //Debug.Log(layer2.biases[0]);
        //layer2.biases = new float[] { 0.1f };
        //Debug.Log(layer2.biases[0]);
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

    bool ForwardPass(float[] inputs) {
        layer1.Forward(inputs);
        layer2.Forward(layer1.output);
        return layer2.output[0] > 0.5 ? true : false;
    }

    // Update is called once per frame
    void FixedUpdate() {
        //distance to closest obstacle sent by ray
        //5 speeds, 6 gamemodes, onground
        int rays = 18; //is actually rays+1 amount of rays because of <=
        int inputLength = rays + 12 + 1; //+1 extra ray
        float[] gameState = new float[inputLength];

        fitness += 0.01f;
        int counter = 0;
        for (int angle = 0; angle <= 180; angle += 180/rays) {
            Quaternion rotation = Quaternion.Euler(0, 0, (angle + 270) % 360);
            Vector3 direction = rotation * Vector3.right;

            float maxDistance = 10f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance);

            float distance = 0;
            if (hit) {
                distance = hit.distance;
                //Debug.Log("Distance to closest obstacle: " + distance);
                Debug.DrawRay(transform.position, direction * distance, Color.red);
            } else {
                Debug.DrawRay(transform.position, direction * maxDistance, Color.green);
            }
            gameState[counter] = distance;
            counter += 1;
        }

        for (int i = 0; i<5; i++) {
            gameState[counter + i] = movement.CurrentSpeed == (Speeds)i ? 1f : 0f;
        }
        for (int i = 0; i < 6; i++) {
            gameState[counter + 5 + i] = movement.CurrentGamemode == (Gamemodes)i ? 1f : 0f;
        }
        gameState[counter + 11] = movement.OnGround() ? 1f : 0f;
        //TODO: is on orb
        print(gameState);
    }

    void returnInfoOnDeath() {
        geneticAlgorithm.addAgentInfo(getAgentInfo());
    }

    public GeneticAlgorithm.NNinfo getAgentInfo() {
        GeneticAlgorithm.NNinfo agentInfo;
        agentInfo.weights1 = layer1.weights;
        agentInfo.biases1 = layer1.biases;
        agentInfo.weights2 = layer2.weights;
        agentInfo.biases2 = layer2.biases;
        agentInfo.fitness = fitness;
        return agentInfo;
    }

}