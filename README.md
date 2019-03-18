# Keyboard-Genetic-Algorithm
Final Project for Artificial Intelligence Class

## Purpose
In the late 19th century, the QWERTY keyboard was invented. This keyboard was invented due to the clashing within the typewriters when users typed common combinations like “st” and “th”. In an alphabetical keyboard, typewriters would jam constantly. Since this new keyboard did not jam, it was wildly successful in the market. 

Since the keyboard was more focused on the typewriter not breaking, and not on the typing efficiency, many tried to create new keyboards. One of the more popular of these attempts was the DVORAK keyboard, created in 1936. DVORAK claimed that it uses less finger motion and permits faster typing. Since everyone was already used to the QWERTY keyboard, it did not succeed on the market. Although the DVORAK keyboard push was a failure, most major modern operation systems allow you to switch to the DVORAK layout. 

My application is meant to do what the DVORAK attempted to do. Find the most efficient keyboard using a genetic algorithm. My plan is to create a fitness value function that goes through a large text document and figures out the best keyboard combination that checks for these three things:

The least finger movements
Alternating hand = better fitness value
Prioritize Pointer Finger over Middle Finger over Pinky over Ring Finger.

## Fitness Value

The fitness value with the following best qualities will be the fittest chromosome:

The least finger movements
Alternating hand = better fitness value
Prioritize Pointer Finger over Middle Finger over Pinky over Ring Finger.

When my program starts, it will calculate the FV of QWERTY and DVORAK. If DVORAK is better than QWERTY, this means that my fitness value is on the right track, because DVORAK has been proven to be better than QWERTY. As my program increases generations, it will compare its best chromosome with QWERTY and DVORAK and tell us when we meet + exceed these benchmarks. After it reaches these benchmarks, the program will keep going. The user will be able to stop the program once they notice that the keyboard is not improving anymore.

Fitness Value = (1 / (Σ distance between keys) / (# of characters in document))

The simple version of this formula is above, but there are plenty of modifiers that affect this value. Distance between keys depends on the placement on the keyboard. In the FitnessCalc class, there is a function called CalcDistance, which calculates the distance between two keys based on where they are on the keyboard. For example, G to H on the qwerty keyboard is 1 distance. But G to Y is 1.5 because it is diagonal. 

Besides these modifiers, such as rewarding alternating hands, making sure fingers go back to resting points after inactivity, and slightly prioritizing pointer finger the other fingers. 

## Screenshots

![Screenshot 1](https://i.imgur.com/h5WScrU.png)

In the above screenshot, I decided to test BigData.txt from Data Structures & Algorithms class. When testing BigData.txt, it took about 15 seconds per generation. After one generation, I found a fitness value which was larger than QWERTY. After 32 generations, I reached a fitness value above DVORAK, which was my goal. I decided to see how much it would improve after that. From 32 to 146, my fitness value raised from 2.49 to 3.01. After 146, my fitness value did not increase at all. Shown in the screenshot is an extremely efficient keyboard if you were to type out this 45,000 dissertation. 

The colors represent each finger and the finger prints represent the rest points. As you can see, Most of the vowels are on the rest positions of the keyboard and keys that aren’t used, like Z and X are to the far right side. 

![Screenshot 2](https://i.imgur.com/V266wA7.png)

This screenshot shows the keyboard settings. So, if you only typed with two fingers, this program can find the fitness value for the best keyboard that uses two keyboards. 


![Screenshot 3](https://i.imgur.com/QjRtYPj.png)

I also added a screenshot of my own custom keyboard (the way I personally type), solving the first chapter of a tale of two cities. Notice that this fitness value is a lot lower here. This is because the way I type is not as effective as typing with all 8 fingers, on either keyboard. 

![Screenshot 4](https://i.imgur.com/YIw3O05.png)

Variables that are used for this GA. 

