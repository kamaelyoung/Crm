using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data.Organization;
using Coldew.Api.Organization;
using NHibernate.Criterion;

namespace Coldew.Core.Organization
{
    public class SignInLogService : ISignInLogService
    {
        public SignInLogService(OrganizationManagement organizationManager)
        {
            this.OrganizationManager = organizationManager;
        }

        public OrganizationManagement OrganizationManager { set; get; }

        public IList<SignInLog> GetLogs(SignInLogSearchInfo info, out int count)
        {
            List<SignInLogModel> models = this.Select(info, out count);
            if (models != null)
            {
                return models.Select(x => this.Map(x)).ToList();
            }
            return null;
        }

        /// <summary>
        /// 获取全部操作日志
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="count">总记录数</param>
        /// <returns>日志列表</returns>
        private List<SignInLogModel> Select(SignInLogSearchInfo searchInfo, out int count)
        {
            ICriterion criterion = null;
            if (searchInfo.SignInStartDate.HasValue && searchInfo.SignInEndDate.HasValue)
            {
                criterion = Expression.Where<SignInLogModel>((x =>
                    x.SignInTime >= searchInfo.SignInStartDate && x.SignInTime <= searchInfo.SignInEndDate));
            }
            else if (searchInfo.SignInStartDate.HasValue)
            {
                criterion = Expression.Where<SignInLogModel>(x => x.SignInTime >= searchInfo.SignInStartDate);
            }
            else if (searchInfo.SignInEndDate.HasValue)
            {
                criterion = Expression.Where<SignInLogModel>(x => x.SignInTime <= searchInfo.SignInEndDate);
            }

            if (!string.IsNullOrWhiteSpace(searchInfo.Account))
            {
                var accountCriterion = Expression.Like("Account", searchInfo.Account, MatchMode.Anywhere);
                if (criterion != null)
                {
                    criterion = Expression.And(criterion, accountCriterion);
                }
                else
                {
                    criterion = accountCriterion;
                }
            }

            if (searchInfo.Result != null && searchInfo.Result.Count > 0)
            {
                var singInResultCriterion = Expression.In("SignInResult", searchInfo.Result);
                if (criterion != null)
                {
                    criterion = Expression.And(criterion, singInResultCriterion);
                }
                else
                {
                    criterion = singInResultCriterion;
                }
            }

            if (!string.IsNullOrEmpty(searchInfo.Ip))
            {
                var clientInfoCriterion = Expression.Like("Ip", searchInfo.Ip, MatchMode.Anywhere);
                if (criterion != null)
                {
                    criterion = Expression.And(criterion, clientInfoCriterion);
                }
                else
                {
                    criterion = clientInfoCriterion;
                }
            }

            List<SignInLogModel> models = null;
            IEnumerable<SignInLogModel> queryFuture;
            if (criterion != null)
            {
                queryFuture = NHibernateHelper.CurrentSession.QueryOver<SignInLogModel>()
                    .Where(criterion)
                    .Future();
            }
            else
            {
                queryFuture = NHibernateHelper.CurrentSession.QueryOver<SignInLogModel>()
                    .Future();
            }
            var listCount = queryFuture.Count();
            count = listCount;

            IOrderedEnumerable<SignInLogModel> orderedOperationLog = null;
            if (searchInfo.Order.HasValue)
            {
                if (searchInfo.OrderByDescending)
                {
                    switch (searchInfo.Order.Value)
                    {
                        case SignInLogOrder.Account:
                            orderedOperationLog = queryFuture.OrderByDescending(x => x.Account);
                            break;
                        case SignInLogOrder.ClientInfo:
                            orderedOperationLog = queryFuture.OrderByDescending(x => x.Ip);
                            break;
                        case SignInLogOrder.Result:
                            orderedOperationLog = queryFuture.OrderByDescending(x => x.SignInResult);
                            break;
                        case SignInLogOrder.SignInTime:
                            orderedOperationLog = queryFuture.OrderByDescending(x => x.SignInTime);
                            break;
                        case SignInLogOrder.IP:
                            orderedOperationLog = queryFuture.OrderByDescending(x => x.Ip);
                            break;
                    }
                }
                else
                {
                    switch (searchInfo.Order.Value)
                    {
                        case SignInLogOrder.Account:
                            orderedOperationLog = queryFuture.OrderBy(x => x.Account);
                            break;
                        case SignInLogOrder.ClientInfo:
                            orderedOperationLog = queryFuture.OrderBy(x => x.Ip);
                            break;
                        case SignInLogOrder.Result:
                            orderedOperationLog = queryFuture.OrderBy(x => x.SignInResult);
                            break;
                        case SignInLogOrder.SignInTime:
                            orderedOperationLog = queryFuture.OrderBy(x => x.SignInTime);
                            break;
                        case SignInLogOrder.IP:
                            orderedOperationLog = queryFuture.OrderBy(x => x.Ip);
                            break;
                    }
                }
            }
            else
            {
                orderedOperationLog = queryFuture.OrderByDescending(x => x.SignInTime);
            }
            models = orderedOperationLog
                .Skip(searchInfo.StartIndex)
                .Take(searchInfo.EndIndex - searchInfo.StartIndex + 1)
                .ToList();
            return models;
        }

        private SignInLog Map(SignInLogModel model)
        {
            return new SignInLog
            {
                ID = model.ID,
                Account = model.Account,
                Ip = model.Ip,
                Result = (SignInResult)model.SignInResult,
                SignInTime = model.SignInTime
            };
        }
    }
}
