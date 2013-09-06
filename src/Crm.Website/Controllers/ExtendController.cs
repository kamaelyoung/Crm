using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crm.Api;
using Crm.Website.Models;
using Newtonsoft.Json;

namespace Crm.Website.Controllers
{
    public class ExtendController : Controller
    {
        [HttpGet]
        public ActionResult ViewSetup(GridViewType viewType, FormType formType)
        {
            List<ViewSetupFieldModel> fields = new List<ViewSetupFieldModel>();
            List<FieldInfo> fieldInfos = WebHelper.FormService.GetFields(formType);
            fields.AddRange(fieldInfos.Select(x => new ViewSetupFieldModel { fieldId = x.ID, name = x.Name }));

            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(viewType, WebHelper.CurrentUserAccount);
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
        public ActionResult ViewSetup(GridViewType viewType, string columnsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<GridViweColumnSetupModel> columnModels = JsonConvert.DeserializeObject<List<GridViweColumnSetupModel>>(columnsJson);
                List<GridViewColumnSetupInfo> columns = columnModels.Select(x => new GridViewColumnSetupInfo { FieldId = x.fieldId, Width = x.width }).ToList();
                WebHelper.GridViewService.SetViewColumns(viewType, WebHelper.CurrentUserAccount, columns);
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
        public ActionResult SearchFields(FormType formType)
        {
            List<FieldInfo> fields = WebHelper.FormService.GetFields(formType);
            this.ViewBag.fields = fields.Where(x => !x.CanModify).ToList();
            return PartialView();
        }

        [HttpGet]
        public ActionResult Fields(FormType formType)
        {
            List<FieldInfo> fields = WebHelper.FormService.GetFields(formType);
            this.ViewBag.fields = fields.Where(x => !x.CanModify).ToList();
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditFields(FormType formType)
        {
            List<FieldInfo> fields = WebHelper.FormService.GetFields(formType);
            this.ViewBag.fields = fields.Where(x => !x.CanModify).ToList();
            return PartialView();
        }
    }
}
