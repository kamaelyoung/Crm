using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Api;
using Coldew.Website.Models;
using Newtonsoft.Json;

namespace Coldew.Website.Controllers
{
    public class ExtendController : Controller
    {
        [HttpGet]
        public ActionResult ViewSetup(string viewId, string formId)
        {
            List<ViewSetupFieldModel> fields = new List<ViewSetupFieldModel>();
            List<FieldInfo> fieldInfos = WebHelper.FormService.GetFields(formId);
            fields.AddRange(fieldInfos.Select(x => new ViewSetupFieldModel { fieldId = x.ID, name = x.Name }));

            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(viewId);
            foreach (GridViewColumnInfo column in viewInfo.Columns)
            {
                ViewSetupFieldModel model = fields.Find(x => x.fieldId == column.FieldId);
                if (model != null)
                {
                    model.selected = true;
                    model.width = column.Width;
                }
            }

            this.ViewBag.fields = fields;
            return this.PartialView();
        }

        [HttpPost]
        public ActionResult SetViewSetup(string viewId, string columnsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<GridViweColumnSetupModel> columnModels = JsonConvert.DeserializeObject<List<GridViweColumnSetupModel>>(columnsJson);
                List<GridViewColumnSetupInfo> columns = columnModels.Select(x => new GridViewColumnSetupInfo { FieldId = x.fieldId, Width = x.width }).ToList();
                WebHelper.GridViewService.Modify(viewId, columns);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchFields(string formId)
        {
            List<FieldInfo> fields = WebHelper.FormService.GetFields(formId);
            this.ViewBag.fields = fields.ToList();
            return PartialView();
        }

        [HttpGet]
        public ActionResult Fields(string formId)
        {
            List<FieldInfo> fields = WebHelper.FormService.GetFields(formId);
            this.ViewBag.fields = fields.Where(x => x.CanInput).ToList();
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditFields(string formId)
        {
            List<FieldInfo> fields = WebHelper.FormService.GetFields(formId);
            this.ViewBag.fields = fields.Where(x => x.CanInput).ToList();
            return PartialView();
        }
    }
}
