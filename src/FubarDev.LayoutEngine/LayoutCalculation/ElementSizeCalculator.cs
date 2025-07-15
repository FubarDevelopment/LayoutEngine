using System.Collections.Generic;
using System.Linq;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.LayoutCalculation;

internal abstract class ElementSizeCalculator(
    int availableSizeForFactors,
    double factorSum)
{
    protected abstract ElementInfo CalculateElementInfo(
        ILayoutItem item,
        int remainingSize,
        double remainingFactors);

    public Dictionary<ILayoutItem, int> Calculate(IReadOnlyCollection<ILayoutItem> children)
    {
        var factorSizeRemaining = availableSizeForFactors;
        var factorSumRemaining = factorSum;
        var firstCalculationElements = children
            .Select(x => CalculateElementInfo(x, factorSizeRemaining, factorSumRemaining))
            .ToList();

        // Finde alle Elemente mit endgültiger Größe
        var finalElements = firstCalculationElements.Where(x => x.Final).ToList();
        var nonFinalElements = firstCalculationElements.Where(x => !x.Final).ToList();
        
        // Finde die Elemente mit endgültiger Größe, die auf einem Faktor basieren
        var finalFactorElements = finalElements
            .Where(x => x.Factor != null)
            .ToList();
        while (finalFactorElements.Count != 0)
        {
            // Berechne die verbleibende Größe und Summe der Faktoren
            factorSumRemaining -= finalFactorElements.Select(x => x.Factor!.Value).Sum();
            factorSizeRemaining -= finalFactorElements.Select(x => x.CalculatedSize).Sum();

            // Berechne die nicht-finalen Elemente neu, basierend auf der verbleibenden Größe und Summe der Faktoren
            var calculated = nonFinalElements
                .Where(x => !x.Final)
                .Select(x => CalculateElementInfo(x.Item, factorSizeRemaining, factorSumRemaining))
                .ToList();

            finalFactorElements = calculated.Where(x => x.Final).ToList();
            nonFinalElements = calculated
                .Where(x => !x.Final)
                .ToList();
            
            finalElements.AddRange(calculated.Where(x => x.Final));
        }
        
        finalElements.AddRange(nonFinalElements);

        return finalElements.ToDictionary(x => x.Item, x => x.CalculatedSize);
    }
}
