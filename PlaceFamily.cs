using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    public class PlaceFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;

            //Get Family Symbol
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> symbols = collector.OfClass(typeof(FamilySymbol)).WhereElementIsElementType().ToElements();

            FamilySymbol symbol = null;
            foreach (Element ele in symbols)
            {
                if (ele.Name == "1525 x 762mm")
                {
                    symbol = ele as FamilySymbol;
                    break;
                }
            }

            try
            {
                using (Transaction trans = new Transaction(doc, "Place Family"))
                {
                    trans.Start();

                    if (!symbol.IsActive)
                    {
                        symbol.Activate();
                    }

                    doc.Create.NewFamilyInstance(new XYZ(0, 0, 0), symbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

                    trans.Commit();
                }


                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

        }
    }
}
