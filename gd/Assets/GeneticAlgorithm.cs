using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneticAlgorithm : MonoBehaviour {
    public struct NNinfo {
        public float[][] weights1;
        public float[] biases1;
        public float[][] weights2;
        public float[] biases2;
        public float fitness;
    }

    int numOfAgents = 100;
    List<NNinfo> population = new List<NNinfo>();

    public void addAgentInfo(NNinfo nnInfo) {
        population.Add(nnInfo);
    }

    // Start is called before the first frame update
    void Start() {
        Debug.Log(population);
    }

    void evolve() {
        List<NNinfo> sortedPopulation = population.OrderByDescending(individual => individual.fitness).ToList();
        List<NNinfo> breedingPool = sortedPopulation.Take(20).ToList();
        List<NNinfo> newPopulation;


    }

    void crossover(NNinfo a, NNinfo b) {

    }
    // Update is called once per frame
    void Update() {

    }
}