using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OperationsLogic.Model;

namespace OperationsLogic.Misc;
public class ModelConverter
{
    public static CanonicalTableau ConvertToCanonicalTableau(LinearModel model)
    {
        // Essentially performing the same setup as in the constructor of CanonicalTableau
        ArgumentNullException.ThrowIfNull(model);

        if (model.ObjectiveCoefficients == null || model.Constraints == null || model.SignRestrictions == null)
            throw new ArgumentException("Model components cannot be null.");
        if (model.ObjectiveCoefficients.Count == 0)
            throw new ArgumentException("Objective function must have at least one coefficient.");
        if (model.Constraints.Count == 0)
            throw new ArgumentException("Model must have at least one constraint.");
        if (model.SignRestrictions.Length != model.ObjectiveCoefficients.Count)
            throw new ArgumentException("Sign restrictions must match the number of decision variables.");

        int decisionVars = model.ObjectiveCoefficients.Count;
        int numConstraints = model.Constraints.Count;
        int slackVars = model.Constraints.Count(c => c.Relation == "<=") + model.Constraints.Count(c => c.Relation == "=");
        int excessVars = model.Constraints.Count(c => c.Relation == ">=") + model.Constraints.Count(c => c.Relation == "=");
        int totalVars = decisionVars + excessVars + slackVars;
        int rows = numConstraints + 1;

        double[,] tableau = new double[rows, totalVars + 1];
        double[,] nonCanonicalTableau = new double[rows, totalVars + 1];

        bool isMaximization = model.Type.Equals("max", StringComparison.CurrentCultureIgnoreCase);
        for (int col = 0; col < decisionVars; col++)
        {
            tableau[0, col] = isMaximization ? -model.ObjectiveCoefficients[col] : model.ObjectiveCoefficients[col];
            nonCanonicalTableau[0, col] = model.ObjectiveCoefficients[col];
        }
        tableau[0, totalVars] = 0;
        nonCanonicalTableau[0, totalVars] = 0;

        int slackIndex = 0;
        int excessIndex = 0;
        for (int row = 0; row < numConstraints; row++)
        {
            Constraint constraint = model.Constraints[row];
            if (constraint.Coefficients == null)
                throw new ArgumentException($"Constraint {row + 1} has null coefficients.");
            if (constraint.Coefficients.Count > decisionVars)
                throw new ArgumentException($"Constraint {row + 1} has too many coefficients.");
            if (!new[] { "<=", ">=", "=" }.Contains(constraint.Relation))
                throw new ArgumentException($"Invalid relation in constraint {row + 1}: {constraint.Relation}");

            for (int col = 0; col < decisionVars; col++)
            {
                double coeff = col < constraint.Coefficients.Count ? constraint.Coefficients[col] : 0;
                tableau[row + 1, col] = constraint.Relation == ">=" ? -coeff : coeff;
                nonCanonicalTableau[row + 1, col] = coeff;
            }

            tableau[row + 1, totalVars] = constraint.Relation == ">=" ? -constraint.RHS : constraint.RHS;
            nonCanonicalTableau[row + 1, totalVars] = constraint.RHS;

            if (constraint.Relation == "<=")
            {
                tableau[row + 1, decisionVars + excessVars + slackIndex] = 1;
                nonCanonicalTableau[row + 1, decisionVars + excessVars + slackIndex] = 1;
                slackIndex++;
            }
            else if (constraint.Relation == ">=")
            {
                tableau[row + 1, decisionVars + excessIndex] = 1;
                nonCanonicalTableau[row + 1, decisionVars + excessIndex] = -1;
                excessIndex++;
            }
            else if (constraint.Relation == "=")
            {
                tableau[row + 1, decisionVars + excessVars + slackIndex] = 1;
                nonCanonicalTableau[row + 1, decisionVars + excessVars + slackIndex] = 1;
                tableau[row + 1, decisionVars + excessIndex] = 1;
                nonCanonicalTableau[row + 1, decisionVars + excessIndex] = -1;
                slackIndex++;
                excessIndex++;
            }
        }

        string[] validOperators = { "+", "-", "urs", "int", "bin" };
        foreach (string restriction in model.SignRestrictions)
        {
            if (!validOperators.Contains(restriction))
                throw new ArgumentException($"Invalid sign restriction: {restriction}");
        }

        CanonicalTableau resultTableau = new(
            decisionVars: decisionVars,
            excessVars: excessVars,
            slackVars: slackVars,
            isMaximization: isMaximization,
            signRestrictions: model.SignRestrictions,
            tableau: tableau,
            noncanonicaltableau: nonCanonicalTableau
        );

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < totalVars + 1; col++)
            {
                if (double.IsNaN(tableau[row, col]) || double.IsInfinity(tableau[row, col]))
                    throw new ArgumentException($"Invalid value in Tableau at [{row},{col}]: NaN or Infinity.");
                if (double.IsNaN(nonCanonicalTableau[row, col]) || double.IsInfinity(nonCanonicalTableau[row, col]))
                    throw new ArgumentException($"Invalid value in NonCanonicalTableau at [{row},{col}]: NaN or Infinity.");
            }
        }

        _ = resultTableau.GetBasicVariableIndices();

        return resultTableau;
    }

    public static LinearModel ConvertToLinearModel(CanonicalTableau tableau)
    {
        List<double> objectiveCoeffs = [];
        for (int i = 0; i < tableau.DecisionVars; i++)
        {
            objectiveCoeffs.Add(tableau.NonCanonicalTableau[0, i]);
        }

        List<Constraint> constraints = [];
        for (int row = 1; row < tableau.Rows; row++)
        {
            List<double> coeffs = [];

            for (int col = 0; col < tableau.DecisionVars; col++)
            {
                coeffs.Add(tableau.NonCanonicalTableau[row, col]);
            }

            double rhs = tableau.NonCanonicalTableau[row, tableau.TotalVars];

            string relation;
            if (tableau.IsSlackVariable(tableau.DecisionVars + tableau.ExcessVars + row - 1))
                relation = "<=";
            else if (tableau.IsSlackVariable(tableau.DecisionVars + row - 1))
                relation = "=";
            else
                relation = ">=";

            constraints.Add(new Constraint(coeffs, relation, rhs));
        }

        return new LinearModel(
            type: tableau.IsMaximization ? "max" : "min",
            objectiveCoefficients: objectiveCoeffs,
            constraints: constraints,
            signRestrictions: tableau.SignRestrictions
        );
    }

}
