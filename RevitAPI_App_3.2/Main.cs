using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITraining_lab3._2
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            TaskDialog.Show("Выберите трубы", "Выберите трубы, после выбор нажмите Finish");

            IList<Reference> selectedPipesRefList = uidoc.Selection.PickObjects(ObjectType.Element, new PipeFilter(), "Выберите трубы");
            double length_pipe = 0;
            foreach (var selectedPipeRef in selectedPipesRefList)
            {
                Pipe pipe = doc.GetElement(selectedPipeRef) as Pipe;
                Parameter lengthParameter = pipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                if (lengthParameter.StorageType == StorageType.Double)
                { length_pipe += lengthParameter.AsDouble(); }
            }
            double lengthMeters = Math.Round(UnitUtils.ConvertFromInternalUnits(length_pipe, UnitTypeId.Meters), 2);

            TaskDialog.Show("Общая длина выбранных труб",
            $"Общая длина труб: {Environment.NewLine}{lengthMeters.ToString()} м. =  {length_pipe.ToString()} '");

            return Result.Succeeded;
        }
    }
}