﻿using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class DeleteElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;
            try 
            {
                //Pick Object
                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                //Delete Element
                if (pickedObj != null)
                { 
                using(Transaction trans = new Transaction(doc, "Delete Element"))
                    {
                        trans.Start();
                        doc.Delete(pickedObj.ElementId);

                        TaskDialog tDialog = new TaskDialog("Delete Element");
                        tDialog.MainContent = "Are you sure you want to delete this element?";
                        tDialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

                        if(tDialog.Show() == TaskDialogResult.Ok)
                        {
                            trans.Commit();
                            TaskDialog.Show("Delete", pickedObj.ElementId.ToString() + " Deleted");
                        }
                        else
                        {
                            trans.RollBack();
                            TaskDialog.Show("Delete", pickedObj.ElementId.ToString() + " Not deleted");
                        }
                    }
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
