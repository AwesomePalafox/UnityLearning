using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class Test : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float turnSpeed = 50f;

    #region Light Switch
    private Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        LightSwitch();
        TurnAndMove();
    }
    #endregion


    void LightSwitch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            myLight.enabled = !myLight.enabled;
        }
    }


    void TurnAndMove()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);


    }

    #region Class
    /*
        public class Stuff
        {
            public int projectileA;
            public int projectileB;
            public int projectileC;

            public float fuel;

            public Stuff(int prA, int prB, int prC)
            {
                projectileA = prA;
                projectileB = prB;
                projectileC = prC;
            }

            public Stuff(int prA, float fu)
            {
                projectileA = prA;
                fuel = fu;
            }



            public Stuff()
            {
                projectileA = 1;
                projectileB = 1;
                projectileC = 1;
            }
        }

        public Stuff myStuff = new Stuff(50, 5, 5);

        public Stuff myOtherStuff = new Stuff(50, 1.5f);

        */
    #endregion

    #region Instantiate
    /*
    public Rigidbody projectile;
    public Transform barrelEnd;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody projectileInstance;
            projectileInstance = Instantiate(projectile, barrelEnd.position, barrelEnd.rotation) as Rigidbody;
            projectileInstance.AddForce(barrelEnd.up * 350f);
        }
    }

    public class ProjectileDestruction : MonoBehaviour
    {
        void Start()
        {
            Destroy(gameObject, 3.5f);
        }


    }

    */
    #endregion

    #region Arrays
    /*
    public class Arrays : MonoBehaviour
    {
        int[] myIntArray = new int[5];

        void Start()
        {
            myIntArray[0] = 12;
            myIntArray[1] = 76;
            myIntArray[2] = 8;
            myIntArray[3] = 937;
            myIntArray[4] = 21;
        }
    }
    public class ArraysInOtherForm : MonoBehaviour
    {
        int[] myOtherIntArray = { 12, 76, 8, 937, 21 };
    }


    public class Arrays : MonoBehaviour
    {
        public GameObject[] players;

        void Start()
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++)
            {
                Debug.Log("Player Number" + i + "is named" + players[i].name);
            }
        }
    }
    */
    #endregion

    #region Invoke
    /*
        public class InvokeScript : MonoBehaviour
        {
            public GameObject target;

            void Start()
            {
                Invoke("SpawnObject", 2);
            }

            void SpawnObject()
            {
                Instantiate(target, new Vector3(0, 2, 0), Quaternion.identity);
            }



            public class InvokeRepeatingScript : MonoBehaviour
            {
                public GameObject target;

                void Start()
                {
                    InvokeRepeating("SpawnObject", 2, 1);

                    CancelInvoke("SpawnObject");
                }

                void SpawnObject()
                {
                    float x = Random.Range(-2.0f, 2.0f);
                    float z = Random.Range(-2.0f, 2.0f);
                    Instantiate(target, new Vector3(x, 2, z), Quaternion.identity);
                }
            }

        }
        */
    #endregion

    #region enum
    /*
        public class CardinalDirection : MonoBehaviour
        {
            // enum Direction : short {North, East, South, West};
            enum Direction { North = 10, East = 15, South = 18, West = 30 };
            void Start()
            {
                Direction myDirection;

                myDirection = Direction.North;
            }

            Direction ReverseDirection(Direction dir)
            {
                if (dir == Direction.North)
                    dir = Direction.South;
                else if (dir == Direction.South)
                    dir = Direction.North;
                else if (dir == Direction.East)
                    dir = Direction.West;
                else if (dir == Direction.West)
                    dir = Direction.East;
                return dir;
            }
   

}
 */
    #endregion

    #region switch
    /*
        public class ConversationScript : MonoBehaviour
        {
            public int intelligence = 5;

            void Greeting()
            {
                switch (intelligence)
                {
                    case 5:
                        print("My intelligenci is 5");
                        break;
                    case 4:
                        print("My intelligenci is 4");
                        break;
                    case 3:
                        print("My intelligenci is 3");
                        break;
                    case 2:
                        print("My intelligenci is 2");
                        break;
                    case 1:
                        print("My intelligenci is 1");
                        break;
                    default:
                        print("Incorrect intelligence level.");
                        break;
                }

        }

    }
    */
    #endregion

    #region properities

    /*
        public class Player
        {
            private int experience;

            public int Experience
            {
                get
                {
                    return experience;
                }
                set
                {
                    experience = value;
                }
            }

            public int Level
            {
                get
                {
                    return experience / 1000;
                }
                set
                {
                    experience = value * 1000;
                }
            }

            public int Health { get; set; }
        }
        */
    #endregion

    #region TernaryOperator
    /*
        public class TernaryOperator : MonoBehaviour
        {
            void Start()
            {
                int health = 10;
                string message;

                message = health > 0 ? "Player is Alive" : "Player is Dead";
            }

        }
    */
    #endregion

    #region Method Overloading
    /*
        public class SomeClass
        {
            public int Add(int num1, int num2)
            {
                return num1 + num2;
            }

            public string Add(string str1, string str2)
            {
                return str1 + str2;
            }
        }

        public class SomeOtherClass : MonoBehaviour
        {
            void Start()
            {
                SomeClass myClass = new SomeClass();

                myClass.Add(1, 2);
                myClass.Add("Hello", "World");
        }
    }
    */
    #endregion

    #region Generics 
    /*
        public class GenericClass<T>
        {
            T item;

            public void AssignItem(T newItem)
            {
                item = newItem;
            }
        }

        public class GenericClassExample : MonoBehaviour
        {
            void Start()
            {
                GenericClass<int> myClass = new GenericClass<int>();

                myClass.AssignItem(5);
            }
        }
        public class SomeClass
        {
            public T GenericMethod<T>(T parm)
            {
                return parm;
            }
        }

        */
    #endregion

    #region Inheritance  
    /*
    public class Fruit
    {

    }
    public class Appple : Fruit
    {
        public Apple() : base("apple")
        {
        // ConstructorBuilder Code...
        }
     }
*/
    #endregion








}
