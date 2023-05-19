using UnityEngine;

namespace Framework
{
    public static class GameObjectExtension
    {
        public static GameObject Show(this GameObject selfObj)
        {
            selfObj.SetActive(true);
            return selfObj;
        }

        public static T Show<T>(this T selfComponent) where T : Component
        {
            selfComponent.gameObject.Show();
            return selfComponent;
        }

        public static GameObject Hide(this GameObject selfObj)
        {
            selfObj.SetActive(false);
            return selfObj;
        }

        public static T Hide<T>(this T selfComponent) where T : Component
        {
            selfComponent.gameObject.Hide();
            return selfComponent;
        }

        public static GameObject Parent(this GameObject selfObj, GameObject parentGameObj)
        {
            selfObj.transform.SetParent(parentGameObj == null ? null : parentGameObj.transform);
            return selfObj;
        }
    }

    public static class TransformExtension
    {
        public static T Parent<T>(this T selfComponent, Component parentComponent) where T : Component
        {
            selfComponent.transform.SetParent(parentComponent == null ? null : parentComponent.transform);
            return selfComponent;
        }
    }
}