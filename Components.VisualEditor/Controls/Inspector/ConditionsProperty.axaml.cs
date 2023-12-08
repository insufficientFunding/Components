using Avalonia;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using Components.Render.TypeDescription.Conditions;
using Components.VisualEditor.Models;
using Components.VisualEditor.ViewModels.Validation;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Components.VisualEditor.Controls.Inspector;

public class ConditionsProperty : TemplatedControl
{
    #region Collection
    public static readonly StyledProperty<ObservableCollection<ConditionStatementViewModel>> ConditionsCollectionProperty =
        AvaloniaProperty.Register<ConditionsProperty, ObservableCollection<ConditionStatementViewModel>> (
            "ConditionsCollection");

    public ObservableCollection<ConditionStatementViewModel> ConditionsCollection
    {
        get => GetValue (ConditionsCollectionProperty);
        set => SetValue (ConditionsCollectionProperty, value);
    }
    #endregion

    #region Commands and Command Handlers
    public IRelayCommand<ConditionStatementViewModel> RemoveConditionCommand { get; }
    private void RemoveCondition (ConditionStatementViewModel? condition)
    {
        if (condition is null)
            return;
        
        ConditionsCollection.Remove (condition);

        ConditionsCollection = new ObservableCollection<ConditionStatementViewModel> (ConditionsCollection);
    }

    public IRelayCommand AddConditionCommand { get; }
    private void AddCondition ()
    {
        ConditionsCollection.Add (new ConditionStatementViewModel ());

        ConditionsCollection = new ObservableCollection<ConditionStatementViewModel> (ConditionsCollection);
    }
    #endregion

    public ConditionsProperty ()
    {
        ConditionsCollection =
        [
            new ConditionStatementViewModel ("Horizontal"),
        ];

        RemoveConditionCommand = new RelayCommand<ConditionStatementViewModel> (RemoveCondition);
        AddConditionCommand = new RelayCommand (AddCondition);
    }

    public string Flatten ()
    {
        if (ConditionsCollection is { Count: 0 })
            return string.Empty;

        StringBuilder builder = new StringBuilder ();
        for (int i = 0; i < ConditionsCollection.Count; i++)
        {
            var item = ConditionsCollection [i];

            string flattenedOperator = item.Operator == ConditionTree.ConditionOperator.AND ? "," : "|";

            builder.Append ('[');

            if (i > 0)
                builder.Append ($"{flattenedOperator}");

            builder.Append (item.Statement);
            builder.Append (']');
        }

        var result = builder.ToString ();
        
        return result;
    }

    public void Parse (string conditions)
    {
        var regex = new Regex (@"\[(.*?)\]");
        var matches = regex.Matches (conditions);

        foreach (Match match in matches)
        {
            var trimmed = match.Value.TrimStart ('[').TrimEnd (']');

            var isOR = trimmed.StartsWith ('|');
            var isAND = trimmed.StartsWith (',');

            if (isOR || isAND)
                trimmed = trimmed.Substring (1);
            
            var condition = new ConditionStatementViewModel (trimmed)
            {
                Operator = isOR? ConditionTree.ConditionOperator.OR : ConditionTree.ConditionOperator.AND,
            };

            ConditionsCollection.Add (condition);
        }
    }
}
