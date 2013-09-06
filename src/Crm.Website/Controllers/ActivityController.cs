using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crm.Api;
using Crm.Website.Models;
using Newtonsoft.Json;
using Crm.Api.Organization;
using System.IO;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using System.Data;
using System.Text.RegularExpressions;

namespace Crm.Website.Controllers
{
    public class ActivityController : BaseController
    {
        public ActionResult Index()
        {
            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(GridViewType.ActivityManage, WebHelper.CurrentUserAccount);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            return View();
        }

        [HttpGet]
        public ActionResult ViewSetup(GridViewType viewType)
        {
            this.ViewBag.viewType = viewType;
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                ActivityCreateModel model = JsonConvert.DeserializeObject<ActivityCreateModel>(json);
                List<PropertyOperationInfo> propertys = MetadataHelper.MapPropertyOperationInfos(model.extends);
                WebHelper.ActivityService.Create(WebHelper.CurrentUserAccount, model.subject, model.contactId, propertys);
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
        public ActionResult Edit(string activityId)
        {
            ActivityInfo activityInfo = WebHelper.ActivityService.GetActivityById(activityId);
            ActivityEditModel editModel = new ActivityEditModel(activityInfo);
            this.ViewBag.activityInfoJson = JsonConvert.SerializeObject(editModel);

            return View();
        }

        [HttpPost]
        public ActionResult EditPost(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                ActivityEditPostModel model = JsonConvert.DeserializeObject<ActivityEditPostModel>(json);
                List<PropertyOperationInfo> propertys = MetadataHelper.MapPropertyOperationInfos(model.extends);
                WebHelper.ActivityService.Modify(WebHelper.CurrentUserAccount, model.id, model.subject, propertys);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string activityId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                WebHelper.ActivityService.Delete(WebHelper.CurrentUserAccount, activityId);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Activitys(string keyword, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount;
                List<ActivityInfo> activityInfos = null;
                if (string.IsNullOrEmpty(keyword))
                {
                    activityInfos = WebHelper.ActivityService.GetActivitys(WebHelper.CurrentUserAccount, start, size, out totalCount);
                }
                else
                {
                    Regex regex = new Regex("\\s*");
                    keyword = regex.Replace(keyword, " ");
                    activityInfos = WebHelper.ActivityService.Search(WebHelper.CurrentUserAccount, keyword.Split(' ').ToList(), start, size, out  totalCount);
                }
                List<ActivityGridJObjectModel> models = activityInfos.Select(x => new ActivityGridJObjectModel(x, this)).ToList();
                resultModel.data = new DatagridModel { count = totalCount, list = models };
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

    }
}
