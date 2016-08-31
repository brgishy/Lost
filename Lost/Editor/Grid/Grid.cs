//-----------------------------------------------------------------------
// <copyright file="Grid.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    
    public class Grid
    {
        public static readonly int RowHeight = 20;
        public static readonly int Padding = 4;

        private static GUIStyle columnHeaderGuiStyle = null;
        private static GUIStyle rowGuiStyle = null;

        private static List<Color> backgroundColors = new List<Color>();
        private GridDefinition gridDefinition;
        private int currentColumnIndex = 0;

        static Grid()
        {
            if (LostEditorUtil.IsProTheme())
            {
                backgroundColors.Add(new Color(0.204f, 0.094f, 0.094f));  // dark red
                backgroundColors.Add(new Color(0.208f, 0.176f, 0.106f));  // dark yellow
                backgroundColors.Add(new Color(0.180f, 0.204f, 0.208f));  // dark blue
            }
            else
            {
                backgroundColors.Add(new Color(0.612f, 0.282f, 0.282f));  // red
                backgroundColors.Add(new Color(0.624f, 0.528f, 0.318f));  // yellow
                backgroundColors.Add(new Color(0.540f, 0.612f, 0.624f));  // blue
            }
        }

        public Grid(GridDefinition gridDefinition)
        {
            this.gridDefinition = gridDefinition;
        }

        public void BeginGrid()
        {
            if (this.gridDefinition.DrawHeader)
            {
                this.DrawHeader();
            }
        }

        public void EndGrid()
        {
            //// private bool DrawAddButton(SerializedProperty property, float totalWidth)
            //// {
            ////     // drawing the add button at the bottom of the grid
            ////     Rect addButtonPosition;
            ////     using (new BeginVerticalHelper(out addButtonPosition, GUILayout.Width(totalWidth)))
            ////     {
            ////         EditorGUILayout.Space();
            ////         EditorGUILayout.Space();
            //// 
            ////         addButtonPosition.x += Padding;
            ////         addButtonPosition.y += 3;
            ////         addButtonPosition.width = totalWidth;
            ////         addButtonPosition.height = 13;
            //// 
            ////         if (GUI.Button(addButtonPosition, "+"))
            ////         {
            ////             property.InsertArrayElementAtIndex(property.arraySize);
            ////             property.serializedObject.ApplyModifiedProperties();
            ////             return true;
            ////         }
            ////     }
            //// 
            ////     return false;
            //// }

            EditorGUILayout.Space();
        }

        public void BeginRow()
        {
            this.currentColumnIndex = 0;

            float totalWidth = this.GetTotalCellWidth();
                            
            if (rowGuiStyle == null)
            {
                rowGuiStyle = new GUIStyle();
                rowGuiStyle.padding = new RectOffset(0, 0, 0, 0);
                rowGuiStyle.margin = new RectOffset(0, 0, 1, 1);
            }

            rowGuiStyle.normal.background = LostEditorUtil.MakeTexture(this.GetCurrentRowColor());

            EditorGUILayout.BeginHorizontal(rowGuiStyle, GUILayout.Height(RowHeight), GUILayout.Width(totalWidth));
        
            //// public BeginGridRowHelper(float width, int height, Color rowColor, out Rect position)
            //// {
            ////     GUIStyle newStyle = new GUIStyle();
            ////     newStyle.padding = new RectOffset();
            ////     newStyle.margin = new RectOffset();
            ////     newStyle.normal.background = LostEditorUtil.MakeTexture(rowColor);
            //// 
            ////     position = EditorGUILayout.BeginHorizontal(newStyle, GUILayout.Height(height), GUILayout.Width(width));
            //// }
        }
         
        public void EndRow()
        {
            //// using (new BeginHorizontalHelper())
            //// {
            ////     // drawing the delete button
            ////     Rect deleteButtonPosition;
            ////     using (new BeginHorizontalHelper(out deleteButtonPosition))
            ////     {
            ////         deleteButtonPosition.x += 4;
            ////         deleteButtonPosition.y += 4;
            ////         deleteButtonPosition.width = 15;
            ////         deleteButtonPosition.height = 13;
            //// 
            ////         if (GUI.Button(deleteButtonPosition, "-"))
            ////         {
            ////             currentProperty.DeleteArrayElementAtIndex(j);
            ////             currentProperty.serializedObject.ApplyModifiedProperties();
            ////             isDirty = true;
            ////             break;
            ////         }
            ////     }
            //// }
        
            EditorGUILayout.EndHorizontal();
        }
        
        #region Cell Drawing

        public void DrawLabel(string value)
        {
            var column = this.GetNextColumn();
            GUILayout.Space(5);
            EditorGUILayout.LabelField(value, GUILayout.Width(column.Width - 10));
            GUILayout.Space(1);
        }

        public string DrawString(string stringValue)
        {
            var column = this.GetNextColumn();

            GUILayout.Space(5);
            var newValue = EditorGUILayout.TextField(GUIContent.none, stringValue, GUILayout.Width(column.Width - 10));
            GUILayout.Space(1);

            return newValue;
        }

        public bool DrawBool(bool boolValue)
        {
            var column = this.GetNextColumn();

            float actualToggleControlWidth = 10.0f;
            float spaceWidth = (column.Width / 2) - (actualToggleControlWidth / 2);
            float toggleWidth = column.Width - spaceWidth;
            GUILayout.Space(spaceWidth - 2);

            return EditorGUILayout.Toggle(boolValue, GUILayout.Width(toggleWidth - 2));
        }

        public Color DrawColor(Color colorValue)
        {
            var column = this.GetNextColumn();

            GUILayout.Space(5);
            var newValue = EditorGUILayout.ColorField(colorValue, GUILayout.Width(column.Width - 10));
            GUILayout.Space(1);

            return newValue;
        }

        public float DrawFloat(float floatValue)
        {
            return this.DrawFloat(floatValue, float.MinValue, float.MaxValue);
        }

        public float DrawFloat(float floatValue, float minValue, float maxValue)
        {
            var column = this.GetNextColumn();

            GUILayout.Space(5);
            float newValue = EditorGUILayout.FloatField(GUIContent.none, floatValue, GUILayout.Width(column.Width - 10));
            GUILayout.Space(1);

            return Mathf.Clamp(newValue, minValue, maxValue);
        }

        public int DrawInt(int intValue)
        {
            return this.DrawInt(intValue, int.MinValue, int.MaxValue);
        }

        public int DrawInt(int intValue, int minValue, int maxValue)
        {
            var column = this.GetNextColumn();

            GUILayout.Space(5);
            int newValue = EditorGUILayout.IntField(GUIContent.none, intValue, GUILayout.Width(column.Width - 10));
            GUILayout.Space(1);

            return Mathf.Clamp(newValue, minValue, maxValue);
        }

        //// public void DrawEnum<T>(ref T enumValue) where T : struct, IConvertible
        //// {
        ////     var column = this.GetNextColumn();
        //// 
        ////     int currentIndex = enumValue.To;
        //// 
        ////     GUILayout.Space(3);
        ////     int newIndex = EditorGUILayout.Popup(currentIndex, Enum.GetValues(typeof(T)).ToArray(), GUILayout.Width(column.Width - 3));
        ////     
        ////     if (newIndex != property.enumValueIndex)
        ////     {
        ////         property.enumValueIndex = newIndex;
        ////     }
        //// }

        public Sprite DrawSprite(Sprite value, bool allowSceneObjects)
        {
            var column = this.GetNextColumn();

            GUILayout.Space(5);
            var newValue = (Sprite)EditorGUILayout.ObjectField(value, typeof(Sprite), allowSceneObjects, GUILayout.Width(column.Width - 10));
            GUILayout.Space(1);

            return newValue;
        }

        public UnityEngine.Object DrawObject(UnityEngine.Object value, bool allowSceneObjects)
        {
            var column = this.GetNextColumn();
            
            GUILayout.Space(5);
            var newValue = EditorGUILayout.ObjectField(GUIContent.none, value, typeof(UnityEngine.Object), allowSceneObjects, GUILayout.Width(column.Width - 10), GUILayout.ExpandHeight(false));
            GUILayout.Space(1);

            return newValue;
        }

        public T DrawObject<T>(T value, bool allowSceneObjects) where T : UnityEngine.Object
        {
            var column = this.GetNextColumn();

            GUILayout.Space(5);
            var newValue = EditorGUILayout.ObjectField(GUIContent.none, value, typeof(T), allowSceneObjects, GUILayout.Width(column.Width - 10)) as T;
            GUILayout.Space(1);

            return newValue;
        }

        public Vector3 DrawVector3(Vector3 value)
        {
            var column = this.GetNextColumn();

            using (new BeginHorizontalHelper())
            {
                float width = (column.Width / 3.0f) - 6;

                GUILayout.Space(5);
                float x = EditorGUILayout.FloatField(GUIContent.none, value.x, GUILayout.Width(width));
                float y = EditorGUILayout.FloatField(GUIContent.none, value.y, GUILayout.Width(width));
                float z = EditorGUILayout.FloatField(GUIContent.none, value.z, GUILayout.Width(width));

                return new Vector3(x, y, z);
            }
        }

        #endregion
        
        private void DrawHeader()
        {
            if (columnHeaderGuiStyle == null)
            {
                columnHeaderGuiStyle = new GUIStyle(GUI.skin.box);
                columnHeaderGuiStyle.padding = new RectOffset(0, 0, 3, 3);
                columnHeaderGuiStyle.margin = new RectOffset(0, 0, 0, 0);
                columnHeaderGuiStyle.alignment = TextAnchor.LowerCenter;
                columnHeaderGuiStyle.stretchWidth = true;

                if (LostEditorUtil.IsProTheme())
                {
                    columnHeaderGuiStyle.normal.textColor = Color.white;
                }
                else
                {
                    columnHeaderGuiStyle.normal.background = LostEditorUtil.MakeTexture(new Color(0.7f, 0.7f, 0.7f));
                    columnHeaderGuiStyle.normal.textColor = Color.black;
                }
            }

            using (new BeginHorizontalHelper())
            {
                for (int i = 0; i < this.gridDefinition.ColumnCount; i++)
                {
                    var column = this.gridDefinition[i];

                    using (new BeginHorizontalHelper(GUILayout.MaxWidth(column.Width), GUILayout.MinWidth(column.Width)))
                    {
                        string tooltip = column.Tooltip;

                        if (string.IsNullOrEmpty(tooltip))
                        {
                            tooltip = column.Name;
                        }

                        EditorGUILayout.LabelField(new GUIContent(column.Name, tooltip), columnHeaderGuiStyle);
                    }
                }
            }
        }

        private Color GetCurrentRowColor()
        {
            if (LostEditorUtil.IsProTheme())
            {
                return new Color(0.298f, 0.298f, 0.298f);
            }
            else
            {
                return new Color(0.867f, 0.867f, 0.867f);
            }

            // return backgroundColors[i % backgroundColors.Count];
        }

        private GridDefinition.Column GetNextColumn()
        {
            int currentIndex = this.currentColumnIndex;
            this.currentColumnIndex++;
            return this.gridDefinition[currentIndex];
        }

        private float GetTotalCellWidth()
        {
            float total = 0;

            for (int i = 0; i < this.gridDefinition.ColumnCount; i++)
            {
                total += this.gridDefinition[i].Width;
            }

            return total;
        }
    }
}