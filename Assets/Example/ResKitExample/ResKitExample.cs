using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Example
{
    public class Student
    {
        public int ID;
    }

    public class ResKitExample : MonoBehaviour
    {
        Cube m_Cube;

        [Button("ResKitLoadMethodExample")]
        void ResKitLoadMethodExample()
        {
            Student student = ResManager.Instance.Load<Student>();
            student.ID = 2;
            Debug.Log(student.ID.ToString());
            ResManager.Instance.Load<Cube>("Cube");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A) && m_Cube == null)
            {
                ResManager.Instance.LoadGameObjAsync<Cube>("Cube", CallBack);
            }
            if (Input.GetKeyDown(KeyCode.S) && m_Cube != null)
            {
                PoolManager.Instance.Recycle(m_Cube.gameObject);
                m_Cube = null;
            }
        }

        void CallBack(Cube cube)
        {
            m_Cube = cube;
        }
    }
}