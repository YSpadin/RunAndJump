﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace RunAndJump.LevelCreator
{
    public class PaletteWindow : EditorWindow
    {
        private string _path = "Assets/Prefabs/LevelPieces";
        private List<PaletteItem> _items;
        private Dictionary<PaletteItem.Category, List<PaletteItem>> _categorizedItems;
        private Dictionary<PaletteItem, Texture2D> _previews;
        private Vector2 _scrollPosition;
        private const float ButtonWidth = 80;
        private const float ButtonHeight = 90;
        private List<PaletteItem.Category> _categories;
        private List<string> _categoryLabels;
        public static PaletteWindow instance;
        private PaletteItem.Category _categorySelected;

        public delegate void itemSelectedDelegate(PaletteItem item, Texture2D
preview);
        public static event itemSelectedDelegate ItemSelectedEvent;
        public static void ShowPalette()
        {
            instance = (PaletteWindow)EditorWindow.GetWindow
            (typeof(PaletteWindow));
            instance.titleContent = new GUIContent("Palette");
        }
        private void OnEnable()
        {
            // Debug.Log("OnEnable called...");
            if (_categories == null)
            {
                InitCategories();
            }
            if (_categorizedItems == null)
            {
                InitContent();
            }
        }

        private void InitContent()
        {
            Debug.Log("InitContent called...");
            // Set the ScrollList
            _items = EditorUtils.GetAssetsWithScript<PaletteItem>(_path);
            _categorizedItems = new Dictionary<PaletteItem.Category,
            List<PaletteItem>>();
            _previews = new Dictionary<PaletteItem, Texture2D>();
            // Init the Dictionary
            foreach (PaletteItem.Category category in _categories)
            {
                _categorizedItems.Add(category, new List<PaletteItem>());
            }
            // Assign items to each category
            foreach (PaletteItem item in _items)
            {
                _categorizedItems[item.category].Add(item);
            }
        }

        private void InitCategories()
        {
            Debug.Log("InitCategories called...");
            _categories = EditorUtils.GetListFromEnum<PaletteItem.Category>();
            _categoryLabels = new List<string>();
            foreach (PaletteItem.Category category in _categories)
            {
                _categoryLabels.Add(category.ToString());
            }
        }

        private void DrawTabs()
        {
            int index = (int)_categorySelected;
            index = GUILayout.Toolbar(index, _categoryLabels.ToArray());
            _categorySelected = _categories[index];
        }
        private void OnDisable()
        {
            Debug.Log("OnDisable called...");
        }
        private void OnDestroy()
        {
            Debug.Log("OnDestroy called...");
        }
        private void OnGUI()
        {
            DrawTabs();
            DrawScroll();
        }
        private void Update()
        {
            // Debug.Log("OnGUI called...");
            if (_previews.Count != _items.Count)
            {
                GeneratePreviews();
            }
        }
        private GUIContent[] GetGUIContentsFromItems()
        {
            List<GUIContent> guiContents = new List<GUIContent>();
            if (_previews.Count == _items.Count)
            {
                int totalItems = _categorizedItems[_categorySelected].Count;
                for (int i = 0; i < totalItems; i++)
                {
                    GUIContent guiContent = new GUIContent();
                    guiContent.text = _categorizedItems[_categorySelected]
                    [i].itemName;
                    guiContent.image = _previews[_categorizedItems[_categorySelected][i]];
                    guiContents.Add(guiContent);
                }
            }
            return guiContents.ToArray();
        }
        private void DrawScroll()
        {
            if (_categorizedItems[_categorySelected].Count == 0)
            {
                EditorGUILayout.HelpBox("This category is empty!",
                MessageType.Info);
                return;
            }

            int rowCapacity =
            Mathf.FloorToInt(position.width / (ButtonWidth));
            _scrollPosition =
            GUILayout.BeginScrollView(_scrollPosition);
            int selectionGridIndex = -1;
            selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, GetGUIContentsFromItems(), rowCapacity, GetGUIStyle());
            GetSelectedItem(selectionGridIndex);
            GUILayout.EndScrollView();
        }
        private GUIStyle GetGUIStyle()
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.alignment = TextAnchor.LowerCenter;
            guiStyle.imagePosition = ImagePosition.ImageAbove;
            guiStyle.fixedWidth = ButtonWidth;
            guiStyle.fixedHeight = ButtonHeight;
            return guiStyle;
        }
        private void GetSelectedItem(int index)
        {
            if (index != -1)
            {
                PaletteItem selectedItem =
                _categorizedItems[_categorySelected][index];
                Debug.Log("Selected Item is: " +
                selectedItem.itemName);
                if (ItemSelectedEvent != null)
                {
                    ItemSelectedEvent(selectedItem, _previews[selectedItem]);
                }
            }
        }
        private void GeneratePreviews()
        {
            foreach (PaletteItem item in _items)
            {
                if (!_previews.ContainsKey(item))
                {
                    Texture2D preview = AssetPreview.GetAssetPreview(item.gameObject);


                    if (preview != null)
                    {
                        _previews.Add(item, preview);
                    }
                }

            }
        }
    }
}