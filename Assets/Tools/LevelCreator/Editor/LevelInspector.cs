using UnityEngine;
using UnityEditor;
namespace RunAndJump.LevelCreator
{
    [CustomEditor(typeof(Level))]
    public class LevelInspector : Editor
    {
        private int _newTotalColumns;
        private int _newTotalRows;
        private Level _myTarget;
        private PaletteItem _itemSelected;
        private Texture2D _itemPreview;
        private LevelPiece _pieceSelected;
        private void OnEnable()
        {
            Debug.Log("OnEnable was called...");
            _myTarget = (Level)target;

            _myTarget = (Level)target;
            InitLevel();
            ResetResizeValues();
            SubscribeEvents();
            // Debug.Log ("OnEnable was called...");
            _myTarget = (Level)target;
            InitLevel();
            ResetResizeValues();

        }
        private void UpdateCurrentPieceInstance(PaletteItem item, Texture2D preview)
        {
            _itemSelected = item;
            _itemPreview = preview;
            _pieceSelected = (LevelPiece)item.GetComponent<LevelPiece>();
            Repaint();
        }
        private void UnsubscribeEvents()
        {
            PaletteWindow.ItemSelectedEvent -= new PaletteWindow.
itemSelectedDelegate(UpdateCurrentPieceInstance);
        }
        private void DrawPieceSelectedGUI()
        {
            EditorGUILayout.LabelField("Piece Selected", EditorStyles.
            boldLabel);
            if (_pieceSelected == null)
            {
                EditorGUILayout.HelpBox("No piece selected!", MessageType.
                Info);
            }
            else
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField(new GUIContent(_itemPreview),
                GUILayout.Height(40));
                EditorGUILayout.LabelField(_itemSelected.itemName);
                EditorGUILayout.EndVertical();
            }
        }
        private void SubscribeEvents()
        {
            PaletteWindow.ItemSelectedEvent += new PaletteWindow.itemSelectedDelegate(UpdateCurrentPieceInstance);
        }
        private void OnDisable()
        {
            Debug.Log("OnDisable was called...");
            UnsubscribeEvents();
        }
        private void OnDestroy()
        {
            Debug.Log("OnDestroy was called...");
        }
        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();
            DrawLevelDataGUI();
            DrawLevelSizeGUI();
            DrawPieceSelectedGUI();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(_myTarget);
            }
        }
        private void DrawLevelSizeGUI()
        {
            EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical();
            _newTotalColumns = EditorGUILayout.IntField("Columns", Mathf.Max
(1, _newTotalColumns));
            _newTotalRows = EditorGUILayout.IntField("Rows", Mathf.Max(1, _newTotalRows));
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            // with this variable we can enable or disable GUI
            bool oldEnabled = GUI.enabled;
            GUI.enabled = (_newTotalColumns != _myTarget.TotalColumns || _newTotalRows != _myTarget.TotalRows);
            bool buttonResize = GUILayout.Button("Resize", GUILayout.Height(2
            * EditorGUIUtility.singleLineHeight));
            if (buttonResize)
            {
                if (EditorUtility.DisplayDialog(
                "Level Creator",
                "Are you sure you want to resize the level?\nThis action cannote undone.",
                "Yes",
                "No"))
                {
                    ResizeLevel();
                }
            }
            bool buttonReset = GUILayout.Button("Reset");
            if (buttonReset)
            {
                ResetResizeValues();
            }
            GUI.enabled = oldEnabled;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        private void InitLevel()
        {
            if (_myTarget.Pieces == null || _myTarget.Pieces.Length == 0)
            {
                Debug.Log("Initializing the Pieces array...");
                _myTarget.Pieces = new LevelPiece[_myTarget.TotalColumns *
                _myTarget.TotalRows];
            }
        }
        private void ResetResizeValues()
        {
            _newTotalColumns = _myTarget.TotalColumns;
            _newTotalRows = _myTarget.TotalRows;
        }
        private void ResizeLevel()
        {
            LevelPiece[] newPieces = new LevelPiece[_newTotalColumns * _newTotalRows];
            for (int col = 0; col < _myTarget.TotalColumns; ++col)
            {
                for (int row = 0; row < _myTarget.TotalRows; ++row)
                {
                    if (col < _newTotalColumns && row < _newTotalRows)
                    {
                        newPieces[col + row * _newTotalColumns] =
                        _myTarget.Pieces[col + row * _myTarget.
                        TotalColumns];
                    }
                    else
                    {
                        LevelPiece piece = _myTarget.Pieces[col + row * _myTarget.TotalColumns];
                        if (piece != null)
                        {
                            // we must to use DestroyImmediate in a Editor context
                            Object.DestroyImmediate(piece.gameObject);
                        }
                    }
                }
            }
            _myTarget.Pieces = newPieces;
            _myTarget.TotalColumns = _newTotalColumns;
            _myTarget.TotalRows = _newTotalRows;
        }
        private void DrawLevelDataGUI()
        {
            EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            _myTarget.TotalTime = EditorGUILayout.IntField("Total Time", Mathf.
            Max(0, _myTarget.TotalTime));

            _myTarget.Gravity = EditorGUILayout.FloatField("Gravity",
            _myTarget.Gravity);
            _myTarget.Bgm = (AudioClip)EditorGUILayout.ObjectField("Bgm",
            _myTarget.Bgm, typeof(AudioClip), false);
            _myTarget.Background = (Sprite)EditorGUILayout.ObjectField
            ("Background", _myTarget.Background, typeof(Sprite), false);
            EditorGUILayout.EndVertical();
        }
    }
}

