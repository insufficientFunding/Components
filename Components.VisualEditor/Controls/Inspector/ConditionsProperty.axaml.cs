using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using Components.Interfaces.TypeDescription;
using Components.Render.TypeDescription.Conditions;
using Components.VisualEditor.ViewModels;
using Components.VisualEditor.ViewModels.Validation;
using Components.Xml.Parsers.Conditions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

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
        if (condition is not null)
            Console.WriteLine (ConditionsCollection.Remove (condition));

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
            new ConditionStatementViewModel ("$Type == Gay"),
            new ConditionStatementViewModel (""),
            new ConditionStatementViewModel ("!Horizontal"),
            new ConditionStatementViewModel ("!Gay"),
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

            if (i > 0)
                builder.Append ($" {flattenedOperator} ");

            builder.Append (item.Statement);
        }

        var result = builder.ToString ();

        Console.WriteLine (result);
        
        return result;
    }
}
