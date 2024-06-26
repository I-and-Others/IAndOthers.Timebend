//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(HexRuleSet))]
//public class HexRuleSetEditor : Editor
//{
//    private SerializedProperty hexRuleProperty;

//    private void OnEnable()
//    {
//        hexRuleProperty = serializedObject.FindProperty("hexRule");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        SerializedProperty nameProperty = hexRuleProperty.FindPropertyRelative("name");
//        SerializedProperty prefabProperty = hexRuleProperty.FindPropertyRelative("hexPrefab");
//        SerializedProperty connectionsProperty = hexRuleProperty.FindPropertyRelative("connections");

//        EditorGUILayout.BeginVertical("box");
//        nameProperty.stringValue = EditorGUILayout.TextField("Name", nameProperty.stringValue);
//        EditorGUILayout.PropertyField(prefabProperty);

//        for (int j = 0; j < connectionsProperty.arraySize; j++)
//        {
//            SerializedProperty connectionProperty = connectionsProperty.GetArrayElementAtIndex(j);
//            SerializedProperty faceDirectionProperty = connectionProperty.FindPropertyRelative("faceDirection");
//            SerializedProperty allowedNeighborRulesProperty = connectionProperty.FindPropertyRelative("allowedNeighborRules");

//            EditorGUILayout.LabelField(faceDirectionProperty.enumDisplayNames[faceDirectionProperty.enumValueIndex]);

//            for (int k = 0; k < allowedNeighborRulesProperty.arraySize; k++)
//            {
//                SerializedProperty neighborRuleProperty = allowedNeighborRulesProperty.GetArrayElementAtIndex(k);
//                SerializedProperty hexRuleSetProperty = neighborRuleProperty.FindPropertyRelative("hexRuleSet");
//                SerializedProperty neighborFaceDirectionsProperty = neighborRuleProperty.FindPropertyRelative("neighborFaceDirections");

//                EditorGUILayout.BeginHorizontal();
//                EditorGUILayout.PropertyField(hexRuleSetProperty, GUIContent.none);
//                EditorGUILayout.PropertyField(neighborFaceDirectionsProperty, new GUIContent("Neighbor Faces"), true);
//                if (GUILayout.Button("-", GUILayout.Width(20)))
//                {
//                    allowedNeighborRulesProperty.DeleteArrayElementAtIndex(k);
//                }
//                EditorGUILayout.EndHorizontal();
//            }

//            if (GUILayout.Button("Add Neighbor Rule"))
//            {
//                allowedNeighborRulesProperty.InsertArrayElementAtIndex(allowedNeighborRulesProperty.arraySize);
//            }

//            EditorGUILayout.Space();
//        }

//        EditorGUILayout.EndVertical();

//        serializedObject.ApplyModifiedProperties();
//        EditorUtility.SetDirty(target);
//    }
//}
