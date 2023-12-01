using CommunityToolkit.Mvvm.ComponentModel;
using Components.Render.TypeDescription.Conditions;
using Components.VisualEditor.Controls.Inspector;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
namespace Components.VisualEditor.ViewModels.Validation;

public partial class ConditionStatementViewModel : ObservableValidator
{
    public static readonly Regex ConditionStatementRegex = new Regex (@"^([!]?Horizontal)|((\$\w+)(\s*(==|!=|>|<|>=|<=)\s*(\w+))?)$");

    #region Statement
    [ObservableProperty]
    [CustomValidation (typeof (ConditionStatementViewModel), nameof (ValidateStatement))]
    private string? _statement;

    public static ValidationResult? ValidateStatement (string? value, ValidationContext context)
    {
        if (string.IsNullOrWhiteSpace (value))
            return new ValidationResult ($"Condition statement cannot be empty.");
        
        if (!ConditionStatementRegex.IsMatch (value))
            return new ValidationResult ($"Invalid condition statement.");

        return ValidationResult.Success;
    }
    #endregion

    [ObservableProperty] private ConditionTree.ConditionOperator _operator = ConditionTree.ConditionOperator.AND;

    public void ChangeOperator (string? parameter)
    {
        var next = Operator + 1;
        if (next > ConditionTree.ConditionOperator.OR)
            next = ConditionTree.ConditionOperator.AND;

        Operator = next;
    }

    public ConditionStatementViewModel (string? statement = null)
    {
        statement ??= "Horizontal";

        Statement = statement;
    }
}
