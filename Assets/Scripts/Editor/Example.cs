/* 5 main principles of coding
    1. dry : dont repeat yourself
    2. Solid:
    S - Single-responsiblity Principle
        (each function / class should do one thing)
    O - Open-closed Principle
        (functions should be open to be extended but closed to modification)
    L - Liskov Substitution Principle
        (its about how to implement inheritance while making it easier not harder, read up)
    I - Interface Segregation Principle
        (nothing should be forced to implement an interface it doesnt use, but in general u need more interfacing imo)
    D - Dependency Inversion Principle
        (inversion of control and dependency injection)
    3. kiss: keep it simple stupid
*/

public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
{
    List<string> ignoreClassFullNames = new List<string> { "TMPro.TMP_FontAsset" };

    EditorGUI.BeginProperty(position, label, property);

    var fieldType = GetFieldType();

    if (fieldType == null || ignoreClassFullNames.Contains(fieldType.FullName))
    {
        EditorGUI.PropertyField(position, property, label);
        EditorGUI.EndProperty();
        return;
    }

    DrawMainPropertyField(position, property, label, fieldType);
    DrawExpandedObjectProperties(position, property);

    EditorGUI.EndProperty();
}

private void DrawMainPropertyField(Rect position, SerializedProperty property, GUIContent label, Type fieldType)
{
    var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

    if (property.objectReferenceValue != null && AreAnySubPropertiesVisible(property))
    {
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);
    }
    else
    {
        foldoutRect.x += 12;
        EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true, EditorStyles.label);
    }

    var indentedPosition = EditorGUI.IndentedRect(position);
    var indentOffset = indentedPosition.x - position.x;
    var propertyRect = new Rect(position.x + (EditorGUIUtility.labelWidth - indentOffset), position.y, position.width - (EditorGUIUtility.labelWidth - indentOffset), EditorGUIUtility.singleLineHeight);

    if (property.objectReferenceValue != null || property.hasMultipleDifferentValues)
    {
        propertyRect.width -= buttonWidth;
    }

    EditorGUI.ObjectField(propertyRect, property, fieldType, GUIContent.none);

    if (GUI.changed)
    {
        property.serializedObject.ApplyModifiedProperties();
    }
}

private void DrawExpandedObjectProperties(Rect position, SerializedProperty property)
{
    var buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);

    if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null)
    {
        DrawExpandedObjectFields(position, property);
    }
    else
    {
        DrawCreateButton(property, buttonRect);
    }

    property.serializedObject.ApplyModifiedProperties();
}

private void DrawExpandedObjectFields(Rect position, SerializedProperty property)
{
    var data = (ScriptableObject)property.objectReferenceValue;

    if (property.isExpanded)
    {
        DrawScriptableObjectFields(position, property, data);
    }
}

private void DrawScriptableObjectFields(Rect position, SerializedProperty property, ScriptableObject data)
{
    GUI.Box(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, Screen.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");

    EditorGUI.indentLevel++;
    SerializedObject serializedObject = new SerializedObject(data);

    SerializedProperty prop = serializedObject.GetIterator();
    float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

    if (prop.NextVisible(true))
    {
        do
        {
            if (prop.name == "m_Script") continue;
            float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width - buttonWidth, height), prop, true);
            y += height + EditorGUIUtility.standardVerticalSpacing;
        }
        while (prop.NextVisible(false));
    }

    if (GUI.changed)
    {
        serializedObject.ApplyModifiedProperties();
    }

    serializedObject.Dispose();
    EditorGUI.indentLevel--;
}

private void DrawCreateButton(SerializedProperty property, Rect buttonRect)
{
    if (GUI.Button(buttonRect, "Create"))
    {
        string selectedAssetPath = GetSelectedAssetPath(property);

        property.objectReferenceValue = CreateAssetWithSavePrompt(GetFieldType(), selectedAssetPath);
    }
}

private string GetSelectedAssetPath(SerializedProperty property)
{
    string selectedAssetPath = "Assets";

    if (property.serializedObject.targetObject is MonoBehaviour)
    {
        MonoScript ms = MonoScript.FromMonoBehaviour((MonoBehaviour)property.serializedObject.targetObject);
        selectedAssetPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(ms));
    }

    return selectedAssetPath;
}