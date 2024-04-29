using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControlle : MonoBehaviour
{
    public GameObject Bullet;
    public DataStrucuture.Stack<GameObject> stack;

    // Start is called before the first frame update
    void Start()
    {
        stack = new DataStrucuture.Stack<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(Bullet);
            obj.GetComponent<Bullet>().Init(transform.position, stack);
            stack.Push(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        GameObject Bul = stack.Pop();
        Bul.GetComponent<Bullet>().Init(transform.position, stack);
        Bul.SetActive(true);

    }
}
